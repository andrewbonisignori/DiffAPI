using AutoMapper;
using Diff.Domain.Repository;
using Diff.Repository;

namespace Diff.Mapper
{
    /// <summary>
    /// AutoMapper configurations corresponding to the current assembly.
    /// </summary>
    public sealed class DiffDomainMapperProfile : Profile
    {
        public DiffDomainMapperProfile()
        {
            // This mapping demonstrates some separation between repository and domain.
            CreateMap<InMemoryDiffData, DiffData>().ReverseMap();
        }
    }
}