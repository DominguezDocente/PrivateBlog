using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Exceptions
{
    public class MediatorException : Exception
    {
        public MediatorException(string message) : base(message)
        {

        }
    }
}
