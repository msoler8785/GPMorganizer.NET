using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GPMornanizer.NET
{
    class Program
    {

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: morganize <file>");
                Environment.Exit(-1);
                return;
            }

            List<ReportEntry> entries = new List<ReportEntry>();

            using (var fs = new FileStream(args[0], FileMode.Open))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineItems = sr.ReadLine().Split(',');

                    // Create ReportEntry for each line in source file.
                    var entry = new ReportEntry(lineItems[0], lineItems[1], lineItems[2]);
                    entries.Add(entry);
                }
            }

            using (var fs = new FileStream("Output.csv", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                // Select distinct dates
                var dates = entries.Select(x => x.EntryDate).Distinct();

                // Create the CSV header.
                sw.Write($"IP Address,");
                foreach (var date in dates)
                {
                    sw.Write($"{date.ToShortDateString()},");
                }
                sw.WriteLine();

                // Group entries by IPAddress
                var groupings = entries.GroupBy(x => x.IPAddress);
                foreach (var ip in groupings)
                {
                    Console.WriteLine($"Processing {ip.Key.ToString()}");
                    sw.Write($"{ip.Key.ToString()},");

                    // Select reputation if there is an entry for that date.
                    var rowItems = dates.Select(date=>
                    {
                        var entry = ip.FirstOrDefault(item => item.EntryDate == date);

                        return entry?.Reputation ?? String.Empty;
                    });

                    // Create the CSV row of data.
                    string rowData = String.Join(",", rowItems);                         

                    sw.WriteLine(rowData);
                }

                Console.WriteLine("Processing complete.");
            }
        }
    }
}
