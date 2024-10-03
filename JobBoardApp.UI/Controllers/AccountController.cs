﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobBoardApp.Application.Common.Models;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.UI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using JobBoardApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace JobBoardApp.UI.Controllers
{
    public class AccountController(IAuthService authService,
        ITokenProvider tokenProvider,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        #region Checking uniqueness methods (Email,UserName)
        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExist = await _authService.EmailExist(email);
            if (emailExist.Data)
            {
                return Json(new { exists = true });
            }

            return Json(new { exists = false });
        }

        [HttpGet]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var usernameExist = await _authService.UserNameExist(username);
            if (usernameExist.Data)
            {
                return Json(new { exists = true });
            }

            return Json(new { exists = false });
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error with entered values");
                return View(model);
            }

            // Check if a new resume has been uploaded
            if (model.Resume != null)
            {
                var resumeFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/resumes");
                var uniqueResumeFileName = Guid.NewGuid().ToString() + "_" + model.Resume.FileName;
                var resumeFilePath = Path.Combine(resumeFolder, uniqueResumeFileName);

                if (!Directory.Exists(resumeFolder))
                    Directory.CreateDirectory(resumeFolder);

                using (var resumeStream = new FileStream(resumeFilePath, FileMode.Create))
                {
                    await model.Resume.CopyToAsync(resumeStream);
                }

                model.ResumePath = "/uploads/resumes/" + uniqueResumeFileName;
            }

            var result = await _authService.RegisterAsync(model);

            if (result.Success)
            {
                TempData["success"] = result.Message;
                return RedirectToAction(nameof(Login));
            }

            TempData["error"] = result.Message;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _authService.LoginAsync(loginModel);

            if (result.Success)
            {
                // sign in user applied
                await SignInUser(result.Data);

                // set token for user
                _tokenProvider.SetToken(result.Data);

                TempData["success"] = "Login successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = result.Message;

            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        #region Private Methods
        // Sign In User
        private async Task SignInUser(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            // adding claims
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        #endregion
    }
}
