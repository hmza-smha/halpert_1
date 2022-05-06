using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using halpert.Models;
using halpert.ViewModels;

namespace halpert.Controllers
{
    [Authorize(Roles = RoleName.Developer)]
    public class DeveloperController : Controller
    {
        private ApplicationDbContext db;
        public DeveloperController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Developer
        public ActionResult Index()
        {
            var loggedInUser = db.Users.Single(u => u.Email == User.Identity.Name);

            var projects = db.WorksOn
                .Include(p => p.Project)
                .Include(u => u.User)
                .Where(w => w.UserId == loggedInUser.Id)
                .ToList();

            return View(projects);
        }

        public ActionResult ProjectTasks(int id)
        {
            var loggedInUser = db.Users.Single(u => u.Email == User.Identity.Name);

            var viewModel = new DeveloperProjectDetails
            {
                Tasks = db.Tasks
                .Where(t => t.DevId == loggedInUser.Id && t.ProjectId == id)
                .ToList(),
                ProjectName = db.Projects.Single(p => p.Id == id).Name
            };

            return View(viewModel);
        }

        public ActionResult TaskDetails(int id)
        {
            return View(db.Tasks.Single(t => t.Id == id));
        }

        int[] updatedTasks = new int[10];
        public ActionResult UpdateDuration(Task task)
        {
            // validation for task's duration
            if(task.Optimistic < 1 || task.MostLikely < 1 || task.Pessimistic < 1)
            {
                return Content("TIMES MUST BE GREATER THAN ZERO !!");
            }

            if(task.Pessimistic >= task.MostLikely && task.MostLikely >= task.Optimistic)
            {
                var taskInDb = db.Tasks.Single(t => t.Id == task.Id);

                taskInDb.Optimistic = task.Optimistic;
                taskInDb.MostLikely = task.MostLikely;
                taskInDb.Pessimistic = task.Pessimistic;
                taskInDb.Accepted = null;

                int duration = (int)(task.Optimistic + (4 * task.MostLikely) + task.Pessimistic) / 6;

                taskInDb.Duration = duration;

                db.SaveChanges();

                return RedirectToAction("TaskDetails", new { id = task.Id });
            }
            else
            {
                return Content("Pessimistic time should be greater than MostLikely time, and MostLikely time should be greater than Pessimistic time");
            }
            
        }


        public ActionResult UpdateProgress(Task task)
        {

            var tsk = db.Tasks.Single(t => t.Id == task.Id);
            
            tsk.Progress = task.Progress;
            db.SaveChanges();
            
            return RedirectToAction("ProjectTasks", new { id = tsk.ProjectId });
        }
        
        public ActionResult DeleteHalsteadResults(Task task)
        {
            var tsk = db.Tasks.Single(t => t.Id == task.Id);

            tsk.Length = null;
            tsk.Vocabulary = null;
            tsk.Volume = null;
            tsk.InsideTheBoundaries = null;
            tsk.Difficulty = null;
            tsk.Effort = null;
            tsk.TimeToUnderStand = null;

            tsk.Bugs = null;

            db.SaveChanges();

            return RedirectToAction("TaskDetails", new { id = task.Id });
        }


        [Route("Developer/UpdateHalsteadResults/{taskId:int}/{length:int}/{vocabulary:int}/{volume:int}/{insideTheBoundaries:bool}/{difficulty:int}/{effort:int}/{timeToUnderstand:int}/{bugs:int}")]
        public ActionResult UpdateHalsteadResults(int taskId, int length, int vocabulary, int volume,
            bool insideTheBoundaries, int difficulty, int effort, int timeToUnderstand, int bugs)
        {
            var task = db.Tasks.Single(t => t.Id == taskId);
            task.Length = length;
            task.Vocabulary = vocabulary;
            task.Volume = volume;
            task.InsideTheBoundaries = insideTheBoundaries;
            task.Difficulty = difficulty;
            task.Effort = effort;
            task.TimeToUnderStand = timeToUnderstand;
            task.Bugs = bugs;

            db.SaveChanges();

            return RedirectToAction("TaskDetails", new { id = taskId });
        }
    }
}