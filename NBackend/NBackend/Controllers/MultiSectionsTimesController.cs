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
    public class MultiSectionsTimesController : ApiController
    {
        private NBackendContext db = new NBackendContext();

        // GET: api/MultiSectionsTimes
        public IQueryable<MultiSectionsTime> GetMultiSectionsTimes()
        {
            return db.MultiSectionsTimes;
        }

        // GET: api/MultiSectionsTimes/5
        [ResponseType(typeof(MultiSectionsTime))]
        public async Task<IHttpActionResult> GetMultiSectionsTime(int id)
        {
            MultiSectionsTime multiSectionsTime = await db.MultiSectionsTimes.FindAsync(id);
            if (multiSectionsTime == null)
            {
                return NotFound();
            }

            return Ok(multiSectionsTime);
        }

        // PUT: api/MultiSectionsTimes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMultiSectionsTime(int id, MultiSectionsTime multiSectionsTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != multiSectionsTime.SecId)
            {
                return BadRequest();
            }

            db.Entry(multiSectionsTime).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultiSectionsTimeExists(id))
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

        // POST: api/MultiSectionsTimes
        [ResponseType(typeof(MultiSectionsTime))]
        public async Task<IHttpActionResult> PostMultiSectionsTime(MultiSectionsTime multiSectionsTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MultiSectionsTimes.Add(multiSectionsTime);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MultiSectionsTimeExists(multiSectionsTime.SecId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = multiSectionsTime.SecId }, multiSectionsTime);
        }

        // DELETE: api/MultiSectionsTimes/5
        [ResponseType(typeof(MultiSectionsTime))]
        public async Task<IHttpActionResult> DeleteMultiSectionsTime(int id)
        {
            MultiSectionsTime multiSectionsTime = await db.MultiSectionsTimes.FindAsync(id);
            if (multiSectionsTime == null)
            {
                return NotFound();
            }

            db.MultiSectionsTimes.Remove(multiSectionsTime);
            await db.SaveChangesAsync();

            return Ok(multiSectionsTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MultiSectionsTimeExists(int id)
        {
            return db.MultiSectionsTimes.Count(e => e.SecId == id) > 0;
        }
    }
}