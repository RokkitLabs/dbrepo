using System;
using System.Collections.Generic;
using System.Text;

namespace DbRepo.Exceptions
{
	public class ExpressionBuilderException : Exception
	{
		public ExpressionBuilderException()
		{
		}

		public ExpressionBuilderException(Exception ex) : base("", ex)
		{
		}
	}
}
