using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DbRepo
{
	public static class ContextExtensions
	{
		private static PropertyInfo? GetPropFromType<T>(object search) where T : class
		{ 
			foreach (PropertyInfo info in search.GetType().GetProperties())
			{
				if (info.PropertyType == typeof(T))
				{
					return info;
				}
			}
			return null;
		}
		public static DbRepo.DbRepo<T> GetRepo<T>(this DbContext db) where T : class
		{
			string name = typeof(T).Name;
		
			PropertyInfo? property = db.GetType().GetProperty(name);
			if (property == null)
			{
				//Attempt if pluralised exists
				property = db.GetType().GetProperty(name + "s");
				//Loop as a last resort, to avoid performance
				property = GetPropFromType<DbSet<T>>(db);
				if (property == null)
					throw new Exception($"A DbSet for {name} does not exist in the DbContext");
			}
			DbSet<T>? set = property.GetValue(db) as DbSet<T>;
			if (set == null)
			{
				property = GetPropFromType<DbSet<T>>(db);
				if(property == null)
					throw new Exception($"A DbSet for {name} does not exist in the DbContext");

				set = property.GetValue(db) as DbSet<T>;
				if (set == null)
					throw new Exception($"{name} is not a valid DbSet");
			}
			return new DbRepo<T>(set, db);
		}
	}
}
