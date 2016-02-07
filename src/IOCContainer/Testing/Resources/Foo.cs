using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.Testing.Resources
{
    public class Foo :IFoo
    {
        private static int _instanceCount;
        private readonly int _myInstaceId;
        public Foo()
        {
            _instanceCount++;
            _myInstaceId = _instanceCount;
        }
        public string GetFoo()
        {
            return "This is Foo " + _myInstaceId + " of " + _instanceCount;
        }
    }
}
