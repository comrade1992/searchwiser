using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace FrontwiseRecruitingApp.Models
{
    public class RecruitingAppContext : DbContext
    {
        public RecruitingAppContext()
            : base("name=searchwiser_dbEntities1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WordsCategory> Words { get; set; }
        public DbSet<SearchStringsCategory> SearchStrings { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<UsersSaved> UsersSaved { get; set; }
    }
}