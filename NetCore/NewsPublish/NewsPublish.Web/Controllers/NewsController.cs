﻿using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsService _newsService;
        private readonly CommentService _commentService;

        public NewsController(NewsService newsService, CommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }

        public ActionResult Classify(int classifyId)
        {
            if (classifyId <= 0)
            {
                Response.Redirect("/Home/Index/");
            }

            var classify = _newsService.GetNewsClassify(classifyId);
            if (classify.Code == 0)
            {
                Response.Redirect("/Home/Index/");
            }

            if (classify.Code > 0)
            {
                ViewData["ClassifyName"] = classify.Data.Name;

                var newsList = _newsService.GetNewsList(t => t.NewsClassifyId == classifyId, int.MaxValue);
                ViewData["NewsList"] = newsList;

                var latestCommentNewsList = _newsService.GetLatestNewsListByComment(t => t.NewsClassifyId == classifyId, 6);
                ViewData["LatestCommentNewsList"] = latestCommentNewsList;
            }

            return View(_newsService.GetNewsClassifyList());
        }

        public IActionResult Detail(int newsId)
        {
            ViewData["Title"] = "详情页";
            ViewData["News"] = new ResponseModel();
            ViewData["RecommendNewsList"] = new ResponseModel();
            ViewData["CommentList"] = new ResponseModel();

            if (newsId < 0)
            {
                Response.Redirect("/Home/Index");
            }

            var news = _newsService.GetNews(newsId);
            if (news.Code == 0)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewData["Title"] = news.Data.Title + "-" + ViewData["Title"];
                ViewData["News"] = news;

                var recommendNewsList = _newsService.GetRecommendNewsList(newsId);
                ViewData["RecommendNewsList"] = recommendNewsList;

                var commentList = _commentService.GetCommentList(t => t.NewsId == newsId);
                ViewData["CommentList"] = commentList;
            }

            return View(_newsService.GetNewsClassifyList());
        }
    }
}
