using Heus.Core.DependencyInjection;
using Heus.Core;
using Heus.Ddd.Data.Filtering;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Data
{
    internal class DataFilter : IDataFilter, ISingletonDependency
    {
        private readonly ConcurrentDictionary<Type, object> _filters = new ConcurrentDictionary<Type, object>();

        private readonly IServiceProvider _serviceProvider;

        public DataFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public IDisposable Enable<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().Enable();
        }

        public IDisposable Disable<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().Disable();
        }

        public bool IsEnabled<TFilter>()
            where TFilter : class
        {
            return GetFilter<TFilter>().IsEnabled;
        }

        private IDataFilter<TFilter> GetFilter<TFilter>()
            where TFilter : class
        {
            return _filters.GetOrAdd(
                typeof(TFilter), () => _serviceProvider.GetRequiredService<IDataFilter<TFilter>>()
            ) as IDataFilter<TFilter>;
        }
    }

    internal class DataFilter<TFilter> : IDataFilter<TFilter>
        where TFilter : class
    {
        private readonly AsyncLocal<DataFilterState> _filter = new AsyncLocal<DataFilterState>();
        public bool IsEnabled => Current.IsEnabled;

        protected DataFilterState Current
        {
            get
            {
                if (_filter.Value != null)
                {
                    return _filter.Value;
                }

                _filter.Value = new DataFilterState(true);
                return _filter.Value;
            }
        }
        public IDisposable Enable()
        {
            if (IsEnabled)
            {
                return NullDisposable.Instance;
            }

            Current.IsEnabled = true;

            return DisposeAction.Create(() => Disable());
        }

        public IDisposable Disable()
        {
            if (!IsEnabled)
            {
                return NullDisposable.Instance;
            }

            Current.IsEnabled = false;

            return DisposeAction.Create(() => Enable());
        }


    }
}
