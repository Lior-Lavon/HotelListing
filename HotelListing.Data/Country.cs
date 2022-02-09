using System.Collections.Generic;

namespace HotelListing.Data
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        // hold the list of hotels in a country X
        public virtual IList<Hotel> Hotels { get; set; }

        public int Color { get; set; }


    }
}
