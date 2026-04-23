using BlackBarberAPI.Process;
using BlackBarberAPI.Services;
using BlackBarberAPI.Services.Contratos;

namespace BlackBarberAPI.Utilidades
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddAutoMapper(typeof(AutoMapperProfile));

            #region Servicios

            services.AddScoped(typeof(IUsuarioService<>), typeof(UsuarioService<>));
            services.AddScoped(typeof(IAnadidoServicioService<>), typeof(AnadidoServicioService<>));
            services.AddScoped(typeof(IBarberoService<>), typeof(BarberoService<>));
            services.AddScoped(typeof(ICitaService<>), typeof(CitaService<>));
            services.AddScoped(typeof(IDetalleCitaService<>), typeof(DetalleCitaService<>));
            services.AddScoped(typeof(IPerfumeService<>), typeof(PerfumeService<>));
            services.AddScoped(typeof(IPreferenciasClienteService<>), typeof(PreferenciasClienteService<>));
            services.AddScoped(typeof(IPromocionService<>), typeof(PromocionService<>));
            services.AddScoped(typeof(IResenaService<>), typeof(ResenaService<>));
            services.AddScoped(typeof(IRolService<>), typeof(RolService<>));
            services.AddScoped(typeof(IServicioService<>), typeof(SercicioService<>));
            services.AddScoped(typeof(IServicioCitaService<>), typeof(ServicioCitaService<>));
            services.AddScoped(typeof(ITipoServicioService<>), typeof(TipoServicioService<>));

            #endregion

            #region Procesos

            services.AddScoped(typeof(UsuarioProceso));
            services.AddScoped(typeof(PasswordEncrtyption));
            services.AddScoped(typeof(BarberoProceso));
            services.AddScoped(typeof(PerfumeProceso));
            services.AddScoped(typeof(PromocionProceso));

            #endregion

        }
    }
}
