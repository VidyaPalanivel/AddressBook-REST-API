using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AddressBook.Util;

namespace AddressBook.Entities.Models
{
    public static class Common
    {
        public static void OnModelCreation(ModelBuilder modelBuilder)
        {
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ConvertToSnakeCase());
                var storeObjectIdentifier = StoreObjectIdentifier.Table(entity.GetTableName(), entity.GetSchema());
                foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in entity.GetProperties())
                {

                    property.SetColumnName(property.GetColumnName(storeObjectIdentifier).ConvertToSnakeCase());
                }

                foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableKey key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ConvertToSnakeCase());
                }

                foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ConvertToSnakeCase());
                }

                foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableIndex index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ConvertToSnakeCase());
                }
            }
        }
    }
}