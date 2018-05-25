using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace MiP.ObjectDump.Tests
{
    public class Dump_Tests
    {
        [Fact]
        public void Dump_Simple()
        {
            var test = new
            {
                String = "This is a string",
                Number = 13,
                Date = DateTime.Now,
                SubObject = new { Id = 29, Name = "Sub object 1" },
                Null = (string)null,
                Empty = "",
                Strings = new[] { "One", "Two", "Three" },
                List = new List<string> { "Four", "Five", "Six" },

                ComplexArray = new object[]
                {
                    new{Name="Ich", Id=1},
                    "asoisdas",
                    4,
                    new{Name="Du", Id=2},
                    new{Name="Er", Id=3},
                    new{Bla="Blubb"},
                    new[]{4,5,6},
                    new List<string> { "Seven","Eight", "between Eight and Nine", "Nine" }
                }
            };

            string html = Dump.ToHtml(test);

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }
    }
}
