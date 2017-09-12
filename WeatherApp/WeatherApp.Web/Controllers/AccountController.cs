using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherApp.Data.Entities;
using WeatherApp.Web.Models.AccountViewModels;
using WeatherApp.Web.Services;

namespace WeatherApp.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger logger;
        private IStringLocalizer<AccountController> localizer;

#pragma warning disable Nix05 // Too many parameters in constructor
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            IStringLocalizer<AccountController> localizer)
#pragma warning restore Nix05 // Too many parameters in constructor
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = loggerFactory.CreateLogger<AccountController>();
            this.localizer = localizer;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromServices]IOptions<IdentityCookieOptions> identityCookieOptions, string returnUrl = null)
        {
            await this.HttpContext.Authentication.SignOutAsync(identityCookieOptions.Value.ExternalCookieAuthenticationScheme);

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation(1, $"INFO - {DateTime.Now} - User {model.Email} logged in.");
                    return this.RedirectToLocal(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, this.localizer["Invalid login attempt."]);
                    return this.View(model);
                }
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, [FromServices]IEmailSender emailSender, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = this.Url.Action(
                        nameof(this.ConfirmEmail),
                        nameof(AccountController),
                        values: new { userId = user.Id, code = code },
                        protocol: this.HttpContext.Request.Scheme);

                    await emailSender.SendEmailAsync(
                        model.Email,
                        this.localizer["Confirm your account"],
                        this.localizer["Please confirm your account by clicking this link: <a href='{0}'>link</a>", callbackUrl]);

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    await this.userManager.AddToRoleAsync(user, "user");

                    this.logger.LogInformation(3, $"INFO - {DateTime.Now} - New user registered.");

                    return this.RedirectToLocal(returnUrl);
                }

                this.AddErrors(result);
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            this.logger.LogInformation(4, $"INFO - {DateTime.Now} - User logged out.");

            return this.RedirectToAction(nameof(WeatherController.Index), "Weather");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, [FromServices]IEmailSender emailSender)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await this.userManager.IsEmailConfirmedAsync(user)))
                {
                    return this.View("ForgotPasswordConfirmation");
                }

                var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = this.Url.Action(
                    nameof(this.ResetPassword),
                    "Account",
                    new { userId = user.Id, code = code },
                    protocol: this.HttpContext.Request.Scheme);

                await emailSender.SendEmailAsync(
                    model.Email,
                    this.localizer["Reset Password"],
                    this.localizer["Please reset your password by clicking here: <a href='{0}'>link</a>", callbackUrl]);

                return this.View("ForgotPasswordConfirmation");
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? this.View("Error") : this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return this.RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                return this.RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }

            this.AddErrors(result);
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.View("Error");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.View("Error");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(nameof(WeatherController.Index), "Weather");
            }
        }

        #endregion
    }
}