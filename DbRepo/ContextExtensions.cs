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
		private static PropertyInfo? GetPropFromType<T>(object search) where T : class => search.GetType().GetProperties().First(p => p.PropertyType == typeof(T));

		public static DbRepo<T> GetRepo<T>(this DbContext db) where T : class
		{
			string name = nameof(T);

			PropertyInfo? property = db
				.GetType().GetProperties()
				//Attempt if pluralised exists
				.First(p => (p.Name == name || p.Name == name + "s") && p.PropertyType == typeof(T))
				//Loop as a last resort, to avoid performance
				?? GetPropFromType<DbSet<T>>(db);
			
			if (property == null)
				throw new Exception($"A DbSet for {name} was not found in the DbContext");

			if(!(property.GetValue(db) is DbSet<T> set))
				throw new Exception($"{name} is not a valid DbSet");

			return new DbRepo<T>(set, db);
		}
	}
}