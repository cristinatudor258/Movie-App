using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieApp;
using MovieApp.Controllers;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MovieApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //check get movie returns a view
        [TestMethod]
        public void TestHomeControllerMovieView()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.GetMovie(256) as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        //check home controller returns correct tempData
        [TestMethod]
        public void TestHomeControllerTempData()
        {
            HomeController controller = new HomeController();
            MovieModel model = new MovieModel();
            ViewResult result = controller.RateMovie(3) as ViewResult;
            Assert.AreEqual("testTempData", result.TempData["test"]);
        }
    }
}
