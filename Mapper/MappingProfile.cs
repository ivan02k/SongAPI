using AutoMapper;
using Data.Entities;
using Data.Entities.IdentityClass;
using ViewModels;
using ViewModels.Data.ViewModels;

namespace Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArtistViewModel, Artist>()
            .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name));
            CreateMap<Artist, ArtistViewModel>()
            .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name));

            CreateMap<SongViewModel, Song>()
            .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
            .ForMember(dest => dest.Genre, act => act.MapFrom(src => src.Genre))
            .ForMember(dest => dest.Year, act => act.MapFrom(src => src.Year));
            CreateMap<Song, SongViewModel>()
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
            CreateMap<SongSpecification, SongSpecificationViewModel>()
            .ForMember(dest => dest.Beats, act => act.MapFrom(src => src.Beats))
            .ForMember(dest => dest.Energy, act => act.MapFrom(src => src.Energy))
            .ForMember(dest => dest.Danceability, act => act.MapFrom(src => src.Danceability))
            .ForMember(dest => dest.Valence, act => act.MapFrom(src => src.Valence))
            .ForMember(dest => dest.Length, act => act.MapFrom(src => src.Length))
            .ForMember(dest => dest.Acousticness, act => act.MapFrom(src => src.Acousticness));

            CreateMap<ApplicationUser, RegisterRequest>()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username));
            CreateMap<RegisterRequest, ApplicationUser> ()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username));

            CreateMap<ApplicationUser, UpdateRequest>()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username));
            CreateMap<UpdateRequest, ApplicationUser>()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username));

            CreateMap<ApplicationUser, AuthenticateResponse>()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username))
            .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role));
            CreateMap<AuthenticateResponse, ApplicationUser>()
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Username))
            .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role));
        }

    }
}