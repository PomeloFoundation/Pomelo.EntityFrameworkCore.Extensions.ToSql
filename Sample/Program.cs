using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    class TestModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public DateTime Time { get; set; }
    }

    class SqlServerContext : DbContext
    {
        public DbSet<TestModel> Models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost;uid=sa;pwd=123456;database=tosqltest");
        }
    }
    class MySqlContext : DbContext
    {
        public DbSet<TestModel> Models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("Server=localhost;uid=root;pwd=123456;database=tosqltest");
        }
    }

    class PostgreSQLContext : DbContext
    {
        public DbSet<TestModel> Models { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Server=localhost;uid=postgres;pwd=123456;database=tosqltest");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var SqlServerContext = new SqlServerContext();
            Console.WriteLine(SqlServerContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count))
                .Where(x => x.Title.Skip(3).Take(4).ToString() == "Hello")
                .ToSql());

            var MySqlContext = new MySqlContext();
            Console.WriteLine(MySqlContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count))
                .Where(x => x.Title.Skip(3).Take(4).ToString() == "Hello")
                .ToSql());

            var PostgreSQLContext = new PostgreSQLContext();
            Console.WriteLine(PostgreSQLContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count))
                .Where(x => x.Title.Skip(3).Take(4).ToString() == "Hello")
                .ToSql());

            Console.Read();
        }
    }
}
