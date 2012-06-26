using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Core.RavenDBMembership
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Enum)]
    public class SRValueAttribute : Attribute
    {
        public string Value;

        public SRValueAttribute(string Value)
        {
            this.Value = Value;
        }
    }
}
