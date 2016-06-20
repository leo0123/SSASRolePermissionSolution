﻿using SqlToMdxLib.Models;
using System.Data.Entity;



namespace SqlToMdxLib.DAL
{
    public class UsersLineContext : DbContext
    {
        public UsersLineContext() : base("UsersLineContext") { }

        public DbSet<UsersLine> UsersLinePs { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}
