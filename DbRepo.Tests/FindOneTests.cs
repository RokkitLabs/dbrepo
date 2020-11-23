using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DbRepo.Tests
{
	[TestFixture]
	public class FindOneTests
	{
		public TestContext TestDB { get; set; }
		public DbRepo<User> UserRepo { get; set; }

		[SetUp]
		public void Setup()
		{
			TestDB = TestContext.GetMockDB();
			UserRepo = new DbRepo<User>(TestDB.Users, TestDB);
		}

		[Test]
		public async Task FindOneTest()
		{
			User firstLinqUser = await TestDB.Users.FirstOrDefaultAsync(u => u.Id == 1);
			User firstDbRepoUser = await UserRepo.FindOneAndForgetAsync(new { Id = 1 });

			Assert.AreEqual(firstLinqUser, firstDbRepoUser);
		}
	}
}