﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingCoe800.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()//news and events tab
        {
            //ViewBag.Message = "The latest news and events:";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MallTracker()
        {
            return View();
        }

        public ActionResult StoreDirectory()
        {
            return View();
        }

        public ActionResult MallConfig()
        {
            return View();
        }
    }
}