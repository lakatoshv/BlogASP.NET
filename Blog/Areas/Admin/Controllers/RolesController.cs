﻿using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.Areas.Admin.ViewModels;
using Blog.Areas.Admin.ViewModels.Users;
using BLog.Data;
using Blog.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog.Areas.Admin.Controllers
{
    /// <summary>
    /// Roles controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        /// <summary>
        /// The upload from file service.
        /// </summary>
        private readonly IUploadFromFileService _uploadFromFileService;

        /// <summary>
        /// The role manager.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="uploadFromFileService">The upload from file service.</param>
        public RolesController(IUploadFromFileService uploadFromFileService)
        {
            _uploadFromFileService = uploadFromFileService;
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BlogContext()));
            ViewBag.AdminView = true;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>ActionResult</returns>
        // GET: Admin/Roles
        public async Task<ActionResult> Index()
        {
            ViewBag.Controller = "Roles";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();

            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: Roles/Create
        /// <summary>
        /// Create role page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create() => View();

        // POST: Roles/Create
        /// <summary>
        /// Create role action.
        /// </summary>
        /// <param name="roleModel">roleModel.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(IdentityRole roleModel)
        {
            try
            {
                //post.CreatedAt = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return null;
                }
                await _roleManager.CreateAsync(roleModel);
                return RedirectToAction("index", "Roles", new { area = "Admin" });
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        /// <summary>
        /// Edit role page.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Posts");
            }

            var role = await _roleManager.FindByIdAsync(id);

            return View(new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            });
        }

        // POST: Roles/Edit/5
        /// <summary>
        /// Edit role action.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="editedRole">editedRole.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, EditRoleViewModel editedRole)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Roles");
            }

            if (!ModelState.IsValid)
            {
                return View(editedRole);
            }

            try
            {
                var roleModel = await _roleManager.FindByIdAsync(id);
                roleModel.Name = editedRole.Name;
                
                await _roleManager.UpdateAsync(roleModel);
                return RedirectToAction("Index", "Roles");
            }
            catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", dex.Message);

                return RedirectToAction("Index", "Roles");
            }
        }

        // POST: Roles/Delete/5
        /// <summary>
        /// Delete post action.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="collection">collection.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id, FormCollection collection)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Uploads the items from excel.
        /// </summary>
        /// <param name="uploadFile">The upload file.</param>
        /// <returns>Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadItemsFromExcel(UploadFileViewModel uploadFile)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var currentUserId = User.Identity.GetUserId();
            var result = await _uploadFromFileService
                .UploadRolesFromExcel(uploadFile?.ExcelFile.InputStream, currentUserId).ConfigureAwait(false);

            ViewBag.Controller = "Roles";
            ViewBag.UploadFileViewModel = new UploadFileViewModel();
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.ExceptionMessage);

            return View("Index");
        }
    }
}