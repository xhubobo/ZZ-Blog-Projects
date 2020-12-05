using Microsoft.AspNetCore.Mvc;
using NewsPublish.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsService _newsService;

        private readonly BannerService _bannerService;

        public HomeController(NewsService newsService, BannerService bannerService)
        {
            _newsService = newsService;
            _bannerService = bannerService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "首页";
            return View(_newsService.GetNewsClassifyList());
        }

        [HttpGet]
        public JsonResult GetBanner()
        {
            return Json(_bannerService.GetBannerList());
        }

        [HttpGet]
        public JsonResult GetNewsCount()
        {
            return Json(_newsService.GetNewsCount(t => true));
        }

        [HttpGet]
        public JsonResult GetHomeNews()
        {
            return Json(_newsService.GetNewsList(t => true, 6));
        }

        [HttpGet]
        public JsonResult GetLatestNewsListByComment()
        {
            return Json(_newsService.GetLatestNewsListByComment(t => true, 5));
        }

        [HttpGet]
        public JsonResult SearchOneNews(string keyword)
        {
            return Json(string.IsNullOrWhiteSpace(keyword) 
                ? new ResponseModel(0, "关键字不能为空") 
                : _newsService.SearchOneNews(t => t.Title.Contains(keyword)));
        }

        public IActionResult PageNotFound()
        {
            ViewData["Title"] = "404";
            return View(_newsService.GetNewsClassifyList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
