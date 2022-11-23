using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;
[CollectionDefinition(nameof(IntegratedTestCollection))]
public class IntegratedTestCollection: ICollectionFixture<IntegratedTest<Program>>
{
}
