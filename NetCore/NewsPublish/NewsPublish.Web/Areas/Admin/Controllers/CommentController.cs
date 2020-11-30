using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Response;
using NewsPublish.Service;

namespace NewsPublish.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View(_commentService.GetCommentList(t => true));
        }

        [HttpPost]
        public JsonResult DeleteComment(int commentId)
        {
            return Json(commentId <= 0 
                ? new ResponseModel(0, "参数有误") 
                : _commentService.DeleteComment(commentId));
        }
    }
}
