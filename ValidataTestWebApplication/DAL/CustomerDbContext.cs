﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.DAL
{
    public class CustomerDbContext : DbContext, ICustomerDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }

        DbContext ICustomerDbContext.DBContext => this;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}