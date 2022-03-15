using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models
{
    public class Result
    {
        public string poster_path { get; set; }
        public bool adult { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public DateTime? release_date { get; set; }
        public decimal vote_average { get; set; }
    }

}