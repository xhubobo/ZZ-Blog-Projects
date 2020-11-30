using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly NewsService _newsService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public NewsController(NewsService newsService, IHostingEnvironment hostingEnvironment)
        {
            _newsService = newsService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region 新闻

        // GET: NewsController
        public ActionResult Index()
        {
            var newsClassifyList = _newsService.GetNewsClassifyList();
            return View(newsClassifyList);
        }

        [HttpGet]
        public JsonResult GetNews(int pageIndex, int pageSize, int classifyId, string keyword)
        {
            var whereList = new List<Expression<Func<News, bool>>>();
            if (classifyId > 0)
            {
                whereList.Add(t => t.NewsClassifyId == classifyId);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                whereList.Add(t => t.Title.Contains(keyword));
            }

            var news = _newsService.QueryNewsPage(pageSize, pageIndex, out var total, whereList);
            return Json(new {total = total, data = news.Data});
        }

        public ActionResult AddNews()
        {
            var newsClassifyList = _newsService.GetNewsClassifyList();
            return View(newsClassifyList);
        }

        [HttpPost]
        public async Task<JsonResult> AddNews(AddNews addNews, IFormCollection formCollection)
        {
            if (addNews.NewsClassifyId <= 0 || string.IsNullOrWhiteSpace(addNews.Title) ||
                string.IsNullOrWhiteSpace(addNews.Contents))
            {
                return Json(new ResponseModel(0, "参数有误"));
            }

            var files = formCollection.Files;
            if (files.Count == 0)
            {
                return Json(new ResponseModel(0, "请上传图片文件"));
            }

            var webRootPath = _hostingEnvironment.WebRootPath;
            var absoluteDirPath = Path.Combine(webRootPath, "NewsPic");
            var fileTypes = new[] {".gif", ".jpg", ".jpeg", ".png", ".bmp"};
            var extension = Path.GetExtension(files[0].FileName);
            if (!fileTypes.Contains(extension))
            {
                return Json(new ResponseModel(0, "图片格式有误"));
            }

            if (!Directory.Exists(absoluteDirPath))
            {
                Directory.CreateDirectory(absoluteDirPath);
            }

            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}{extension}";
            var filePath = Path.Combine(absoluteDirPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files[0].CopyToAsync(stream);
            }

            addNews.Image = $"/NewsPic/{fileName}";
            return Json(_newsService.AddNews(addNews));
        }

        [HttpPost]
        public JsonResult DeleteNews(int newsId)
        {
            return Json(newsId <= 0
                ? new ResponseModel(0, "新闻不存在")
                : _newsService.DeleteNews(newsId));
        }

        #endregion

        #region 新闻类别

        // GET: NewsController/NewsClassify
        public ActionResult NewsClassify()
        {
            var newsClassifyList = _newsService.GetNewsClassifyList();
            return View(newsClassifyList);
        }

        // GET: NewsController/AddNewsClassify
        public ActionResult AddNewsClassify()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddNewsClassify(AddNewsClassify addNewsClassify)
        {
            return Json(string.IsNullOrEmpty(addNewsClassify.Name)
                ? new ResponseModel(0, "请输入新闻类别名称")
                : _newsService.AddNewsClassify(addNewsClassify));
        }

        // GET: NewsController/EditNewsClassify
        public ActionResult EditNewsClassify(int newsClassifyId)
        {
            return View(_newsService.GetNewsClassify(newsClassifyId));
        }

        [HttpPost]
        public JsonResult EditNewsClassify(EditNewsClassify editNewsClassify)
        {
            return string.IsNullOrEmpty(editNewsClassify.Name)
                ? Json(new ResponseModel(0, "请输入新闻类别名称"))
                : Json(_newsService.EditNewsClassify(editNewsClassify));
        }

        #endregion
    }
}
