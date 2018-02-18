using AutoMapper;

namespace Diff.Mapper
{
    /// <summary>
    /// AutoMapper configurations corresponding to current assembly.
    /// This <see cref="Profile"/> should be loaded into <see cref="AutoMapperConfig"/> class.
    /// </summary>
    public sealed class DiffMapperProfile : Profile
    {
        /// <summary>
        /// Initialize a new instance of <see cref="DiffMapperProfile"/>.
        /// </summary>
        public DiffMapperProfile()
        {
            // This mapping demonstrates some separation between domain and View.
            // Here the Models.Diff.DiffResult converts a enum value to a string,
            // simulating a supposed adaptation needed to be sent to the client.
            CreateMap<Domain.DiffResult, Models.Diff.DiffResult>()
                .ForMember(m => m.Status, p => p.MapFrom(x => x.Status.ToString()));
            CreateMap<Domain.DiffBlock, Models.Diff.DiffBlock>();
        }
    }
}