using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly BannerService _bannerService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BannerController(BannerService bannerService, IHostingEnvironment hostingEnvironment)
        {
            _bannerService = bannerService;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: BannerController
        public ActionResult Index()
        {
            var bannerList = _bannerService.GetBannerList();
            return View(bannerList);
        }

        // GET: BannerController/BannerAdd
        public ActionResult BannerAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddBanner(AddBanner addBanner, IFormCollection collection)
        {
            var files = collection.Files;
            if (files.Count == 0)
            {
                return Json(new ResponseModel(0, "请上传图片文件"));
            }

            var webRootPath = _hostingEnvironment.WebRootPath;
            var relativeDirPath = "BannerPic";
            var absolutePath = Path.Combine(webRootPath, relativeDirPath);

            var fileTypes = new[] {".gif", ".jpg", ".jpeg", ".png", ".bmp"};
            var extension = Path.GetExtension(files[0].FileName);
            if (!fileTypes.Contains(extension.ToLower()))
            {
                return Json(new ResponseModel(0, "图片格式有误"));
            }

            //检测文件夹
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}{extension}";
            var filePath = Path.Combine(absolutePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await files[0].CopyToAsync(stream);
            }

            addBanner.Image = $"/BannerPic/{fileName}";
            return Json(_bannerService.AddBanner(addBanner));
        }

        [HttpPost]
        public JsonResult DeleteBanner(int bannerId)
        {
            if (bannerId <= 0)
            {
                return Json(new ResponseModel(0, "参数有误"));
            }

            return Json(_bannerService.DeleteBanner(bannerId));
        }

        // GET: BannerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BannerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BannerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BannerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BannerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BannerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
