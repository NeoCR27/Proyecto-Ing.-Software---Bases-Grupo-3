using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProyectoPI.Models;

namespace ProyectoPI.Controllers
{
    public class SeguridadController : Controller
    {
        private ApplicationUserManager _userManager;



        public SeguridadController()
        {
            

        }

       
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<ActionResult> ChangeRol(System.Security.Principal.IPrincipal User, string rol)
        {
            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            await UserManager.AddToRoleAsync(user.Id, rol);
            return View();

        }

    }
}