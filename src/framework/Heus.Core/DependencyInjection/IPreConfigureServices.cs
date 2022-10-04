﻿namespace Heus.Core.DependencyInjection;

public interface IPreConfigureServices
{
    void PreConfigureServices(ServiceConfigurationContext context);
}
public interface IPostConfigureServices
{
    void PostConfigureServices(ServiceConfigurationContext context);
}