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
    public class TeacherBroadcastsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/TeacherBroadcasts
        public IQueryable<TeacherBroadcast> GetTeacherBroadcasts()
        {
            return db.TeacherBroadcasts;
        }

        // GET: api/TeacherBroadcasts/5
        [ResponseType(typeof(TeacherBroadcast))]
        public async Task<IHttpActionResult> GetTeacherBroadcast(int id)
        {
            TeacherBroadcast teacherBroadcast = await db.TeacherBroadcasts.FindAsync(id);
            if (teacherBroadcast == null)
            {
                return NotFound();
            }

            return Ok(teacherBroadcast);
        }

        // PUT: api/TeacherBroadcasts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeacherBroadcast(int id, TeacherBroadcast teacherBroadcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacherBroadcast.teacherId)
            {
                return BadRequest();
            }

            db.Entry(teacherBroadcast).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherBroadcastExists(id))
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

        // POST: api/TeacherBroadcasts
        [ResponseType(typeof(TeacherBroadcast))]
        public async Task<IHttpActionResult> PostTeacherBroadcast(TeacherBroadcast teacherBroadcast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeacherBroadcasts.Add(teacherBroadcast);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeacherBroadcastExists(teacherBroadcast.teacherId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = teacherBroadcast.teacherId }, teacherBroadcast);
        }

        // DELETE: api/TeacherBroadcasts/5
        [ResponseType(typeof(TeacherBroadcast))]
        public async Task<IHttpActionResult> DeleteTeacherBroadcast(int id)
        {
            TeacherBroadcast teacherBroadcast = await db.TeacherBroadcasts.FindAsync(id);
            if (teacherBroadcast == null)
            {
                return NotFound();
            }

            db.TeacherBroadcasts.Remove(teacherBroadcast);
            await db.SaveChangesAsync();

            return Ok(teacherBroadcast);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherBroadcastExists(int id)
        {
            return db.TeacherBroadcasts.Count(e => e.teacherId == id) > 0;
        }
    }
}