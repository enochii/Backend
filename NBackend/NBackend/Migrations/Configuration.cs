namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NBackend.Models.NBackendContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NBackend.Models.NBackendContext";
        }

        protected override void Seed(NBackend.Models.NBackendContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.Users.AddOrUpdate(
            //  p => p.user_name,
            //  new Models.User {
            //      Id = 000000,
            //      user_name = "袁阿姨",
            //      department = "软件学院",
            //      password = "123456",
            //      phone_number = "13112525575",
            //      mail = "144@11.com",
            //      avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
            //      role = "teacher"
            //  },
            //  new Models.User
            //  {
            //      Id = 000001,
            //      user_name = "穆院长",
            //      department = "软件学院",
            //      password = "123456",
            //      phone_number = "13112525575",
            //      mail = "144@11.com",
            //      avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
            //      role = "teacher"
            //  }
            //);
            context.Courses.AddOrUpdate(
              p => p.course_name,
              new Models.Course
              {
                  CourseId = 100000,
                  course_name = "数据结构",
                  credits = 4,
                  avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
                  description = "冲冲冲！"
              },
              new Models.Course
              {
                  CourseId = 000001,
                  course_name= "数据库课程设计",
                  credits = 1,
                  avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
                  description = "袁阿姨真无敌！"
              }
            );
            //context.SectionTimes.AddOrUpdate(
            //  p => p.SectionTimeId,
            //  new Models.SectionTime
            //  {
            //      SectionTimeId=0000,
            //      //day = "周一",
            //      start_section = 1,
            //      length = 2,
            //      //start_week = 1,
            //      //end_week = 17,
            //      //single_or_double = 1
            //  },
            //  new Models.SectionTime
            //  {
            //      SectionTimeId = 0001,
            //      //day = "周一",
            //      start_section = 3,
            //      length = 2,
            //      //    start_week = 1,
            //      //    end_week = 17,
            //      //    single_or_double = 1
            //  }
            //);
            //context.Sections.AddOrUpdate(
            //  p => p.courseId,
            //  new Models.Section
            //  {
            //      SecId=1,
            //      courseId=000000,
            //      semester = "Spring",
            //      year = 2019,
            //      building = "济事楼",
            //      room_numer = "516",
            //      //section_timeId = 0000,
            //      avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr"
            //  },
            //  new Models.Section
            //  {
            //      SecId = 1,
            //      courseId = 000001,
            //      semester = "Spring",
            //      year = 2019,
            //      building = "济事楼",
            //      room_numer = "516",
            //      //section_timeId = 0001,
            //      avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr"
            //  }
            //);

            //context.Teachers.AddOrUpdate(
            //  p => p.TeacherId,
            //  new Models.Teacher
            //  {
            //      TeacherId=000000,
            //      job_title = "教授",
            //      is_manager = false                
            //  },
            //  new Models.Teacher
            //  {
            //      TeacherId = 000001,
            //      job_title = "教授",
            //      is_manager = false
            //  }
            //);

            //context.Teaches.AddOrUpdate(
            //  p => p.TeacherId,
            //  new Models.Teach
            //  {
            //      TeacherId=000000,
            //      SecId = 1,
            //      courseId = 000000,
            //      semester = "Spring",
            //      year = 2019,
            //  },
            //  new Models.Teach
            //  {
            //      TeacherId = 000001,
            //      SecId = 1,
            //      courseId = 000001,
            //      semester = "Spring",
            //      year = 2019,
            //  }
            //);

        }
    }
}
