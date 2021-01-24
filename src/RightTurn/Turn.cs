using System;
using Microsoft.Extensions.DependencyInjection;

namespace RightTurn
{
    public sealed class Turn : ITurn
    {
        public ITurnDirections Directions { get; private set; }

        public Turn()
        {
            Directions = new TurnDirections();
            Directions.Add<IServiceCollection>(new ServiceCollection());
        }

        public static void Take<TService, TImplementation>(Action<TService> run, Action<IServiceCollection> services = null)
           where TService : class
           where TImplementation : class, TService => new Turn()
               .WithServices(services)
               .Take<TService, TImplementation>(run);

        public static int Take<TService, TImplementation>(Func<TService, int> run, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .WithServices(services)
                .Take<TService, TImplementation>(run);

        public static void Take<TService, TImplementation>(string[] args, Action<TService> run, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .WithDirections()
                .AddDirection(args)
                .WithServices(services)
                .Take<TService, TImplementation>(run);

        public static int Take<TService, TImplementation>(string[] args, Func<TService, int> run, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .WithDirections()
                .AddDirection(args)
                .WithServices(services)
                .Take<TService, TImplementation>(run);


        public static TService Take<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return new Turn()
                .WithServices((services) => { services.AddTransient<TService, TImplementation>(); })
                .Take()
                .GetRequiredService<TService>();
        }


        public int Take<TService, TImplementation>(Func<TService, int> run)
            where TService : class
            where TImplementation : class, TService
        {
            Directions.Get<IServiceCollection>().AddTransient<TService, TImplementation>();
            return Take((provider) => run(provider.GetRequiredService<TService>()));
        }


        public void Take<TService, TImplementation>(Action<TService> run)
            where TService : class
            where TImplementation : class, TService
        {
            Directions.Get<IServiceCollection>().AddTransient<TService, TImplementation>();
            Take((provider) => run(provider.GetRequiredService<TService>()));
        }


        public int Take(Func<IServiceProvider, int> provider)
        {
            try
            {
                return provider(Take());
            }
            catch (Exception ex)
            {
                if (!Directions.Have<Func<Exception, ITurn, int>>(out var handler))
                    throw;

                return handler.Invoke(ex, this);
            }
        }

        public void Take(Action<IServiceProvider> provider)
        {
            try
            {
                provider(Take());
            }
            catch (Exception ex)
            {
                if (!Directions.Have<Func<Exception, ITurn, int>>(out var handler))
                    throw;

                handler.Invoke(ex, this);
            }
        }

        public IServiceProvider Take()
        {
            if (Directions.Have<ITurnConfiguration>(out var configuration))
                configuration.AddConfiguration(this);

            if (Directions.Have<ITurnLogging>(out var logging))
                logging.AddLogging(this);

            return Directions.Add<IServiceProvider>(Directions.Get<IServiceCollection>()
                .BuildServiceProvider());
        }
    }
}
