using System;
using System.Collections.Generic;
using System.Text;

namespace DbRepo.Exceptions
{
	public class RepoInstantiationException : Exception
	{
		public RepoInstantiationException()
		{
		}
		public RepoInstantiationException(string errorMessage) : base(errorMessage)
		{
		}
		public RepoInstantiationException(Exception ex) : base("", ex)
		{
		}
	}
}
