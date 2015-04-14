﻿using System;
using System.Linq.Expressions;

namespace Telerik.JustMock.Core.Context
{
	internal static class LocalMockingContextResolver
	{
		private static MocksRepository repository;

		public static MocksRepository ResolveRepository(UnresolvedContextBehavior unresolvedContextBehavior)
		{
			if (repository == null)
			{
				repository = new MocksRepository(null, null);
			}
			return repository;
		}

		public static bool RetireRepository()
		{
			return false;
		}

		public static Action<string, Exception> GetFailMethod()
		{
			return GetFailMethod(typeof(MockAssertionFailedException));
		}

		public static Action<string, Exception> GetFailMethod(Type assertFailedExceptionType)
		{
			var messageParam = Expression.Parameter(typeof(string));
			var innerExceptionParam = Expression.Parameter(typeof(Exception));
			var ctor = assertFailedExceptionType.GetConstructor(new[] { typeof(string), typeof(Exception) });
			return (Action<string, Exception>)Expression.Lambda(Expression.Throw(Expression.New(ctor, messageParam, innerExceptionParam)),
				messageParam, innerExceptionParam).Compile();
		}
	}
}
