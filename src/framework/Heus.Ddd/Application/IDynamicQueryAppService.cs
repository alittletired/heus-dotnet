namespace Heus.Ddd.Application;

public interface IDynamicQueryAppService<TQueryResult> :IDynamicQueryAppService<TQueryResult,TQueryResult>
{
    
}
public interface IDynamicQueryAppService<TQueryResult, in TQueryInput>:IApplicationService
{
    Task<PagedResultDto<TQueryResult>> GetListAsync(TQueryInput input);
    
}