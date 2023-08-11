using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Shared
{
    public class TestAddress
    {
        public int  AddresID { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string StateProvince{ get; set; }

        public string CountryRegion { get; set; }

        public string PostalCode { get; set; }

        public Guid guid { get; set; }

        public DateTime ModifiedtDate { get; set; }
        public long UpdateTick { get; set; }


    }
}
