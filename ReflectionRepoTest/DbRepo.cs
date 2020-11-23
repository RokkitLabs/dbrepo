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

		private Expression<Func<T, bool>> BuildExpression(object obj)
		{
			T epicObj = obj.ToType<T>();
			// db.Users.Where(u => u.Id == id);
			// db.UserRepo.FindOne(new {Id = id});

			PropertyInfo[] properties = obj.GetType().GetProperties();

			foreach (PropertyInfo prop in properties)
			{
				try
				{
					object? obj2 = prop.GetValue(obj);
					if (obj2 == null) continue;
					ParameterExpression left = Expression.Parameter(prop.PropertyType, prop.Name);
					Expression right = Expression.Constant(obj2);
					Expression expr = Expression.Equal(left, right);

					MethodCallExpression whereCallExpression = Expression.Call(
						typeof(Queryable),
						"Where",
						new Type[] { _set.AsQueryable().ElementType },
						_set.AsQueryable().Expression,
						Expression.Lambda<Func<int, bool>>(expr, new ParameterExpression[] { left })
					);

					IQueryable<T> Queryable = _set.AsQueryable().Provider.CreateQuery<T>(whereCallExpression);
					foreach(T entry in Queryable)
					{
						Console.WriteLine(entry);
					}
					return null;
				}
				catch
				{
					return null; // ignored
				}
			}
			return null;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public async Task<T> FindOne(object obj) {
			Expression<Func<T, bool>> expression = BuildExpression(obj);
			return await _set.FirstOrDefaultAsync(expression).ConfigureAwait(false);
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