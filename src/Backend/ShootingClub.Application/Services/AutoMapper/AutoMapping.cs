using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Entities;

namespace ShootingClub.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestUsuarioJson, Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.Ignore())
                .ForMember(dest => dest.CPF, opt => opt.Ignore());

            CreateMap<RequestClubeJson, Clube>()
                .ForMember(dest => dest.CNPJ, opt => opt.Ignore());

            CreateMap<RequestArmaBaseJson, ArmaBase>()
                .Include<RequestArmaExercitoJson, ArmaExercito>()
                .Include<RequestArmaPFJson, ArmaPF>()
                .Include<RequestArmaPorteJson, ArmaPortePessoal>();

            CreateMap<RequestArmaExercitoJson, ArmaExercito>();
            CreateMap<RequestArmaPFJson, ArmaPF>();
            CreateMap<RequestArmaPorteJson, ArmaPortePessoal>();
        }

        private void DomainToResponse()
        {
            CreateMap<Clube, ResponseClubeProfileJson>();

            CreateMap<Usuario, ResponseUsuarioProfileJson>();
            CreateMap<Usuario, ResponseUsuarioShortJson>();
            CreateMap<Usuario, ResponseUsuarioJson>();

            CreateMap<ArmaBase, ResponseRegisteredArmaJson>();
            CreateMap<ArmaBase, ResponseArmaBaseJson>()
                .Include<ArmaExercito, ResponseArmaExercitoJson>()
                .Include<ArmaPF, ResponseArmaPFJson>()
                .Include<ArmaPortePessoal, ResponseArmaPortePessoalJson>()
                .ForMember(dest => dest.TipoPosse, opt => opt.MapFrom(src => src.TipoPosse.ToString()));

            CreateMap<ArmaExercito, ResponseArmaExercitoJson>();
            CreateMap<ArmaPF, ResponseArmaPFJson>();
            CreateMap<ArmaPortePessoal, ResponseArmaPortePessoalJson>();

            CreateMap<ArmaBase, ResponseArmaShortJson>()
                .Include<ArmaExercito, ResponseArmaExercitoShortJson>()
                .Include<ArmaPF, ResponseArmaPFShortJson>()
                .Include<ArmaPortePessoal, ResponseArmaPortePessoalShortJson>()
                .ForMember(dest => dest.TipoPosse, opt => opt.MapFrom(src => src.TipoPosse.ToString()));

            CreateMap<ArmaExercito, ResponseArmaExercitoShortJson>();
            CreateMap<ArmaPF, ResponseArmaPFShortJson>();
            CreateMap<ArmaPortePessoal, ResponseArmaPortePessoalShortJson>();
        }
    }
}