using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QSDec.Steps._5
{
    public static class Idx
    {


        public static void Process(string policyNumber, List<Bookmark> bookmarks)
        {
            var idx = Create(policyNumber, bookmarks);

            var filePath = $"{AppSettings.ImageRightFormsTextImportPath}{policyNumber}.IDX";

            File.WriteAllLines(filePath, idx);
        }

        public static List<string> Create(string policyNumber, List<Bookmark> bookmarks)
        {
            var results = new List<string>();

            for (var x = 0; x < bookmarks.Count; x++)
            {
                var line = CreateLine(policyNumber, bookmarks[x], x + 1);

                results.Add(line);
            }

            return results;
        }

        public static string CreateLine(string policyNumber, Bookmark bookmark, int number)
        {
            string filename = policyNumber + "_" + number + ".pdf";

            string title = GetTitle(bookmark, number);

            string line = filename.PadRight(28) + "$#IY#$" + policyNumber.PadRight(128) + title.PadRight(50) + "C100";

            //string line = $"{policyNumber}_{number}.pdf{28.Spaces()}$#IY#${policyNumber}{128.Spaces()}{title}{50.Spaces()}C100";

            return line;
        }

        public static string GetTitle(Bookmark bookmark, int number)
        {
            if (bookmark.Title.Contains(")"))
                return bookmark.Title.Substring(0, bookmark.Title.IndexOf(")") + 1).Replace(" ", "-", "(", ")");

            // What is #1 vs others mean?
            if (number == 1)
                return bookmark.Title.Substring(0, 25);

            return bookmark.Title.Substring(0, 15);
        }

        public static string Spaces(this int count) => new string(' ', count);

        public static string Replace(this string value, params string[] exclusions)
        {
            string result = value;

            foreach (var exclusion in exclusions)
                result = result.Replace(exclusion, "");

            return result;
        }


    }
}
