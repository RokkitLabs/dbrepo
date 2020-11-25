using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using DbRepo.Attributes;
namespace DbRepo.Tests
{
	[Table("users")]
	public class User
	{
		[RepoColumnName("id")]
		[Column("id")]
		public int Id { get; set; }

		[Column("email")]
		[StringLength(70)]
		public string Email { get; set; }

		[Column("username")]
		[StringLength(20)]
		public string Username { get; set; }

		public static User Random(int id) => new User() { Id = id, Username = RandomUtils.GenerateName(12), Email = $"{new Random().Next(10000)}@example.com" };

		public override bool Equals(object obj)
		{
			try
			{
				User u = (User)obj;
				if (
					Email == u.Email &&
					Username == u.Username &&
					Id == u.Id
					) return true;

				return base.Equals(obj);
			} catch
			{
				return false;
			}
		}
	}
}
