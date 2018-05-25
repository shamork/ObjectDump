﻿using System;
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
                }
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
            string html = Dump.ToHtml(GetTestObject());

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_SimpleTypes()
        {
            string html = Dump.ToHtml(DayOfWeek.Friday);

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }

        [Fact]
        public void Dump_ListTypes()
        {
            string html = Dump.ToHtml(new[] { "One", "Two", "Three" });

            string filename = Guid.NewGuid() + ".html";
            File.WriteAllText(filename, html);
            Process.Start(filename);
        }
    }
}
