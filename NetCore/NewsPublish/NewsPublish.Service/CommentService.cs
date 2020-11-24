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
    public class CommentService
    {
        private readonly Db _db;
        private readonly NewsService _newsService;

        public CommentService(Db db, NewsService newsService)
        {
            _db = db;
            _newsService = newsService;
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="addComment">评论信息</param>
        /// <returns>执行结果</returns>
        public ResponseModel AddComment(AddComment addComment)
        {
            var news = _newsService.GetNews(addComment.NewsId);
            if (news.Code == 0)
            {
                return new ResponseModel(0, "新闻不存在");
            }

            var newsComment = new NewsComment()
            {
                AddTime = DateTime.Now,
                NewsId = addComment.NewsId,
                Contents = addComment.Comments
            };
            _db.NewsComment.Add(newsComment);

            var ret = _db.SaveChanges();
            if (ret > 0)
            {
                return new ResponseModel(200, "新闻评论添加成功")
                {
                    Data = new
                    {
                        contents = addComment.Comments,
                        floor = $"#{(news.Data.CommentCount + 1)}",
                        addTime = newsComment.AddTime
                    }
                };
            }

            return new ResponseModel(0, "新闻评论添加失败");
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentId">评论ID</param>
        /// <returns>执行结果</returns>
        public ResponseModel DeleteComment(int commentId)
        {
            var comment = _db.NewsComment.Find(commentId);
            if (comment == null)
            {
                return new ResponseModel(0, "评论不存在");
            }

            _db.NewsComment.Remove(comment);

            var ret = _db.SaveChanges();
            return ret > 0
                ? new ResponseModel(200, "新闻评论删除成功")
                : new ResponseModel(0, "新闻评论删除失败");
        }

        /// <summary>
        /// 获取评论集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>评论集合</returns>
        public ResponseModel GetCommentList(Expression<Func<NewsComment, bool>> where)
        {
            var commentList = _db.NewsComment.Include("News").Where(where).OrderBy(t => t.AddTime).ToList();
            var response = new ResponseModel(200, "评论获取成功") {Data = new List<CommentModel>()};

            var floor = 1;
            foreach (var comment in commentList)
            {
                response.Data.Add(new CommentModel()
                    {
                        Id = comment.Id,
                        NewsName = comment.News.Title,
                        Contents = comment.Contents,
                        AddTime = comment.AddTime,
                        Remark = comment.Remark,
                        Floor = $"#{floor++}"
                    }
                );
            }

            response.Data.Reverse();
            return response;
        }
    }
}
