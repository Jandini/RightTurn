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

When your code grows the logic in Main method might grow too. RightTurn will help to keep it clean and tidy. 



## Quick Start

###### with NuGet packages

`RightTurn`

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

Here you are... 

```
Hello world
```



### Logging

###### with NuGet packages

`RightTurn`, `RightTurn.Extensions.Logging`

Use [RightTurn.Extensions.Logging](https://github.com/Jandini/RightTurn.Extensions.Logging) to start your application with logging and keep you code tidy. It provides `Microsoft.Extensions.Logging` as it's dependency.

- Add **RightTurn.Extensions.Logging** nuget package.

- Add `ILogger<QuickService> _logger` to **QuickService** class.

  ######  QuickService.cs

  ```C#
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class QuickService : IQuickService
      {
          readonly ILogger<QuickService> _logger;
  
          public QuickService(ILogger<QuickService> logger)
          {
              _logger = logger;
          }
  
          public void Run()
          {
              _logger.LogInformation("Hello World");            
          }
      }
  }
  ```

* Add logging of your choice. *See the examples below.* 

  

#### Logging with Microsoft Console Logging

###### with NuGet packages

`RightTurn`, `RightTurn.Extensions.Logging`, `Microsoft.Extensions.Logging.Console`

- Add **Microsoft.Extensions.Logging.Console** nuget package.
- Add `WithLogging` to program's `Main` method

- ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Logging;
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main() => new Turn()
              .WithLogging((builder) => builder.AddConsole())
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

That's it ... 

```
info: QuickStart.QuickService[0]
      Hello World
```



#### Logging with Serilog

###### with NuGet packages

`RightTurn`, `RightTurn.Extensions.Logging`, `Serilog.Extensions.Logging`, `Serilog.Sinks.Console`



Following steps shows how to add console logging from `Serilog`.

* Add **Serilog.Extensions.Logging** nuget package.
* Add **Serilog.Sinks.Console** nuget package.

- Add `WithLogging` to program's `Main` method

  ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Logging;
  using Serilog;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main() => new Turn()
              .WithLogging((builder) => builder.AddSerilog(
                  new LoggerConfiguration()
                  .WriteTo.Console()
                  .CreateLogger()))
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

That's it ...

```
[21:50:43 INF] Hello World
```



#### Logging with RightTurn.Extensions.Serilog

###### with NuGet packages

`RightTurn`, `RightTurn.Extensions.Serilog`



Package [RightTurn.Extensions.Serilog](https://github.com/Jandini/RightTurn.Extensions.Serilog) provides [Serilog](https://github.com/serilog/serilog) for out of the box. 

- Add **RightTurn.Extensions.Serilog** nuget package

- Add `WithLogging` to program's `Main` method

  ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Serilog;
  using Serilog;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main() => new Turn()
              .WithSerilog((config) => config.WriteTo.Console())
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  
  ```

> Note: The `WithSerilog()` without parameters requires `IConfiguration` in directions container. Using `WithSerilog()` without configuration directions will throw exception:

>*Unhandled exception. System.Collections.Generic.KeyNotFoundException: >The given key 'Microsoft.Extensions.Configuration.IConfiguration' was >not present in the dictionary.*



### Configuration

###### with NuGet packages

`RightTurn`,  `RightTurn.Extensions.Configuration`




















## Quick Insight 

The start-up _Directions Container_ is a simple `Dictionary<Type, object>` that implements `ITurnDirections` interface.  The interface offers access to objects stored in the container which will be referred as "directions".  

To start your .NET application with turn you need to follow this pattern:

new `Turn()`

`With`... 

> add services, configuration, logging, command line parser etc...

`Take`...

> execute your code here 



### Turn

In most cases you will use `new Turn()` to create new start-up container.  

```C#
static void Main(string[] args) => new Turn()
    ...
```

The `new Turn()` creates `IServiceCollection` and adds it to _Directions_ container.



### With

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



### Take

The `Take` will do following: 

`AddConfiguration`

> Add configuration if _directions container_ have `ITrunConfiguration` 

`AddLogging`

> Add logging if _directions container_ have `ITurnLogging` 

`BuildServiceProvider`

> Build service provider and add `IServiceCollection` to _directions container_ container 



```C#
if (Directions.Have<ITurnConfiguration>(out var configuration))
     configuration.AddConfiguration(this);

if (Directions.Have<ITurnLogging>(out var logging))
    logging.AddLogging(this);

return Directions.Add<IServiceProvider>(Directions.Get<IServiceCollection>().BuildServiceProvider());

```





### Directions

This is what you can find in directions container 

- `ITurnDirections` 
- `ITurnConfiguration` 
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







```