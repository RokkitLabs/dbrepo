using System;
using System.Reflection;

namespace ReflectionRepoTest
{
	public class DbRepo<T>
	{
		public T Test(T val)
		{
			return val;
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