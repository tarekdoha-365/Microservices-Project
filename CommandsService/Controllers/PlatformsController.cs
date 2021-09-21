using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo= commandRepo;
            _mapper=mapper;
        }
        [HttpGet]
        public ActionResult <IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting All Platform From Command Service");
            var PlateformItems = _commandRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(PlateformItems));
        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}