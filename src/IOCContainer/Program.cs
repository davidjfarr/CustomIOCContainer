using System;
using IOCContainer.Container;
using IOCContainer.CustomException;
using IOCContainer.Testing.Resources;

namespace IOCContainer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var c = Builder.CreateContainer();
                c.Register<IFoo, Foo>(Lifetime.Instance);
                c.Register<IBar, Bar>(Lifetime.Singleton);

                Console.WriteLine(c.Resolve<IBar>().GetBar());
                Console.WriteLine(c.Resolve<IFoo>().GetFoo());
                Console.WriteLine(c.Resolve<IFoo>().GetFoo());
                Console.WriteLine(c.Resolve<IFoo>().GetFoo());
                Console.WriteLine(c.Resolve<IFoo>().GetFoo());
                Console.WriteLine(c.Resolve<IBar>().GetBar()); //Should be the same Bar and nested Foo as the first one but will show total foo count to have increased to 5

                c.Resolve<string>(); //should throw 
            }
            catch (Exception ex)
            {
                if (ex is NotRegisteredException)
                {
                    Console.WriteLine("Caught Exception Type: " + ex.GetType());
                    Console.WriteLine("Well done! We threw, as expected");
                }
                else if (ex is InvalidOperationException)
                {
                    Console.WriteLine("Caught Exception Type: " + ex.GetType());
                    Console.WriteLine(ex.Message);
                }
                else if (ex is ArgumentException)
                {
                    Console.WriteLine("Caught Exception Type: " + ex.GetType());
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
