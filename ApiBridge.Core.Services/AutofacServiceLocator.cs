﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Collections;
using Microsoft.Practices.ServiceLocation;

namespace ApiBridge.Core.Services
{
    public sealed class AutofacServiceLocator : ServiceLocatorImplBase
    {
        readonly IComponentContext _container;

        public AutofacServiceLocator(IComponentContext container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            _container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null ? _container.ResolveNamed(key, serviceType) : _container.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);

            object instance = _container.Resolve(enumerableType);
            return ((IEnumerable)instance).Cast<object>();
        }
    }

}
