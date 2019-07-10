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
using NBackend.Biz;

namespace NBackend.Controllers
{
    public class AttentionsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/attendance")]
        public object GetAttendanceRecords(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.AttendanceBiz.GetAttendanceRecords(json);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/attendance_records")]
        public object PostAttendanceRecords(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.AttendanceBiz.PostAttendance(json);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("api/new_attendance")]
        public object PutAttendanceRecords(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.AttendanceBiz.EditAttendanceRecords(json);
        }

        // GET: api/Attentions
        public IQueryable<Attention> GetAttentions()
        {
            return db.Attentions;
        }

        // GET: api/Attentions/5
        [ResponseType(typeof(Attention))]
        public async Task<IHttpActionResult> GetAttention(int id)
        {
            Attention attention = await db.Attentions.FindAsync(id);
            if (attention == null)
            {
                return NotFound();
            }

            return Ok(attention);
        }

        // PUT: api/Attentions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttention(int id, Attention attention)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attention.StudentId)
            {
                return BadRequest();
            }

            db.Entry(attention).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttentionExists(id))
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

        // POST: api/Attentions
        [ResponseType(typeof(Attention))]
        public async Task<IHttpActionResult> PostAttention(Attention attention)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attentions.Add(attention);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AttentionExists(attention.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = attention.StudentId }, attention);
        }

        // DELETE: api/Attentions/5
        [ResponseType(typeof(Attention))]
        public async Task<IHttpActionResult> DeleteAttention(int id)
        {
            Attention attention = await db.Attentions.FindAsync(id);
            if (attention == null)
            {
                return NotFound();
            }

            db.Attentions.Remove(attention);
            await db.SaveChangesAsync();

            return Ok(attention);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttentionExists(int id)
        {
            return db.Attentions.Count(e => e.StudentId == id) > 0;
        }

        [AllowAnonymous]
        [HttpOptions]
        [Route("api/attendance")]
        [Route("api/attendance_records")]
        [Route("api/new_attendance")]
        public object Options()
        {
            return null;
        }
    }
}