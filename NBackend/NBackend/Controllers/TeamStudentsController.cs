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
    public class TeamStudentsController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/TeamStudents
        public IQueryable<TeamStudent> GetTeamStudents()
        {
            return db.TeamStudents;
        }

        // GET: api/TeamStudents/5
        [ResponseType(typeof(TeamStudent))]
        public async Task<IHttpActionResult> GetTeamStudent(int id)
        {
            TeamStudent teamStudent = await db.TeamStudents.FindAsync(id);
            if (teamStudent == null)
            {
                return NotFound();
            }

            return Ok(teamStudent);
        }

        // PUT: api/TeamStudents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeamStudent(int id, TeamStudent teamStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teamStudent.teamId)
            {
                return BadRequest();
            }

            db.Entry(teamStudent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamStudentExists(id))
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

        // POST: api/TeamStudents
        [ResponseType(typeof(TeamStudent))]
        public async Task<IHttpActionResult> PostTeamStudent(TeamStudent teamStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamStudents.Add(teamStudent);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamStudentExists(teamStudent.teamId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = teamStudent.teamId }, teamStudent);
        }

        // DELETE: api/TeamStudents/5
        [ResponseType(typeof(TeamStudent))]
        public async Task<IHttpActionResult> DeleteTeamStudent(int id)
        {
            TeamStudent teamStudent = await db.TeamStudents.FindAsync(id);
            if (teamStudent == null)
            {
                return NotFound();
            }

            db.TeamStudents.Remove(teamStudent);
            await db.SaveChangesAsync();

            return Ok(teamStudent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamStudentExists(int id)
        {
            return db.TeamStudents.Count(e => e.teamId == id) > 0;
        }
    }
}