using System;
using System.Diagnostics;
using System.Reflection;

namespace Mammatus.Helpers
{
    public class StackTraceHelper
    {
        private const int stackOffSet = 1;

        public StackTraceHelper()
        {

        }

        public MethodBase GetCurrentMethod()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(stackOffSet).GetMethod();
        }

        public Type GetEnclosingType()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(stackOffSet).GetMethod().DeclaringType;
        }

        public MethodBase GetCallingMethod()
        {
            //-> Get current executing call stack.
            StackTrace stackTrace = new StackTrace();

            //-> Get calling method.
            return stackTrace.GetFrame(stackOffSet + 1).GetMethod();
        }

        public MethodBase GetMethodAt(int methodIndex)
        {
            methodIndex = stackOffSet + methodIndex;

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
            methodIndex = stackOffSet;

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
                methodIndex -= stackOffSet;
            else
                methodIndex = 0;

            return hasMethod;
        }
    }
}
