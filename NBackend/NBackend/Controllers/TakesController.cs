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
    public class TakesController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Takes
        public IQueryable<Take> GetTakes()
        {
            return db.Takes;
        }

        // GET: api/Takes/5
        [ResponseType(typeof(Take))]
        public async Task<IHttpActionResult> GetTake(int id)
        {
            Take take = await db.Takes.FindAsync(id);
            if (take == null)
            {
                return NotFound();
            }

            return Ok(take);
        }

        // PUT: api/Takes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTake(int id, Take take)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != take.StudentId)
            {
                return BadRequest();
            }

            db.Entry(take).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TakeExists(id))
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

        // POST: api/Takes
        [ResponseType(typeof(Take))]
        public async Task<IHttpActionResult> PostTake(Take take)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Takes.Add(take);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TakeExists(take.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = take.StudentId }, take);
        }

        // DELETE: api/Takes/5
        [ResponseType(typeof(Take))]
        public async Task<IHttpActionResult> DeleteTake(int id)
        {
            Take take = await db.Takes.FindAsync(id);
            if (take == null)
            {
                return NotFound();
            }

            db.Takes.Remove(take);
            await db.SaveChangesAsync();

            return Ok(take);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TakeExists(int id)
        {
            return db.Takes.Count(e => e.StudentId == id) > 0;
        }
    }
}