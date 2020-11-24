using System.Collections.Generic;

namespace NewsPublish.Model.Entity
{
    public class NewsClassify
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }

        //新闻列表
        public virtual ICollection<News> News { get; set; }

        public NewsClassify()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            News = new HashSet<News>();
        }
    }
}
