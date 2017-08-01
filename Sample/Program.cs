using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    enum Sex
    {
        Male,
        Female
    }

    class TestModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public Sex Sex { get; set; }

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

    class GroupType
    {
        public Sex Key { get; set; }

        public int Count { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var SqlServerContext = new SqlServerContext();
            var SqlServerQuery = SqlServerContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count))
                .Where(x => x.Title.Skip(3).Take(4).ToString() == "Hello");
            Console.WriteLine("SQL Server Generated:");
            Console.WriteLine(SqlServerQuery.ToSql());
            Console.WriteLine("Unevaluated:");
            Console.WriteLine(string.Join(
                Environment.NewLine, 
                SqlServerQuery
                    .ToUnevaluated()));
            Console.WriteLine();

            var MySqlContext = new MySqlContext();
            var MySqlQuery = MySqlContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count));
            Console.WriteLine("MySQL Generated:");
            Console.WriteLine(MySqlQuery.ToSql());
            Console.WriteLine("Unevaluated:");
            Console.WriteLine(string.Join(
                Environment.NewLine, 
                MySqlQuery
                    .ToUnevaluated()));
            Console.WriteLine();

            var PostgreSQLContext = new PostgreSQLContext();
            var PostgreSQLQuery = PostgreSQLContext.Models
                .Where(x => x.Title.Contains("Pomelo"))
                .Where(x => new[] { 2, 3, 5, 7, 11 }.Contains(x.Count))
                .GroupBy(x => x.Sex)
                .Select(x => new GroupType { Key = x.Key, Count = Math.Abs(x.Count()) });
            Console.WriteLine("PostgreSQL Generated:");
            Console.WriteLine(PostgreSQLQuery.ToSql());
            Console.WriteLine("Unevaluated:");
            Console.WriteLine(string.Join(
                Environment.NewLine, 
                PostgreSQLQuery
                    .ToUnevaluated()));
            Console.WriteLine();

            Console.Read();
        }
    }
}
