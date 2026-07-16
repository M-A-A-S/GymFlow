using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schoolify.Common.Utilities
{
    public class MenuItem
    {
        public string Key { get; set; } = "";
        public string Icon { get; set; } = "";

        public string? Controller { get; set; }
        public string? Action { get; set; }

        public List<MenuItem> Children { get; set; } = new();

        public bool HasChildren => Children.Any();
    }
}
