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
        //check get movie returns a view even if page is 0 
        [TestMethod]
        public void TestIndexView()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index(0) as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        //check GetMovie returns paging ViewBag
        [TestMethod]
        public void TestGetMovie()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.GetMovie(4) as ViewResult;
            Assert.IsNotNull(result.ViewBag.Paging);
        }

        //check home controller returns correct tempData
        [TestMethod]
        public void TestTempData()
        {
            HomeController controller = new HomeController();
            MovieModel model = new MovieModel();
            ViewResult result = controller.RateMovie(3, "testTitle", "test") as ViewResult;
            Assert.AreEqual("testTempData", result.TempData["test"]);
        }
    }
}
