﻿
using BaseLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Employee4RetailStore> Employee4RetailStores { get; set; }
        public DbSet<IoTDevice> ioTDevices { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreArea> StoreAreas { get; set; }

    }
}