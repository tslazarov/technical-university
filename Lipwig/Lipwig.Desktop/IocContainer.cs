using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop
{
    public static class IocContainer
    {
        private static IKernel kernel;

        public static IKernel Kernel
        {
            get
            {
                if(kernel == null)
                {
                    kernel = new StandardKernel();
                }

                return kernel;
            }
        }
    }
}
