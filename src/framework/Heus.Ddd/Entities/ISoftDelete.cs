namespace Heus.Ddd.Entities;

public interface ISoftDelete
{
    /// <summary>
    /// 软删除标记
    /// </summary>
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
}
