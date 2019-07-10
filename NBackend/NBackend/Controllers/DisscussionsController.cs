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
    public class DisscussionsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/Disscussions
        public IQueryable<Disscussion> GetDisscussions()
        {
            return db.Disscussions;
        }

        // GET: api/Disscussions/5
        [ResponseType(typeof(Disscussion))]
        public async Task<IHttpActionResult> GetDisscussion(int id)
        {
            Disscussion disscussion = await db.Disscussions.FindAsync(id);
            if (disscussion == null)
            {
                return NotFound();
            }

            return Ok(disscussion);
        }

        // PUT: api/Disscussions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDisscussion(int id, Disscussion disscussion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != disscussion.DisscussionId)
            {
                return BadRequest();
            }

            db.Entry(disscussion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisscussionExists(id))
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

        // POST: api/Disscussions
        [ResponseType(typeof(Disscussion))]
        public async Task<IHttpActionResult> PostDisscussion(Disscussion disscussion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Disscussions.Add(disscussion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DisscussionExists(disscussion.DisscussionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = disscussion.DisscussionId }, disscussion);
        }

        // DELETE: api/Disscussions/5
        [ResponseType(typeof(Disscussion))]
        public async Task<IHttpActionResult> DeleteDisscussion(int id)
        {
            Disscussion disscussion = await db.Disscussions.FindAsync(id);
            if (disscussion == null)
            {
                return NotFound();
            }

            db.Disscussions.Remove(disscussion);
            await db.SaveChangesAsync();

            return Ok(disscussion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DisscussionExists(int id)
        {
            return db.Disscussions.Count(e => e.DisscussionId == id) > 0;
        }
    }
}