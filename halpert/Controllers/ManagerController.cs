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
    [Authorize(Roles = RoleName.Manager)]
    public class ManagerController : Controller
    {
        private ApplicationDbContext db;
        public ManagerController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Manager
        public ActionResult Index()
        {
            var loggedInUser = db.Users.Single(u => u.Email == User.Identity.Name);
            var viewModel = new ManagerIndexVM
            {
                Projects = db.Projects.Where(proj => proj.OwnerId == loggedInUser.Id).ToList(),
            };

            return View(viewModel);
        }

        public ActionResult CreateProject(ManagerIndexVM viewModel)
        {
            var loggedInUser = db.Users.Single(u => u.Email == User.Identity.Name);
            if (!ModelState.IsValid)
            {                var vm = new ManagerIndexVM
                {
                    Projects = db.Projects.Where(proj => proj.OwnerId == loggedInUser.Id).ToList(),
                };
                return View("Index", vm);
            }

            db.Projects.Add(new Project
            {
                Name = viewModel.NewProjectName,
                OwnerId = loggedInUser.Id
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ProjectDetails(int id)
        {
            var tasks = db.Tasks.Include(u => u.User).Where(task => task.ProjectId == id).ToList();
            var viewModel = new ProjectDetailsVM
            {
                Project = db.Projects.Single(proj => proj.Id == id),
                Tasks = tasks
            };

            string ids = "";
            string names = "";
            string durations = "";
            string progresses = "";
            string predecessors = "";
            string dates = "";
            string tmp;

            List<PredecessorTask> preTasks;

            foreach (var task in tasks)
            {
                ids = ids + task.Id.ToString() + "-!*";
                names = names + task.Name + "-!*";
                durations = durations + task.Duration.ToString() + "-!*";
                progresses = progresses + task.Progress.ToString() + "-!*";
                preTasks = db.PredecessorTasks.Where(p => p.TaskId == task.Id).ToList();
                dates = dates + task.StartTime.ToString() + "-!*";
                tmp = "";

                if (preTasks.Count == 0)
                {
                    tmp = " ";
                } 
                else
                {
                    foreach (var preTask in preTasks)
                    {
                        tmp = tmp + preTask.PreTaskId.ToString() + ",";
                    }
                }
                predecessors = predecessors + tmp + "-!*";
            }

            viewModel.NumOfTasks = tasks.Count;
            viewModel.Ids = ids;
            viewModel.Names = names;
            viewModel.Durations = durations;
            viewModel.Progresses = progresses;
            viewModel.Predecessors = predecessors;
            viewModel.Dates = dates;

            return View(viewModel);
        }

        public ActionResult TaskDetails(int id)
        {
            var task = db.Tasks
                .Include(u => u.User)
                .Include(p => p.Project)
                .Single(t => t.Id == id);

            var preTasks = db.PredecessorTasks
                .Include(preTsk => preTsk.PreTask)
                .Where(pre => pre.TaskId == task.Id)
                .ToList();

            var viewModel = new TaskDetailsVM
            {
                Task = task,
                PreTasks = preTasks
            };

            return View(viewModel);
        }

        public ActionResult TeamMembers(int id)
        {
            var viewModel = new TeamMembersVM
            {
                TeamMembers = db.WorksOn
                .Include(u => u.User)
                .Where(w => w.ProjectId == id)
                .ToList(),
                ProjectId = id,
                ProjectName = db.Projects.Single(p => p.Id == id).Name

            };

            return View(viewModel);
        }

        public ActionResult AddMember(TeamMembersVM viewModel)
        {
            var vm = new TeamMembersVM
            {
                TeamMembers = db.WorksOn
                        .Include(u => u.User)
                        .Where(w => w.ProjectId == viewModel.ProjectId)
                        .ToList(),
                ProjectId = viewModel.ProjectId,
                ProjectName = db.Projects.Single(p => p.Id == viewModel.ProjectId).Name

            };

            if (!ModelState.IsValid)
            {
                return View("TeamMembers", vm);
            }

            var user = db.Users.SingleOrDefault(u => u.Email == viewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "This email does not refer to any one!");
                return View("TeamMembers", vm);
            }
            else if (db.WorksOn.Any(wo => wo.UserId == user.Id && wo.ProjectId == viewModel.ProjectId))
            {
                ModelState.AddModelError("", "This mamber is currently working on this project!");
                return View("TeamMembers", vm);
            }
            else if (!db.UserRolesMapping.Any(d => d.UserId == user.Id && d.RoleId == RoleName.DeveloperId))
            {
                ModelState.AddModelError("", "This email does not refer to a developer!");
                return View("TeamMembers", vm);
            }

            db.WorksOn.Add(
                new WorksOn {
                    UserId = user.Id,
                    ProjectId = viewModel.ProjectId
                });

            db.SaveChanges();

            return RedirectToAction("TeamMembers", new { id = viewModel.ProjectId });
        }

        public ActionResult DeleteMamber(string email, int projectId)
        {
            var user = db.Users.Single(u => u.Email == email);

            db.WorksOn.Remove(
                db.WorksOn.Single(w => w.UserId == user.Id && w.ProjectId == projectId)
                );
            db.SaveChanges();

            return RedirectToAction("TeamMembers", new { id = projectId });
        }

        public ActionResult CreateTask(int id)
        {
            var project = db.Projects.Single(p => p.Id == id);

            var workers = db.WorksOn.Where(w => w.ProjectId == id).ToList();

            List<User> developers = new List<User>();

            foreach (var worker in workers)
            {
                developers.Add(db.Users.Single(u => u.Id == worker.UserId));
            }
            
            var viewModel = new CreateTaskVM
            {
                ProjectId = id,
                ProjectName = project.Name,
                Developers = developers,
                PreTasks = db.Tasks.Where(t => t.ProjectId == id).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateTask(CreateTaskVM viewModel)
        {
            var task = new Task
            {
                Name = viewModel.Name,
                Details = viewModel.Details,
                DevId = viewModel.DeveloperId,
                ProjectId = viewModel.ProjectId,
                StartTime = viewModel.StartTime,
                Optimistic = 1, // assign the default value of duration
                MostLikely = 1,
                Pessimistic = 1,
                Duration = 1 
            };

            db.Tasks.Add(task);

            int[] preTaskIds = viewModel.PreTasksIds;
            var len = preTaskIds.Length;

                for (int preTaskId = 0; preTaskId < len; preTaskId++)
                {
                    if (preTaskIds[preTaskId] == 0)
                        continue;

                    db.PredecessorTasks.Add(
                        new PredecessorTask
                        {
                            TaskId = task.Id,
                            PreTaskId = preTaskIds[preTaskId]
                        }
                    );
                }

            db.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = viewModel.ProjectId });
        }

        public ActionResult AcceptedDuration(int id)
        {
            var task = db.Tasks.Single(t => t.Id == id);
            task.Accepted = true;
            db.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = task.ProjectId });
        }
        public ActionResult DeniedDuration(int id)
        {
            var task = db.Tasks.Single(t => t.Id == id);
            task.Accepted = false;
            db.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = task.ProjectId });
        }

        public ActionResult DeleteProject(int id)
        {
            var project = db.Projects.Single(p => p.Id == id);
            db.Projects.Remove(project);

            var developers = db.WorksOn.Where(d => d.ProjectId == id).ToList();
            foreach (var dev in developers)
            {
                db.WorksOn.Remove(dev);
            }

            var tasks = db.Tasks.Where(t => t.ProjectId == id).ToList();
            foreach (var task in tasks)
            {
                var predecessors = db.PredecessorTasks
                    .Where(pre => pre.TaskId == task.Id || pre.PreTaskId == task.Id);
                foreach (var pre in predecessors)
                {
                    db.PredecessorTasks.Remove(pre);
                }

                db.Tasks.Remove(task);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteTask(int id)
        {
            var task = db.Tasks.Single(t => t.Id == id);

            db.Tasks.Remove(task);

            var predecessors = db.PredecessorTasks
                .Where(pre => pre.TaskId == task.Id || pre.PreTaskId == task.Id);

            foreach (var tsk in predecessors)
            {
                db.PredecessorTasks.Remove(tsk);
            }

            db.SaveChanges();

            return RedirectToAction("ProjectDetails", new { id = task.ProjectId });
        }
    }
}