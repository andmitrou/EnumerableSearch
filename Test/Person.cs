using ExpressionBuilder.Attributes;
using ExpressionBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Person
    {
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string Surname { get; set; }
        [Searchable]
        public int Age { get; set; }

    }
}
