using Newtonsoft.Json;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MovieApp.Models.RatingsDb;

namespace MovieApp.Controllers
{
    public class HomeController : Controller
    {
        private string apiKey = "a63b1ef2eebf8d6a196080444128dc84";
        public ActionResult Index(string searchedMovie, int? page)
        {
            CallAPI(searchedMovie, page);
            Models.MovieModel movie = new Models.MovieModel();
            return View(movie);
        }
        [HttpPost]
        public ActionResult Index(MovieModel movie, string searchedText)
        {
            if (ModelState.IsValid)
            {
                CallAPI(searchedText, 0);
            }
            return View(movie);
        }

        public void CallAPI(string searchedText, int? page)
        {
            int pageNo = page == null || Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page);

            /*Calling API https://developers.themoviedb.org/3/search/search-movie */
            HttpWebRequest apiRequest = (searchedText != "" && searchedText != null) ? WebRequest.Create("https://api.themoviedb.org/3/search/movie?api_key=" + apiKey + "&language=en-US&query=" + searchedText + "&page=" + pageNo) as HttpWebRequest : WebRequest.Create("https://api.themoviedb.org/3/movie/top_rated?api_key=" + apiKey + "&language=en-US&page=" + pageNo) as HttpWebRequest;

            string apiResponse = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                            | SecurityProtocolType.Tls
                            | SecurityProtocolType.Tls11
                            | SecurityProtocolType.Tls12;
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            ResponseSearchMovie rootObject = JsonConvert.DeserializeObject<ResponseSearchMovie>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"resultDiv\"><p>Top rated movies</p>");
            foreach (Result result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string link = Url.Action("GetMovie", "Home", new { id = result.id });

                sb.Append("<div class=\"result\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img src=\"" + image + "\" />" + "<p>" + result.title + "</a></p></div>");

            }

            ViewBag.Result = sb.ToString();

            int pageSize = 20;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }

        public ActionResult GetMovie(int id)
        {
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/movie/" + id + "?api_key=" + apiKey + "&language=en-US") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            ResponseMovie rootObject = JsonConvert.DeserializeObject<ResponseMovie>(apiResponse);
            MovieModel movie = new MovieModel();
            movie.adult = rootObject.adult;
            movie.backdrop_path = rootObject.backdrop_path;
            //movie.belongs_to_collection = rootObject.belongs_to_collection;
            movie.budget = rootObject.budget;
            movie.genres = rootObject.genres;
            movie.homepage = rootObject.homepage;
            movie.id = rootObject.id;
            movie.imdb_id = rootObject.imdb_id;
            movie.original_language = rootObject.original_language;
            movie.original_title = rootObject.original_title;
            movie.poster_path = rootObject.poster_path;
            movie.title = rootObject.title;
            movie.release_date = rootObject.release_date;
            movie.overview = rootObject.overview;
            movie.popularity = rootObject.popularity;
            movie.production_companies = rootObject.production_companies;
            movie.production_countries = rootObject.production_countries;
            movie.revenue = rootObject.revenue;
            movie.runtime = rootObject.runtime;
            movie.spokenLanguages = rootObject.spokenLanguages;
            movie.status = rootObject.status;
            movie.tagline = rootObject.tagline;
            movie.video = rootObject.video;
            movie.vote_average = rootObject.vote_average;
            movie.vote_count = rootObject.vote_count;
            return View(movie);
        }

        [HttpPost]
        public ActionResult RateMovie(string title, string rating)
        {
            TempData["test"] = "testTempData";
            //insert rating in database
            try
            {
                using (var db = new AppMoviesDbContext())
                {
                    Rating ratingRow = new Rating();
                    ratingRow.User = HttpContext.User.Identity.Name;
                    //ratingRow.MovieID = id2;
                    ratingRow.MovieTitle = title;
                    ratingRow.UserRating = rating;

                    db.Ratings.Add(ratingRow);
                    db.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Please try again!');</script>");
            }
        }
    }
}