using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.Testing.Resources
{
    public class Bar : IBar
    {
        private static int _instanceCount;
        private readonly int _myInstaceId;
        private readonly IFoo _foo;
        public Bar(IFoo foo)
        {
            _instanceCount++;
            _myInstaceId = _instanceCount;
            _foo = foo;
        }
        public string GetBar()
        {
            return "This is Bar " + _myInstaceId + " of " + _instanceCount + ", and I have a Foo : " + _foo.GetFoo();
        }
    }
}
