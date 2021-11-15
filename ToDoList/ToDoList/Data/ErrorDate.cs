using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Data
{
    public class ErrorDate: Exception
    {
        public ErrorDate() 
        {
        }

        public ErrorDate (string message) : base(message)
        {

        }

        public ErrorDate(string message,Exception innerException): base(message, innerException)
        {

        }
    }
}
