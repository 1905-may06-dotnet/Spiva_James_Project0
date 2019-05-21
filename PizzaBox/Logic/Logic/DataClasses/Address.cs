using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.PizzaBox.DataClasses
{
    public class Address
    {
        public int? ID { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }

        public Address (Data.PizzaBox.DataModel.Address address)
        {
            ID = address.Id;
            Street1 = address.Street1;
            Street2 = address.Street2;
            City = address.City;
            State = address.State;
            Country = address.Country;
            Zipcode = address.Zipcode;
        }

        public string AsString ()
        {
            string s = $"{Street1} ";
            if (Street2 != null) s += $"{Street2} ";
            s += $"{City} {State}, ({Country}) {Zipcode}";
            return s;
        }
    }
}
