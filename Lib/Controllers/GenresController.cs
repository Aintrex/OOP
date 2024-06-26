﻿using Lib.ModelReq;
using Lib.Models;
using Lib.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/<GenresController>
        [HttpGet("getAllGenres")]
        public async Task<IActionResult> GetGenres()
        {
            IEnumerable<string> gnr = await _genreService.GetAllGenres();
            return Ok(gnr);
        }
        [HttpDelete("deleteGenre")]
        public IActionResult DeleteGenre(string gnr) {
            var genre = _genreService.GetGenreByName(gnr);
            if(genre == null)
            {
                return NotFound(new { success = false, message = "Genre not found" });
            }
            var BooksWithGenre = _genreService.AssociatedGen(genre.Id);
            if(BooksWithGenre)
            {
                return BadRequest(new { success = false, message = "Cannot delete genre because it's connected" });
            }
            if (!_genreService.DeleteGenre(gnr))
                return NotFound();
            return Ok(new { success = true, message = "Genre deleted successfully" });
        }
        // POST api/<GenresController>
        [HttpPost("addGenre")]
        public async Task<IActionResult> Creategen([FromBody] GenreCreate model)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                if(!_genreService.ValidString(model.Name)) { return BadRequest(new { success = false, message = "Genre field can't contain digits or special symbols!" }); }
            }
            int aut = await _genreService.CreateGenre(model.Name);

            if (aut != 0)
            {
                return Ok(new {success = true});
            }

            return Ok(new { success = false, message = "Genre could not be added. It might already exist." });
        }

        // PUT api/<GenresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
