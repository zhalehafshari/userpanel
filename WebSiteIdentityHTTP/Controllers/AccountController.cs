using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSiteIdentityHTTP.Models;
using WebsiteCore.services;
using Infrastructure.Efcore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebSiteIdentityHTTP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> usermanager;
        private readonly SignInManager<User> signInManager;
        private readonly ProjectDbContext dbContext;
        private readonly IViewRenderService viewRender;

        public AccountController(UserManager<User> usermanager
            , SignInManager<User> signInManager
            , ProjectDbContext dbContext
            , IViewRenderService viewRender)
        {
            this.usermanager = usermanager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.viewRender = viewRender;
        }

        #region Register
        [Route("Account")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Account")]
        [HttpPost]
        public async Task<IActionResult> RegiaterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", "model");
            }
            var errors = await CreatUserAsync(model);
            if (!string.IsNullOrEmpty(errors))
            {
                ModelState.AddModelError("", errors);
                return View("Register", model);
            }

            return View("SuccessRegister", model);
        }
        public async Task<string> CreatUserAsync(UserRegisterViewModel model)
        {
            var user = new User
            {
                ID = model.Username,
                UserName = model.Username,
                Password = model.Password,
                Email = model.Email.ToLower().Trim().ToString(),
                IsActive = false,
                RegisterDate = DateTime.Now,
                ActiveCode = GenerateUniqCode.GenerateActiveCode(),

            };
            var result = await usermanager.CreateAsync(user, user.Password);
            var ErrorBuilder = result.Errors.Aggregate(new StringBuilder(), (seed, error) =>
            {
                seed.Append(error.Description + Environment.NewLine);
                return seed;
            });
            return ErrorBuilder.ToString();


        }

        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            var user = await usermanager.FindByIdAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", $"user{model.UserName} dose not exist");
                return View("Login", model);
            }
            if (user.IsActive)
            {

                var signinresult = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
                if (!signinresult.Succeeded)
                {
                    var Error = CreatErrorMessage(signinresult);
                    ModelState.AddModelError("", Error);
                    return View("Login", model);
                }
            }
            else
            {
                ModelState.AddModelError("", "your Account dose not active  ");
                return View("Login", model);
            }

            return RedirectToAction("Index", "Home");

        }
        private string CreatErrorMessage(SignInResult signinresult)
        {
            if (signinresult.IsLockedOut)
            {
                return ("Entered User Is Locked");
            }
            if (signinresult.IsNotAllowed)
            {
                return ("Entered User Is NotAllowed");

            }
            return ("User Can Not Login");
        }

        #endregion

        #region ActiveAccount

        public IActionResult ActiveAccount(string id)
        {


            ViewBag.IsActive = ActiveAccountUser(id);
            return View();
        }
        private bool ActiveAccountUser(string activecode)
        {
            var user = dbContext.Users.SingleOrDefault(p => p.ActiveCode == activecode);
            if (user != null)
            {
                user.IsActive = true;
                user.ActiveCode = GenerateUniqCode.GenerateActiveCode();
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult logout()
        {
            signInManager.SignOutAsync();
            return Redirect("Home");
        }
        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string FixedEmail = model.Email.Trim().ToLower();
            var user = usermanager.Users.SingleOrDefault(p => p.Email == FixedEmail);
            if (user == null)
            {
                ModelState.AddModelError("Email", "dose not exist user");
                return View(model);
            }
            string bodyEmail = viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی حساب کاربری", bodyEmail);
            ViewBag.IsSuccess = true;



            return View();
        }
      
        #endregion

        #region ResetPassword
        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = dbContext.Users.SingleOrDefault(p => p.ActiveCode == model.ActiveCode);

            if (user == null)
                return NotFound();

            string hashNewPassword = EncodePasswordHash.EncodePasswordMd5(model.Password);
            user.Password = hashNewPassword;
            await usermanager.UpdateAsync(user);

            return Redirect("/Login");
        }
    }
}
#endregion

