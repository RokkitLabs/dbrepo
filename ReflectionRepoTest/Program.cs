using System;
using System.Linq;
namespace ReflectionRepoTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			User[] fatty = new User[]{ new User { Username = "Fuck off cunt" }, new User { Id = 69420 } };
			DbRepo<User> userRepo = new DbRepo<User>(fatty.AsQueryable(), null);
			userRepo.FindOne(new { Id = 69420 }).GetAwaiter().GetResult();
		}
	}
}