using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EldorAnnualLeave.Web.DTOs;
using Microsoft.AspNetCore.Identity;
using EldorAnnualLeave.Controllers;
using System.Security.Claims;
using EldorAnnualLeave.Web.ViewModels;
using Microsoft.AspNetCore.Routing;
using EldorAnnualLeave.Core.Models;

namespace EldorAnnualLeave.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
        }

        /*public IActionResult Login()
        {
            return View();
        }*/

        public IActionResult LoginPage()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Administrator"))
            {
                return RedirectToAction("EmployeeTable", "Admin");
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("Manager"))
            {
                return RedirectToAction("EmployeeTable", "Manager");
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                return RedirectToAction("EmployeeTable", "Member");
            }

            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            //Response.Redirect("~/Views/Member/Index.cshtml", false);
            return View();

            //var routeValue = new RouteValueDictionary(new { action = "Index", controller = "Member" });
            //return RedirectToRoute(routeValue);
            //return RedirectToAction(actionName:"Index", controllerName:"Member");
            //return Redirect(ReturnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userlogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(userlogin.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = new Microsoft.AspNetCore.Identity.SignInResult();
                    try
                    {
                        //var asd = await userManager.GeneratePasswordResetTokenAsync(user);
                        //var result2 = await userManager.ResetPasswordAsync(user, asd, "Eldor_111");
                        result =await signInManager.PasswordSignInAsync(user.UserName, userlogin.PasswordHash, userlogin.RememberMe, false);
                    }
                    catch (Exception ex)
                    {
                        var asd = 0;
                    }
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        await userManager.GetLoginsAsync(user);

                        if (TempData["ReturnUrl"] != null)
                        {
                            var path = TempData["ReturnUrl"].ToString();
                            return Redirect(path);
                        }

                        return RedirectToAction("LoginPage", "Home");
                    }
                    else
                    {
                        await userManager.AccessFailedAsync(user);

                        int fail = await userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("", $" {fail} kez başarısız giriş.");
                        ModelState.AddModelError("", "Email adresiniz veya şifreniz yanlış.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                }
            }

            return View("LoginPage", userlogin);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
