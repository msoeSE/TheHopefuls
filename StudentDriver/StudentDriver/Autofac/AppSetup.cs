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
        private IServiceController _serviceController;
        public AppSetup() { }

        public AppSetup(IServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        public IContainer CreateContainer()
        {
            return (_serviceController != null) ? CreateContainerExplicit(_serviceController) : CreateContainerImplicit();
        }

        public IContainer CreateContainerImplicit()
        {
            var cb = new ContainerBuilder();
            RegisterDependencies(cb);
            return cb.Build();
        }

        public IContainer CreateContainerExplicit(IServiceController serviceController)
        {
            var cb = new ContainerBuilder();
            RegisterDependencies(cb,serviceController);
            return cb.Build();
        }

        private void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<OAuthController>().As<IOAuthController>().SingleInstance();
            cb.RegisterType<SQLiteDatabase>().As<ISQLiteDatabase>().SingleInstance();
            cb.RegisterType<DatabaseController>()
              .As<IDatabaseController>()
              .WithParameter((pi, ctx) => pi.ParameterType == typeof(ISQLiteDatabase),
                             (pi, ctx) => ctx.Resolve<ISQLiteDatabase>());
            cb.RegisterType<ServiceController>()
              .As<IServiceController>()
              .WithParameter(
                  (pi, ctx) => pi.ParameterType == typeof(IOAuthController),
                  (pi, ctx) => ctx.Resolve<IOAuthController>())
              .WithParameter(
                (pi,ctx) => pi.ParameterType == typeof(IDatabaseController),
                (pi,ctx) => ctx.Resolve<IDatabaseController>());
        }

        private void RegisterDependencies(ContainerBuilder cb, IServiceController serviceController)
        {
            
            //cb.RegisterType<OAuthController>().As<IOAuthController>().SingleInstance();
            //cb.RegisterType<DatabaseController>().As<IDatabaseController>().SingleInstance();

            cb.RegisterInstance(serviceController)
              .As<IServiceController>();
        }
    }
}
