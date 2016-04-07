using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Checkables.Tests
{
    [TestClass]
    public class GuardedTests
    {
        [TestMethod]
        public void Guarded_test_false_case_for_nocheck_HasValue()
        {
            Func<string> nullString = () => (string)null;
            Assert.IsFalse(new Guarded<string>(nullString).HasValue);
        }

        [TestMethod]
        public void Guarded_test_false_cases_for_check_HasValue()
        {
            Func<string> fooString = () => "foo";
            Func<bool> failCheck = () => false;
            Assert.IsFalse(new Guarded<string>(fooString, failCheck).HasValue);

            Func<string> nullString = () => (string)null;
            Func<bool> passCheck = () => true;
            Assert.IsFalse(new Guarded<string>(nullString, passCheck).HasValue);
        }

        [TestMethod]
        public void Guarded_test_true_case_for_nocheck_HasValue()
        {
            Func<string> fooString = () => "foo";
            Assert.IsTrue(new Guarded<string>(fooString).HasValue);
        }

        [TestMethod]
        public void Guarded_test_true_case_for_check_HasValue()
        {
            Func<string> fooString = () => "foo";
            Func<bool> passCheck = () => true;
            Assert.IsTrue(new Guarded<string>(fooString, passCheck).HasValue);
        }

        [TestMethod]
        public void Guarded_test_check_is_evaluated_by_reference()
        {
            bool pass = true;

            Func<string> fooString = () => "foo";
            Func<bool> doCheck = () => pass;

            Guarded<string> guarded = new Guarded<string>(fooString, doCheck);
            Assert.IsTrue(guarded.HasValue);

            pass = false;
            Assert.IsFalse(guarded.HasValue);
        }

        [TestMethod]
        [ExpectedException(typeof(CheckableException))]
        public void Guarded_test_nocheck_Value_throws()
        {
            Func<string> nullString = () => (string)null;
            string foo = new Guarded<string>(nullString).Value; // throw
        }

        [TestMethod]
        [ExpectedException(typeof(CheckableException))]
        public void Guarded_test_check_Value_throws1()
        {
            Func<string> fooString = () => "foo";
            Func<bool> failCheck = () => false;
            string foo = new Guarded<string>(fooString, failCheck).Value; // throw
        }

        [TestMethod]
        [ExpectedException(typeof(CheckableException))]
        public void Guarded_test_check_Value_throws2()
        {
            Func<string> nullString = () => (string)null;
            Func<bool> passCheck = () => true;
            string foo = new Guarded<String>(nullString, passCheck).Value; // throw
        }

        [TestMethod]
        public void Guarded_test_nocheck_Value_succeeds()
        {
            Func<string> fooString = () => "foo";
            Assert.AreEqual("foo", new Guarded<string>(fooString).Value);
        }

        [TestMethod]
        public void Guarded_test_check_Value_succeeds()
        {
            Func<string> fooString = () => "foo";
            Func<bool> passCheck = () => true;
            Assert.AreEqual("foo", new Guarded<string>(fooString, passCheck).Value);
        }
    }
}
