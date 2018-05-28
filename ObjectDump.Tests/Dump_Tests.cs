using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace MiP.ObjectDump.Tests
{
    public class Dump_Tests
    {
        private object GetTestObject()
        {
            // initializ a thrown exception
            Exception ex = new InvalidOperationException("Wow bad idea.");
            try
            {
                throw ex;
            }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
            catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
            {
            }

            return new
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
                    new[]{4,5,6},
                    new{Name="Du", Id=2},
                    new{Name="Er", Id=3},
                    new{Name="Someone", Bla="Blubber"},
                    new{Bla="Blubb"},

                    new List<string> { "Seven","Eight", "between Eight and Nine", "Nine" }
                },

               // Exception = ex,
            };
        }

        [Fact]
        public void Dump_ViaJObject_Simple()
        {
            string html = Dump.ViaJObject(GetTestObject());

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_ViaReflection_Simple()
        {
            string html = Dump.ViaReflection(GetTestObject());

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_SimpleTypes()
        {
            string html = Dump.ViaReflection(DayOfWeek.Friday);

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_ListTypes()
        {
            string html = Dump.ViaReflection(new[] { "One", "Two", "Three" });

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_Exception()
        {
            Exception ex = new InvalidOperationException("Wow bad idea.");
            try
            {
                throw ex;
            }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
            catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
            {
            }

            string html = Dump.ViaReflection(ex);

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_Type()
        {
            string html = Dump.ViaReflection(typeof(int));

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }
    }
}
