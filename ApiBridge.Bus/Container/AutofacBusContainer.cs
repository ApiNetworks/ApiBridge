﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ApiBridge.Bus.Interfaces;
using ApiBridge.Bus.Core;

namespace ApiBridge.Bus.Container
{
    public class AutofacBusContainer : IBusContainer
    {
        IContainer container;
        ContainerBuilder builder = new ContainerBuilder();


        public AutofacBusContainer()
        {
            this.container = builder.Build();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">Optional autofac container owned by the calling application.
        /// If one is not provided, a new one will be created.</param>
        public AutofacBusContainer(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Resolve component type of T with optional arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return container.Resolve<T>();
        }

        /// <summary>
        /// Resolve component with optional arguments.
        /// </summary>
        /// <param name="t">The type to resolve.</param>
        /// <returns></returns>
        public object Resolve(Type t)
        {
            return container.Resolve(t);
        }

        /// <summary>
        /// Register an implementation for a service type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="perInstance">True creates an instance each time resolved.  False uses a singleton instance for the entire lifetime of the process.</param>
        public void Register(Type serviceType, Type implementationType, bool perInstance = false)
        {
            var reg = builder.RegisterType(implementationType).As(serviceType);
            if (perInstance)
            {
                reg.InstancePerDependency();
            }
            else
            {
                reg.SingleInstance();
            }
        }

        /// <summary>
        /// Registers the configuration instance with the bus if it is not already registered
        /// </summary>
        public void RegisterConfiguration()
        {
            if (!IsRegistered(typeof(IBusConfiguration)))
            {
                builder.Register(item => BusConfiguration.Instance).As<IBusConfiguration>().SingleInstance();
            }
        }

        /// <summary>
        /// Build the container if needed.
        /// </summary>
        public void Build()
        {
            if (container == null)
            {
                container = builder.Build();
            }
            else
            {
                builder.Update(container);
            }
            builder = new ContainerBuilder();
        }

        /// <summary>
        /// Return true if the given type is registered with the container.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsRegistered(Type type)
        {
            return container != null && container.IsRegistered(type);
        }
    }
}
