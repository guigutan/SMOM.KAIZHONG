using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Items.Items
{
    public class ItemControllerTest :  IClassFixture<TestStarup>
    { 
        [Fact]
        public void Method1()
        {
            RT.Service.Resolve<ContextControllerTest>().InitContext();
        }
        [Fact]
        public void Method1w()
        {
            RT.Service.Resolve<ContextControllerTest>().InitContext(); 
        }
        [Fact]
        public void Method1e()
        {
            RT.Service.Resolve<ContextControllerTest>().InitContext(); 
        } 
    } 
}