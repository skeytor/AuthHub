using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Persistence.IntegrationTest.Initialization;

internal static class DataInitializer
{
    internal static void ClearData(AppDbContext context)
    {
        var entities = new[]
        {
            typeof(Role).FullName,
            typeof(User).FullName,
        };
        foreach (var entityName in entities)
        {
            IEntityType entity = context.Model.FindEntityType(entityName!)!;
            string tableName = entity.GetTableName()!;
            string schemaName = entity.GetSchema()!;
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            //context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
        }
    }
    internal static void ReseedData(AppDbContext context)
    {
        ProcessInsert(context, context.Roles, SampleData.Roles);
        ProcessInsert(context, context.Users, SampleData.Users);
        static void ProcessInsert<TEntity>(
            AppDbContext context, 
            DbSet<TEntity> table, 
            List<TEntity> records) where TEntity : class
        {
            if (table.Any())
            {
                return;
            }
            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transacction = context.Database.BeginTransaction();
                table.AddRange(records);
                context.SaveChanges();
                transacction.Commit();
            });
        }
    }
    public static void ClearAndReseedDatabase(AppDbContext context)
    {
        ClearData(context);
        ReseedData(context);
    }
}
