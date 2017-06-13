using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MusicAPI.Mashup
{
    using System.Threading;

    using MusicAPI.Mashup.Logic;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var taskLoop = new ThreadStart(TaskLoop);
            //var serviceTask = new Thread(taskLoop);
            //serviceTask.Start();
        }

        //public void ConsumeQueue()
        //{
        //    try
        //    {
        //        IEnumerable<IReloadableResource> allReloadableResources =
        //            ServiceLocator.Current.GetAllInstances<IReloadableResource>().ToArray();

        //        foreach (var reloadableResource in allReloadableResources)
        //        {
        //            try
        //            {
        //                reloadableResource.ReloadData();
        //            }
        //            catch (Exception ex)
        //            {
        //                ServiceLocator.Current.GetInstance<Serilog.ILogger>()
        //                    .Error(ex, "Could not reload " + reloadableResource.GetType());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceLocator.Current.GetInstance<Serilog.ILogger>()
        //            .Fatal(ex, "Could not create IReloadableResources. Something is wrong in IoC");
        //    }

        //    // Schedule it again
        //    this.AddTask(key, (TimeSpan)cachedObject);
        //}

        //private static void TaskLoop()
        //{
        //    while (true)
        //    {
        //        // Execute scheduled task
        //        ScheduledTask();

        //        // Wait for certain time interval
        //        Thread.Sleep(TimeSpan.FromSeconds(1.1));
        //    }
        //}

        //private static void ScheduledTask()
        //{
        //    // Task code which is executed periodically
        //    var queue = new Queue<string>();
        //    queue.Enqueue("mbid");

        //    var mbid = queue.Dequeue();
        //}
    }
}
