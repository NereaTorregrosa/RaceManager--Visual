﻿using Microsoft.EntityFrameworkCore;

namespace BD_MySQL
{
    public class MySqlDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseMySQL("server=192.168.3.216;database=race_manager;uid=root;pwd=");

            //optionBuilder.UseMySQL("server=10.100.1.25;database=race_manager;uid=root;pwd=");
            //optionBuilder.UseMySQL("server=127.0.0.1;database=race_manager;uid=root;pwd=admin");
        }
    }
}
