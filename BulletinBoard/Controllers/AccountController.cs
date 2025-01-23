using BulletinBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace BulletinBoard.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>
    {
        new User { Id = 1, Username = "user1", Password = "pass1", FavoriteGenres = [] },
        new User { Id = 2, Username = "user2", Password = "pass2", FavoriteGenres = [] }
    };

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Response.Cookies.Append("UserId", user.Id.ToString());
                ViewBag.Role = user.Role; // Передаем роль в ViewBag
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        public IActionResult Logout()
        {
            // Удаляем куки
            Response.Cookies.Delete("UserId");
            return RedirectToAction("Login");
        }
    }
}
