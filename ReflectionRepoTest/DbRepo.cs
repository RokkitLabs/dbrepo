using System;
using System.Reflection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ReflectionRepoTest
{
	public class DbRepo<T> where T : class
	{
		private readonly IQueryable<T> _set;
		private readonly DbContext _db;

		/// <summary>
		/// Creates a new database repository
		/// ```cs
		/// DbRepo<User> = new DbRepo<User>(Users, this);
		/// ```
		/// </summary>
		/// <param name="set">The DbSet for the entity to make the repository for</param>
		/// <param name="db">The DbContext</param>
		public DbRepo(IQueryable<T> set, DbContext db) {
			this._set = set;
			this._db = db;
		}

		private Expression<Func<T, bool>>BuildExpression(object obj)
		{
			try
			{
				T convertedObj = obj.ToType<T>();
				// db.Users.Where(u => u.Id == id);
				// db.UserRepo.FindOne(new {Id = id});
				ParameterExpression input = Expression.Parameter(convertedObj.GetType(), "Object");
				BinaryExpression finalExpression = null;

				PropertyInfo[] properties = obj.GetType().GetProperties();
				foreach (PropertyInfo prop in properties)
				{
					object? obj2 = prop.GetValue(obj);
					if (obj2 == null) continue;
					// Is db.Users.Where(u => u == new User {Id = 1}) possible on normal linq/efcore?

					MemberExpression property = Expression.Property(input, prop.Name);
					Expression comparison = Expression.Constant(obj2);
					BinaryExpression result = Expression.Equal(property, comparison);
					if (finalExpression == null)
						finalExpression = result;
					else
						finalExpression = Expression.And(finalExpression, result);
				}
				return Expression.Lambda<Func<T, bool>>(finalExpression, input);
			}
			catch
			{
				return null;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public async Task<T> FindOne(object obj) {
			Expression<Func<T, bool>>? expression = BuildExpression(obj);
			return null;
			//return await _set.FirstOrDefaultAsync(expression).ConfigureAwait(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public async Task<T> FindOneAndForget(object obj) {
			Expression<Func<T, bool>> expression = BuildExpression(obj);
			return await _set.AsNoTracking().FirstOrDefaultAsync(expression).ConfigureAwait(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public async Task<T> InsertOne(T obj) {
			//_set.Add(obj);
			int result = await _db.SaveChangesAsync().ConfigureAwait(false);
			return obj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objArr"></param>
		/// <returns></returns>
		public async Task<IEnumerable<T>> InsertMany(IEnumerable<T> objArr)
		{
			//_set.AddRange(objArr);
			int result = await _db.SaveChangesAsync().ConfigureAwait(false);
			return objArr;
		}
		
		public T Testing(object val)
		{
			Type type = typeof(T);
			PropertyInfo[] properties = val.GetType().GetProperties();

			foreach (PropertyInfo prop in properties)
			{
				try
				{
					object? obj = prop.GetValue(val);
					if (obj == null) continue;
					Console.WriteLine($"{prop.Name}");
				}
				catch
				{
					// ignored
				}
			}
			
			return val.ToType<T>();
		}
	}
}