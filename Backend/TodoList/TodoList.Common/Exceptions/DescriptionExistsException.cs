using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Common.Exceptions
{
    public class DescriptionExistsException : Exception
    {
        public DescriptionExistsException()
            : base()
        {
        }

        public DescriptionExistsException(string description)
            : base($"Description \"{description}\" already exists.")
        {
        }

        public DescriptionExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
