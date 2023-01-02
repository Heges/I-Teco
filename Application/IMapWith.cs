using AutoMapper;

namespace Application
{
    public interface IMapWith<T>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}
