using System.Collections.Generic;
using NUnit.Framework;
using BulletinBoard.Models;

namespace BulettinBoardTests
{
    [TestFixture]
    public class AdvertisementTests
    {
        [Test]
        public void AddToFavorites_ShouldAddGenre()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser", Password = "testpass", FavoriteGenres = new List<string>() };
            var genre = "Игры";

            // Act
            user.FavoriteGenres.Add(genre);

            // Assert
            Assert.Contains(genre, user.FavoriteGenres);
        }

        [Test]
        public void Login_ShouldReturnTrueForValidCredentials()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "pass1" },
                new User { Id = 2, Username = "user2", Password = "pass2" }
            };
            var username = "user1";
            var password = "pass1";

            // Act
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            // Assert
            Assert.IsNotNull(user); // Пользователь должен быть найден
            Assert.AreEqual(username, user.Username); // Имя пользователя должно совпадать
        }

        [Test]
        public void CalculateGenreStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "pass1", FavoriteGenres = new List<string> { "Игры", "Фильмы" } },
                new User { Id = 2, Username = "user2", Password = "pass2", FavoriteGenres = new List<string> { "Игры" } },
                new User { Id = 3, Username = "user3", Password = "pass3", FavoriteGenres = new List<string> { "Фильмы", "Музыка" } }
            };

            // Act
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

            // Assert
            Assert.AreEqual(2, genreStatistics["Игры"]); // Ожидаем 2 пользователя с жанром "Игры"
            Assert.AreEqual(2, genreStatistics["Фильмы"]); // Ожидаем 2 пользователя с жанром "Фильмы"
            Assert.AreEqual(1, genreStatistics["Музыка"]); // Ожидаем 1 пользователя с жанром "Музыка"
        }
    }
}