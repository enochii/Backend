using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class NBackendContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public NBackendContext() : base("name=NBackendContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TBACKEND");
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<NBackend.Models.Teacher> Teachers { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.SectionTime> SectionTimes { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Section> Sections { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Teach> Teaches { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.CourseWare> CourseWares { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Exam> Exams { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Question> Questions { get; set; }

        //public System.Data.Entity.DbSet<NBackend.Models.Disscussion> Disscussions { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Broadcast> Broadcasts { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Take> Takes { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Attention> Attentions { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Twitter> Twitters { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.TeacherBroadcast> TeacherBroadcasts { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.TeamStudent> TeamStudents { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Team> Teams { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.ExamQuestion> ExamQuestions { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.TakesExam> TakesExams { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.MultiSectionTimes> MultiSectionTimes { get; set; }

        public System.Data.Entity.DbSet<NBackend.Models.Discussion> Discussions { get; set; }

        //public System.Data.Entity.DbSet<NBackend.Models.Discussion> Discussions { get; set; }

        //public System.Data.Entity.DbSet<NBackend.Models.MultiSectionsTime> MultiSectionsTimes { get; set; }
    }
}
