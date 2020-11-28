using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
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

        // GET: NewsController
        public ActionResult Index()
        {
            return View();
        }

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
    }
}
