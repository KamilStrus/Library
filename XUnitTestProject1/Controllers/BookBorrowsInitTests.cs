using Library.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1.Controllers
{
    public class BookBorrowsInitTests
    {

        private readonly TestServer _server;
        private readonly HttpClient _client;


        public BookBorrowsInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();


            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.BookBorrow.Add(new BookBorrow
                {
                    IdUser = 1,
                    IdBook = 1,
                    Comments = "komentarz"
                });

                _db.User.Add(new User
                {
                    IdUser = 1,
                    Email = "jd@pja.edu.pl",
                    Name = "Jakub",
                    Surname = "Szałek",
                    Login = "js",
                    Password = "ASNDKWQOJRJOP!JO@JOP"
                });

                _db.Book.Add(new Book
                {
                    IdBook = 1
                });


                _db.SaveChanges();
            }
        }

        [Fact]
        public async Task PostBookBorrows_200Ok()
        {

            var newBookBorrow = new BookBorrow()
            {
                IdUser = 2,
                IdBook = 2,
                Comments = "brak"
            };

            var serializedUser = JsonConvert.SerializeObject(newBookBorrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponse = await _client
                .PostAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows", payload);

            postResponse.EnsureSuccessStatusCode();
            var com = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("brak",com);
        }

        [Fact]
        public async Task PutBookBorrows_200Ok()
        {

            var updatedBookBorrow = new BookBorrow()
            {
                IdUser = 1,
                IdBook = 1,
                Comments = "brak"
            };

            var serializedUser = JsonConvert.SerializeObject(updatedBookBorrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponse = await _client
                .PutAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows/1", payload);

            postResponse.EnsureSuccessStatusCode();
        }

    }
}
