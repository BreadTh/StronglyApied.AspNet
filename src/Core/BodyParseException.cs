using System;
using System.Collections.Generic;

namespace BreadTh.StronglyApied.AspNet.Core
{
    public class BodyParseException : Exception
    {
        public List<ErrorDescription> errors;

        public BodyParseException(List<ErrorDescription> errors)
        {
            this.errors = errors;
        }
    }
}
