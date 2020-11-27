using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;

namespace NewsPublish.Service
{
    /// <summary>
    /// Banner服务
    /// </summary>
    public class BannerService
    {
        private readonly Db _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BannerService(Db db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 添加Banner
        /// </summary>
        /// <param name="addBanner">Banner信息</param>
        /// <returns>执行结果</returns>
        public ResponseModel AddBanner(AddBanner addBanner)
        {
            var banner = new Banner()
            {
                AddTime = DateTime.Now,
                Image = addBanner.Image,
                Url = addBanner.Url,
                Remark = addBanner.Remark
            };
            _db.Banner.Add(banner);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "Banner添加成功")
                : new ResponseModel(0, "Banner添加失败");
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="bannerId">Banner ID</param>
        /// <returns>执行结果</returns>
        public ResponseModel DeleteBanner(int bannerId)
        {
            var banner = _db.Banner.Find(bannerId);
            if (banner == null)
            {
                return new ResponseModel(0, "Banner不存在");
            }

            _db.Banner.Remove(banner);
            var ret = _db.SaveChanges();
            if (ret > 0)
            {
                //删除Banner图片
                var webRootPath = _hostingEnvironment.WebRootPath;
                var imagePath = banner.Image.TrimStart('/');
                var filePath = Path.Combine(webRootPath, imagePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return ret > 0
                ? new ResponseModel(200, "Banner删除成功")
                : new ResponseModel(0, "Banner删除失败");
        }

        /// <summary>
        /// 获取Banner集合
        /// </summary>
        /// <returns>Banner集合</returns>
        public ResponseModel GetBannerList()
        {
            var banners = _db.Banner.ToList().OrderByDescending(t => t.AddTime);
            var response = new ResponseModel(200, "Banner集合获取成功")
            {
                Data = new List<BannerModel>()
            };
            foreach (var banner in banners)
            {
                response.Data.Add(new BannerModel()
                {
                    Id = banner.Id,
                    Image = banner.Image,
                    Url = banner.Url,
                    Remark = banner.Remark
                });
            }

            return response;
        }
    }
}
