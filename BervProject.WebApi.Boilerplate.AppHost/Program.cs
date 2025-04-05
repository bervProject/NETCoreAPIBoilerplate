var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache").WithRedisInsight();
var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var postgresdb = postgres.AddDatabase("postgresdb");
var serviceBus = builder.AddAzureServiceBus("messaging").RunAsEmulator();
var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var blobs = storage.AddBlobs("blobs");
var queues = storage.AddQueues("queues");
var tables = storage.AddTables("tables");

var migration = builder.AddProject<Projects.BervProject_WebApi_Boilerplate_MigrationService>("migrations")
    .WithReference(postgresdb, connectionName: "BoilerplateConnectionString")
    .WithExplicitStart();

builder.AddProject<Projects.BervProject_WebApi_Boilerplate>("apiservice")
    .WithHttpEndpoint()
    .WithReference(cache, connectionName: "Redis")
    .WithReference(postgresdb, connectionName: "BoilerplateConnectionString")
    .WithReference(blobs, connectionName: "AzureStorageBlob")
    .WithReference(queues, connectionName: "AzureStorageQueue")
    .WithReference(tables, connectionName: "AzureStorageTable")
    .WithReference(serviceBus, connectionName: "AzureServiceBus")
    .WaitFor(cache)
    .WaitFor(postgresdb)
    .WaitFor(blobs)
    .WaitFor(queues)
    .WaitFor(tables)
    .WaitFor(serviceBus)
    .WaitForCompletion(migration);

builder.Build().Run();
