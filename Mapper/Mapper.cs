using AutoMapper;
using Data.Entities;
using ViewModels;
using ViewModels.Data.ViewModels;

namespace Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ArtistViewModel, Artist>()
            .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name));

            CreateMap<SongViewModel, Song>()
            .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
            .ForMember(dest => dest.Genre, act => act.MapFrom(src => src.Genre))
            .ForMember(dest => dest.Year, act => act.MapFrom(src => src.Year));

            CreateMap<SongSpecificationViewModel, SongSpecification>()
            .ForMember(dest => dest.Beats, act => act.MapFrom(src => src.Beats))
            .ForMember(dest => dest.Energy, act => act.MapFrom(src => src.Energy))
            .ForMember(dest => dest.Danceability, act => act.MapFrom(src => src.Danceability))
            .ForMember(dest => dest.Valence, act => act.MapFrom(src => src.Valence))
            .ForMember(dest => dest.Length, act => act.MapFrom(src => src.Length))
            .ForMember(dest => dest.Acousticness, act => act.MapFrom(src => src.Acousticness));
        }

    }
}