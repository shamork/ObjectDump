using System;
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
                Null = (string)null
            };

            string html = Dump.ToHtml(test);

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }
    }
}
