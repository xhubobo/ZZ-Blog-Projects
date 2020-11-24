using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;

namespace NewsPublish.Service
{
    public class NewsService
    {
        private readonly Db _db;

        public NewsService(Db db)
        {
            _db = db;
        }

        #region 新闻类别

        /// <summary>
        /// 添加新闻类别
        /// </summary>
        /// <param name="addNewsClassify">新闻类别信息</param>
        /// <returns>执行结果</returns>
        public ResponseModel AddNewsClassify(AddNewsClassify addNewsClassify)
        {
            if (_db.NewsClassify.FirstOrDefault(t => t.Name == addNewsClassify.Name) != null)
            {
                return new ResponseModel(0, "该新闻类别已存在");
            }

            var newsClassify = new NewsClassify()
            {
                Name = addNewsClassify.Name,
                Sort = addNewsClassify.Sort,
                Remark = addNewsClassify.Remark
            };
            _db.NewsClassify.Add(newsClassify);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "添加新闻类别成功")
                : new ResponseModel(0, "添加新闻类别失败");
        }

        /// <summary>
        /// 编辑新闻类别
        /// </summary>
        /// <param name="editNewsClassify">新闻类别信息</param>
        /// <returns>执行结果</returns>
        public ResponseModel EditNewsClassify(EditNewsClassify editNewsClassify)
        {
            var newsClassify = _db.NewsClassify.FirstOrDefault(t => t.Id == editNewsClassify.Id);
            if (newsClassify == null)
            {
                return new ResponseModel(0, "该新闻类别不存在");
            }

            newsClassify.Name = editNewsClassify.Name;
            newsClassify.Sort = editNewsClassify.Sort;
            newsClassify.Remark = editNewsClassify.Remark;
            _db.NewsClassify.Update(newsClassify);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "新闻类别编辑成功")
                : new ResponseModel(0, "新闻类别编辑失败");
        }

        /// <summary>
        /// 获取新闻类别
        /// </summary>
        /// <param name="newsClassifyId">新闻类别ID</param>
        /// <returns>新闻类别</returns>
        public ResponseModel GetNewsClassify(int newsClassifyId)
        {
            var newsClassify = _db.NewsClassify.Find(newsClassifyId);
            if (newsClassify == null)
            {
                return new ResponseModel(0, "该新闻类别不存在");
            }

            return new ResponseModel(200, "新闻类别获取成功")
            {
                Data = new NewsClassifyModel()
                {
                    Id = newsClassify.Id,
                    Name = newsClassify.Name,
                    Sort = newsClassify.Sort,
                    Remark = newsClassify.Remark
                }
            };
        }

        /// <summary>
        /// 获取新闻类别集合
        /// </summary>
        /// <returns>新闻类别集合</returns>
        public ResponseModel GetNewsClassifyList()
        {
            var newsClassifies = _db.NewsClassify.OrderByDescending(t => t.Sort).ToList();
            var response = new ResponseModel(200, "新闻类别获取成功") {Data = new List<NewsClassifyModel>()};

            foreach (var newsClassify in newsClassifies)
            {
                response.Data.Add(new NewsClassifyModel()
                {
                    Id = newsClassify.Id,
                    Name = newsClassify.Name,
                    Sort = newsClassify.Sort,
                    Remark = newsClassify.Remark
                });
            }

            return response;
        }

        #endregion

        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="addNews">新闻信息</param>
        /// <returns>执行结果</returns>
        public ResponseModel AddNews(AddNews addNews)
        {
            var newsClassify = _db.NewsClassify.Find(addNews.NewsClassifyId);
            if (newsClassify == null)
            {
                return new ResponseModel(0, "该新闻类别不存在");
            }

            var news = new News()
            {
                NewsClassifyId = addNews.NewsClassifyId,
                Title = addNews.Title,
                Image = addNews.Image,
                Contents = addNews.Contents,
                PublishDate = DateTime.Now,
                Remark = addNews.Remark
            };
            _db.News.Add(news);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "新闻添加成功")
                : new ResponseModel(0, "新闻添加失败");
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="newsId">新闻ID</param>
        /// <returns>执行结果</returns>
        public ResponseModel DeleteNews(int newsId)
        {
            var news = _db.News.FirstOrDefault(t => t.Id == newsId);
            if (news == null)
            {
                return new ResponseModel(0, "该新闻不存在");
            }

            _db.News.Remove(news);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "新闻删除成功")
                : new ResponseModel(0, "新闻删除失败");
        }

        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="newsId">新闻ID</param>
        /// <returns>新闻信息</returns>
        public ResponseModel GetNews(int newsId)
        {
            var news = _db.News.Include("NewsClassify").Include("NewsComment")
                .FirstOrDefault(t => t.Id == newsId);
            if (news == null)
            {
                return new ResponseModel(0, "该新闻不存在");
            }

            return new ResponseModel(200, "新闻获取成功")
            {
                Data = new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                }
            };
        }

        /// <summary>
        /// 分页查询新闻
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="total">新闻总数</param>
        /// <param name="where">查询条件</param>
        /// <returns>新闻信息集合</returns>
        public ResponseModel QueryNewsPage(int pageSize, int pageIndex, out int total,
            List<Expression<Func<News, bool>>> where)
        {
            var newsList = _db.News.Include("NewsClassify").Include("NewsComment");
            foreach (var expr in where)
            {
                newsList = newsList.Where(expr);
            }

            total = newsList.Count();
            var pageData = newsList.OrderByDescending(t => t.PublishDate)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            var response = new ResponseModel(200, "分页新闻获取成功") {Data = new List<NewsModel>()};
            foreach (var news in pageData)
            {
                response.Data.Add(new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                });
            }

            return response;
        }

        /// <summary>
        /// 查询新闻列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="topCount">请求个数</param>
        /// <returns>新闻列表</returns>
        public ResponseModel GetNewsList(Expression<Func<News, bool>> where, int topCount)
        {
            var newsList = _db.News.Include("NewsClassify").Include("NewsComment")
                .Where(where).OrderByDescending(t => t.PublishDate).Take(topCount);

            var response = new ResponseModel(200, "新闻列表获取成功") {Data = new List<NewsModel>()};
            foreach (var news in newsList)
            {
                response.Data.Add(new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents.Length > 50
                        ? news.Contents.Substring(50)
                        : news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                });
            }

            return response;
        }

        /// <summary>
        /// 获取最新评论新闻集合
        /// </summary>
        /// <param name="topCount">请求个数</param>
        /// <returns>新闻集合</returns>
        public ResponseModel GetLatestNewsListByComment(int topCount)
        {
            var newsIds = _db.NewsComment.OrderByDescending(t => t.AddTime)
                .GroupBy(t => t.NewsId).Select(t => t.Key).Take(topCount);
            var newsList = _db.News.Include("NewsClassify").Include("NewsComment")
                .Where(t => newsIds.Contains(t.Id)).OrderByDescending(t => t.PublishDate);

            var response = new ResponseModel(200, "最新评论新闻获取成功") {Data = new List<NewsModel>()};
            foreach (var news in newsList)
            {
                response.Data.Add(new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                });
            }

            return response;
        }

        /// <summary>
        /// 搜索一个新闻
        /// </summary>
        /// <param name="where">搜索条件</param>
        /// <returns>新闻信息</returns>
        public ResponseModel SearchOneNews(Expression<Func<News, bool>> where)
        {
            var news = _db.News.Where(where).FirstOrDefault();
            if (news == null)
            {
                return new ResponseModel(0, "新闻搜索失败");
            }

            return new ResponseModel(200, "新闻搜索成功")
            {
                Data = new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                }
            };
        }

        /// <summary>
        /// 获取新闻个数
        /// </summary>
        /// <param name="where">搜索条件</param>
        /// <returns>新闻个数</returns>
        public ResponseModel GetNewsCount(Expression<Func<News, bool>> where)
        {
            var count = _db.News.Where(where).Count();
            return new ResponseModel(200, "新闻个数获取成功")
            {
                Data = count
            };
        }

        /// <summary>
        /// 获取推荐新闻列表
        /// </summary>
        /// <param name="newsId">新闻ID</param>
        /// <returns>新闻列表</returns>
        public ResponseModel GetRecommendNewsList(int newsId)
        {
            var currentNews = _db.News.FirstOrDefault(t => t.Id == newsId);
            if (currentNews == null)
            {
                return new ResponseModel(0, "新闻不存在");
            }

            var newsList = _db.News.Include("NewsComment")
                .Where(t => t.NewsClassify.Id == currentNews.NewsClassifyId && t.Id != newsId)
                .OrderByDescending(t => t.PublishDate).ThenBy(t => t.NewsComment.Count)
                .Take(6).ToList();

            var response = new ResponseModel(200, "最新评论新闻获取成功") {Data = new List<NewsModel>()};
            foreach (var news in newsList)
            {
                response.Data.Add(new NewsModel()
                {
                    Id = news.Id,
                    NewsClassifyName = news.NewsClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents.Length > 50
                        ? news.Contents.Substring(50)
                        : news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count,
                    Remark = news.Remark
                });
            }

            return response;
        }
    }
}
