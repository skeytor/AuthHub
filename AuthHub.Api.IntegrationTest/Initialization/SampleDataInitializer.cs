using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            typeof(Permission).FullName,
        ];
        foreach (var entityName in entities)
        {
            IEntityType? entity = context.Model.FindEntityType(entityName!);
            var tableName = entity?.GetTableName();
            var schemaName = entity?.GetSchema();

            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");

            bool isGuidKey = CheckGuidKey(entity!);

            if (!isGuidKey)
            {
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
            }
        }
    }
    internal static void SeedData(AppDbContext context)
    {
        ProcessInsert(context, context.Permissions, SampleData.Permissions);
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
                using IDbContextTransaction transaccion = context.Database.BeginTransaction();
                IEntityType? metaData = context.Model.FindEntityType(typeof(TEntity).FullName!);
                bool isGuidKey = CheckGuidKey(metaData!);
                if (!isGuidKey)
                {
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
                }
                table.AddRange(records);
                context.SaveChanges();

                if (!isGuidKey)
                {
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
                }
                transaccion.Commit();
            });
        }
    }
    private static bool CheckGuidKey(IEntityType metadata)
    {
        IProperty property = metadata.FindPrimaryKey()!.Properties[0];
        return property.ClrType == typeof(Guid);
    }
    public static void ClearAndReseedDatabase(AppDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
}