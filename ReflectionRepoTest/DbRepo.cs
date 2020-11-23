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
				//Convert object to the Type we have received using extension method
				T convertedObj = obj.ToType<T>();
				//This is the initial input, we will name the input Object so this will essentially be
				//Object =>
				ParameterExpression input = Expression.Parameter(convertedObj.GetType(), "Object");
				//Init as null, this will allow us to check whether to add an AND or to instantiate further on in the loop
				BinaryExpression finalExpression = null;

				//Get a list of properties of the object, so that we can get key, value
				PropertyInfo[] properties = obj.GetType().GetProperties();
				foreach (PropertyInfo prop in properties) //Loop through
				{
					//Get the value from the object, we are not using the converted object as default values
					//for fields create issues, so if there was an empty field it may end up being added as an AND
					object? obj2 = prop.GetValue(obj);
					//If null continue with loop
					if (obj2 == null) continue;
					
					//Get property name from Object => Declared above, will essentially be Object.PropName
					MemberExpression property = Expression.Property(input, prop.Name);
					//Create a constant value using the property value
					Expression comparison = Expression.Constant(obj2);
					//Compare with the property of the object with the constant to ensure that they are equal
					BinaryExpression result = Expression.Equal(property, comparison);

					//If final expression is null set the value to the result
					if (finalExpression == null)
						finalExpression = result;
					else //if it is not null then "append" with an AND statement.
						finalExpression = Expression.And(finalExpression, result);
				}
				//Return the created lambda function
				return Expression.Lambda<Func<T, bool>>(finalExpression, input);
			}
			catch
			{
				//We need to add an error here of some sort.
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