using FluentMigrator;
using Microsoft.Extensions.Configuration;
using ShootingClub.Infrastructure.Security.Cryptography;
namespace ShootingClub.Infrastructure.Migrations.Versions
{
    [Migration(3, "Create table to save the refresh token")]
    public class Version0000003 : VersionBase
    {
        private readonly IConfiguration _configuration;

        public Version0000003(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Up()
        {
            var adminNome = _configuration.GetValue<string>("Settings:AdminUser:Nome");
            var adminEmail = _configuration.GetValue<string>("Settings:AdminUser:Email");
            var adminSenha = _configuration.GetValue<string>("Settings:AdminUser:Senha");
            var adminCpf = _configuration.GetValue<string>("Settings:AdminUser:Cpf");
            var adminDataNascimento = _configuration.GetValue<string>("Settings:AdminUser:DataNascimento");
            var adminEnderecoPais = _configuration.GetValue<string>("Settings:AdminUser:EnderecoPais");
            var adminEnderecoEstado = _configuration.GetValue<string>("Settings:AdminUser:EnderecoEstado");
            var adminEnderecoCidade = _configuration.GetValue<string>("Settings:AdminUser:EnderecoCidade");
            var adminEnderecoBairro = _configuration.GetValue<string>("Settings:AdminUser:EnderecoBairro");
            var adminEnderecoRua = _configuration.GetValue<string>("Settings:AdminUser:EnderecoRua");
            var adminEnderecoNumero = _configuration.GetValue<string>("Settings:AdminUser:EnderecoNumero");
            var adminCr = _configuration.GetValue<string>("Settings:AdminUser:Cr");
            var adminDataVencimentoCr = _configuration.GetValue<string>("Settings:AdminUser:DataVencimentoCR");
            var adminSFPCVinculacao = _configuration.GetValue<string>("Settings:AdminUser:SfpcVinculacao");
            var adminNumeroFiliacao = _configuration.GetValue<string>("Settings:AdminUser:NumeroFiliacao");
            var adminDataFiliacao = _configuration.GetValue<string>("Settings:AdminUser:DataFiliacao");
            var adminDataRenovacaoFiliacao = _configuration.GetValue<string>("Settings:AdminUser:DataRenovacaoFiliacao");


            var passwordEncrypter = new BCryptNet();
            var hashedPassword = passwordEncrypter.Encrypt(adminSenha!);

            Insert.IntoTable("Usuarios")
                .Row(new
                {

                    CriadoEm = DateTime.UtcNow,
                    AtualizadoEm = DateTime.UtcNow,
                    Ativo = 1,
                    Nivel = 2,
                    Nome = adminNome,
                    Email = adminEmail,
                    Senha = hashedPassword,
                    CPF = adminCpf,
                    DataNascimento = adminDataNascimento,
                    EnderecoPais = adminEnderecoPais,
                    EnderecoEstado = adminEnderecoEstado,
                    EnderecoCidade = adminEnderecoCidade,
                    EnderecoBairro = adminEnderecoBairro,
                    EnderecoRua = adminEnderecoRua,
                    EnderecoNumero = adminEnderecoNumero,
                    CR = adminCr,
                    DataVencimentoCR = adminDataVencimentoCr,
                    NumeroFiliacao = adminNumeroFiliacao,
                    DataFiliacao = adminDataFiliacao,
                    DataRenovacaoFiliacao = adminDataRenovacaoFiliacao,
                    SFPCVinculacao = adminSFPCVinculacao,
                    IdentificadorUsuario = Guid.NewGuid(),
                    ClubeId = 0
                });
        }
    }
}
