using System.Reflection;

namespace DotNet.Cookbook.Caching.DynamicProxy
{
    internal class MethodExecutionContext
    {
        private readonly object _target;
        private readonly MethodInfo _info;

        public MethodExecutionContext(object target, MethodInfo info)
        {
            _target = target;
            _info = info;
        }

        public object Invoke(params object[] arguments)
        {
            return _info.Invoke(_target, arguments);
        }
    }
}
