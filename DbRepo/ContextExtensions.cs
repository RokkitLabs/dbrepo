using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DbRepo
{
	public static class ContextExtensions
	{
		public static DbRepo.DbRepo<T> GetRepo<T>(this DbContext db) where T : class
		{
			string name = typeof(T).Name;
		
			PropertyInfo? property = db.GetType().GetProperty(name);
			if (property == null)
			{
				//Attempt if pluralised exists
				property = db.GetType().GetProperty(name + "s");
				//Loop as a last resort, to avoid performance
				foreach (PropertyInfo info in db.GetType().GetProperties())
				{
					if (info.PropertyType == typeof(DbSet<T>))
					{
						property = info;
						break;
					}
				}
				if (property == null)
					throw new Exception($"A DbSet for {name} does not exist in the DbContext");
			}

			DbSet<T>? set = property.GetValue(db) as DbSet<T>;
			if (set == null)
			{
				
				throw new Exception($"{name} is not a valid DbSet");
			}


			return new DbRepo<T>(set, db);
		}
	}
}
