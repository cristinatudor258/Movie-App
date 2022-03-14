using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models.RatingsDb
{
    public class Rating
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserRating { get; set; }
    }
}