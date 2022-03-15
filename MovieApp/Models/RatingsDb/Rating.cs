using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models.RatingsDb
{
    public class Rating
    {
        public int ID { get; set; }
        public string User { get; set; }
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string UserRating { get; set; }
    }
}