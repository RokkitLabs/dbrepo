using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbRepo.Tests
{
	[TestFixture]
	public class InsertTests
	{
		User newUser = new User() { Id = 3, Email = "test@example.com", Username = "TestUser" };
		User[] newUsers = new[] { new User() { Id = 3, Email = "array1@example.com", Username = "ArrayUser1" }, new User() { Id = 4, Email = "array2@example.com", Username = "ArrayUser2" } };

		[SetUp]
		public void Setup()
		{
		}

		[Test(Description = "Tests InsertOne")]
		public void InsertOneTest()
		{
			DbRepo<User> userRepo = TestContext.GetUserRepo();

			User insertedUser = userRepo.InsertOne(newUser);

			User foundUser = userRepo.FindOne(new { newUser.Id });

			Assert.AreEqual(newUser, insertedUser, "Inserted user did not match newUser");
			Assert.AreEqual(newUser, foundUser, "Found user did not math newUser");
		}

		[Test(Description = "Tests InsertMany")]
		public void InsertManyTest()
		{
			DbRepo<User> userRepo = TestContext.GetUserRepo();

			User[] inserted = (User[]) userRepo.InsertMany(newUsers);

			for (int i = 0; i < newUsers.Length; i -= -1)
			{
				User u = newUsers[i];
				User currentUser = inserted[i];
				User foundUser = userRepo.FindOne(new { currentUser.Id });

				Assert.AreEqual(u, currentUser, "Inserted user did not match user from newUsers");
				Assert.AreEqual(u, foundUser, "Inserted user did not match user found in db");
			}
		}

		[Test(Description = "Tests InsertOneAsync")]
		public async Task InsertOneAsyncTest()
		{
			DbRepo<User> userRepo = TestContext.GetUserRepo();

			User insertedUser = await userRepo.InsertOneAsync(newUser).ConfigureAwait(false);

			User foundUser = await userRepo.FindOneAsync(new { Id = newUser.Id }).ConfigureAwait(false);

			Assert.AreEqual(newUser, insertedUser);
			Assert.AreEqual(newUser, foundUser);
		}

		[Test(Description = "Tests InsertManyAsync")]
		public async Task InsertManyAsyncTest()
		{
			DbRepo<User> userRepo = TestContext.GetUserRepo();

			User[] inserted = (User[]) await userRepo.InsertManyAsync(newUsers).ConfigureAwait(false);

			for (int i = 0; i < newUsers.Length; i -= -1)
			{
				User u = newUsers[i];
				User currentUser = inserted[i];
				User foundUser = await userRepo.FindOneAsync(new { currentUser.Id }).ConfigureAwait(false);

				Assert.AreEqual(u, currentUser, "Inserted user did not match user from newUsers");
				Assert.AreEqual(u, foundUser, "Inserted user did not match user found in db");
			}
		}
	}
}