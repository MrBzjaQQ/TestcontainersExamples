namespace ApiTestsExamples.Infrastructure;

[CollectionDefinition(nameof(ApiTestsCollection))]
public class ApiTestsCollection: ICollectionFixture<ApiTestsFixture>
{
}