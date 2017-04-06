﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Scrutor
{
    internal class AssemblySelector : IAssemblySelector, ISelector
    {
        protected List<ISelector> Selectors { get; } = new List<ISelector>();

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssemblyOf<T>()
        {
            return InternalFromAssembliesOf(new[] { typeof(T).GetTypeInfo() });
        }

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssembliesOf(params Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            return InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
        }

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssembliesOf(IEnumerable<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            return InternalFromAssembliesOf(types.Select(t => t.GetTypeInfo()));
        }

        private IImplementationTypeSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos)
        {
            return InternalFromAssemblies(typeInfos.Select(t => t.Assembly));
        }

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            return InternalFromAssemblies(assemblies);
        }

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            return InternalFromAssemblies(assemblies);
        }

        private IImplementationTypeSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return AddSelector(assemblies.SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType()));
        }

        void ISelector.Populate(IServiceCollection services, SelectorOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            foreach (var selector in Selectors)
            {
                selector.Populate(services, options);
            }
        }

        private IImplementationTypeSelector AddSelector(IEnumerable<Type> types)
        {
            var selector = new ImplementationTypeSelector(types);

            Selectors.Add(selector);

            return selector;
        }
    }
}
