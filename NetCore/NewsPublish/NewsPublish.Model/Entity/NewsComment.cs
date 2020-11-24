using System;

namespace NewsPublish.Model.Entity
{
    public class NewsComment
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public string Remark { get; set; }

        //外键
        public virtual News News { get; set; }
    }
}
