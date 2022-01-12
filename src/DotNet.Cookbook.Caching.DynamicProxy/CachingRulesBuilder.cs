using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNet.Cookbook.Caching.DynamicProxy
{
    public class CachingRulesBuilder<T>
    {
        internal readonly IDictionary<MethodInfo, MethodExecutionContext> Rules = new Dictionary<MethodInfo, MethodExecutionContext>();

        public CachingRulesBuilder<T> ForMethod<T0, T1, T2, T3, T4>(
            Expression<Func<T, Func<T0, T1, T2, T3, T4>>> methodFunc,
            Func<T0, T1, T2, T3, string> keyFunc)
        {
            Add(methodFunc.Body, keyFunc.Target, keyFunc.Method);

            return this;
        }

        public CachingRulesBuilder<T> ForMethod<T0, T1, T2, T3>(
            Expression<Func<T, Func<T0, T1, T2, T3>>> methodFunc,
            Func<T0, T1, T2, string> keyFunc)
        {
            Add(methodFunc.Body, keyFunc.Target, keyFunc.Method);

            return this;
        }

        public CachingRulesBuilder<T> ForMethod<T0, T1, T2>(
            Expression<Func<T, Func<T0, T1, T2>>> methodFunc,
            Func<T0, T1, string> keyFunc)
        {
            Add(methodFunc.Body, keyFunc.Target, keyFunc.Method);

            return this;
        }

        public CachingRulesBuilder<T> ForMethod<T0, T1>(
            Expression<Func<T, Func<T0, T1>>> methodFunc,
            Func<T0, T1, string> keyFunc)
        {
            Add(methodFunc.Body, keyFunc.Target, keyFunc.Method);

            return this;
        }

        private void Add(Expression expression, object keyFuncTarget, MethodInfo keyFuncInfo)
        {
            var unaryExpression = (UnaryExpression)expression;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var constantExpression = (ConstantExpression)methodCallExpression.Object;
            var methodInfo = (MethodInfo)constantExpression.Value;

            Rules.Add(methodInfo, new MethodExecutionContext(keyFuncTarget, keyFuncInfo));
        }
    }
}
