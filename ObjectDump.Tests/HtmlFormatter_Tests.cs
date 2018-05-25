using MiP.ObjectDump;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ObjectDump.Tests
{
    public class HtmlFormatter_Tests
    {
        private readonly ITestOutputHelper _output;

        public HtmlFormatter_Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestMethod1()
        {
            var test = new
            {
                String = "This is a string",
                Number = 13,
                Date = DateTime.Now,
                SubObject = new  { Id = 29, Name = "Sub object 1" },
                Null = (string)null
            };

            JObject obj = JObject.FromObject(test);

            using (StringWriter writer = new StringWriter())
            {
                JObjectFormatter formatter = new JObjectFormatter(writer);
                formatter.Format(obj);

                string htmlTable = writer.ToString();
                _output.WriteLine(htmlTable);
            }
        }
    }
}
