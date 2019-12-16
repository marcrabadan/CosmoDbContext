# CosmosDbFramework

## Packages:

| Package | NuGet Stable | 
| ------- | ------------ |
| [CosmosDbFramework](https://www.nuget.org/packages/CosmosDbFramework/) | [![CosmosDbFramework](https://img.shields.io/nuget/v/CosmosDbFramework.svg)](https://www.nuget.org/packages/CosmosDbFramework/) |
| [CosmosDbFramework.Autofac](https://www.nuget.org/packages/CosmosDbFramework.Autofac/) | [![CosmosDbFramework.Autofac](https://img.shields.io/nuget/v/CosmosDbFramework.Autofac.svg)](https://www.nuget.org/packages/CosmosDbFramework.Autofac/) | 
| [CosmosDbFramework.CastleWindsor](https://www.nuget.org/packages/CosmosDbFramework.CastleWindsor/) | [![CosmosDbFramework.CastleWindsor](https://img.shields.io/nuget/v/CosmosDbFramework.CastleWindsor.svg)](https://www.nuget.org/packages/CosmosDbFramework.CastleWindsor/) |
| [CosmosDbFramework.Extensions.DependencyInjection](https://www.nuget.org/packages/CosmosDbFramework.Extensions.DependencyInjection/) | [![CosmosDbFramework.Extensions.DependencyInjection](https://img.shields.io/nuget/v/CosmosDbFramework.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/CosmosDbFramework.Extensions.DependencyInjection/) |

## IoC Providers:
 - Service Collection
 - Autofac
 - Windsor Castle

## Installation & Configuration: 

| Package Manager | .NET CLI | 
| --------------- | -------- |
| Install-Package CosmosDbFramework -Version 1.0.0 | dotnet add package CosmosDbFramework --version 1.0.0 |
| Install-Package CosmosDbFramework.Autofac -Version 1.0.0 | dotnet add package CosmosDbFramework.Autofac --version 1.0.0 |
| Install-Package CosmosDbFramework.CastleWindsor -Version 1.0.0 | dotnet add package CosmosDbFramework.CastleWindsor --version 1.0.0 |
| Install-Package CosmosDbFramework.Extensions.DependencyInjection -Version 1.0.0 | dotnet add package CosmosDbFramework.Extensions.DependencyInjection --version 1.0.0 |

```csharp
                .AddCosmosDbContext<VideoStoreContext>(config =>
                {
                    config.Endpoint("https://localhost:8081");
                    config.AuthKeyOrResourceToken("XXXXXXXXX");
                    config.Configure(options =>
                    {
                        options.SerializerOptions = new CosmosSerializationOptions
                        {
                            IgnoreNullValues = false,
                            Indented = true,
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                        };
                    });
                })
```

## CosmosDbContext:

```csharp
 public class VideoStoreContext : CosmosDbContext
    {
        public VideoStoreContext(CosmosDbOptions<VideoStoreContext> options)
            : base(options)
        {

        }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Document<Actor>()
                .WithDatabase("videostore")
                .WithCollection("actors")
                .WithThroughput(400)
                .WithPartitionKey(c => c.Country);

            modelBuilder.Document<Movie>()
                .WithDatabase("videostore")
                .WithCollection("movies")
                .WithThroughput(400)
                .WithPartitionKey(c => c.Category);
        }

        public ICosmosCollection<Movie> Movies { get; set; }
        public ICosmosCollection<Actor> Actors { get; set; }
    }
```
