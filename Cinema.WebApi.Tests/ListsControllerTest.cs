using Cinema.Persistence;
using Cinema.Persistence.DTO;
using Cinema.Persistence.Services;
using Cinema.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Cinema.WebApi.Tests
{
    public class ListsControllerTest : IDisposable
    {
        private readonly CinemaDbContext _context;
        private readonly CinemaService _service;
        private readonly ListsController _controller;
        private readonly MoviesController _movieController;
        private readonly ScreeningsController _screeningsController;

        public ListsControllerTest()
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new CinemaDbContext(options);

            TestDbInitializer.Initialize(_context);

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_context), null,
                new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);

            var user = new ApplicationUser { UserName = "testName", Id = "testId" };
            userManager.CreateAsync(user, "testPassword").Wait(); //Azért kell a wait mert a konstruktor nem lehet async


            _service = new CinemaService(_context);
            _controller = new ListsController(_service,userManager);
            _movieController = new MoviesController(_service);
            _screeningsController = new ScreeningsController(_service);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testName"),
                new Claim(ClaimTypes.NameIdentifier, "testId"),
            });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetListsTest()
        {
            // Act
            var result = _controller.GetLists(); //Ez egy IActionResulttal tér vissza, result.valueben lesz a ListDto-k listája

            // Assert
            var content = Assert.IsAssignableFrom<IEnumerable<ListDto>>(result.Value);
            Assert.Equal(1, content.Count());
        }

        [Theory]
        [InlineData(1)]//Ez lesz az id lentebb
        public void GetListByIdTest(Int32 id)
        {
            // Act
            var result = _controller.GetList(id);

            // Assert
            var content = Assert.IsAssignableFrom<ListDto>(result.Value);
            Assert.Equal(id, content.Id);
        }

        [Fact]
        public void GetInvalidListTest()
        {
            // Arrange
            var id = 4;

            // Act
            var result = _controller.GetList(id);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public void PostListTest() //List hozzáadás testje
        {
            // Arrange
            var newList = new ListDto { Name = "New test list" };
            var count = _context.Lists.Count();

            // Act
            var result = _controller.PostList(newList);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<ListDto>(objectResult.Value);
            Assert.Equal(count + 1, _context.Lists.Count());
        }

        [Fact]
        public void GetMoviesTest()
        {
            // Act
            var result = _movieController.GetMovies(1); 

            // Assert
            var content = Assert.IsAssignableFrom<IEnumerable<MovieDto>>(result.Value);
            Assert.Equal(7, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]//Ez lesz az id lentebb
        public void GetMovieByIdTest(Int32 id)
        {
            // Act
            var result = _movieController.GetMovie(id);

            // Assert
            var content = Assert.IsAssignableFrom<MovieDto>(result.Value);
            Assert.Equal(id, content.Id);
        }


        [Fact]
        public void PostMovieTest()
        {
            var newMovie = new MovieDto {
                Title = "New Title",
                ReleaseDate = DateTime.Now,
                ListId = 1,
                ScreeningTimes = "12:30"
            };
            var count = _context.Movies.Count();

            // Act
            var result = _movieController.PostMovie(newMovie);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<MovieDto>(objectResult.Value);
            Assert.Equal(count + 1, _context.Movies.Count());
        }

        [Fact]
        public void GetScreeningsTest()
        {
            // Act
            var result = _screeningsController.GetScreenings(1);

            // Assert
            var content = Assert.IsAssignableFrom<IEnumerable<ScreeningDto>>(result.Value);
            Assert.Equal(4, content.Count());
        }

        [Fact]
        public void PostScreeningTest()
        {
            var newHall = new Hall
            {
                Name = "A",
                RowCount = 10,
                ColumnCount = 10
            };

            var newScreening = new ScreeningDto
            {
                MovieId = 1,
                ScreenTime = "12:45",
                Name = " ",
                PhoneNumber = " ",
                ScreeningHall = newHall,
                Seats = new List<Seat>(100)

            };
            var count = _context.Screenings.Count();

            // Act
            var result = _screeningsController.PostScreening(newScreening);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<ScreeningDto>(objectResult.Value);
            Assert.Equal(count + 1, _context.Screenings.Count());
        }

        [Theory]
        [InlineData(1)]
        //Ez lesz az id lentebb
        public void GetScreeingsByIdTest(Int32 id)
        {
            // Act
            var result = _screeningsController.GetScreening(id);

            // Assert
            var content = Assert.IsAssignableFrom<ScreeningDto>(result.Value);
            Assert.Equal(id, content.Id);
        }
    }
}
