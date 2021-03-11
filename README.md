# RightTurn
> Take right turn to run your .NET application with dependency injection, configuration, logging, exception handling and command line parser.
>

RightTurn is .NET application start-up container. You can start application with your own directions or take the turn with one of the provided extensions.


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




## Extensions

### [Configuration](https://github.com/Jandini/RightTurn.Extensions.Configuration)
Provides Configuration extensions.

### [Logging](https://github.com/Jandini/RightTurn.Extensions.Logging)
Provides Logging extensions.

### [Serilog](https://github.com/Jandini/RightTurn.Extensions.Logging)
Provides [Serilog](https://github.com/serilog/serilog) extensions.

### [CommandLine](https://github.com/Jandini/RightTurn.Extensions.CommandLine)
Provides [CommandLine Parser](https://github.com/commandlineparser/commandline) extensions.






