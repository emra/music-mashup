[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MusicAPI.Mashup.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MusicAPI.Mashup.App_Start.NinjectWebCommon), "Stop")]

namespace MusicAPI.Mashup.App_Start
{
    using System;
    using System.Net.Http;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using MusicAPI.Mashup.Repositories;
    using MusicAPI.Mashup.Services;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var userAgent = "Music mashup API";

            var musicBrainzClient = new HttpClient() { BaseAddress = new Uri("http://musicbrainz.org") };
            musicBrainzClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            kernel.Bind<IMusicBrainzRepository>().To<MusicBrainzRepository>()
                .WithConstructorArgument(musicBrainzClient);

            var wikipediaClient = new HttpClient() { BaseAddress = new Uri("https://en.wikipedia.org") };
            wikipediaClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            kernel.Bind<IWikipediaRepository>().To<WikipediaRepository>().WithConstructorArgument(wikipediaClient);

            var coverArtArchiveClient = new HttpClient() { BaseAddress = new Uri("http://coverartarchive.org/") };
            coverArtArchiveClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            kernel.Bind<ICoverArtArchiveRepository>().To<CoverArtArchiveRepository>()
                .WithConstructorArgument(coverArtArchiveClient);

            kernel.Bind<IMashupService>().To<MashupService>();
        }        
    }
}
