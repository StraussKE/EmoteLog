using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EmoteLog.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmoteLog.Controllers
{
    [Authorize(Roles = "_Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<EmoteLogUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleMgr, UserManager<EmoteLogUser> userMgr)
        {
            _roleManager = roleMgr;
            _userManager = userMgr;
        }

        public IActionResult AdminUserManagement()
        {
            return View(_userManager.Users);
        }

        public IActionResult AdminRoleManagement()
        {
            ViewBag.userManager = _userManager;
            return View(_roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> AdminDeleteUser(string id)
        {
            EmoteLogUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminUserManagement", _userManager.Users);
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("AdminUserManagement", _userManager.Users);
        }

        // I don't have time to master editing users for this project

        /*public async Task<IActionResult> AdminEditUser(string id)
        {
            EmoteLogUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("AdminRoleManager");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditUser(EmoteLogUser editedUser)
        {
            EmoteLogUser user = await _userManager.FindByIdAsync(editedUser.Id);
            if (user != null)
            {
                {
                    user = editedUser;
                    IdentityResult result = await _userManager.UpdateAsync(editedUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }*/


        public IActionResult AdminCreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> AdminCreateRole([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminRoleManagement");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminRoleManagement");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return View("AdminRoleManagement", _roleManager.Roles);
        }

        public async Task<IActionResult> AdminEditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<EmoteLogUser> members = new List<EmoteLogUser>();
            List<EmoteLogUser> nonMembers = new List<EmoteLogUser>();
            foreach (EmoteLogUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    EmoteLogUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    EmoteLogUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(AdminRoleManagement));
            }
            else
            {
                return await AdminEditRole(model.RoleId);
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}