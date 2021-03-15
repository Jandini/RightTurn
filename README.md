# RightTurn
> Take the right turn to start your .NET application with dependency injection container, configuration, logging, exception handling and command line parser.
>

RightTurn is .NET application start-up container. You can start application with your own directions or take the turn with available extensions.



## Right Goal

The goal is to organize your application start-up code as well as provide reusable components via extensions.

Your program start-up can look like this ...

```C#
using RightTurn;

namespace RightStart
{
    class Program
    {
        static void Main() => new Turn()
            .Take<IQuickService, QuickService>((quick) => quick.Run());
    }
}
```

instead of that...

```C#
using Microsoft.Extensions.DependencyInjection;

namespace QuickStart
{
    class Program
    {
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IQuickService, QuickService>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetRequiredService<IQuickService>().Run();
        }
    }
}
```

... and that is only the beginning. When your code grows the logic in Main method too. RightTurn will help to keep it clean and tidy. 



## Quick Insight 

The start-up _Directions Container_ is a simple `Dictionary<Type, object>` that implements `ITurnDirections` interface.  The interface offers access to objects stored in the container which will be referred as "directions".  

To start your .NET application with turn you need to follow this pattern:

new `Turn()`

`With`... 

> add services, configuration, logging, command line parser etc...

`Take`...

> execute your code here 



#### Turn

In most cases you will use `new Turn()` to create new start-up container.  

```C#
static void Main(string[] args) => new Turn()
    ...
```

The `new Turn()` creates `IServiceCollection` and adds it to _Directions_ container.



#### With

In this section you can apply available extensions and provide services and configurations required to start your program. 



`WithParser`

> Change default command line parser

`ParseVerbs`

> Parse command line verbs 

`ParseOptions`

> Parse command line options



`WithDirections`

> Add _Directions_ container as `ITurnDirections` singleton to `IServiceCollection`

`WithServices`

> Add services required services

`WithTurn`

> Access or add directions to Directions container 

`WithUnhandledExceptionLogging`

> Add unhandled exception handler



`WithConfiguration`

> Build and provide your own configuration 

`WithConfigurationFile`

> Provide configuration from a file. Default is optional `appsettings.config`



`WithLogging`

> Provide logging configuration builder

`WithSerilog`

> Add Serilog logging 



#### Take

The turn you `Take` will do following

`AddConfiguration`

> If _Directions container_ have `ITrunConfiguration` provided by `WithConfiguration` or `WithConfigurationFile`

`AddLogging`

> If _Directions container_ have object that implements `ITurnLogging`interface

`BuildServiceProvider`

> Add `IServiceCollection` to `Directions` container 



```C#
if (Directions.Have<ITurnConfiguration>(out var configuration))
     configuration.AddConfiguration(this);

if (Directions.Have<ITurnLogging>(out var logging))
    logging.AddLogging(this);

return Directions.Add<IServiceProvider>(Directions.Get<IServiceCollection>().BuildServiceProvider());
```





## Quick Start

- Create a new project from **Console Application** template. 

- Set *Project name* to `QuickStart`.

- Set *Target Framework* to **.NET 5.0** or **.NET Core 3.1**.

- Add **RightTurn** nuget package.

- Create new interface **IQuickService**. 
   ######  IQuickService.cs
   ```C#
   namespace QuickStart
   {
       internal interface IQuickService
       {
           void Run();
       }
   }
   ```

- Create new class **QuickService** and implement `IQuickService` interface.
   ######  QuickService.cs
   ```C#
   using System;

   namespace QuickStart
   {
       class QuickService : IQuickService
       {
           public void Run()
           {
               Console.WriteLine("Hello World");
           }
       }
   }
   ```
- Add `new Turn()` to program's `Main` method
   ######  Program.cs
   ```C#
    using RightTurn;

    namespace QuickStart
    {
        class Program
        {
            static void Main() => new Turn()
                .Take<IQuickService, QuickService>((quick) => quick.Run());
        }
    }
	```






## Interfaces

- `ITurnDirections` 
- `ITurnConfiguration` expose `AddConfiguration(ITurn turn)` method 
- `ITurnLogging`



## Extensions

* Configuration

* Logging

* Serilog

* CommandLine

  

### [Configuration](https://github.com/Jandini/RightTurn.Extensions.Configuration)
Provides Configuration extensions.


### [Logging](https://github.com/Jandini/RightTurn.Extensions.Logging)
Provides Logging extensions.

### [Serilog](https://github.com/Jandini/RightTurn.Extensions.Logging)
Provides [Serilog](https://github.com/serilog/serilog) extensions.

### [CommandLine](https://github.com/Jandini/RightTurn.Extensions.CommandLine)
Provides [CommandLine Parser](https://github.com/commandlineparser/commandline) extensions.






