﻿using System;
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
    public class ExamsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Exams
        public IQueryable<Exam> GetExams()
        {
            return db.Exams;
        }

        // GET: api/Exams/5
        [ResponseType(typeof(Exam))]
        public async Task<IHttpActionResult> GetExam(int id)
        {
            Exam exam = await db.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }

        // PUT: api/Exams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutExam(int id, Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exam.ExamId)
            {
                return BadRequest();
            }

            db.Entry(exam).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(id))
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

        // POST: api/Exams
        [ResponseType(typeof(Exam))]
        public async Task<IHttpActionResult> PostExam(Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exams.Add(exam);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExamExists(exam.ExamId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = exam.ExamId }, exam);
        }

        // DELETE: api/Exams/5
        [ResponseType(typeof(Exam))]
        public async Task<IHttpActionResult> DeleteExam(int id)
        {
            Exam exam = await db.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            db.Exams.Remove(exam);
            await db.SaveChangesAsync();

            return Ok(exam);
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
    }
}