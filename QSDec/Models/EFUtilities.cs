using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace QSDec
{
    public static class EFUtilities
    {

        /// <summary>
        /// Truncates the table, loads the records provided, and saves the changes
        /// </summary>
        public static void Reload<T>(this DbContext context, DbSet<T> table, IEnumerable<T> records) where T : class, new()
        {
            context.Truncate(table);
            context.Append(table, records);
        }
        public static void Append<T>(this DbContext context, DbSet<T> table, IEnumerable<T> records) where T : class, new()
        {
            table.AddRange(records);
            context.SaveChanges();
        }

        /// <summary>
        /// Truncates the table and saves the changes
        /// </summary>
        public static void Truncate<T>(this DbContext context, DbSet<T> table) where T : class, new()
        {
            table.Truncate();
            context.SaveChanges();
        }


        /// <summary>
        /// Truncates the table.  Does not save changes to DB.
        /// </summary>
        public static void Truncate<T>(this DbSet<T> table) where T : class, new()
             => table.RemoveRange(table.ToList());

    }
}
