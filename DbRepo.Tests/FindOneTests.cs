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
		public int Id = 1;

		[SetUp]
		public void Setup()
		{
			TestDB = TestContext.GetMockDB();
			UserRepo = TestDB.GetRepo<User>();
		}

		[Test(Description = "Tests FindOneAsync")]
		public async Task FindOneAsyncTest()
		{
			User firstLinqUser = await TestDB.Users.FirstOrDefaultAsync(u => u.Id == Id).ConfigureAwait(false);
			User firstDbRepoUser = await UserRepo.FindOneAsync(new { Id }).ConfigureAwait(false);

			Assert.AreEqual(firstLinqUser, firstDbRepoUser);
		}

		[Test(Description = "Tests FindOneAndForgetAsync")]
		public async Task FindOneAndForgetAsyncTest()
		{
			User firstLinqUser = await TestDB.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id).ConfigureAwait(false);
			User firstDbRepoUser = await UserRepo.FindOneAndForgetAsync(new { Id }).ConfigureAwait(false);

			Assert.AreEqual(firstLinqUser, firstDbRepoUser);
		}

		[Test(Description = "Tests FindOne")]
		public void FindOneTest()
		{
			User firstLinqUser = TestDB.Users.FirstOrDefault(u => u.Id == Id);
			User firstDbRepoUser = UserRepo.FindOne(new { Id });

			Assert.AreEqual(firstLinqUser, firstDbRepoUser);
		}

		[Test(Description = "Tests FindOneAndForget")]
		public void FindOneAndForgetTest()
		{
			User firstLinqUser = TestDB.Users.AsNoTracking().FirstOrDefault(u => u.Id == Id);
			User firstDbRepoUser = UserRepo.FindOneAndForget(new { Id });

			Assert.AreEqual(firstLinqUser, firstDbRepoUser);
		}
	}
}