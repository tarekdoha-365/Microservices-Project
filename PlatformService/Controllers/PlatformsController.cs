using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient )
        {
            _repository=platformRepo;
            _mapper=mapper;
            _commandDataClient= commandDataClient;
        }
        [HttpGet]
        public ActionResult <IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platformIems=_repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformIems));
        }
        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult <PlatformReadDto> GetPlatformById(int id)
        {
           
            var platformItem=_repository.GetPlatformById(id);
            if(platformItem!=null){
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task <ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel=_mapper.Map<Platform>(platformCreateDto);
            var addPlatformItem = _repository.AddPlatform(platformModel);
            _repository.SaveChanges();
            var platformReadDto=_mapper.Map<PlatformReadDto>(platformModel);
            try{
                await  _commandDataClient.SendPlatformToCommand(platformReadDto);
            }catch(Exception ex)
            {
             Console.WriteLine($"--> Could not send synchronously:{ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),new {Id=platformReadDto.Id},platformReadDto);
        }
        [HttpDelete]
        public ActionResult<int> DeletePlatform(Platform platform)
        {
            var deletedItem=_repository.DeletePlatform(platform);
            return deletedItem;
        }
    }
}