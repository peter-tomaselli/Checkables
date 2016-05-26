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
}
