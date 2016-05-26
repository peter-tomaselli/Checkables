using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Checkables.Tests
{
    [TestClass]
    public class ICheckableExtensionsActionsTests
    {
        [TestMethod]
        public void Checkable_Do_arity_one_short_circuits()
        {
            Action<string> explode = s =>
            {
                throw new InvalidOperationException();
            };
            new Maybe<string>(null).Do(explode);
        }

        [TestMethod]
        public void Checkable_Do_arity_one_succeeds()
        {
            string fooCaps = null;
            new Maybe<string>("foo").Do(s =>
            {
                fooCaps = s.ToUpper();
            });
            Assert.AreEqual(fooCaps, "FOO");
        }

        [TestMethod]
        public void Checkable_Do_arity_two_short_circuits()
        {
            Action<string, string> explode = (s1, s2) =>
            {
                throw new InvalidOperationException();
            };
            new Maybe<string>(null).Do(new Maybe<string>(null), explode);
            new Maybe<string>("foo").Do(new Maybe<string>(null), explode);
            new Maybe<string>(null).Do(new Maybe<string>("foo"), explode);
        }

        [TestMethod]
        public void Checkable_Do_arity_two_succeeds()
        {
            string fooBarCaps = null;
            new Maybe<string>("foo").Do(new Maybe<string>("bar"), (f, b) =>
            {
                fooBarCaps = f.ToUpper() + b.ToUpper();
            });
            Assert.AreEqual(fooBarCaps, "FOOBAR");
        }
    }
}
