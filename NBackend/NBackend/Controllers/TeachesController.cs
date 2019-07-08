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
    public class TeachesController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Teaches
        public IQueryable<Teach> GetTeaches()
        {
            return db.Teaches;
        }

        // GET: api/Teaches/5
        [ResponseType(typeof(Teach))]
        public async Task<IHttpActionResult> GetTeach(int id)
        {
            Teach teach = await db.Teaches.FindAsync(id);
            if (teach == null)
            {
                return NotFound();
            }

            return Ok(teach);
        }

        // PUT: api/Teaches/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeach(int id, Teach teach)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teach.TeacherId)
            {
                return BadRequest();
            }

            db.Entry(teach).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachExists(id))
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

        // POST: api/Teaches
        [ResponseType(typeof(Teach))]
        public async Task<IHttpActionResult> PostTeach(Teach teach)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teaches.Add(teach);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeachExists(teach.TeacherId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = teach.TeacherId }, teach);
        }

        // DELETE: api/Teaches/5
        [ResponseType(typeof(Teach))]
        public async Task<IHttpActionResult> DeleteTeach(int id)
        {
            Teach teach = await db.Teaches.FindAsync(id);
            if (teach == null)
            {
                return NotFound();
            }

            db.Teaches.Remove(teach);
            await db.SaveChangesAsync();

            return Ok(teach);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeachExists(int id)
        {
            return db.Teaches.Count(e => e.TeacherId == id) > 0;
        }
    }
}