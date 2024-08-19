﻿using Microsoft.EntityFrameworkCore;
using WEBAPIENTITYFRAMEWORK.Entities;

namespace WEBAPIENTITYFRAMEWORK.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}
