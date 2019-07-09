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
    public class MultiSectionTimesController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/MultiSectionTimes
        public IQueryable<MultiSectionTimes> GetMultiSectionTimes()
        {
            return db.MultiSectionTimes;
        }

        // GET: api/MultiSectionTimes/5
        [ResponseType(typeof(MultiSectionTimes))]
        public async Task<IHttpActionResult> GetMultiSectionTimes(int id)
        {
            MultiSectionTimes multiSectionTimes = await db.MultiSectionTimes.FindAsync(id);
            if (multiSectionTimes == null)
            {
                return NotFound();
            }

            return Ok(multiSectionTimes);
        }

        // PUT: api/MultiSectionTimes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMultiSectionTimes(int id, MultiSectionTimes multiSectionTimes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != multiSectionTimes.SecId)
            {
                return BadRequest();
            }

            db.Entry(multiSectionTimes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultiSectionTimesExists(id))
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

        // POST: api/MultiSectionTimes
        [ResponseType(typeof(MultiSectionTimes))]
        public async Task<IHttpActionResult> PostMultiSectionTimes(MultiSectionTimes multiSectionTimes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MultiSectionTimes.Add(multiSectionTimes);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MultiSectionTimesExists(multiSectionTimes.SecId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = multiSectionTimes.SecId }, multiSectionTimes);
        }

        // DELETE: api/MultiSectionTimes/5
        [ResponseType(typeof(MultiSectionTimes))]
        public async Task<IHttpActionResult> DeleteMultiSectionTimes(int id)
        {
            MultiSectionTimes multiSectionTimes = await db.MultiSectionTimes.FindAsync(id);
            if (multiSectionTimes == null)
            {
                return NotFound();
            }

            db.MultiSectionTimes.Remove(multiSectionTimes);
            await db.SaveChangesAsync();

            return Ok(multiSectionTimes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MultiSectionTimesExists(int id)
        {
            return db.MultiSectionTimes.Count(e => e.SecId == id) > 0;
        }
    }
}