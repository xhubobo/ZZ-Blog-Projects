using System;
using System.Collections.Generic;

namespace NewsPublish.Model.Entity
{
    public class News
    {
        public int Id { get; set; }
        public int NewsClassifyId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Contents { get; set; }
        public DateTime PublishDate { get; set; }
        public string Remark { get; set; }

        //外键
        public virtual NewsClassify NewsClassify { get; set; }
        public virtual ICollection<NewsComment> NewsComment { get; set; }

        public News()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            NewsComment =  new HashSet<NewsComment>();
        }
    }
}
