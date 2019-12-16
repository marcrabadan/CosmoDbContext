using Autofac;
using Autofac.Core;
using CosmosDbFramework.Autofac;
using CosmosDbFramework.Options;
using CosmosDbFramework.Options.Builders;
using System;

namespace CosmosDbFramework.Autofac
{
    public static class Installer
    {
        public static ContainerBuilder AddCosmosDbContext<TContext>(
               this ContainerBuilder containerBuilder,
               Action<CosmosDbOptionBuilder> options,
               LifeTime contextLifetime = LifeTime.Scoped,
               LifeTime optionsLifetime = LifeTime.Scoped)
               where TContext : CosmosDbContext
        {
            containerBuilder.AddCosmosDbContext<TContext>((p, b) => options.Invoke(b), contextLifetime, optionsLifetime);
            return containerBuilder;
        }

        private static ContainerBuilder AddCosmosDbContext<TContext>(
            this ContainerBuilder containerBuilder,
            Action<IComponentContext, CosmosDbOptionBuilder> options,
            LifeTime contextLifetime = LifeTime.Scoped,
            LifeTime optionsLifetime = LifeTime.Scoped)
            where TContext : CosmosDbContext
        {
            if (containerBuilder == null)
                throw new InvalidOperationException($"{nameof(containerBuilder)} is null.");

            if (contextLifetime == LifeTime.Singleton)
            {
                optionsLifetime = LifeTime.Singleton;
            }

            var optionRegistration = containerBuilder
                .Register<CosmosDbOptions<TContext>>(c => CosmosDbOptionsFactory<TContext>(c, options))
                .OnlyIf(reg => !reg.IsRegistered(new TypedService(typeof(CosmosDbOptions<TContext>))));

            switch (optionsLifetime)
            {
                case LifeTime.Singleton:
                    optionRegistration.SingleInstance();
                    break;
                case LifeTime.Scoped | LifeTime.Thread:
                    optionRegistration.InstancePerLifetimeScope();
                    break;
                case LifeTime.Transient:
                    optionRegistration.InstancePerDependency();
                    break;
                case LifeTime.Request:
                    optionRegistration.InstancePerRequest();
                    break;
            }

            var contextRegistration = containerBuilder
                .RegisterType<TContext>()
                .AsSelf()
                .OnlyIf(reg => !reg.IsRegistered(new TypedService(typeof(TContext))));

            switch (contextLifetime)
            {
                case LifeTime.Singleton:
                    contextRegistration.SingleInstance();
                    break;
                case LifeTime.Scoped | LifeTime.Thread:
                    contextRegistration.InstancePerLifetimeScope();
                    break;
                case LifeTime.Transient:
                    contextRegistration.InstancePerDependency();
                    break;
                case LifeTime.Request:
                    contextRegistration.InstancePerRequest();
                    break;
            }

            return containerBuilder;
        }

        private static CosmosDbOptions<TContext> CosmosDbOptionsFactory<TContext>(
            IComponentContext componentContext,
            Action<IComponentContext, CosmosDbOptionBuilder> optionsAction)
            where TContext : CosmosDbContext
        {
            var builder = new CosmosDbOptionBuilder<TContext>();

            optionsAction.Invoke(componentContext, builder);

            return (CosmosDbOptions<TContext>)builder.Build();
        }
    }
}
