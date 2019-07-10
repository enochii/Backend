using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NBackend.Biz;
using NBackend.Models;


namespace NBackend.Controllers
{
    public class ExamsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Exams
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/exams")]
        //public IQueryable<Exam> GetExams()
        //{
        //    return db.Exams;
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("api/exams")]
        public object PostExam(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.postExam(token, json);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/exam_creation")]
        public object PostExamQuestions(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.postQuestionOfExam(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/exam_results")]
        public object GetExamResult(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.getExamResult(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/class_exams")]
        public object GetClassExams(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.getExamsOfClass(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/exam_questions")]
        public object GetExamQuestions(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.getQuestionsOfExam(token, json);
        }

        //题库相关
        [AllowAnonymous]
        [HttpPost]
        [Route("api/course_questions")]
        public object GetCourseQuestions(object json)
        {
            var token = Request.Headers.Authorization.Parameter;
            return ExamBiz.getQuestionsOfCourse(token, json);
        }

        //post一个题目
        [AllowAnonymous]
        [HttpPost]
        [Route("api/questions")]
        public object PostQuestion(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.postQuestion(token, json);
        }
        //删除一个题目
        [AllowAnonymous]
        [HttpDelete]
        [Route("api/questions")]
        public object DeleteQuestion(object json)
        {
            return json;
        }
        //修改一个题目
        [AllowAnonymous]
        [HttpPut]
        [Route("api/questions")]
        public object PutQuestion(object json)
        {
            var token = Request.Headers.Authorization.Parameter;
            return ExamBiz.deleteQuestion(token, json);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExamExists(int id)
        {
            return db.Exams.Count(e => e.ExamId == id) > 0;
        }
        [AllowAnonymous]
        [HttpOptions]
        [Route("api/exams")]
        public object Options()
        {
            return null;
        }
    }
}