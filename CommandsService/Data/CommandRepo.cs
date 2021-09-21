using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context= context;
        }

        public void CreateCommandForPlatform(int platformId, Command command)
        {
            if(command==null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId=platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform==null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
                _context.Platforms.Add(platform);
                _context.SaveChanges();
        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return _context.Platforms.Any(x=> x.ExternalId == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int commandId, int platformId)
        {
            return _context.Commands.Where(c=>c.PlatformId==platformId).FirstOrDefault(x=>x.Id==commandId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(x=>x.PlatformId== platformId)
            .OrderBy(n=>n.Platform.Name);           
        }

        public bool PlatformExist(int platformId)
        {
            return _context.Platforms.Any(x=>x.Id==platformId);
        }

        public bool SaveChanges()
        {
            return ( _context.SaveChanges() >= 0);
        }
    }
}