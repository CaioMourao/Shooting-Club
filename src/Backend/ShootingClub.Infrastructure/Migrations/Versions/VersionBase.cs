using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
namespace ShootingClub.Infrastructure.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
        {
            return Create.Table(table)
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CriadoEm").AsDateTime().NotNullable()
                .WithColumn("AtualizadoEm").AsDateTime().NotNullable()
                .WithColumn("Ativo").AsBoolean().NotNullable();
        }

        protected ICreateTableWithColumnSyntax CreateTptChildTable(string childTableName, string parentTableName)
        {
            return Create.Table(childTableName)
                .WithColumn("Id").AsInt32().PrimaryKey().ForeignKey(parentTableName, "Id");
        }
    }
}
