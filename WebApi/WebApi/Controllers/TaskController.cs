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
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TaskController : ApiController
    {
        private DatabasEntities db = new DatabasEntities();

        // GET: api/Task
        public IQueryable<Tasks> GetTasks()
        {
            return db.Tasks;
        }

        // GET: api/Task/5
        [ResponseType(typeof(Tasks))]
        public async Task<IHttpActionResult> GetTasks(int id)
        {
            Tasks tasks = await db.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        // PUT: api/Task/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTasks(int id, Tasks tasks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tasks.TaskID)
            {
                return BadRequest();
            }

            db.Entry(tasks).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
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

        // POST: api/Task
        [ResponseType(typeof(Tasks))]
        public async Task<IHttpActionResult> PostTasks(Tasks tasks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tasks.Add(tasks);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TasksExists(tasks.TaskID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tasks.TaskID }, tasks);
        }

        // DELETE: api/Task/5
        [ResponseType(typeof(Tasks))]
        public async Task<IHttpActionResult> DeleteTasks(int id)
        {
            Tasks tasks = await db.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(tasks);
            await db.SaveChangesAsync();

            return Ok(tasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TasksExists(int id)
        {
            return db.Tasks.Count(e => e.TaskID == id) > 0;
        }
    }
}