using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.Container
{
    public class Builder
    {
        public static IContainer CreateContainer()
        {
            return new Container();
        }
    }
}
