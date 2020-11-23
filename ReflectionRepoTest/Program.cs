using System;
using System.Linq;
namespace ReflectionRepoTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			User[] fatty = new User[]{ new User { Username = "Fuck off cunt" }, new User { Id = 69420, Username = "Chad it worked" } };
			DbRepo<User> userRepo = new DbRepo<User>(fatty.AsQueryable(), null);
			User result = userRepo.FindOne(new { Username = "Chad it worked", Id = 69420}).GetAwaiter().GetResult();
			Console.WriteLine(result.Username);
		}
	}
}