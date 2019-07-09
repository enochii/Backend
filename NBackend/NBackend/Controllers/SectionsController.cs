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
    public class SectionsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        [HttpGet]
        [Route("api/total_classes")]
        public object GetSections()
        {
            //String token = Request.Headers.Authorization.Parameter;
            return Biz.ClassBiz.GetAllClasses();
        }

        [HttpGet]
        [Route("api/one_class")]
        public object GetOneSection(object json)
        {
            //String token = Request.Headers.Authorization.Parameter;
            return Biz.ClassBiz.GetOneClass(json);
        }

        [HttpGet]
        [Route("api/part_classes")]
        public object GetPartSections(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.GetPartClass(json, token);
        }

        [HttpGet]
        [Route("api/waiting_classes")]
        public object GetWaitingSections(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.GetWaitingClass(json, token);
        }

        [HttpGet]
        [Route("api/waiting_students")]
        public object GetWaitingStudents(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.GetWaitingStudents(json);
        }

        [HttpGet]
        [Route("api/class_details")]
        public object GetClassDetails(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.GetOneClassDetails(json);
        }

        [HttpPost]
        [Route("api/student_class")]
        public object PostStudentClass(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.JoinClass(json,token);
        }

        [HttpPost]
        [Route("api/class")]
        public object PostSection(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.CreateClass(json);
        }

        [HttpPut]
        [Route("api/permission")]
        public object PutApplication(object json)
        {
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                return Helper.JsonConverter.Error(401, "你还没登录？");
            }
            return Biz.ClassBiz.PermitApplication(json);
        }

        // GET: api/Sections/5
        [ResponseType(typeof(Section))]
        public async Task<IHttpActionResult> GetSection(int id)
        {
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }

        // PUT: api/Sections/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSection(int id, Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != section.SecId)
            {
                return BadRequest();
            }

            db.Entry(section).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(id))
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

        // POST: api/Sections
        [ResponseType(typeof(Section))]
        public async Task<IHttpActionResult> PostSection(Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sections.Add(section);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SectionExists(section.SecId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = section.SecId }, section);
        }

        // DELETE: api/Sections/5
        [ResponseType(typeof(Section))]
        public async Task<IHttpActionResult> DeleteSection(int id)
        {
            Section section = await db.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            db.Sections.Remove(section);
            await db.SaveChangesAsync();

            return Ok(section);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionExists(int id)
        {
            return db.Sections.Count(e => e.SecId == id) > 0;
        }
    }
}