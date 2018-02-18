using AutoMapper;
using Diff.Domain;
using Diff.Domain.Repository;
using Diff.IoC;
using Diff.Mapper;
using Diff.Repository;
using System.Web.Http.Dependencies;
using Unity;

namespace Diff.Ioc
{
    /// <summary>
    /// Register dependencies that are going to be used along application.
    /// </summary>
    public sealed class DependencyRegistrations
    {
        /// <summary>
        /// Generate the global dependecy resolver that are going to be used along application.
        /// </summary>
        /// <returns>The dependency resolver.</returns>
        public static IDependencyResolver GetDependencyResolver()
        {
            var container = new UnityContainer();

            container.RegisterType<IDiffAnalyser, DiffAnalyser>();
            container.RegisterType<IDiffRepositoryManager, DiffRepositoryManager>();
            container.RegisterType<IRepository, FakeRepository>();
            container.RegisterInstance(typeof(IMapper), AutoMapperConfig.GetMappings());

            return new UnityResolver(container);
        }
    }
}