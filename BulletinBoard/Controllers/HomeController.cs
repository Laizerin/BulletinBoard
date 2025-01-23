using BulletinBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace BulletinBoard.Controllers
{
    public class HomeController : Controller
    {
        private static List<Advertisement> advertisements = new List<Advertisement>
    {
        new Advertisement { Id = 1, Title = "Ad 1", Description = "��������", Genre = "����" },
        new Advertisement { Id = 2, Title = "Ad 2", Description = "��������", Genre = "������" },
        new Advertisement { Id = 3, Title = "Ad 3", Description = "��������", Genre = "������" },
        new Advertisement { Id = 4, Title = "Ad 4", Description = "��������", Genre = "������" },
        new Advertisement { Id = 5, Title = "Ad 5", Description = "��������", Genre = "�����" },
        new Advertisement { Id = 6, Title = "Ad 6", Description = "��������", Genre = "����" },
        new Advertisement { Id = 7, Title = "Ad 7", Description = "��������", Genre = "������" }
    };

        private static List<User> users = new List<User>
    {
        new User { Id = 1, Username = "user1", Password = "pass1", Role = "Admin" },
        new User { Id = 2, Username = "user2", Password = "pass2", Role = "User" },
        new User { Id = 2, Username = "user3", Password = "pass3", Role = "User" }
    };

        // ��������, ��������� �� ���������� � ���������
        private bool IsFavorite(int userId, string genre)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null || user.FavoriteGenres == null)
            {
                return false;
            }
            return user.FavoriteGenres.Contains(genre);
        }

        public IActionResult Index(string search)
        {
            if (!Request.Cookies.TryGetValue("UserId", out var userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var model = advertisements;
            if (!string.IsNullOrEmpty(search))
            {
                // ����� �� ����� (��� ����� ��������)
                model = advertisements
                    .Where(a => a.Genre.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // �������� ��������� ����� ������������
            var user = users.FirstOrDefault(u => u.Id == userId);
            ViewBag.FavoriteGenres = user?.FavoriteGenres ?? new List<string>();

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var advertisement = advertisements.FirstOrDefault(a => a.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            // �������� ��������� ����� ������������
            if (Request.Cookies.TryGetValue("UserId", out var userIdStr) && int.TryParse(userIdStr, out var userId))
            {
                var user = users.FirstOrDefault(u => u.Id == userId);
                ViewBag.FavoriteGenres = user?.FavoriteGenres ?? new List<string>();
            }
            else
            {
                ViewBag.FavoriteGenres = new List<string>();
            }

            return View(advertisement);
        }

        public IActionResult AddToFavorites(int id)
        {
            if (!Request.Cookies.TryGetValue("UserId", out var userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = users.FirstOrDefault(u => u.Id == userId);
            var advertisement = advertisements.FirstOrDefault(a => a.Id == id);

            if (user != null && advertisement != null)
            {
                // �������������� FavoriteGenres, ���� �� ����� null
                if (user.FavoriteGenres == null)
                {
                    user.FavoriteGenres = new List<string>();
                }

                if (user.FavoriteGenres.Contains(advertisement.Genre))
                {
                    // ������� ���� �� ����������, ���� �� ��� ��� ����
                    user.FavoriteGenres.Remove(advertisement.Genre);
                }
                else
                {
                    // ��������� ���� � ���������
                    user.FavoriteGenres.Add(advertisement.Genre);
                }
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult AddAdvertisement(string title, string description, string genre)
        {
            if (!Request.Cookies.TryGetValue("UserId", out var userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null || user.Role != "Admin")
            {
                return RedirectToAction("Index");
            }

            // ������� ����� ����������
            var newAd = new Advertisement
            {
                Id = advertisements.Count + 1, // ��������� ������ Id
                Title = title,
                Description = description,
                Genre = genre
            };

            // ��������� ���������� � ������
            advertisements.Add(newAd);

            return RedirectToAction("Profile");
        }

        private Dictionary<string, int> GetGenreStatistics()
        {
            var genreStatistics = new Dictionary<string, int>();

            foreach (var user in users)
            {
                if (user.FavoriteGenres != null)
                {
                    foreach (var genre in user.FavoriteGenres)
                    {
                        if (genreStatistics.ContainsKey(genre))
                        {
                            genreStatistics[genre]++;
                        }
                        else
                        {
                            genreStatistics[genre] = 1;
                        }
                    }
                }
            }

            return genreStatistics;
        }

        public IActionResult Profile()
        {
            if (!Request.Cookies.TryGetValue("UserId", out var userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Username = user.Username;
            ViewBag.Role = user.Role;

            if (user.Role == "Admin")
            {
                // �������� ���������� �� ������
                ViewBag.GenreStatistics = GetGenreStatistics();
            }

            return View(user.FavoriteGenres ?? new List<string>());
        }
    }
}
