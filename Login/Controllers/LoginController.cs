using Microsoft.AspNetCore.Mvc;
using Login.Models;
using System.Collections.Generic;
namespace Login.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public List<UserModel> PutValue() 
        {
            var user = new List<UserModel>
            {
                new UserModel{Id=1, Username="admin", Password="admin123"},
                new UserModel{Id=2, Username="user", Password="user123"}
            };

            return user;
        }

        [HttpPost]
        public IActionResult Login(UserModel usr)
        {
            var u = PutValue();
            var ue = u.Where(u => u.Username.Equals(usr.Username));

            var up = ue.Where(p => p.Password.Equals(usr.Password));

            if(up.Count() == 1)
            {
                return Redirect("Home"); 
            } else
            {
                ViewBag.message = "Login Failed";
                return View("Login");
            }
        }

        public async  Task<IActionResult> Logout()
        {
            return Redirect("Login");
        }
    }
}
