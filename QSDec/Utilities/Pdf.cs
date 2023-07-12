using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace QSDec
{


    public static class Pdf
    {

        #region Page Extraction
        public static void Extract(string source, string destination, int startPage, int endPage, string ownerPassword = null)

        {

            var original = File.ReadAllBytes(source);
            var pages = Enumerable.Range(startPage, endPage - startPage + 1).ToArray();
            var copy = Extract(original, ownerPassword, pages);

            File.WriteAllBytes(destination, copy);

        }

        public static byte[] Extract(byte[] source, string ownerPassword, params int[] pages)
        {
            var reader = GetReader(source, ownerPassword);
            try
            {
                return Extract(reader);
            }
            catch (Exception)
            {
                throw;
            }
            finally { reader.Close(); }
        }

        public static byte[] Extract(PdfReader reader, int startPage, int endPage) => Extract(reader, Enumerable.Range(startPage, endPage - startPage + 1).ToArray());

        public static byte[] Extract(PdfReader reader, params int[] pages)
        {
            //https://www.codeproject.com/Articles/559380/Splitting-and-Merging-PDF-Files-in-Csharp-Using-iT

            var doc = new Document(reader.GetPageSizeWithRotation(pages[0])); // assumes all pages are same size & rotation

            using (var ms = new MemoryStream())
            {
                var copier = new PdfCopy(doc, ms);

                doc.Open();

                foreach (var page in pages)
                    Copy(copier, reader, page);

                copier.Close();
                doc.Close();

                return ms.ToArray();
            }
        }

        public static PdfReader GetReader(byte[] source, string ownerPassword)
            => String.IsNullOrWhiteSpace(ownerPassword) ? new PdfReader(source) : new PdfReader(source, Encoding.ASCII.GetBytes(ownerPassword));

        public static void Copy(PdfCopy copier, PdfReader reader, int page)
            => copier.AddPage(copier.GetImportedPage(reader, page));

        #endregion

        #region Bookmarks


        public static List<Bookmark> GetBookmarks(string filePath, string ownerPassword = null, bool withPages = true) => GetBookmarks(File.ReadAllBytes(filePath), ownerPassword, withPages);

        public static List<Bookmark> GetBookmarks(byte[] source, string ownerPassword, bool withPages = true)
        {
            var reader = GetReader(source, ownerPassword);
            var result = GetBookmarks(reader, withPages);

            reader.Close();

            return result;
        }

        public static List<Bookmark> GetBookmarks(PdfReader reader, bool withPages = true)
        {
            var marks = GetBookmarks(SimpleBookmark.GetBookmark(reader));

            FillInLastPageNumbers(marks, reader.NumberOfPages);

            if (withPages)
                FillInPages(reader, marks);

            return marks;

        }

        public static List<Bookmark> GetBookmarks(ArrayList items)
        {
            var results = new List<Bookmark>();

            if (items == null)// if no bookmarks exist, items can be null
                return results;

            foreach (Hashtable item in items)
                results.Add(GetBookmark(item));

            results.Sort();

            return results;
        }

        public static Bookmark GetBookmark(Hashtable values)
        {
            var result = new Bookmark()
            {
                Action = GetValue<string>(values, "Action"),
                Title = GetValue<string>(values, "Title"),
                Color = GetValue<string>(values, "Color"),
                Style = GetValue<string>(values, "Style"),
                Page = GetValue<string>(values, "Page"),
                Kids = values.ContainsKey("Kids") ? GetBookmarks((ArrayList)values["Kids"]) : new List<Bookmark>(),
                Other = GetOther(values, "Action", "Title", "Color", "Style", "Page", "Kids")
            };

            result.PageNumber = result.Page.ToInt();

            return result;
        }

        public static Dictionary<string, object> GetOther(Hashtable values, params string[] excluding)
        {
            var results = new Dictionary<string, object>();

            foreach (string key in values.Keys)
            {
                if (excluding.Contains(key))
                    continue;

                results.Add(key, values[key]);
            }

            return results;
        }


        public static T GetValue<T>(Hashtable values, string key)
        {
            // Explicit & fast find
            if (values.ContainsKey(key))
                return (T)values[key];

            // Implicit & slow find
            foreach (string k in values.Keys)
            {
                if (k.ToLowerInvariant() == key.ToLowerInvariant())
                    return (T)values[k];
            }

            return default(T);
        }

        public static void FillInLastPageNumbers(List<Bookmark> bookmarks, int lastPageNumber)
        {
            if (bookmarks.Count == 0)
                return;

            Bookmark previous = null;

            foreach (var mark in bookmarks)
            {
                if (previous != null)
                {
                    previous.LastPageNumber = mark.PageNumber - 1;

                    if (previous.Kids.Count > 0)
                        FillInLastPageNumbers(previous.Kids, previous.LastPageNumber);
                }

                previous = mark;
            }

            previous.LastPageNumber = lastPageNumber;
            FillInLastPageNumbers(previous.Kids, previous.LastPageNumber);
        }

        public static void FillInPages(PdfReader reader, List<Bookmark> marks) => marks.ForEach(mark => FillInPages(reader, mark));

        public static void FillInPages(PdfReader reader, Bookmark mark)
        {
            mark.Pages = Extract(reader, mark.PageNumber, mark.LastPageNumber);

            FillInPages(reader, mark.Kids);
        }


        public static int ToInt(this string value) => string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(Regex.Match(value, @"\d+").ToString());

        #endregion

    }

}
