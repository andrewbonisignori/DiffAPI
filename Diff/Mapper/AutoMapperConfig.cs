using AutoMapper;

namespace Diff.Mapper
{
    /// <summary>
    /// AutoMapper configurations to be used along entire application.
    /// Contains mapping for current assembly and for dependent assemblies.
    /// Each assembly should have their specific <see cref="Profile"/> loaded.
    /// </summary>
    public sealed class AutoMapperConfig
    {
        public static IMapper GetMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DiffMapperProfile());
                cfg.AddProfile(new DiffDomainMapperProfile());
            });

            return config.CreateMapper();
        }
    }
}