using System;
using System.Collections.Generic;
using System.Text;

namespace DbRepo.Attributes
{
	public class RepoColumnName : Attribute
	{
		public string Name;
		public RepoColumnName(string _Name)
		{
			Name = _Name;
		}
	}
}
