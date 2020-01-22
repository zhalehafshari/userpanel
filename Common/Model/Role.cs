using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
    public class Role
    {
        public string Id { get; set; }
        public ICollection<User> users { get; set; }
    }
}
