using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.AspNetCore.Services
{
    public interface IHelloService
    {
        void Hello(string name);
        void Failed();
    }

    public class HelloService : IHelloService
    {
        public void Hello(string name)
        {
            Console.WriteLine($"Hello {name}");
        }

        public void Failed()
        {
            throw new ApplicationException("Error testing");
        }
    }
}
