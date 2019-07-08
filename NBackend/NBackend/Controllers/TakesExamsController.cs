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
    public class TakesExamsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/TakesExams
        public IQueryable<TakesExam> GetTakesExams()
        {
            return db.TakesExams;
        }

        // GET: api/TakesExams/5
        [ResponseType(typeof(TakesExam))]
        public async Task<IHttpActionResult> GetTakesExam(int id)
        {
            TakesExam takesExam = await db.TakesExams.FindAsync(id);
            if (takesExam == null)
            {
                return NotFound();
            }

            return Ok(takesExam);
        }

        // PUT: api/TakesExams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTakesExam(int id, TakesExam takesExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != takesExam.StudentId)
            {
                return BadRequest();
            }

            db.Entry(takesExam).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TakesExamExists(id))
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

        // POST: api/TakesExams
        [ResponseType(typeof(TakesExam))]
        public async Task<IHttpActionResult> PostTakesExam(TakesExam takesExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TakesExams.Add(takesExam);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TakesExamExists(takesExam.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = takesExam.StudentId }, takesExam);
        }

        // DELETE: api/TakesExams/5
        [ResponseType(typeof(TakesExam))]
        public async Task<IHttpActionResult> DeleteTakesExam(int id)
        {
            TakesExam takesExam = await db.TakesExams.FindAsync(id);
            if (takesExam == null)
            {
                return NotFound();
            }

            db.TakesExams.Remove(takesExam);
            await db.SaveChangesAsync();

            return Ok(takesExam);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TakesExamExists(int id)
        {
            return db.TakesExams.Count(e => e.StudentId == id) > 0;
        }
    }
}