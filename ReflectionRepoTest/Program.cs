using System;

namespace ReflectionRepoTest
{
	public class Program {
		public static void Main(string[] args)
		{
			DbRepo<User> userRepo = new DbRepo<User>();

			User user = userRepo.Testing(new { Id = 15 });
			Console.WriteLine($"{user.Id}");
		}
	}
}