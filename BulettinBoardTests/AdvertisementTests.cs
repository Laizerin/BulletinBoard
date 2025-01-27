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
            var genre = "����";

            // Act
            user.FavoriteGenres.Add(genre);

            // Assert
            Assert.That(user.FavoriteGenres, Has.Member(genre)); // ���������� Has.Member
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
            Assert.That(user, Is.Not.Null); // ������������ ������ ���� ������
            Assert.That(user.Username, Is.EqualTo(username)); // ��� ������������ ������ ���������
        }

        [Test]
        public void CalculateGenreStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "pass1", FavoriteGenres = new List<string> { "����", "������" } },
                new User { Id = 2, Username = "user2", Password = "pass2", FavoriteGenres = new List<string> { "����" } },
                new User { Id = 3, Username = "user3", Password = "pass3", FavoriteGenres = new List<string> { "������", "������" } }
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
            Assert.That(genreStatistics["����"], Is.EqualTo(2)); // ������� 2 ������������ � ������ "����"
            Assert.That(genreStatistics["������"], Is.EqualTo(2)); // ������� 2 ������������ � ������ "������"
            Assert.That(genreStatistics["������"], Is.EqualTo(1)); // ������� 1 ������������ � ������ "������"
        }
    }
}