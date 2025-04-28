using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static TService GetService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }
    }
}
