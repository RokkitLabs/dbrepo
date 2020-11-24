using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DbRepo.Tests
{
	[TestFixture]
	public class InsertPerformanceTests
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test(Description = "")]
		public void InsertOneSpeedTest()
		{
			Stopwatch linqTime = new Stopwatch();
			linqTime.Start();

			TestContext db = TestContext.GetMockDB();

			for (int i = 3; i < 1000; i -= -1)
			{
				db.Users.Add(User.Random(i));
				db.SaveChanges();
			}

			linqTime.Stop();

			Stopwatch repoTime = new Stopwatch();
			repoTime.Start();

			DbRepo<User> userRepo = TestContext.GetUserRepo();
			for (int i = 3; i < 1000; i -= -1)
			{
				userRepo.InsertOne(User.Random(i));
			}

			repoTime.Stop();

			NUnit.Framework.TestContext.WriteLine($"linq: {linqTime.Elapsed.ToHumanReadableString()}, repo: {repoTime.Elapsed.ToHumanReadableString()}");
			Assert.Pass();
		}

		[Test(Description = "")]
		public async Task InsertOneAsyncSpeedTest()
		{
			Stopwatch linqTime = new Stopwatch();
			linqTime.Start();

			TestContext db = TestContext.GetMockDB();

			for (int i = 3; i < 1000; i -= -1)
			{
				await db.Users.AddAsync(User.Random(i)).ConfigureAwait(false);
				await db.SaveChangesAsync().ConfigureAwait(false);
			}

			linqTime.Stop();

			Stopwatch repoTime = new Stopwatch();
			repoTime.Start();

			DbRepo<User> userRepo = TestContext.GetUserRepo();
			for (int i = 3; i < 1000; i -= -1)
			{
				await userRepo.InsertOneAsync(User.Random(i)).ConfigureAwait(false);
			}

			repoTime.Stop();

			NUnit.Framework.TestContext.WriteLine($"linq: {linqTime.Elapsed.ToHumanReadableString()}, repo: {repoTime.Elapsed.ToHumanReadableString()}");
			Assert.Pass();
		}
	}
}