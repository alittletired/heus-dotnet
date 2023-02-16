using Heus.Core.DependencyInjection;
using Heus.Ddd.Application;
using Microsoft.Extensions.Options;

namespace Heus.Ddd.Internal;

internal class DefaultDbContextResolver : IDbContextResolver, ISingletonDependency
{
 private readonly IOptions<DddOptions> _options;

 public DefaultDbContextResolver(IOptions<DddOptions> options)
 {
  _options = options;
 }

 public Type Resolve(Type entityType)
 {
  return _options.Value.EntityDbContextMappings[entityType];
 }
}