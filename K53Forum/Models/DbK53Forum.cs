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
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Liked> Likeds { get; set; }
        public virtual DbSet<DisLiked> DisLikeds { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<Comment>().HasRequired(c => c.Post).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Reply>().HasRequired(c => c.Comment).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Liked>().HasRequired(c => c.Post).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<DisLiked>().HasRequired(c => c.Post).WithMany().WillCascadeOnDelete(false);
        }
    }
}