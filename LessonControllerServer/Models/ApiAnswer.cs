using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessonControllerServer.Models
{
    public class ApiAnswer<T>
    {
        public T Data { get; set; }
        public string Error { get; set; }
        public bool Success => string.IsNullOrEmpty(Error) &&
            (Data.GetType() == typeof(bool) && Data.ToString() == "True");
    }
}
