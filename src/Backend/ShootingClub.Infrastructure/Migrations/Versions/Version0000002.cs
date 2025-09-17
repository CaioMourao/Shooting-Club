using FluentMigrator;

namespace ShootingClub.Infrastructure.Migrations.Versions
{
    [Migration(2, "Create table to save the refresh token")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            CreateTable("RefreshTokens")
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("FK_RefreshToken_Usuario_Id", "Usuarios", "Id");
        }
    }
}
