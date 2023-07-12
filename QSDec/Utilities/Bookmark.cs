using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSDec
{
    /// <summary>
    /// PDF document bookmark.  Sortable by page number.
    /// </summary>
    public class Bookmark : IComparable<Bookmark>
    {
        public string Action { get; set; }
        
        /// <summary>
        /// Name of the bookmark
        /// </summary>
        public string Title { get; set; }
        public string Color { get; set; }
        public string Style { get; set; }

        /// <summary>
        /// Example "3 Fit"
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// Numeric portion of the "Page"
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The last page the bookmark covers
        /// </summary>
        public int LastPageNumber { get; set; }

        /// <summary>
        /// Extracted byte array of the PDF pages from the original document
        /// </summary>
        public byte[] Pages { get; set; }

        public List<Bookmark> Kids { get; set; }

        public Dictionary<string, object> Other { get; set; }

        public int CompareTo(Bookmark other) => PageNumber.CompareTo(other.PageNumber);
    }

}
