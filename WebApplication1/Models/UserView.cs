using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class UserView<T>
    {
        public IEnumerable<T> Items { get; set; }
        public SelectList Types { get; set; }
        public string Name { get; set; }
    }
}
