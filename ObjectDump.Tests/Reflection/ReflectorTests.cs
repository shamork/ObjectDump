using FluentAssertions;
using MiP.ObjectDump.Reflection;
using System;
using Xunit;

namespace MiP.ObjectDump.Tests.Reflection
{
    public class ReflectorTests
    {
        private Reflector _reflector;

        public ReflectorTests()
        {
            _reflector = new Reflector();
        }

        [Fact]
        public void CreatesNull()
        {
            var result = _reflector.GetDObject(null);

            result.Should().Be(DObject.Null);
        }

        [Fact]
        public void Creates_value_for_string()
        {
            var result = _reflector.GetDObject("Hello");

            result.Should().BeEquivalentTo(new DValue("Hello"));
        }

        [Fact]
        public void Creates_value_for_int()
        {
            var result = _reflector.GetDObject(17);

            result.Should().BeEquivalentTo(new DValue("17"));
        }

        [Fact]
        public void Creates_value_for_bool()
        {
            var result = _reflector.GetDObject(true);

            result.Should().BeEquivalentTo(new DValue("True"));
        }

        [Fact]
        public void Creates_array_of_ints()
        {
            var result = _reflector.GetDObject(new int[] { 1, 2, 3 });

            DArray expected = new DArray
            {
                TypeHeader = "Int32[] (3 items)"
            };

            expected.Add(new DValue("1"));
            expected.Add(new DValue("2"));
            expected.Add(new DValue("3"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Creates_array_of_strings()
        {
            var result = _reflector.GetDObject(new string[] { "One", "Two", "Three" });

            DArray expected = new DArray
            {
                TypeHeader = "String[] (3 items)"
            };

            expected.Add(new DValue("One"));
            expected.Add(new DValue("Two"));
            expected.Add(new DValue("Three"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Creates_complex_with_properties()
        {
            var result = _reflector.GetDObject(new Complex1 { Name_f = "Hello", Name_p = "World", Number_f = 17, Number_p = 42 });

            DComplex expected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex1", null);
            expected.AddProperty(nameof(Complex1.Name_f), new DValue("Hello"));  // fields will always be before properties
            expected.AddProperty(nameof(Complex1.Number_f), new DValue("17"));
            expected.AddProperty(nameof(Complex1.Name_p), new DValue("World"));
            expected.AddProperty(nameof(Complex1.Number_p), new DValue("42"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Creates_array_of_complex()
        {
            Complex2[] complexes =
            {
                new Complex2{N1="Hello", N2="World"},
                new Complex2{N1="Hallo", N2="Welt"},
            };

            var result = _reflector.GetDObject(complexes);

            var expected = new DArray();
            var dcomplex1 = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex2", null);
            var dcomplex2 = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex2", null);

            dcomplex1.AddProperty("N1", new DValue("Hello"));
            dcomplex1.AddProperty("N2", new DValue("World"));
            dcomplex2.AddProperty("N1", new DValue("Hallo"));
            dcomplex2.AddProperty("N2", new DValue("Welt"));

            expected.Add(dcomplex1);
            expected.Add(dcomplex2);

            expected.TypeHeader = "Complex2[] (2 items)";
            expected.AddColumns(new[] { "N1", "N2" });

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Fact]
        public void Creates_complex_and_catches_exceptions_when_reading_properties()
        {
            var result = _reflector.GetDObject(new Complex3());

            var expected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex3", null);
            expected.AddProperty("Throws", new DError("This is expected"));
            expected.AddProperty("Name", new DValue("Hello"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        private class Complex1
        {
            public string Name_f;
            public string Name_p { get; set; }

            public int Number_f;
            public int Number_p { get; set; }
        }

        private class Complex2
        {
            public string N1;
            public string N2;
        }

        public class Complex3
        {
            public string Throws => throw new InvalidOperationException("This is expected!");
            public string Name => "Hello";
        }
    }
}
