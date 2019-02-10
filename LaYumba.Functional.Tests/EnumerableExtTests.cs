using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using static LaYumba.Functional.F;

namespace LaYumba.Functional.Tests
{
    public class EnumerableExtTests
    {
        [Property]
        public void Find_AnyInNonEmptyStrings_First(NonEmptyArray<NonNull<string>> list)
            => Assert.Equal(Some(list.Get[0].Get), list.Get.Map(x => x.Get).Find(_ => true));

        [Property]
        public void Find_AnyInNonEmptyDecimals_First(NonEmptyArray<decimal> list)
            => Assert.Equal(Some(list.Get[0]), list.Get.Find(_ => true));

        [Fact]
        public void Find_EmptyStrings_None()
            => Assert.Equal(None, Enumerable.Empty<string>().Find(_ => true));

        [Fact]
        public void Find_EmptyDecimals_None()
            => Assert.Equal(None, Enumerable.Empty<decimal>().Find(_ => true));
    }
}