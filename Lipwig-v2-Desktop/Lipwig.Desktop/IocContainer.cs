using Ninject;

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
