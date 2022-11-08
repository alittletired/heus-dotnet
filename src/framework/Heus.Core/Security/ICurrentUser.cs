
namespace Heus.Core.Security;
public interface ICurrentUser
{
    long? Id { get; }
    string Name { get; }
  
}
