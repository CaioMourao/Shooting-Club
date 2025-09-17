using FluentMigrator;

namespace ShootingClub.Infrastructure.Migrations.Versions
{
    [Migration(1, "create all database tables")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Usuarios")
                .WithColumn("Nivel").AsInt32().NotNullable()
                .WithColumn("Nome").AsString(100).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable()
                .WithColumn("Senha").AsString(2000).NotNullable()
                .WithColumn("CPF").AsString(20).NotNullable()
                .WithColumn("DataNascimento").AsDate().NotNullable()
                .WithColumn("EnderecoPais").AsString(50).NotNullable()
                .WithColumn("EnderecoEstado").AsString(50).NotNullable()
                .WithColumn("EnderecoCidade").AsString(50).NotNullable()
                .WithColumn("EnderecoBairro").AsString(100).NotNullable()
                .WithColumn("EnderecoRua").AsString(100).NotNullable()
                .WithColumn("EnderecoNumero").AsString(50).NotNullable()
                .WithColumn("CR").AsString(100).Nullable()
                .WithColumn("DataVencimentoCR").AsDate().Nullable()
                .WithColumn("SFPCVinculacao").AsString(100).Nullable()
                .WithColumn("NumeroFiliacao").AsString(100).NotNullable()
                .WithColumn("DataFiliacao").AsDate().NotNullable()
                .WithColumn("DataRenovacaoFiliacao").AsDate().NotNullable()
                .WithColumn("ClubeId").AsInt32().Nullable()
                .WithColumn("IdentificadorUsuario").AsGuid().NotNullable();

            CreateTable("Clubes")
                .WithColumn("Nome").AsString(100).NotNullable()
                .WithColumn("CNPJ").AsString(20).NotNullable()
                .WithColumn("CertificadoRegistro").AsString(100).NotNullable()
                .WithColumn("EnderecoPais").AsString(50).NotNullable()
                .WithColumn("EnderecoEstado").AsString(50).NotNullable()
                .WithColumn("EnderecoCidade").AsString(50).NotNullable()
                .WithColumn("EnderecoBairro").AsString(50).NotNullable()
                .WithColumn("EnderecoRua").AsString(50).NotNullable()
                .WithColumn("EnderecoNumero").AsString(50).NotNullable()
                .WithColumn("ResponsavelId").AsInt32().NotNullable();

            CreateTable("Armas")
                .WithColumn("Tipo").AsString(150).NotNullable()
                .WithColumn("Marca").AsString(150).NotNullable()
                .WithColumn("Calibre").AsString(150).NotNullable()
                .WithColumn("NumeroSerie").AsString(50).NotNullable()
                .WithColumn("TipoPosse").AsInt32().NotNullable() 
                .WithColumn("UsuarioId").AsInt32().NotNullable()
                .WithColumn("ClubeId").AsInt32().NotNullable();

            CreateTptChildTable("ArmasExercito", "Armas")
                .WithColumn("NumeroSigma").AsString(50).NotNullable()
                .WithColumn("DataExpedicaoCRAF").AsDate().NotNullable()
                .WithColumn("ValidadeCRAF").AsDate().NotNullable()
                .WithColumn("LocalRegistro").AsString(100).NotNullable()
                .WithColumn("NumeroGTE").AsString(100).NotNullable()
                .WithColumn("ValidadeGTE").AsDate().NotNullable();

            CreateTptChildTable("ArmasPoliciaFederal", "Armas")
                .WithColumn("NumeroSinarm").AsString(100).NotNullable()
                .WithColumn("NumeroRegistroPF").AsString(100).NotNullable()
                .WithColumn("NumeroNotaFiscal").AsString(100).NotNullable()
                .WithColumn("DataValidadePF").AsDate().NotNullable();

            CreateTptChildTable("ArmasPortePessoal", "Armas")
                .WithColumn("NumeroCertificado").AsString(50).NotNullable()
                .WithColumn("DataCertificacao").AsDate().NotNullable()
                .WithColumn("ValidadeCertificacao").AsDate().NotNullable();

            CreateTable("Emprestimos")
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("ArmaId").AsInt32().NotNullable()
                .WithColumn("UsuarioOrigem").AsInt32().NotNullable()
                .WithColumn("UsuarioDestino").AsInt32().NotNullable()
                .WithColumn("DataAceite").AsDateTime().NotNullable()
                .WithColumn("CaminhoDocumentoAutorizacao").AsString(255).Nullable();

            CreateTable("Habitualidades")
                .WithColumn("TipoUsoArma").AsInt32().NotNullable()
                .WithColumn("Tipo").AsString(30).NotNullable()
                .WithColumn("NomeProva").AsString(100).Nullable()
                .WithColumn("QuantMunicao").AsInt32().NotNullable()
                .WithColumn("DataInicio").AsDateTime().NotNullable()
                .WithColumn("DataFim").AsDateTime().NotNullable()
                .WithColumn("UsuarioId").AsInt32().NotNullable()
                .WithColumn("ClubeId").AsInt32().NotNullable()
                .WithColumn("ArmaId").AsInt32().NotNullable();

            CreateTable("ItensInventarios")
                .WithColumn("TipoItem").AsString(100).Nullable()
                .WithColumn("Nome").AsString(100).Nullable()
                .WithColumn("Quantidade").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IdClube").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("Emprestimos").ForeignColumn("ArmaId")
                .ToTable("Armas").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Emprestimos").ForeignColumn("UsuarioOrigem")
                .ToTable("Usuarios").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Emprestimos").ForeignColumn("UsuarioDestino")
                .ToTable("Usuarios").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Habitualidades").ForeignColumn("ArmaId")
                .ToTable("Armas").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Habitualidades").ForeignColumn("ClubeId")
                .ToTable("Clubes").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Habitualidades").ForeignColumn("UsuarioId")
                .ToTable("Usuarios").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("ItensInventarios").ForeignColumn("IdClube")
                .ToTable("Clubes").PrimaryColumn("Id");
            Create.ForeignKey()
                .FromTable("Clubes").ForeignColumn("ResponsavelId")
                .ToTable("Usuarios").PrimaryColumn("Id");
        }
    }
}