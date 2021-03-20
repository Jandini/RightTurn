using Microsoft.Extensions.DependencyInjection;
using System;

namespace RightTurn
{
    public static class TurnDirectionsExtensions
    {
        public static bool HaveService<T>(this ITurnDirections directions, out T service) => (service = directions.GetService<T>()) != null;
        public static IServiceProvider ServiceProvider(this ITurnDirections directions) => directions.Get<IServiceProvider>();
        public static IServiceCollection ServiceCollection(this ITurnDirections directions) => directions.Get<IServiceCollection>();
        public static T GetService<T>(this ITurnDirections directions)
        {
            return directions.Have<IServiceProvider>(out var provider)
                ? provider.GetService<T>()
                : default;
        }
    }
}
