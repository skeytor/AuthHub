﻿using AuthHub.Domain.Entities;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Persistence.IntegrationTest.Initialization;

public static class SampleDataInitializer
{
    internal static void ClearData(AppDbContext context)
    {
        string[] entities =
        [
            typeof(Role).Name,
            typeof(User).Name,
        ];
        foreach (string entityName in entities)
        {
            IEntityType entity = context.Model.FindEntityType(entityName)!;
            string tableName = entity.GetTableName()!;
            string schemaName = entity.GetSchema()!;
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
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
                using var transaction = context.Database.BeginTransaction();
                IEntityType metaData = context.Model.FindEntityType(typeof(TEntity).FullName!)!;
                IProperty? primaryKey = metaData
                    .FindPrimaryKey()!
                    .Properties
                    .FirstOrDefault(p => p.ClrType == typeof(int));
                if (primaryKey is not null)
                {
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
                    table.AddRange(records);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
                    transaction.Commit();
                }
                else
                {
                    table.AddRange(records);
                    context.SaveChanges();
                    transaction.Commit();
                }
            });
        }
    }
    public static void ClearAndReseedDatabase(AppDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }

}
