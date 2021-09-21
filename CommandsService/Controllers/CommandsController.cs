using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController: ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo= commandRepo;
            _mapper= mapper;
        }
        [HttpGet]
        public ActionResult <IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine("--> Getting Commands For Platform from Command Services");
            Console.WriteLine($"-->Hit GetCommandsForPlatform:{platformId}");
            if(!_commandRepo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var commandsForPlatformItems= _commandRepo.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandsForPlatformItems));
        }

        [HttpGet("{commandId}", Name ="GetCommand")]
        public ActionResult <CommandReadDto> GetCommand(int commandId, int platformId)
        {
            Console.WriteLine($"--> Get Command by ID {commandId} in a specific Platform Id {platformId}");
            if(!_commandRepo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var commandItem= _commandRepo.GetCommand(commandId, platformId);
            if(commandItem==null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
            if(!_commandRepo.PlatformExist(platformId))
            {
                return NotFound();
            }
            var command =_mapper.Map<Command>(commandDto);
            _commandRepo.CreateCommandForPlatform(platformId, command);
            _commandRepo.SaveChanges();

            var commandReadDto= _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandsForPlatform),
            new {platformId=platformId, CommandId = commandReadDto.Id},commandReadDto);
        }
    }
}