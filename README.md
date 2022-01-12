# RightTurn
> Take the right turn to start your .NET application with dependency injection container, configuration, logging, exception handling and command line parser.
>

[![.NET](https://github.com/Jandini/RightTurn/actions/workflows/build.yml/badge.svg)](https://github.com/Jandini/RightTurn/actions/workflows/build.yml)

RightTurn is .NET application start-up container. The goal is to organize your application start-up code as well as provide reusable components via extensions.

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

When your code grows the logic in `Main` method might grow too. RightTurn will help to keep it clean and tidy. 



## Quick Start

- Create **QuickStart** Console Application for .NET 5.0 or .NET Core 3.1 in Visual Studio

- Add **RightTurn.Extensions.CommandLine** NuGet package.

- Add **RightTurn.Extensions.Serilog** NuGet package.

- Add following files to your project:

  ###### IQuickOptions.cs
  
  ```C# 
  namespace QuickStart
  {
      interface IQuickOptions
      {
          string Name { get; }
      }
  }
  ```
  ###### QuickOptions.cs
  ```C# 
  using CommandLine;
  
  namespace QuickStart
  {
      class QuickOptions : IQuickOptions
      {
          [Option(HelpText = "Your name.")]
          public string Name { get; set; }
      }
  }
  ```
  ###### IQuickSettings.cs
  
  ```C# 
  namespace QuickStart
  {
      internal interface IQuickSettings
      {
          string Message { get; }
          string Question { get; }
      }
  }
  ```
  ###### QuickSettings.cs
  ```C# 
  namespace QuickStart
  {
      class QuickSettings : IQuickSettings
      {
          public string Message { get; set; }
          public string Question { get; set; }
      }
  }
  ```
  ###### IQuickService.cs
  ```C#
  namespace QuickStart
  {
      interface IQuickService
      {
          void Run();
      }
  }
  ```
  ###### QuickService.cs
  ```C# 
  using Microsoft.Extensions.Logging;
  using System;
  
  namespace QuickStart
  {
      class QuickService : IQuickService
      {
          readonly ILogger<QuickService> _logger;
          readonly IQuickOptions _options;
          readonly IQuickSettings _settings;
  
          public QuickService(ILogger<QuickService> logger, IQuickOptions options, IQuickSettings settings)
          {
              _logger = logger;
              _options = options;
              _settings = settings;
          }
  
          public void Run()
          {
              if (_options.Name is null)
                  throw new Exception(_settings.Question);
  
            _logger.LogInformation(_settings.Message, _options.Name);
          }
      }
  }
  ```
  ###### appsettings.json
  *Set **Copy to Output Directory** to **Copy if newer***

  ```JSON 
  {
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information"
      },
      "WriteTo": [
        {
          "Name": "Console",
          "Args": {
            "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Literate, Serilog.Sinks.Console",
            "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u5}] {Message:lj}{NewLine}{Exception}"
          }
        }
      ]
    },
    "QuickSettings": {
      "Message": "Hello {name}, Welcome to RightTurn.",
      "Question": "Who are you?"
    }
  }  
  ```
- Add start-up with RightTurn to your program's Main method.

  ###### Program.cs
  ```C#
  using RightTurn;
  using RightTurn.Extensions.CommandLine;
  using RightTurn.Extensions.Configuration;
  using RightTurn.Extensions.Logging;
  using RightTurn.Extensions.Serilog;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main(string[] args) => new Turn()
              .ParseOptions<QuickOptions>(args)
              .WithOptionsAsSingleton<IQuickOptions, QuickOptions>()
              .WithConfigurationSettings<IQuickSettings, QuickSettings>("QuickSettings")
              .WithSerilog()
              .WithUnhandledExceptionLogging()
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Run application.

  ```
  [2021-03-20 22:39:24 FATL] Who are you?
  ```

- Run application with **--help** command line parameter.

  ```
  QuickStart 1.0.0
  Copyright (C) 2021 QuickStart
  
    --name       Your name.
  
    --help       Display this help screen.
  
    --version    Display version information.
  ```

- Run application with **--name Matt** command line parameter.

  ```
  [2021-03-20 22:40:04 INFO] Hello Matt, Welcome to RightTurn.
  ```

  

### Quick Examples

See how RightTurn can help to start-up your application.

[Janda.CTF](https://github.com/Jandini/Janda.CTF/blob/develop/src/Janda.CTF/CTF.cs#L40)






## Step by Step

- Create a new project from **Console Application** template. 

- Set *Project name* to `QuickStart`.

- Set *Target Framework* to **.NET 5.0** or **.NET Core 3.1**.

- Add **RightTurn** NuGet package.

- Create new interface **IQuickService**. 
   ######  IQuickService.cs
   ```C#
   namespace QuickStart
   {
       interface IQuickService
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
   
- Add `new Turn()` to program's `Main` method.
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

- Run application.
    ```
    Hello world
    ```



### Logging

- Add **RightTurn.Extensions.Logging** NuGet package.

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

- Add **Microsoft.Extensions.Logging.Console** NuGet package.
- Add `WithLogging` to program's `Main` method.

  ######  Program.cs

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

- Run application.
    ```
    info: QuickStart.QuickService[0]
          Hello World
    ```



#### Logging with Serilog

* Add **Serilog.Extensions.Logging** NuGet package.
* Add **Serilog.Sinks.Console** NuGet package.
* Add `WithLogging` to program's `Main` method.

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
                      .CreateLogger(),
  		        dispose: true))            
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Run application.
    ```
    [21:50:43 INF] Hello World
    ```



#### Logging with RightTurn Serilog

##### Serilog with inline configuration by code

- Add **RightTurn.Extensions.Serilog** NuGet package.

- Add **RightTurn.Extensions.Configuration** NuGet package.

- Add `WithLogging` to program's `Main` method.

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

##### Serilog with configuration provided from *appsettings.json* file

- Add **New Item** to your project.

- Select **JSON File** and provide **Name** as `appsettings.json`.

- Add Serilog configuration to appsettings.json file.

  ###### appsettings.json

  ```json
  {
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information"
      },
      "WriteTo": [
        {
          "Name": "Console",
          "Args": {
            "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
            "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
          }
        }
      ]
    }
  }
  ```

- Use `WithConfigurationFile()` and `WithSerilog()` in program's `Main` method.

    ```C#
    using RightTurn;
    using RightTurn.Extensions.Serilog;
    
    namespace QuickStart
    {
        class Program
        {
            static void Main() => new Turn()
                .WithConfigurationFile()
                .WithSerilog()
                .Take<IQuickService, QuickService>((quick) => quick.Run());
        }
    }
    ```

    

- Add logging to file for Serilog in appsettings.json file.

  ###### appsettings.json

  ```json
  {
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information"
      },
      "WriteTo": [
        {
          "Name": "Console",
          "Args": {
            "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
            "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
          }
        },
        {
          "Name": "File",
          "Args": {
            "path": "logs/.log",
            "rollingInterval": "Day"
          }
        }
      ]
    }
  }
  ```

  


### Configuration

- Add **RightTurn.Extensions.Configuration** NuGet package.
- Add **New Item** to your project.
- Select **JSON File** and provide **Name** as `appsettings.json`.



#### Configuration for Microsoft Console Logging

- Add Logging configuration to appsettings.json file.

  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
  ```

- Add `WithConfigurationFile()` and configuration to logging builder in `WithLogging`.

  ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Logging;
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class Program
      {
           static void Main() => new Turn()
             .WithConfigurationFile()
             .WithLogging((builder, turn) => builder
                  .AddConsole()
                  .AddConfiguration(turn.Directions.Configuration().GetSection("Logging")))         
             .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Add "Console" section to Logging in appsettings.json file to customize console logging behaviour. For example, you can change logging level.

  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information"
      },
      "Console": {      
        "LogLevel": {
          "Default": "Warning"
        }
      }
    }
  }
  ```



#### Bind configuration settings to singleton class or service

- Add **Settings** class to your project.

  ######  Settings.cs

  ```C#
  namespace QuickStart
  {
      class Settings
      {
          public string Description { get; set; }
      }
  }
  ```

- Add "Quick" section to appsettings.json file.

  ###### appsettings.json

  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information"
      }    
    },
    "Quick": {
      "Description": "Hello world from configuration file"
    }
  }
  ```

- Add ` .WithConfigurationSettings<Settings>("Quick")()` to main method.

  ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Logging;
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class Program
      {
           static void Main() => new Turn()
               .WithConfigurationFile()
               .WithLogging((builder, turn) => builder
                   .AddConsole()
                   .AddConfiguration(turn.Directions.Configuration().GetSection("Logging")))
               .WithConfigurationSettings<Settings>("Quick")             
               .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Add `Settings` to **QuickService** class.

  ######  QuickService.cs

  ```C#
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class QuickService : IQuickService
      {
          readonly ILogger<QuickService> _logger;
          readonly Settings _settings;
  
          public QuickService(ILogger<QuickService> logger, Settings settings)
          {
              _logger = logger;
              _settings = settings;
          }
  
          public void Run()
          {
              _logger.LogInformation(_settings.Description);            
          }
      }
  }
  ```

- Run application.

  ```
  info: QuickStart.QuickService[0]
        Hello world from configuration file
  ```

  ##### Hide Settings behind interface

- Add **ISettings** interface to your project.

  ######  ISettings.cs

  ```C#
  namespace QuickStart
  {
      interface ISettings
      {
          public string Description { get; }
      }
  }
  ```

- Add the interface to **Settings** class.

  ######  Settings.cs

  ```C#
  namespace QuickStart
  {
      class Settings : ISettings
      {
          public string Description { get; set; }
      }
  }
  ```

- Add `ISettings` to **QuickService** class.

  ######  QuickService.cs

  ```C#
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class QuickService : IQuickService
      {
          readonly ILogger<QuickService> _logger;
          readonly ISettings _settings;
  
          public QuickService(ILogger<QuickService> logger, ISettings settings)
          {
              _logger = logger;
              _settings = settings;
          }
  
          public void Run()
          {
              _logger.LogInformation(_settings.Description);            
          }
      }
  }
  ```

- Add ` .WithConfigurationSettings<ISettings, Settings>("Quick")()` to main method.

  ######  Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.Logging;
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class Program
      {
           static void Main() => new Turn()
  			.WithConfigurationFile()
  			.WithLogging((builder, turn) => builder
  				.AddConsole()
  				.AddConfiguration(turn.Directions.Configuration().GetSection("Logging")))
  			.WithConfigurationSettings<ISettings, Settings>("Quick")             
  			.Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

  

### Command Line Parser

- Add **RightTurn.Extensions.CommandLine** NuGet package.

- Add **QuickOptions** class to your project.

  ######  QuickOptions.cs

  ```C#
  using CommandLine;
  
  namespace QuickStart
  {
      class QuickOptions
      {
          [Option(Required = true)]
          public string Name { get; set; }
      }
  }
  ```

- Use `ParseOptions` to program's `Main` method.

  ###### Program.cs

  ```C#
  using RightTurn;
  using RightTurn.Extensions.CommandLine;
  using RightTurn.Extensions.Logging;
  using RightTurn.Extensions.Serilog;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main(string[] args) => new Turn()
              .ParseOptions<QuickOptions>(args)
              .WithSerilog()
              .WithUnhandledExceptionLogging()
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```


- Run application.

  ```
  QuickStart 1.0.0
  Copyright (C) 2021 QuickStart
  
  ERROR(S):
    Required option 'name' is missing.
  
    --name       Required.
  
    --help       Display this help screen.
  
    --version    Display version information.
  ```

- Run application with `--name "Hello command line"` parameter. 

  ```
  [21:39:28 INF] Hello world
  ```

  The parameters were successfully parsed.

- Add `.WithOptionsAsSingleton<QuickOptions>()`

  ```C#
  using RightTurn;
  using RightTurn.Extensions.CommandLine;
  using RightTurn.Extensions.Logging;
  using RightTurn.Extensions.Serilog;
  
  namespace QuickStart
  {
      class Program
      {
          static void Main(string[] args) => new Turn()
              .ParseOptions<QuickOptions>(args)
              .WithOptionsAsSingleton<QuickOptions>()
              .WithSerilog()
              .WithUnhandledExceptionLogging()
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Add **QuickOptions** to **QuickService** class.

  ```C#
  using Microsoft.Extensions.Logging;
  
  namespace QuickStart
  {
      class QuickService : IQuickService
      {
          readonly ILogger<QuickService> _logger;
          readonly QuickOptions _options;
  
          public QuickService(ILogger<QuickService> logger, QuickOptions options)
          {
              _logger = logger;
              _options = options;
          }
  
          public void Run()
          {
              _logger.LogInformation(_options.Name);
          }
      }
  }
  ```

- Run application with `--name "Hello world from command line"` parameter.

  ```
  [21:44:48 INF] Hello world from command line
  ```

  



### Unhandled Exceptions

The example is based on the code created in **Logging with RightTurn Serilog** section above.

- Add `throw new Exception` to **QuickService** class.

  ######  QuickService.cs

  ```C#
  using System;
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
              throw new Exception("I am unhandled exception");
          }
      }
  }
  ```

- Add `WithUnhandledExceptionHandler` to `Main` method.

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
              .WithSerilog()
              .WithUnhandledExceptionHandler((exception) => Console.WriteLine(exception.Message))
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Run application. 

  ```
  I am unhandled exception
  ```

- Replace `WithUnhandledExceptionHandler` with `WithUnhandledExceptionLogging` 

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
              .WithSerilog()
  			.WithUnhandledExceptionLogging()
              .Take<IQuickService, QuickService>((quick) => quick.Run());
      }
  }
  ```

- Run application.

  ```
  [21:01:46 FTL] I am unhandled exception
  ```






## What's Inside

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

> Change default command line parser.

`ParseVerbs`

> Parse command line verbs.

`ParseOptions`

> Parse command line options.



`WithDirections`

> Add _Directions_ container as `ITurnDirections` singleton to `IServiceCollection`.

`WithServices`

> Add services required services.
> Configuration extensions provides `WithServices` extension with access to `IConfiguration`.

`WithTurn`

> Access or add directions to Directions container.

`WithUnhandledExceptionLogging`

> Add unhandled exception handler.



`WithConfiguration`

> Build and provide your own configuration.

`WithConfigurationFile`

> Provide configuration from a file. Default is optional `appsettings.config`.

`WithConfigurationSettings`

> Create settings object, binds configuration section and add the object as singleton service.



`WithLogging`

> Provide logging configuration builder.

`WithSerilog`

> Add Serilog logging.



### Take

The `Take` will do following: 

`AddConfiguration`

> Add configuration if _directions container_ has `ITrunConfiguration`.

`AddLogging`

> Add logging if _directions container_ has `ITurnLogging`.

`AddServices`

> Add services that require access to `IConfiguration` before are added to ServiceCollection.

`BuildServiceProvider`

> Build service provider and add `IServiceCollection` to _directions container_ container.



```C#
if (Directions.Have<ITurnConfiguration>(out var configuration))
     configuration.AddConfiguration(this);

if (Directions.Have<ITurnLogging>(out var logging))
    logging.AddLogging(this);

if (Directions.Have<ITurnServices>(out var services))
    services.AddServices(this);

return Directions.Add<IServiceProvider>(Directions.Get<IServiceCollection>().BuildServiceProvider());
```





### Extensions

* [RightTurn.Extensions.Configuration](https://github.com/Jandini/RightTurn.Extensions.Configuration)
Provides configuration extensions.

* [RightTurn.Extensions.Logging](https://github.com/Jandini/RightTurn.Extensions.Logging)
Provides logging extensions.

* [RightTurn.Extensions.Serilog](https://github.com/Jandini/RightTurn.Extensions.Serilog)
Provides [Serilog](https://github.com/serilog/serilog) extensions.

*  [RightTurn.Extensions.CommandLine](https://github.com/Jandini/RightTurn.Extensions.CommandLine)
Provides [CommandLine Parser](https://github.com/commandlineparser/commandline) extensions.


