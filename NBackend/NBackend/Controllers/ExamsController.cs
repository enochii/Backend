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
        //创建考试
        public object PostExam(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.postExam(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        //学生提交考试
        [Route("api/finished_exam")]
        public object FinishExam(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.finishExam(token, json);
        }


        //个人成绩总结
        [AllowAnonymous]
        [HttpGet]
        [Route("api/exam_summary")]
        public object ExamSumup()
        {
            string token = Request.Headers.Authorization.Parameter;

            return ExamBiz.examSumup(token);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("api/exam_creation")]
        //public object PostExamQuestions(object json)
        //{
        //    var token = Request.Headers.Authorization.Parameter;

        //    return ExamBiz.postQuestionOfExam(token, json);
        //}

        [AllowAnonymous]
        [HttpPost]
        //教师查看某个班级的所有考试成绩
        [Route("api/exam_results")]
        public object GetExamResult(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.getExamResult(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        //获取班级的所有考试
        [Route("api/class_exams")]
        public object GetClassExams(object json)
        {
            var token = Request.Headers.Authorization.Parameter;

            return ExamBiz.getExamsOfClass(token, json);
        }

        [AllowAnonymous]
        [HttpPost]
        //获取试卷内容，包括学生考试前后，老师的上帝视角
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
            var token = Request.Headers.Authorization.Parameter;
            return ExamBiz.deleteQuestion(token, json);
        }
        //修改一个题目
        [AllowAnonymous]
        [HttpPut]
        [Route("api/questions")]
        public object PutQuestion(object json)
        {
            var token = Request.Headers.Authorization.Parameter;
            return ExamBiz.putQuestion(token, json);
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
        [Route("api/exam_results")]
        [Route("api/exam_creation")]
        [Route("api/exam_questions")]
        [Route("api/class_exams")]
        [Route("api/course_questions")]
        [Route("api/questions")]
        [Route("api/finished_exam")]
        [Route("api/exams")]
        [Route("api/exam_summary")]
        public object Options()
        {
            return null;
        }
    }
}