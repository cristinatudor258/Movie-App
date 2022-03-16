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
        private readonly AppMoviesDbContext dbContext = new AppMoviesDbContext();

        //method called on page changing
        public ActionResult Index(int? page)
        {
            CallAPI("", page);
            Models.MovieModel movie = new Models.MovieModel();
            return View(movie);
        }

        //search a movie by its name
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
            foreach (MovieModel result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string starImg = Url.Content("~/Content/Icons/star.png");
                string link = Url.Action("GetMovie", "Home", new { id = result.id });

                sb.Append("<div class=\"result\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img class=\"img img-thumbnail\" src=\"" + image + "\" />" + "<p>" + result.title + "</a></p><p class=\"voteAvg\"'><img class=\"voteImg\" src=\""+starImg+"\"/>"+result.vote_average+"</p></div>");

            }

            ViewBag.Result = sb.ToString();

            int pageSize = 20;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }

        //redirect to movie details page
        public ActionResult GetMovie(int id)
        {
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/movie/" + id + "?api_key=" + apiKey + "&language=en-US&append_to_response=videos") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            MovieModel movie = JsonConvert.DeserializeObject<MovieModel>(apiResponse);
            return View(movie);
        }

        //add a comment about movie - just for logged users!
        [HttpPost]
        public ActionResult RateMovie(int id, string title, string rating)
        {
            TempData["test"] = "testTempData";
            //insert rating in database
            try
            {
                using (dbContext)
                {
                    Rating ratingRow = new Rating();
                    ratingRow.User = HttpContext.User.Identity.Name;
                    ratingRow.MovieID = id;
                    ratingRow.MovieTitle = title;
                    ratingRow.UserRating = rating;

                    dbContext.Ratings.Add(ratingRow);
                    dbContext.SaveChanges();

                    return Content("Thanks for Feedback!");
                }
            }
            catch (Exception ex)
            {
                return Content("Please try again!");
            }
        }
    }
}