using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.WebService
{
    public class WebServiceException : Exception
    {
        public WebServiceException(String message)
            : base(message)
        {

        }
    }
}
