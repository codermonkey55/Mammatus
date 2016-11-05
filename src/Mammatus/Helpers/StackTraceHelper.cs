using System;
using System.Diagnostics;
using System.Reflection;

namespace Mammatus.Helpers
{
    public class StackTraceHelper
    {
        private const int Stack_OffSet = 1;

        private readonly int _stackOffSet;

        StackTraceHelper()
        {
            _stackOffSet = Stack_OffSet;
        }

        StackTraceHelper(int stackOffSet)
        {
            _stackOffSet = stackOffSet;
        }

        public MethodBase GetCurrentMethod()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(_stackOffSet).GetMethod();
        }

        public Type GetEnclosingType()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(_stackOffSet).GetMethod().DeclaringType;
        }

        public MethodBase GetCallingMethod()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(_stackOffSet + 1).GetMethod();
        }

        public MethodBase GetMethodAt(int methodIndex)
        {
            methodIndex = _stackOffSet + methodIndex;

            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            // Get calling method.
            return stackTrace.GetFrame(methodIndex).GetMethod();
        }

        public bool HasMethod(string methodName)
        {
            int methodIndex;

            return HasMethod(methodName, out methodIndex);
        }

        public bool HasMethod(string methodName, out int methodIndex)
        {
            methodIndex = _stackOffSet;

            var stackTrace = new StackTrace();
            var frameCount = stackTrace.FrameCount;
            var hasMethod = false;

            while (methodIndex < frameCount)
            {
                hasMethod = stackTrace.GetFrame(methodIndex)
                                      .GetMethod()
                                      .Name.Equals(methodName, StringComparison.CurrentCultureIgnoreCase);

                if (hasMethod)
                    break;
                else
                    methodIndex += 1;
            }

            if (hasMethod)
                methodIndex -= _stackOffSet;
            else
                methodIndex = 0;

            return hasMethod;
        }

        public static StackTraceHelper Create()
        {
            return new StackTraceHelper();
        }

        public static StackTraceHelper Create(int stackOffSet)
        {
            return new StackTraceHelper(stackOffSet);
        }
    }
}
