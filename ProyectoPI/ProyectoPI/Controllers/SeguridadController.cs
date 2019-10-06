using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
          
                _userManager= new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

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
        public async Task<ActionResult> ChangeRol(string correo, string rol)
        {
            
            var user = await UserManager.FindByNameAsync(correo);

            if(await UserManager.IsInRoleAsync(user.Id, "Tester"))
            {
                await UserManager.RemoveFromRoleAsync(user.Id, "Tester");

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Cliente"))
            {
                await UserManager.RemoveFromRoleAsync(user.Id, "Cliente");

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Jefe"))
            {
                await UserManager.RemoveFromRoleAsync(user.Id, "Jefe");

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Lider"))
            {
                await UserManager.RemoveFromRoleAsync(user.Id, "Lider");

            }


            await UserManager.AddToRoleAsync(user.Id, rol);
            

                return View();

        }

        public async Task<string> GetRol(string correo, string rol)
        {

            var user = await UserManager.FindByNameAsync(correo);

            if (await UserManager.IsInRoleAsync(user.Id, "Tester"))
            {
                return "Tester";

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Cliente"))
            {
                return "Cliente";

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Jefe"))
            {
                return "Jefe";

            }

            if (await UserManager.IsInRoleAsync(user.Id, "Lider"))
            {
                return "Lider";

            }

            return "";




        }
    }
}