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
            return new JsonResult(await _booksRepository.GetAllBooks());
        }

        [HttpPost]
        public async Task<JsonResult> Add(string title,string description,string author,decimal price)
        {
            Guid id = await _booksRepository.Add(title, description, author, price);
            return new JsonResult(id);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            return new JsonResult(await _booksRepository.Delete(id));
        }

        [HttpPut]
        public async Task<JsonResult> Update(Guid id,string newTitle,string newDescription,string newAuthor,decimal newPrice)
        {
            return new JsonResult(await _booksRepository.Update(id,newTitle,newDescription,newAuthor,newPrice));
        }
    }
}