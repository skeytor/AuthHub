using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthHub.Api.IntegrationTest.Initialization;

public static class SampleDataInitializer
{
    internal static void ClearData(AppDbContext context)
    {
        string?[] entities =
        [
            typeof(User).FullName,
            typeof(Role).FullName,
        ];
        foreach (var entityName in entities)
        {
            var entity = context.Model.FindEntityType(entityName!);
            var tableName = entity?.GetTableName();
            var schemaName = entity?.GetSchema();
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            //context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\"),);
        }
    }
    internal static void SeedData(AppDbContext context)
    {
        ProcessInsert(context, context.Roles, SampleData.Roles);
        ProcessInsert(context, context.Users, SampleData.Users);
        static void ProcessInsert<TEntity>(
            AppDbContext context, DbSet<TEntity> table, List<TEntity> records) 
            where TEntity : class
        {
            if (table.Any())
            {
                return;
            }
            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaccion = context.Database.BeginTransaction();
                var metaData = context.Model.FindEntityType(typeof(TEntity).FullName!);
                context.Database.ExecuteSqlRaw(
                    $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} O");
                table.AddRange(records);
                context.SaveChanges();
                context.Database.ExecuteSqlRaw(
                    $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} O");
                transaccion.Commit();
            });
        }
    }
    public static void ClearAndReseedDatabase(AppDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
}