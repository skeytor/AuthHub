using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthHub.Api.IntegrationTest.Initialization;

/// <summary>
/// Provides methods for clearing and seeding data in the database
/// Used for setting up and resetting the database for integration tests or initial data setup
/// </summary>
public static class SampleDataInitializer
{
    /// <summary>
    /// Clears all data from the specified tables in the database.
    /// Delete all rows from tables and reseeds identity columns if the primary keys are a integer.
    /// </summary>
    /// <param name="context">The database context to perform the data deletion on.</param>
    internal static void ClearData(AppDbContext context)
    {
        string?[] entities =
        [
            typeof(User).FullName,
            typeof(Role).FullName,
            typeof(Permission).FullName,
            typeof(RolePermission).FullName,
        ];
        foreach (var entityName in entities)
        {
            IEntityType? entityMetaData = context.Model.FindEntityType(entityName!);
            var tableName = entityMetaData?.GetTableName();
            var schemaName = entityMetaData?.GetSchema();

            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");

            bool isGuidKey = HasGuidIdentityColumn(entityMetaData!);

            bool isCompositeKey = entityMetaData?.FindPrimaryKey()!.Properties.Count > 1;

            if (!isGuidKey && !isCompositeKey)
            {
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
            }
        }
    }
    /// <summary>
    /// Inserts seed data into the specific tables if no data currently exists.
    /// If the table uses and identity column (non-GUID primary key), identiy inser is enable before insertng the data.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    internal static void SeedData(AppDbContext context)
    {
        ProcessInsert(context, context.Permissions, SampleData.Permissions);
        ProcessInsert(context, context.Roles, SampleData.Roles);
        ProcessInsert(context, context.RolePermissions, SampleData.RolePermissions);
        ProcessInsert(context, context.Users, SampleData.Users);

        /// <summary>
        /// Inserts a list of records into the specified table.
        /// Automatically handles identity insert for tables with non-GUID primary keys.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being inserted.</typeparam>
        /// <param name="context">The database context to insert data into.</param>
        /// <param name="table">The database set representing the table to insert data into.</param>
        /// <param name="records">The list of records to insert into the table.</param>
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
                bool hasGuidIdentityColumn = HasGuidIdentityColumn(metaData!);
                bool isCompositeKey = metaData?.FindPrimaryKey()!.Properties.Count > 1;
                if (!hasGuidIdentityColumn && !isCompositeKey)
                {
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData?.GetSchema()}.{metaData?.GetTableName()} ON");
                }
                table.AddRange(records);
                context.SaveChanges();

                if (!hasGuidIdentityColumn && !isCompositeKey)
                {
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData?.GetSchema()}.{metaData?.GetTableName()} OFF");
                }
                transaccion.Commit();
            });
        }
    }
    private static bool HasGuidIdentityColumn(IEntityType entity) 
        => entity.FindPrimaryKey()!.Properties[0].ClrType == typeof(Guid);

    private static bool HasIdentityColumn(IEntityType entity) =>
        entity?.FindPrimaryKey()!.Properties.Any(p => p.ValueGenerated == ValueGenerated.OnAdd) ?? false;

    /// <summary>
    /// Clears all data and reseeds the database with initial sample data.
    /// This method combines both the data clearing and seeding functions.
    /// </summary>
    /// <param name="context">The database context to reset and reseed.</param>
    public static void ClearAndReseedDatabase(AppDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
}