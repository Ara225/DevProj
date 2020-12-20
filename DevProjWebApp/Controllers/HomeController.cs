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
            try
            {
                UserManager<DynamoDBUser> _UserManager = (UserManager<DynamoDBUser>)HttpContext.RequestServices.GetService(typeof(UserManager<DynamoDBUser>));
                DynamoDBUser User = await _UserManager.GetUserAsync(HttpContext.User);
                Projects = await _dataAccess.GetProjectsByOwnerId(User.Id, false);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Internal Error. Please try again later.";
                ViewBag.AlertClass = "alert-error";
            }
            return View(Projects);
        }

        [Authorize]
        public IActionResult CreateProject()
        {
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
                ProjectDataModel ProjectModal = new ProjectDataModel(modal.Name, modal.Description, User.Id, Convert.ToBoolean(modal.isPrivate));
                ProjectModal.RepositoryURL = modal.RepositoryURL;
                await _dataAccess.SaveItemToDB(ProjectModal, new System.Threading.CancellationToken());
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
