using NBackend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBackend.Models;

using Newtonsoft.Json;

namespace NBackend.Biz
{
    public class ExamBiz
    {
        static string[] exam_types =
        {
            "作业",
            "小测",
            "期中考试",
            "期末考试",
        };


        public static object postExam(string token, object json)
        {
            int user_id = JwtManager.DecodeToken(token);
            NBackendContext ctx = new NBackendContext();
            //验证该用户是否是教学的老师
            User user = UserBiz.getUserById(ctx, user_id);
            if(user == null || user.role != "teacher_edu")
            {
                return Helper.JsonConverter.Error(400, "该用户没有权限创建试卷");
            }

            var body = Helper.JsonConverter.Decode(json);

            int sec_id = int.Parse(body["sec_id"]);
            int course_id = int.Parse(body["course_id"]);
            string semester = body["semester"];
            int year = int.Parse(body["year"]);

            string scope = body["scope"];
            string type = body["type"];

            string start_time = body["start_time"];
            string end_time = body["end_time"];
            string title = body["title"];

            //NBackendContext ctx = new NBackendContext();
            Exam exam = new Exam
            {
                secId = sec_id,
                courseId = course_id,
                semester = semester,
                year = year,
                scope = scope,
                type = type2Id(type),
                start_time = start_time,
                end_time = end_time,
                title = title,
            };

            ctx.Exams.Add(exam);
            ctx.SaveChanges();

            var data = new
            {
                exam_id = exam.ExamId
            };
            return Helper.JsonConverter.BuildResult(data);
        }

        private static int type2Id(string type)
        {
            for(int i = 0; i < 4; i++)
            {
                if(type.Equals(exam_types[i]))
                {
                    return i + 1;
                }
            }
            return -1;
        }
        private static string id2Type(int id)
        {
            return exam_types[id];
        }

        public static bool validateToken(int user_id, string token)
        {
            return user_id == JwtManager.DecodeToken(token);
        }

        //获取班级的所有试卷基本信息
        public static object getExamsOfClass(string token ,object json)
        {
            var body = Helper.JsonConverter.Decode(json);

            int sec_id = int.Parse(body["sec_id"]);
            int course_id = int.Parse(body["course_id"]);
            string semester = body["semester"];
            int year = int.Parse(body["year"]);

            NBackendContext ctx = new NBackendContext();
            int user_id = JwtManager.DecodeToken(token);

            User user = UserBiz.getUserById(ctx, user_id);
            if (user == null)
            {
                return Helper.JsonConverter.Error(400, "查无此人");
            }

            //需要验证该学生、老师是否属于某个班级？？？不用，查出来的只有这个班级

            var q = ctx.Exams.Where(exam => exam.secId == sec_id &&
            exam.courseId == course_id && exam.semester == semester &&
            exam.year == year
            );

            var data = ListToObj(ctx, q.ToList(), user);

            if (data == null)
            {
                return Helper.JsonConverter.Error(400, "你好像没权限(＾Ｕ＾)ノ~ＹＯ");
            }
            else
            {
                return Helper.JsonConverter.BuildResult(data);
            }
        }

        //list为某个班级的考试，在这里加状态字段
        private static object ListToObj(NBackendContext ctx, List<Exam> list, User user)
        {
            List<object> data = new List<object>();
            //NBackendContext ctx = new NBackendContext();

            

            if (user.role.Equals("student"))
            {
                var q = ctx.TakesExams.Where(te => te.StudentId == user.Id).Select(te=>te.ExamId);
                var taken_exams = q.ToList();

                foreach (Exam exam in list)
                {
                    data.Add(new
                    {
                        exam_id = exam.ExamId,
                        scope = exam.scope,
                        type = exam.type,
                        start_time = exam.start_time,
                        end_time = exam.end_time,
                        title = exam.title,
                        exam_status = taken_exams.Contains(exam.ExamId),
                    });
                }
            }
            else if(user.role.Equals("teacher_edu"))
            {
                foreach (Exam exam in list)
                {
                    data.Add(new
                    {
                        exam_id = exam.ExamId,
                        scope = exam.scope,
                        type = exam.type,
                        start_time = exam.start_time,
                        end_time = exam.end_time,
                        title = exam.title,
                    });
                }
            }
            else
            {
                return null;//
            }

            return data;
        }

        //获取某张试卷所有的题目，包括学生考试前后和老师查看
        public static object getQuestionsOfExam(string token, object json)
        {
            var body = Helper.JsonConverter.Decode(json);
            int user_id = JwtManager.DecodeToken(token);

            int exam_id = int.Parse(body["exam_id"]);

            NBackendContext ctx = new NBackendContext();

            //连接考试表和试题表
            var q = ctx.Exams.Join(ctx.ExamQuestions,
                _exam => _exam.ExamId,
                eq => eq.examId,
                (_exam, eq) => eq
                ).Join(ctx.Questions,
                eq => eq.questionId,
                ques => ques.QuestionId,
                (eq, ques) => ques
                );

            //该试卷的所有题目
            var quess = q.ToList();
            User user = UserBiz.getUserById(ctx, user_id);

            if(!quess.Any())
            {
                return Helper.JsonConverter.Error(400, "不是考试没了就是题库崩了？"); 
            }

            Exam exam = getExamById(ctx, exam_id);

            string title = exam.title;
            string start_time = exam.start_time;
            string end_time = exam.end_time;

            var q1 = ctx.ExamQuestions.Where(e => e.examId == exam.ExamId).Join(ctx.Questions,
                 ex => ex.questionId,
                 qu => qu.QuestionId,
                 (ex, qu) => qu
                );
            var questions = q1.ToList();

            object data;
            List<object> qdata = new List<object>();

            if(user.role.Equals("teacher_edu"))
            {
                foreach(var qu in questions)
                {
                    qdata.Add(new
                    {
                        question_id = qu.QuestionId,
                        course_id = qu.courseId,
                        chapter = qu.chapter,
                        content = qu.content,
                        options = qu.options,
                        answer = qu.answer,

                    });
                }
                
            }
            else if(user.role.Equals("student"))
            {
                var q2 = ctx.TakesExams.Where(te => te.StudentId == user_id &&
                te.ExamId == exam_id
                );
                //没参加过这场考试
                if (!q2.Any())
                {
                    foreach (var qu in questions)
                    {
                        qdata.Add(new
                        {
                            question_id = qu.QuestionId,
                            course_id = qu.courseId,
                            chapter = qu.chapter,
                            content = qu.content,
                            options = qu.options,

                        });
                    }
                    data = new
                    {
                        questions = qdata,
                        title = title,
                        start_time = start_time,
                        end_time = end_time,
                        exam_status = false,
                    };
                }
                else
                {
                    foreach (var qu in questions)
                    {
                        qdata.Add(new
                        {
                            question_id = qu.QuestionId,
                            course_id = qu.courseId,
                            chapter = qu.chapter,
                            content = qu.content,
                            options = qu.options,
                            answer = qu.answer,
                        });
                    }
                    data = new
                    {
                        questions = qdata,
                        title = title,
                        start_time = start_time,
                        end_time = end_time,
                        exam_status = true,
                        total_score = q2.Single().score
                    };
                }
            }
            else
            {
                return Helper.JsonConverter.Error(400, "您没有权限(＾Ｕ＾)ノ~ＹＯ");
            }

            return null;
        }

        private static Exam getExamById(NBackendContext ctx, int exam_id)
        {
            var q = ctx.Exams.Where(exam => exam.ExamId == exam_id);

            if (!q.Any())
            {
                return null;
            }
            return q.Single();
        }

        //获取某个课程的所有题目
        public static object getQuestionsOfCourse(string token, object json)
        {
            int user_id = JwtManager.DecodeToken(token);
            var body = Helper.JsonConverter.Decode(json);
            int course_id = int.Parse(body["course_id"]);

            NBackendContext ctx = new NBackendContext();
            var q = ctx.Questions.Where(qu=>qu.courseId == course_id);
            List<Question> questions = q.ToList();

            List<object> data = new List<object>();

            foreach(Question qu in questions)
            {
                data.Add(new
                {
                    question_id = qu.QuestionId,
                    course_id = qu.courseId,
                    chapter = qu.chapter,
                    content = qu.content,
                    answer = qu.answer,
                });
            }

            return Helper.JsonConverter.BuildResult(data);
        }

        private const int POST = 1, DELETE = 2, PUT = 3;
        //删除、修改、提交题目全靠它
        private static object questionHelper(string token, object json, int option)
        {
            int user_id = JwtManager.DecodeToken(token);

            NBackendContext ctx = new NBackendContext();
            User user = UserBiz.getUserById(ctx, user_id);

            if (user == null || !user.role.Equals("teacher_edu"))
            {
                //可以再判断这个老师是不是教这个的
                return Helper.JsonConverter.Error(400, "您未登录或者没有权限");
            }

            var body = Helper.JsonConverter.Decode(json);
            //删除、修改、提交题目分发逻辑
            switch (option)
            {
                default:
                    {
                        int course_id = int.Parse(body["course_id"]);
                        string chapter = body["chapter"];
                        string content = body["content"];
                        string options = body["options"];
                        string answer = body["answer"];

                        if(option == POST)
                        {
                            var newq = new Question
                            {
                                courseId = course_id,
                                chapter = chapter,
                                content = content,
                                options = options,
                                answer = answer,
                            };
                            ctx.Questions.Add(newq);
                            ctx.SaveChanges();
                            object data = new
                            {
                                question_id = newq.QuestionId
                            };
                            return Helper.JsonConverter.BuildResult(data);
                        }
                        else if(option == PUT)
                        {
                            int question_id = int.Parse(body["question_id"]);
                            var q = ctx.Questions.Where(qu => qu.QuestionId == question_id);
                            if (!q.Any())
                            {
                                return Helper.JsonConverter.Error(400, "没有这道题");
                            }
                            else
                            {
                                Question question = q.Single();
                                ctx.Questions.Remove(question);
                            }
                         }
                        return Helper.JsonConverter.BuildResult(null);

                    }
                case DELETE:
                    {
                        int question_id = int.Parse(body["question_id"]);
                        var q = ctx.Questions.Where(qu => qu.QuestionId == question_id);
                        if (!q.Any())
                        {
                            return Helper.JsonConverter.Error(400, "没有这道题");
                        }
                        else
                        {
                            var qu = q.Single();
                            ctx.Questions.Remove(qu);
                            ctx.SaveChanges();
                        }

                        return Helper.JsonConverter.BuildResult(null);
                        //break;
                    }
                    
                
            }
        }

        public static object postQuestion(string token, object json)
        {
            return questionHelper(token, json, POST);
        }

        public static object deleteQuestion(string token, object json)
        {
            return questionHelper(token, json, DELETE);
        }

        public static object putQuestion(string token, object json)
        {
            return questionHelper(token, json, PUT);
        }

        //教师查看某场考试结果
        public static object getExamResult(string token, object json)
        {
            int user_id = Helper.JwtManager.DecodeToken(token);
            NBackendContext ctx = new NBackendContext();

            User user = UserBiz.getUserById(ctx, user_id);
            if (user.role.Equals("teacher_edu"))
            {
                return Helper.JsonConverter.Error(400, "你没有权限(＾Ｕ＾)ノ~ＹＯ");
            }

            var body = Helper.JsonConverter.Decode(json);
            int exam_id = int.Parse(body["exam_id"]);

            //通过exam取出班级，再取出班级的所有人
            //真糖！
            var qstus = ctx.Exams.Join(ctx.Takes, ex => ex.Section, take => take.Section,
                (ex, take) => new { exam_id, take.Student }
                );
            //获得参加本场考试的学生的信息和分数
            var qstus_taken = qstus.Join(ctx.TakesExams, stu => stu.Student, te => te.Student,
                (stu, te) => new { stu, te.score }
                ).Where(stu => stu.stu.exam_id == exam_id).Select(stu_score => new { stu_score.stu.Student, stu_score.score });

            //拿到所有学生
            var qstus_only = qstus.Select(stu => stu.Student);
            var qstus_not_taken = qstus_only.Except(qstus_taken.Select(stu_score => stu_score.Student));

            List<object> all_stu_score= new List<object>();
            //User student = getUserById()

            foreach(var stu  in qstus_taken)
            {
                User _user = UserBiz.getUserById(ctx, stu.Student.StudentId);
                all_stu_score.Add(new
                {
                    student_name = stu.Student.StudentId,
                    student_id = _user.user_name,
                    score = stu.score
                });
            }
            foreach (var stu in qstus_not_taken)
            {
                User _user = UserBiz.getUserById(ctx, stu.StudentId);
                all_stu_score.Add(new
                {
                    student_name = stu.StudentId,
                    student_id = _user.user_name,
                    score = 0
                });
            }

            return Helper.JsonConverter.BuildResult(all_stu_score);
        }

        //public static object getStudentsOfClass(NBackendContext ctx, )

        //创建试卷后为试卷添加题目
        public static object postQuestionOfExam(string token, object json)
        {
            
            int user_id = JwtManager.DecodeToken(token);

            NBackendContext ctx = new NBackendContext();
            User user = UserBiz.getUserById(ctx, user_id);

            if (user == null || !user.role.Equals("teacher_edu"))
            {
                //可以再判断这个老师是不是教这个的
                return Helper.JsonConverter.Error(400, "您未登录或者没有权限");
            }

            //[ { exam_id, question_id, score, index}, {...}]
            var body = Helper.JsonConverter.Decode(json);
            int exam_id = int.Parse(body["exam_id"]);
            string _quess = body["questions"];

            var quess = JsonConvert.DeserializeObject<List<object>>(_quess);

            var questions = ctx.Questions.Select(qu => qu.QuestionId).ToList();
            foreach(object obj in quess)
            {
                var _body = Helper.JsonConverter.Decode(obj);
                int question_id = int.Parse(_body["question_id"]);
                int single_score = int.Parse(_body["score"]);
                int index = int.Parse(_body["index"]);

                if (questions.Contains(question_id))
                {
                    ctx.ExamQuestions.Add(new ExamQuestion
                    {
                        examId = exam_id,
                        questionId = question_id,
                        score = single_score,
                        index = index,
                    });
                }
                else
                {
                    //有一道题找不到？
                }
            }

            ctx.SaveChanges();

            object data = new { exam_id = exam_id };
            return Helper.JsonConverter.BuildResult(data);
        }
    }
}