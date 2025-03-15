using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext(DbContextOptions options) 
    : DbContext(options)
    {   
            public DbSet<AppUser> Users { get; set; }
    }
}