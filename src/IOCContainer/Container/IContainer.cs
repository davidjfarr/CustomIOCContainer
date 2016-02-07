using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.Container
{
    public interface IContainer
    {
        void Register<TContract, TImplementation>(Lifetime expectedLifetime) where TImplementation : class, TContract;
        TContract Resolve<TContract>();
    }
}
