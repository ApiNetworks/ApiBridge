using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Bus.Core
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Execute and catch any exception that is thrown and swallow it.
        /// </summary>
        /// <param name="action">The action to perform</param>
        public static void ExecuteAndReturn(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                
            }
        }

    }
}
