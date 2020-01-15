using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Models
{
    public class School
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
