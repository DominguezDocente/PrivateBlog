using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message)
        {
            
        }
    }
}
