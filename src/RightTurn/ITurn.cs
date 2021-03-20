using System;

namespace RightTurn
{
    public interface ITurn
    {
        ITurnDirections Directions { get; }   
        IServiceProvider Take();
        int Take<TService, TImplementation>(Func<TService, int> run) where TService : class where TImplementation : class, TService;
        void Take<TService, TImplementation>(Action<TService> run) where TService : class where TImplementation : class, TService;
        int Take(Func<IServiceProvider, int> provider);
        void Take(Action<IServiceProvider> provider);
    }
}