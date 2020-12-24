using DevProjWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataAccess;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;

namespace DevProjWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DynamoDBDataAccessLayer _dataAccess;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dataAccess = new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient());
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<ProjectDataModel> Projects = new List<ProjectDataModel>();
            // Get the user object - use to prove that the user has access to projects
            UserManager<DynamoDBUser> _UserManager = (UserManager<DynamoDBUser>)HttpContext.RequestServices.GetService(typeof(UserManager<DynamoDBUser>));
            DynamoDBUser User = await _UserManager.GetUserAsync(HttpContext.User);
            try
            {
                // Handle requests to delete items
                if (Request.QueryString.HasValue)
                {
                    if (Request.QueryString.Value.Contains("?DeleteItem="))
                    {
                        string Id = Request.QueryString.Value.Split("?DeleteItem=")[1];
                        ProjectDataModel Project = await _dataAccess.GetProjectById(Id);
                        if (Project.OwnerId != User.Id)
                        {
                            throw new Exception("Not authorized");
                        }
                        else
                        {
                            // Delete project
                            await _dataAccess.DeleteItem(Project, new System.Threading.CancellationToken());
                            // Delete the attached goals
                            List<GoalDataModel> Goals = await _dataAccess.GetGoalsByProjectId(Project.Id);
                            foreach (GoalDataModel Goal in Goals)
                            {
                                await _dataAccess.DeleteItem(Goal, new System.Threading.CancellationToken());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Unable to delete item";
                ViewBag.AlertClass = "alert-error";
                throw;
            }
            // Get projects to display
            try
            {
                Projects = await _dataAccess.GetProjectsByOwnerId(User.Id, false);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Internal Error. Please try again later.";
                ViewBag.AlertClass = "alert-error";
                throw;
            }
            return View(Projects);
        }

        [Authorize]
        public async Task<IActionResult> CreateProject()
        {
            // Deal with getting 
            if (Request.QueryString.HasValue)
            {
                if (Request.QueryString.Value.Contains("?id="))
                {
                    string Id = Request.QueryString.Value.Split("?id=")[1];
                    try
                    {
                        // Getting current user as their ID needs to be used in the project record
                        UserManager<DynamoDBUser> _UserManager = (UserManager<DynamoDBUser>)HttpContext.RequestServices.GetService(typeof(UserManager<DynamoDBUser>));
                        DynamoDBUser User = await _UserManager.GetUserAsync(HttpContext.User);
                        ProjectDataModel Project = await _dataAccess.GetProjectById(Id);

                        if (Project.OwnerId != User.Id)
                        {
                            ViewBag.Message = "Bad query string.";
                            ViewBag.AlertClass = "alert-error";
                        }
                        else
                        {
                            // Pass vars to the view
                            ViewBag.ProjectId = Project.Id;
                            ViewBag.ProjectName = Project.Name;
                            ViewBag.ProjectPrivacy = Project.isPrivate;
                            ViewBag.ProjectRepositoryURL = Project.RepositoryURL;
                            ViewBag.ProjectDescription = Project.Description;
                            ViewBag.Goals = await _dataAccess.GetGoalsByProjectId(Project.Id);
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "Bad query string.";
                        ViewBag.AlertClass = "alert-error";
                        throw;
                    }
                }
                else 
                {
                    ViewBag.Message = "Bad query string.";
                    ViewBag.AlertClass = "alert-error";
                }
            }
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectViewModel modal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Invalid input.";
                ViewBag.AlertClass = "alert-error";
                return View();
            }
            try
            {
                // Getting current user as their ID needs to be used in the project record
                UserManager<DynamoDBUser> _UserManager = (UserManager<DynamoDBUser>)HttpContext.RequestServices.GetService(typeof(UserManager<DynamoDBUser>));
                DynamoDBUser User = await _UserManager.GetUserAsync(HttpContext.User);
                ProjectDataModel ProjectModal;
                // If the project already exists
                if (modal.Id.Length < 0)
                {
                    ProjectModal = new ProjectDataModel(modal.Name, modal.Description, User.Id, Convert.ToBoolean(modal.isPrivate));
                }
                else
                {
                    ProjectDataModel Project = await _dataAccess.GetProjectById(modal.Id);
                    if (Project.OwnerId != User.Id)
                    {
                        throw new Exception("Not authorized");
                    }
                    ProjectModal = new ProjectDataModel(modal.Name, modal.Description, User.Id, Convert.ToBoolean(modal.isPrivate), modal.Id);
                }
                ProjectModal.RepositoryURL = modal.RepositoryURL;
                await _dataAccess.SaveItemToDB(ProjectModal, new System.Threading.CancellationToken());
                
                GoalDataModel GoalToBeSaved;
                foreach (GoalViewModel Goal in modal.GoalsList)
                {
                    if (Goal.Id.Length < 0)
                    {
                        GoalToBeSaved = new GoalDataModel(Goal.Name, Goal.Description, ProjectModal.Id, Goal.GoalDueBy.ToString());
                    }
                    else
                    { 
                        GoalToBeSaved = new GoalDataModel(Goal.Name, Goal.Description, ProjectModal.Id, Goal.GoalDueBy.ToString(), Goal.Id);
                    }
                    if (Goal.Name == "DELETE_ME")
                    {
                        await _dataAccess.DeleteItem(GoalToBeSaved, new System.Threading.CancellationToken());
                    }
                    else
                    {
                        await _dataAccess.SaveItemToDB(GoalToBeSaved, new System.Threading.CancellationToken());
                    }
                }
                ViewBag.Message = "Project saved.";
                ViewBag.AlertClass = "alert-info";
            }
            catch (Exception e)
            {
                ViewBag.Message = "Internal Error. Please try again later.";
                ViewBag.AlertClass = "alert-error";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
