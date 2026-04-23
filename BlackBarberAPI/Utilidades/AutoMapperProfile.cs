using AutoMapper;
using BlackBarberAPI.DTOs;
using BlackBarberAPI.Models;

namespace BlackBarberAPI.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Usuario

            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.Barberos, opt => opt.Ignore())
                .ForMember(destino => destino.Cita, opt => opt.Ignore())
                .ForMember(destino => destino.IdRolNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.PreferenciasClientes, opt => opt.Ignore())
                .ForMember(destino => destino.Resenas, opt => opt.Ignore());

            #endregion

            #region Barbero

            CreateMap<Barbero, BarberoDTO>();
            CreateMap<BarberoDTO, Barbero>()
                .ForMember(destino => destino.IdUsuarioNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.BarberoServicios, opt => opt.Ignore());

            #endregion

            #region Rol

            CreateMap<Rol, RolDTO>();
            CreateMap<RolDTO, Rol>()
                .ForMember(destino => destino.Usuarios, opt => opt.Ignore());

            #endregion

            #region Cita

            CreateMap<Citum, CitaDTO>();
            CreateMap<CitaDTO, Citum>()
                .ForMember(destino => destino.Resenas, opt => opt.Ignore())
                .ForMember(destino => destino.IdClienteNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.ServicioCita, opt => opt.Ignore());

            #endregion

            #region Perfume

            CreateMap<Perfume, PerfumeDTO>();
            CreateMap<PerfumeDTO, Perfume>()
                .ForMember(destino => destino.PreferenciasClientes, opt => opt.Ignore());

            #endregion

            #region Promocion

            CreateMap<Promocion, PromocionDTO>();
            CreateMap<PromocionDTO, Promocion>();

            #endregion

            #region Servicio

            CreateMap<Servicio, ServicioDTO>();
            CreateMap<ServicioDTO, Servicio>()
                .ForMember(destino => destino.BarberoServicios, opt => opt.Ignore())
                .ForMember(destino => destino.ServicioCita, opt => opt.Ignore())
                .ForMember(destino => destino.IdTipoNavigation, opt => opt.Ignore());

            #endregion

            #region Consulta

            CreateMap<Consultas, ConsultaDTO>();
            CreateMap<ConsultaDTO, Consultas>();

            #endregion

            #region DiasHabiles

            CreateMap<DiasHabiles, DiasHabilDTO>();
            CreateMap<DiasHabilDTO, DiasHabiles>();

            #endregion

        }
    }
}
