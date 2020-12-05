using Microsoft.AspNetCore.Mvc;
using NewsPublish.Service;

namespace NewsPublish.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
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

                var newsList = _newsService.GetNewsList(t => t.NewsClassifyId == classifyId, 6);
                ViewData["NewsList"] = newsList;

                var latestCommentNewsList = _newsService.GetLatestNewsListByComment(t => t.NewsClassifyId == classifyId, 6);
                ViewData["LatestCommentNewsList"] = latestCommentNewsList;
            }

            return View(_newsService.GetNewsClassifyList());
        }
    }
}
