using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using SimpleOwin.Core.Services;
using Moq;
using SimpleOwin.Core.Security;
using SimpleOwin.Core.Models;
using OwinAuth.ViewModels;
using System.Security.Claims;

namespace OwinAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationManager _authManager;

        public AccountController()
        {
            //trying to keep this project simple, we are mocking out the service instead of using an IoC library such as Autofac
            var service = new Mock<IUserService>();
            service.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new User { Id = 1, FirstName = "George", UserName = "GeorgeYoloSwag99"});

            _userService = service.Object;
            _authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string url)
        {
            if (!ModelState.IsValid)
                return View(model);

            var manager = new SimpleUserManager(_userService, _authManager);
            var user = await manager.FindAsync(model.UserName, model.Password);
            user.Roles = new System.Collections.Generic.List<string>();
            //user.Roles.Add("Admin");
            user.Roles.Add("Member");
            if (user != null)
            {
                await manager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        public ActionResult Logout()
        {
            var manager = new SimpleUserManager(_userService, _authManager);
            manager.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}