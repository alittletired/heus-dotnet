using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Data;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Ddd.Internal;

internal class DefaultDbContextProvider : IDbContextProvider, IScopedDependency
{
    private readonly IOptions<DddOptions> _options;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DefaultDbContextProvider(IOptions<DddOptions> options
        , IUnitOfWorkManager unitOfWorkManager)
    {
        _options = options;
        _unitOfWorkManager= unitOfWorkManager;
     
    }


    private static DbContext CreateDbContext(IServiceProvider serviceProvider,Type dbContextType)
    {
        var optionFactory = serviceProvider.GetRequiredService<IDbContextOptionsFactory>();
        var options = optionFactory.CreateOptions(dbContextType);
        return  (DbContext)Activator.CreateInstance(dbContextType, options )!;
    }

 
    public  DbContext CreateDbContext<TEntity>() where TEntity : IEntity
    {
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
      
        if (_unitOfWorkManager.Current == null)
        {
            throw new BusinessException("A DbContext can only be created inside a unit of work!");
        }
        return _unitOfWorkManager.Current.AddDbContext(dbContextType.Name, (key) =>
        {
            return CreateDbContext(_unitOfWorkManager.Current.ServiceProvider, dbContextType);
        });
      
    }
  
}
