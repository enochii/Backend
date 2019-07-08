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
    public class SectionTimesController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/SectionTimes
        public IQueryable<SectionTime> GetSectionTimes()
        {
            return db.SectionTimes;
        }

        // GET: api/SectionTimes/5
        [ResponseType(typeof(SectionTime))]
        public async Task<IHttpActionResult> GetSectionTime(int id)
        {
            SectionTime sectionTime = await db.SectionTimes.FindAsync(id);
            if (sectionTime == null)
            {
                return NotFound();
            }

            return Ok(sectionTime);
        }

        // PUT: api/SectionTimes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSectionTime(int id, SectionTime sectionTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sectionTime.SectionTimeId)
            {
                return BadRequest();
            }

            db.Entry(sectionTime).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionTimeExists(id))
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

        // POST: api/SectionTimes
        [ResponseType(typeof(SectionTime))]
        public async Task<IHttpActionResult> PostSectionTime(SectionTime sectionTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SectionTimes.Add(sectionTime);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SectionTimeExists(sectionTime.SectionTimeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sectionTime.SectionTimeId }, sectionTime);
        }

        // DELETE: api/SectionTimes/5
        [ResponseType(typeof(SectionTime))]
        public async Task<IHttpActionResult> DeleteSectionTime(int id)
        {
            SectionTime sectionTime = await db.SectionTimes.FindAsync(id);
            if (sectionTime == null)
            {
                return NotFound();
            }

            db.SectionTimes.Remove(sectionTime);
            await db.SaveChangesAsync();

            return Ok(sectionTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionTimeExists(int id)
        {
            return db.SectionTimes.Count(e => e.SectionTimeId == id) > 0;
        }
    }
}