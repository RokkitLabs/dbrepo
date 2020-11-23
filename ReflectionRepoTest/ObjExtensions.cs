using System;
using System.Reflection;

namespace ReflectionRepoTest
{
	public static class ObjExxtensions
	{
		public static T ToType<T>(this object obj)
		{
			T tmp = Activator.CreateInstance<T>();
			foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
			{
				try
				{
					tmp.GetType().GetProperty(propertyInfo.Name)?.SetValue(tmp, propertyInfo.GetValue(obj, null));
				}
				catch (Exception e)
				{
				}
			}

			return tmp;
		}
	}
}