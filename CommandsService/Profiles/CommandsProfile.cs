using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile: Profile
    {
        public CommandsProfile()
        {
            //Source --> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto,Command>();
            CreateMap<Platform,PlatformReadDto>();
            CreateMap<PlatformReadDto,Platform>();
            CreateMap<PlatformPublishedDto,Platform>()
            //destination To Member
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src =>src.Id));
        }
    }
}