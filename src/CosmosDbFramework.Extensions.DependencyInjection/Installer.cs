using CosmosDbFramework.Extensions.DependencyInjection;
using CosmosDbFramework.Options;
using CosmosDbFramework.Options.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CosmosDbFramework.Extensions.DependencyInjection
{
    public static class Installer
    {
        public static IServiceCollection AddCosmosDbContext<TContext>(
               this IServiceCollection services,
               Action<CosmosDbOptionBuilder> options,
               ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
               ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
               where TContext : CosmosDbContext
        {
            services.AddCosmosDbContext<TContext>((p, b) => options.Invoke(b), contextLifetime, optionsLifetime);
            return services;
        }

        private static IServiceCollection AddCosmosDbContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<IServiceProvider, CosmosDbOptionBuilder> options,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : CosmosDbContext
        {
            if (serviceCollection == null)
                throw new InvalidOperationException($"{nameof(serviceCollection)} is null.");

            if (contextLifetime == ServiceLifetime.Singleton)
            {
                optionsLifetime = ServiceLifetime.Singleton;
            }

            serviceCollection.TryAdd(
                new ServiceDescriptor(
                    typeof(CosmosDbOptions<TContext>),
                    p => MongoDbOptionsFactory<TContext>(p, options),
                    optionsLifetime));

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TContext), typeof(TContext), contextLifetime));

            return serviceCollection;
        }

        private static CosmosDbOptions<TContext> MongoDbOptionsFactory<TContext>(
            IServiceProvider applicationServiceProvider,
            Action<IServiceProvider, CosmosDbOptionBuilder> optionsAction)
            where TContext : CosmosDbContext
        {
            var builder = new CosmosDbOptionBuilder<TContext>();

            optionsAction.Invoke(applicationServiceProvider, builder);

            return (CosmosDbOptions<TContext>)builder.Build();
        }
    }
}
