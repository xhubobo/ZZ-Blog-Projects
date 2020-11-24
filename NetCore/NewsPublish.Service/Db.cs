using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;

namespace NewsPublish.Service
{
    public class Db : DbContext
    {
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<NewsClassify> NewsClassify { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsComment> NewsComment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(
                "server=localhost; userid=root; pwd=root; database=NewsPublish; pooling=false");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
