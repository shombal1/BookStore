using System.Text.Json;
using System.Text.Json.Serialization;
using BookStore.APi.Models;
using BookStore.Postgres.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStore.APi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsRepository _authorsRepository;

        public AuthorsController(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }

        [HttpGet]
        [Route("All")]
        public async Task<JsonResult> GetAll()
        {
            return new JsonResult(await _authorsRepository.GetAll(),
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
        }

        [HttpGet]
        [Route("One")]
        public async Task<JsonResult> Get(Guid id)
        {
            return new JsonResult(await _authorsRepository.Get(id),
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
        }

        [HttpPut]
        public Task Update(Guid id, string firstName, string lastName, string? patronymic, string city)
        {
            return _authorsRepository.Update(id, firstName, lastName, patronymic, city);
        }

        [HttpPost]
        public async Task<JsonResult> Add([FromBody] CreateAuthor createAuthor)
        {
            Guid id = await _authorsRepository.Add(createAuthor.FirstName, createAuthor.LastName,
                createAuthor.Patronymic, createAuthor.City);
            return new JsonResult(id);
        }

        [HttpDelete]
        public Task Delete(Guid authorId)
        {
            return _authorsRepository.Delete(authorId);
        }
    }
}