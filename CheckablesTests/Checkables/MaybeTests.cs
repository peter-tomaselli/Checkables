using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Checkables.Tests
{
    [TestClass]
    public class MaybeTests
    {
        [TestMethod]
        public void Maybe_test_false_cases_for_HasValue()
        {
            Assert.IsFalse(new Maybe<string>().HasValue);
            Assert.IsFalse(new Maybe<string>(null).HasValue);
        }

        [TestMethod]
        [ExpectedException(typeof(CheckableException))]
        public void Maybe_test_throw_case_for_Value_default_constructor()
        {
            string foo = new Maybe<string>().Value;
        }

        [TestMethod]
        [ExpectedException(typeof(CheckableException))]
        public void Maybe_test_throw_case_for_Value_null_value_to_constructor()
        {
            string foo = new Maybe<string>(null).Value;
        }

        [TestMethod]
        public void Maybe_test_true_case_for_HasValue()
        {
            Assert.IsTrue(new Maybe<string>("foo").HasValue);
        }

        [TestMethod]
        public void Maybe_test_success_case_for_Value()
        {
            string foo = "foo";
            Maybe<string> maybe = new Maybe<string>(foo);
            Assert.AreEqual(foo, maybe.Value);
        }
    }

    [TestClass]
    public class MaybeExtensionsTests
    {
        [TestMethod]
        public void Maybe_test_FlatMap_short_circuits()
        {
            Func<string, Maybe<string>> explode = s =>
            {
                throw new InvalidOperationException();
            };
            Maybe<string> safe = new Maybe<string>().FlatMap(explode);
        }

        [TestMethod]
        public void Maybe_test_Map_short_circuits()
        {
            Func<string, string> explode = s =>
            {
                throw new InvalidOperationException();
            };
            Maybe<string> safe = new Maybe<string>().Map(explode);
        }

        [TestMethod]
        public void Maybe_test_FlatMap_succeeds()
        {
            Func<string, Maybe<string>> maybeCaps = s => new Maybe<string>(s == "foo" ? s.ToUpper() : null);
            Maybe<string> maybeFooCaps = new Maybe<string>("foo").FlatMap(maybeCaps);
            Assert.IsTrue(maybeFooCaps.HasValue);
            Assert.AreEqual(maybeFooCaps.Value, "FOO");

            Maybe<string> maybeBarCaps = new Maybe<string>("bar").FlatMap(maybeCaps);
            Assert.IsFalse(maybeBarCaps.HasValue);
        }

        [TestMethod]
        public void Maybe_test_Map_succeeds()
        {
            Maybe<string> fooCaps = new Maybe<string>("foo").Map(s => s.ToUpper());
            Assert.IsTrue(fooCaps.HasValue);
            Assert.AreEqual(fooCaps.Value, "FOO");
        }
    }
}
