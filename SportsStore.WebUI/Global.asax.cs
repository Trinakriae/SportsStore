﻿using SportsStore.Domain.Entities;
using SportsStore.WebUI.Infrastructure;
using SportsStore.WebUI.Infrastructure.Automapper;
using SportsStore.WebUI.Infrastructure.Binders;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Custom code
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
            AutoMapperWebUIConfiguration.Configure();
        }
    }
}
