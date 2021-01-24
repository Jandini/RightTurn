using Microsoft.Extensions.DependencyInjection;
using System;

namespace RightTurn
{
    public static class TurnExtensions
    {
        public static ITurn WithTurn(this ITurn turn, Action<ITurn> with)
        {
            with?.Invoke(turn);
            return turn;
        }

        public static ITurn WithServices(this ITurn turn, Action<IServiceCollection> services)
        {
            services?.Invoke(turn.Directions.Get<IServiceCollection>());
            return turn;
        }

        public static ITurn WithDirections(this ITurn turn)
        {
            turn.WithServices(services => services.AddSingleton(turn.Directions));
            return turn;
        }

        public static ITurn AddDirection<T>(this ITurn turn, T direction)
        {
            turn.Directions.Add(direction);
            return turn;
        }


        public static ITurn WithUnhandledExceptionHandler(this ITurn turn, Action<Exception> handler)
        {
            turn.Directions.Add<Func<Exception, ITurn, int>>((e, t) => { handler.Invoke(e); return 0; });
            return turn;
        }
        public static ITurn WithUnhandledExceptionHandler(this ITurn turn, Func<Exception, int> handler)
        {
            turn.Directions.Add<Func<Exception, ITurn, int>>((e, t) => handler.Invoke(e));
            return turn;
        }

        public static ITurn WithUnhandledExceptionHandler(this ITurn turn, Action<Exception, ITurn> handler)
        {
            turn.Directions.Add<Func<Exception, ITurn, int>>((e, t) => { handler.Invoke(e, t); return 0; });
            return turn;
        }
        
        public static ITurn WithUnhandledExceptionHandler(this ITurn turn, Func<Exception, ITurn, int> handler)
        {
            turn.Directions.Add(handler);
            return turn;
        }
    }
}
