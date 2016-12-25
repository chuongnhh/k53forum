using K53Forum.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    public class DbK53Forum : DbContext
    {
        public DbK53Forum() : base("name=K53Forum")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbK53Forum, Configuration>("K53Forum"));
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Liked> Likeds { get; set; }

        public virtual DbSet<Discussion> Discussions { get; set; }
        public virtual DbSet<Tutorial> Tutorials { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }

        public virtual DbSet<Carousel> Carousels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}