namespace TestIsolationStrategies.xUnit.PerCollection.Infrastructure;

[CollectionDefinition(nameof(DatabaseCollection))]
public class DatabaseCollection : ICollectionFixture<TargetDbFixture> { }