using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPIENTITYFRAMEWORK.Data;
using WEBAPIENTITYFRAMEWORK.Entities;
using WEBAPIENTITYFRAMEWORK.DTOs;

namespace WEBAPIENTITYFRAMEWORK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<SuperHeroController> _logger;

        public SuperHeroController(DataContext context, ILogger<SuperHeroController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDto>>> GetAllHeroes()
        {
            var heroes = await _context.SuperHeroes
                                       .Select(hero => new SuperHeroDto
                                       {
                                           Id = hero.Id,
                                           Name = hero.Name,
                                           FirstName = hero.FirstName,
                                           LastName = hero.LastName,
                                           Place = hero.Place
                                       })
                                       .ToListAsync();

            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHeroDto>> GetHero(int id)
        {
            var hero = await _context.SuperHeroes
                                     .Where(h => h.Id == id)
                                     .Select(h => new SuperHeroDto
                                     {
                                         Id = h.Id,
                                         Name = h.Name,
                                         FirstName = h.FirstName,
                                         LastName = h.LastName,
                                         Place = h.Place
                                     })
                                     .FirstOrDefaultAsync();

            if (hero == null)
            {
                _logger.LogWarning("Hero with id {Id} not found.", id);
                return NotFound("Hero not found.");
            }

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroDto>>> AddHero(SuperHeroDto heroDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hero = new SuperHero
            {
                Name = heroDto.Name,
                FirstName = heroDto.FirstName,
                LastName = heroDto.LastName,
                Place = heroDto.Place
            };

            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await GetHeroDtos());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHeroDto>>> UpdateHero(int id, SuperHeroDto updatedHeroDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dbHero = await _context.SuperHeroes.FindAsync(id);
            if (dbHero == null)
            {
                _logger.LogWarning("Hero with id {Id} not found.", id);
                return NotFound("Hero not found.");
            }

            dbHero.Name = updatedHeroDto.Name;
            dbHero.FirstName = updatedHeroDto.FirstName;
            dbHero.LastName = updatedHeroDto.LastName;
            dbHero.Place = updatedHeroDto.Place;

            await _context.SaveChangesAsync();

            return Ok(await GetHeroDtos());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHeroDto>>> DeleteHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                _logger.LogWarning("Hero with id {Id} not found.", id);
                return NotFound("Hero not found.");
            }

            _context.SuperHeroes.Remove(hero);
            await _context.SaveChangesAsync();

            return Ok(await GetHeroDtos());
        }

        private async Task<List<SuperHeroDto>> GetHeroDtos()
        {
            return await _context.SuperHeroes
                                 .Select(hero => new SuperHeroDto
                                 {
                                     Id = hero.Id,
                                     Name = hero.Name,
                                     FirstName = hero.FirstName,
                                     LastName = hero.LastName,
                                     Place = hero.Place
                                 })
                                 .ToListAsync();
        }
    }
}
