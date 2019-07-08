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
using NBackend.Models;

namespace NBackend.Controllers
{
    public class ExamQuestionsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/ExamQuestions
        public IQueryable<ExamQuestion> GetExamQuestions()
        {
            return db.ExamQuestions;
        }

        // GET: api/ExamQuestions/5
        [ResponseType(typeof(ExamQuestion))]
        public async Task<IHttpActionResult> GetExamQuestion(int id)
        {
            ExamQuestion examQuestion = await db.ExamQuestions.FindAsync(id);
            if (examQuestion == null)
            {
                return NotFound();
            }

            return Ok(examQuestion);
        }

        // PUT: api/ExamQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExamQuestion(int id, ExamQuestion examQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != examQuestion.examId)
            {
                return BadRequest();
            }

            db.Entry(examQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamQuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ExamQuestions
        [ResponseType(typeof(ExamQuestion))]
        public async Task<IHttpActionResult> PostExamQuestion(ExamQuestion examQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExamQuestions.Add(examQuestion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExamQuestionExists(examQuestion.examId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = examQuestion.examId }, examQuestion);
        }

        // DELETE: api/ExamQuestions/5
        [ResponseType(typeof(ExamQuestion))]
        public async Task<IHttpActionResult> DeleteExamQuestion(int id)
        {
            ExamQuestion examQuestion = await db.ExamQuestions.FindAsync(id);
            if (examQuestion == null)
            {
                return NotFound();
            }

            db.ExamQuestions.Remove(examQuestion);
            await db.SaveChangesAsync();

            return Ok(examQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExamQuestionExists(int id)
        {
            return db.ExamQuestions.Count(e => e.examId == id) > 0;
        }
    }
}