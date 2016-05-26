using Checkables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CheckablesTests.Checkables
{
    [TestClass]
    public class ICheckableExtensionsChainingTests
    {
        private static IEnumerable<ICheckable<string>> _badCheckables
        {
            get
            {
                yield return new Maybe<string>();
                yield return new Maybe<string>((string)null);
                yield return new Guarded<string>(() => (string)null);
                yield return new Guarded<string>(() => "foo", () => false);
            }
        }

        private static IEnumerable<ICheckable<string>> _goodCheckables
        {
            get
            {
                yield return new Maybe<string>("foo");
                yield return new Guarded<string>(() => "foo");
                yield return new Guarded<string>(() => "foo", () => true);
            }
        }

        private static readonly Func<string, string> _explode = s =>
        {
            throw new InvalidOperationException();
        };

        private static readonly Func<string, Maybe<string>> _flatExplode = s =>
        {
            throw new InvalidOperationException();
        };

        private static readonly Func<string, string> _upper = s => s.ToUpper();

        private static readonly Func<string, Maybe<string>> _upperOnlyFoo = s => new Maybe<string>(s == "foo" ? s.ToUpper() : null);

        [TestMethod]
        public void Checkable_test_FlatMap_short_circuits()
        {
            Maybe<string> safe;
            foreach (ICheckable<string> checkable in _badCheckables)
            {
                safe = checkable.FlatMap(_flatExplode);
                Assert.IsFalse(safe.HasValue);
            }
        }

        [TestMethod]
        public void Checkable_test_Map_short_circuits()
        {
            Maybe<string> safe;
            foreach (ICheckable<string> checkable in _badCheckables)
            {
                safe = checkable.Map(_explode);
                Assert.IsFalse(safe.HasValue);
            }
        }

        [TestMethod]
        public void Maybe_test_FlatMap_succeeds()
        {
            Maybe<string> maybeFooUpper;
            foreach (ICheckable<string> checkable in _goodCheckables)
            {
                maybeFooUpper = checkable.FlatMap(_upperOnlyFoo);
                Assert.IsTrue(maybeFooUpper.HasValue);
                Assert.AreEqual(maybeFooUpper.Value, "FOO");
            }

            Maybe<string> maybeBarUpper = new Maybe<string>("bar").FlatMap(_upperOnlyFoo);
            Assert.IsFalse(maybeBarUpper.HasValue);
        }

        [TestMethod]
        public void Maybe_test_Map_succeeds()
        {
            Maybe<string> fooUpper;
            foreach (ICheckable<string> checkable in _goodCheckables)
            {
                fooUpper = checkable.Map(_upper);
                Assert.IsTrue(fooUpper.HasValue);
                Assert.AreEqual(fooUpper.Value, "FOO");
            }
        }
    }
}
