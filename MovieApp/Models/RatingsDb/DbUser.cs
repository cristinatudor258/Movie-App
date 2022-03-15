using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models.RatingsDb
{
    public class DbUser
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public int Password { get; set; }
    }
}