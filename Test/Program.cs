using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionBuilder;
using ExpressionBuilder.Attributes;
using ExpressionBuilder.Enums;
namespace Test
{
   
    class Program
    {

        static void Main(string[] args)
        {
            var test = new List<Person>()
            {
                new Person() { Name = "Andreas",Age=18, Surname= "Mitrou" },
                new Person() { Name = "Andreas",Age=18, Surname= "Georgiou" },
                new Person() { Name = "Mistos", Age=19, Surname= "Mistos" },
                new Person() { Name = "Giorgos",Age=18, Surname= "Giorgos" },
            };

          var results =  test.Search(18, mode: SearchOptions.Equal);

          var results2 = test.Search("giorgos", SearchOptions.Equal, new string[] { "Name", "Surname"});

        }
    }
}
