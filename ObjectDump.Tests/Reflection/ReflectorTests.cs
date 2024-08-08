using FluentAssertions;
using MiP.ObjectDump.Reflection;
using System;
using System.Collections.Generic;
using FluentAssertions.Equivalency;

using Xunit;

namespace MiP.ObjectDump.Tests.Reflection
{
    public class ReflectorTests
    {
        private const string Check_if_its_a_Guid = "check if its a guid";
        private Reflector _reflector;

        public ReflectorTests()
        {
            _reflector = new Reflector();
        }

        [Fact]
        public void CreatesNull()
        {
            var result = _reflector.GetDObject(null, 5);

            result.Should().Be(DObject.Null);
        }

        [Fact]
        public void Creates_value_for_string()
        {
            var result = _reflector.GetDObject("Hello", 5);

            result.Should().BeEquivalentTo(new DValue("Hello"), o => o.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_value_for_int()
        {
            var result = _reflector.GetDObject(17, 5);

            result.Should().BeEquivalentTo(new DValue("17",typeof(int)), o => o.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_value_for_bool()
        {
            var result = _reflector.GetDObject(true, 5);

            result.Should().BeEquivalentTo(new DValue("True",typeof(bool)), o => o.IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_array_of_ints()
        {
            var result = _reflector.GetDObject(new int[] { 1, 2, 3 }, 5);

            DArray expected = new DArray
            {
                TypeHeader = "Int32[] (3 items)"
            };

            expected.Add(new DValue("1",typeof(int)));
            expected.Add(new DValue("2",typeof(int)));
            expected.Add(new DValue("3",typeof(int)));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_list_of_ints()
        {
            var result = _reflector.GetDObject(new List<int>() { 1, 2, 3 }, 5);

            DArray expected = new DArray
            {
                TypeHeader = "List<int> (3 items)"
            };

            expected.Add(new DValue("1",typeof(int)));
            expected.Add(new DValue("2",typeof(int)));
            expected.Add(new DValue("3",typeof(int)));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties());
        }
        [Fact]
        public void Creates_array_of_strings()
        {
            var result = _reflector.GetDObject(new string[] { "One", "Two", "Three" }, 5);

            DArray expected = new DArray
            {
                TypeHeader = "String[] (3 items)"
            };

            expected.Add(new DValue("One"));
            expected.Add(new DValue("Two"));
            expected.Add(new DValue("Three"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_complex_with_properties()
        {
            var result = _reflector.GetDObject(new Complex1 { Name_f = "Hello", Name_p = "World", Number_f = 17, Number_p = 42 }, 5);

            DComplex expected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex1", null);
            expected.AddProperty(nameof(Complex1.Name_f), new DValue("Hello"));  // fields will always be before properties
            expected.AddProperty(nameof(Complex1.Number_f), new DValue("17",typeof(int)));
            expected.AddProperty(nameof(Complex1.Name_p), new DValue("World"));
            expected.AddProperty(nameof(Complex1.Number_p), new DValue("42",typeof(int)));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_array_of_complex()
        {
            Complex2[] complexes =
            {
                new Complex2{N1="Hello", N2="World"},
                new Complex2{N1="Hallo", N2="Welt"},
            };

            var result = _reflector.GetDObject(complexes, 5);

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

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties());
        }

        [Fact]
        public void Creates_complex_and_catches_exceptions_when_reading_properties()
        {
            var result = _reflector.GetDObject(new Complex3(), 5);

            var expected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex3", null);

            string systemInvalidoperationexceptionThisIsExpected = "System.InvalidOperationException: This is expected!*";

            expected.AddProperty("Throws", new DError(systemInvalidoperationexceptionThisIsExpected));
            expected.AddProperty("Name", new DValue("Hello"));

            result.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering().IncludingAllRuntimeProperties()
                                                         // only match first part of the exception string
                                                         .Using<string>(a => StringMatching(a, systemInvalidoperationexceptionThisIsExpected))
                                                         .WhenTypeIs<string>()
            );
        }

        [Fact]
        public void Creates_complex_with_cyclic_typing()
        {
            var test = new Complex4 {Five = new Complex5 {Four = new Complex4 {Five = new Complex5()}}};

            var result = _reflector.GetDObject(test, 5);

            // TODO: assert
        }

        [Fact]
        public void Creates_complex_with_cyclic_references()
        {
            var four = new Complex4();
            var five = new Complex5 {Four = four};
            four.Five = five;

            var result = _reflector.GetDObject(four, 5);

            var expected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex4", null);
            var fiveExpected = new DComplex("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex5", null);
            expected.AddProperty("Five", fiveExpected);
            fiveExpected.AddProperty("Four", new CyclicReference("MiP.ObjectDump.Tests.Reflection.ReflectorTests+Complex4", Check_if_its_a_Guid));

            result.Should().BeEquivalentTo(expected, o => o.IncludingAllRuntimeProperties()
                                                         .Using<string>(IsGuid)
                                                         .When(m => m.RuntimeType == typeof(string)
                                                                    &&
                                                                    m.SelectedMemberInfo.Name == nameof(CyclicReference.Reference)
                                                                    &&
                                                                    m.SelectedMemberInfo.DeclaringType == typeof(CyclicReference))
            );
        }

        private void IsGuid(IAssertionContext<string> a)
        {
            if (a.Expectation == null)
            {
                a.Subject.Should().BeNull();
            }
            else
            {
                if (a.Expectation == Check_if_its_a_Guid)
                {

                    Guid guid = Guid.Parse(a.Subject);
                    guid.Should().NotBe(Guid.Empty);
                }
                else
                {
                    a.Subject.Should().Be(a.Expectation);
                }
            }
        }

        private void StringMatching(IAssertionContext<string> a, string expectationReference)
        {
            if (a.Expectation == null)
            {
                a.Subject.Should().BeNull();
            }
            else
            {
                if (ReferenceEquals(a.Expectation, expectationReference))
                    a.Subject.Should().Match(a.Expectation);
                else
                    a.Subject.Should().Be(a.Expectation);
            }
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

        public class Complex4
        {
            public Complex5 Five;
        }

        public class Complex5
        {
            public Complex4 Four;
        }
    }
}
