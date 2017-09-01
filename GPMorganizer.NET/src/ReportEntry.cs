using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GPMornanizer.NET
{
    public class ReportEntry
    {
        public DateTime EntryDate { get; set; }
        public IPAddress IPAddress { get; set; }
        public string Reputation { get; set; }


        public ReportEntry(string entryDate, string ipAddress, string reputation)
        {
            EntryDate = DateTime.Parse(entryDate);
            IPAddress = IPAddress.Parse(ipAddress);
            Reputation = reputation;
        }
    }
}
