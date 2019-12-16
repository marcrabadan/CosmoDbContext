using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CosmosDbFramework.CastleWindsor;
using CosmosDbFramework.Options;
using CosmosDbFramework.Options.Builders;
using System;

namespace CosmosDbFramework.CastleWindsor
{
    public static class Installer
    {
        public static IWindsorContainer AddCosmosDbContext<TContext>(
               this IWindsorContainer containerBuilder,
               Action<CosmosDbOptionBuilder> options,
               LifestyleType contextLifestyle = LifestyleType.Scoped,
               LifestyleType optionsLifestyle = LifestyleType.Scoped)
               where TContext : CosmosDbContext
        {
            containerBuilder.AddCosmosDbContext<TContext>((p, b) => options.Invoke(b), contextLifestyle, optionsLifestyle);
            return containerBuilder;
        }

        private static IWindsorContainer AddCosmosDbContext<TContext>(
            this IWindsorContainer containerBuilder,
            Action<IKernel, CosmosDbOptionBuilder> options,
            LifestyleType contextLifestyle = LifestyleType.Scoped,
            LifestyleType optionsLifestyle = LifestyleType.Scoped)
            where TContext : CosmosDbContext
        {
            if (containerBuilder == null)
                throw new InvalidOperationException($"{nameof(containerBuilder)} is null.");

            if (contextLifestyle == LifestyleType.Singleton)
            {
                optionsLifestyle = LifestyleType.Singleton;
            }

            containerBuilder.Register(
                Component.For<CosmosDbOptions<TContext>>()
                    .UsingFactoryMethod(kernel => CosmosDbOptionsFactory<TContext>(kernel, options))
                    .LifeStyle.Is(optionsLifestyle),
                Component.For<TContext>().ImplementedBy<TContext>().LifeStyle.Is(contextLifestyle)
            );
            return containerBuilder;
        }

        private static CosmosDbOptions<TContext> CosmosDbOptionsFactory<TContext>(
            IKernel kernel,
            Action<IKernel, CosmosDbOptionBuilder> optionsAction)
            where TContext : CosmosDbContext
        {
            var builder = new CosmosDbOptionBuilder<TContext>();

            optionsAction.Invoke(kernel, builder);

            return (CosmosDbOptions<TContext>)builder.Build();
        }
    }
}
