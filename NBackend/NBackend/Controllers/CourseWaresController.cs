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
    public class CourseWaresController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/CourseWares
        public object GetCourseWares(object json)
        {
            return Biz.CoursewareBiz.GetAllCoursewares(json);
        }

        // GET: api/CourseWares/5
        [ResponseType(typeof(CourseWare))]
        public async Task<IHttpActionResult> GetCourseWare(int id)
        {
            CourseWare courseWare = await db.CourseWares.FindAsync(id);
            if (courseWare == null)
            {
                return NotFound();
            }

            return Ok(courseWare);
        }

        // PUT: api/CourseWares/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourseWare(int id, CourseWare courseWare)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseWare.CourseWareId)
            {
                return BadRequest();
            }

            db.Entry(courseWare).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseWareExists(id))
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

        // POST: api/CourseWares
        [ResponseType(typeof(CourseWare))]
        public async Task<IHttpActionResult> PostCourseWare(CourseWare courseWare)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseWares.Add(courseWare);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = courseWare.CourseWareId }, courseWare);
        }

        // DELETE: api/CourseWares/5
        [ResponseType(typeof(CourseWare))]
        public async Task<IHttpActionResult> DeleteCourseWare(int id)
        {
            CourseWare courseWare = await db.CourseWares.FindAsync(id);
            if (courseWare == null)
            {
                return NotFound();
            }

            db.CourseWares.Remove(courseWare);
            await db.SaveChangesAsync();

            return Ok(courseWare);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseWareExists(int id)
        {
            return db.CourseWares.Count(e => e.CourseWareId == id) > 0;
        }
    }
}