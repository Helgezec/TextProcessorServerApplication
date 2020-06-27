using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TextProcessorServer.Models;

namespace TextProcessorServer.DataService
{
    sealed class Context : DbContext
    {
        public DbSet<Word> Words { get; set; }

        private readonly string connectionString;

        public Context(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString/*"Data Source=localhost;Initial Catalog=TextProcessor;Integrated Security=True;"*/);

        }
    }
}
