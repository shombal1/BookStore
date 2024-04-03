using System.Text.Json;
using System.Text.Json.Serialization;
using BookStore.Postgres.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await _booksRepository.GetAll(),
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
        }

        [HttpPost]
        public async Task<JsonResult> Add(Guid authorId,string title,string description,decimal price)
        {
            Guid id = await _booksRepository.Add(authorId,title, description, price);
            return new JsonResult(id);
        }

        [HttpDelete]
        public Task Delete(Guid id)
        {
            return _booksRepository.Delete(id);
        }

        [HttpPut]
        public Task Update(Guid id,string newTitle,string newDescription,decimal newPrice)
        {
            return _booksRepository.Update(id,newTitle,newDescription,newPrice);
        }
    }
}