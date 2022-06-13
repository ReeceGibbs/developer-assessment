using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Common.Models.Common
{
    public class ResponseObject<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
