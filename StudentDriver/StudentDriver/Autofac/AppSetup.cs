using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using OAuth;
using StudentDriver.Services;

namespace StudentDriver.Autofac
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var cb = new ContainerBuilder();
            RegisterDependencies(cb);
            return cb.Build();
        }

        private void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<OAuthController>().As<IOAuthController>().SingleInstance();
            cb.RegisterType<DatabaseController>().As<IDatabaseController>().SingleInstance();
            cb.RegisterType<ServiceController>()
              .As<IServiceController>()
              .WithParameter(
                  (pi, ctx) => pi.ParameterType == typeof(IOAuthController),
                  (pi, ctx) => ctx.Resolve<IOAuthController>())
              .WithParameter(
                (pi,ctx) => pi.ParameterType == typeof(IDatabaseController),
                (pi,ctx) => ctx.Resolve<IDatabaseController>());
        }
    }
}
