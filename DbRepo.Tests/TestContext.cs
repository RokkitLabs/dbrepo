using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbRepo.Tests
{
	public class TestContext : DbContext
	{

		public DbSet<User> Users { get; set; }

		public TestContext(DbContextOptions opts) : base(opts)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity => {
				entity.Property(e => e.Id)
					.IsRequired();

				entity.Property(e => e.Email)
					.IsRequired()
					.HasMaxLength(70);

				entity.Property(e => e.Username)
					.IsRequired()
					.HasMaxLength(20);
			});
		}

		public static TestContext GetMockDB()
		{
			DbContextOptions options = new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
			TestContext context = new TestContext(options);

			context.Database.EnsureCreated();

			context.Users.AddRange(new[] {
				new User() { Id = 1, Email = "mail@ahowe.dev", Username = "00"},
				new User() { Id = 2, Email = "mwmatthew10@gmail.com", Username = "TatoEXP"},
			});

			context.SaveChanges();

			return context;
		}
	}
}
