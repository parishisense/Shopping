﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;

namespace Shopping.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities{ get; set; }

        public DbSet<Country> Countries { get; set; }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<TemporalSale> TemporalSales { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c =>c.Name).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            //De esta forma le indico que solo sera un departamento por pais
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<ProductCategory>().HasIndex("ProductId", "CategoryId").IsUnique();


        }
    }
}
