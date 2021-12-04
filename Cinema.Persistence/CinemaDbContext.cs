using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Cinema.Persistence
{
    public class CinemaDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<List> Lists { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Screening> Screenings { get; set; }

        //public DbSet<Seat> Seats { get; set; }

        public CinemaDbContext(DbContextOptions options) : base(options)
        {

        }


    }
}
