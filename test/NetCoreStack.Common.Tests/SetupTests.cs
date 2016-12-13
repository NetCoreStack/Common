using NetCoreStack.Common.Tests.Types;
using System.Reflection;
using Xunit;

namespace NetCoreStack.Common.Tests
{
    public class SetupTests
    {
        [Fact]
        public void Setup_NetCoreStack()
        {
            var controller = typeof(LookupController);

            // Assert
            Assert.True(typeof(IApiContract).IsAssignableFrom(controller));
        }
    }
}
