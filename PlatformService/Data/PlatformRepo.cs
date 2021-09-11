using System;
using System.Collections.Generic;
using System.Linq;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context=context;
        }
        public Platform AddPlatform(Platform platform)
        {
            if(platform==null)
            {
                throw new ArgumentNullException(nameof(Platform));
            }
            _context.Platforms.Add(platform);
            SaveChanges();
            return platform;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.Id == id);
        }
        public int DeletePlatform(Platform platform)
        {
            var platformItem=_context.Platforms.Remove(platform);
            _context.SaveChanges();
            if(platform!=null){
             return platform.Id;
            }
             return 0;
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}