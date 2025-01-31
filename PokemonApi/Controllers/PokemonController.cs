using Microsoft.AspNetCore.Mvc;
using PokemonApi.Models;
using PokemonApi.Services;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        // Injected service
        private readonly IPokemonService _pokemonService;

        // Constructor injection of the service
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        // Add a Pokemon
        [HttpPost("add")]
        public IActionResult AddPokemon(Pokemon pokemon)
        {
            _pokemonService.AddAsync(pokemon); // Delegates to service
            return Ok(new { Message = "Pokemon added successfully!" });
        }

        // Get all Pokemon
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPokemon()
        {
            // Ensure await is used to resolve the Task to its result
            var pokemons = await _pokemonService.GetAllAsync();
            return Ok(pokemons);
        }

        // Get Pokemon by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPokemonById(string id)
        {
            var pokemon = await _pokemonService.GetByIdAsync(id); // Ensure await is used here
            if (pokemon == null)
            {
                return NotFound(new { Message = "Pokemon not found." });
            }
            return Ok(pokemon);
        }


        // Update Pokemon
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePokemon(string id, [FromBody] Pokemon updatedPokemon)
        {
            var existingPokemon = await _pokemonService.GetByIdAsync(id); // Ensure this is awaited
            if (existingPokemon == null)
            {
                return NotFound(new { Message = "Pokemon not found." });
            }

            await _pokemonService.UpdateAsync(id, updatedPokemon); // Ensure this is awaited
            return Ok(new { Message = "Pokemon updated successfully!" });
        }


        // Delete Pokemon
        [HttpDelete("delete/{id}")]
        public IActionResult DeletePokemon(string id)
        {
            var existingPokemon = _pokemonService.GetByIdAsync(id);
            if (existingPokemon == null) return NotFound(new { Message = "Pokemon not found." });

            _pokemonService.DeleteAsync(id); // Delegates to service
            return Ok(new { Message = "Pokemon deleted successfully!" });
        }
    }
}
