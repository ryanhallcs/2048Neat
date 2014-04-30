using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;

namespace Sharp2048.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static WindsorContainer _container;
        protected void Application_Start()
        {
            _container = new WindsorContainer();
            var installer = new Sharp2048WindsorInstaller();
            _container.Install(installer);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }
    }
}
