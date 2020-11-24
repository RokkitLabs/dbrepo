using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace DbRepo
{
	public static class ContextExtensions
	{
		public static DbRepo<T> GetRepo<T>(this DbContext db) where T : class
		{
			string name = nameof(T);

			PropertyInfo property = db
				.GetType().GetProperties()
				.FirstOrDefault(p => p.PropertyType == typeof(DbSet<T>));
			
			if (property == default)
				throw new Exception($"A DbSet for {name} was not found in the DbContext");

			// Shouldn't ever happen
			if(!(property.GetValue(db) is DbSet<T> set))
				throw new Exception($"{name} is not a valid DbSet");

			return new DbRepo<T>(set, db);
		}
	}
}