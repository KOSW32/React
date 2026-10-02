using Microsoft.AspNetCore.Mvc;
using BooksApi.Models;

namespace BooksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> Books = new()
        {
            new Book { Id = 1, Title = "Кобзар", Author = "Тарас Шевченко", Year = 1840 },
            new Book { Id = 2, Title = "Лісова пісня", Author = "Леся Українка", Year = 1911 },
            new Book { Id = 3, Title = "Тіні забутих предків", Author = "Михайло Коцюбинський", Year = 1911 }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            return Ok(Books);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                return NotFound("Book not found");
            }

            return Ok(book);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Book>> Search([FromQuery] string? title, [FromQuery] string? author)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Параметр title є обов'язковим");
            }

            var result = Books.Where(b =>
                b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(author))
            {
                result = result.Where(b =>
                    b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(result.ToList());
        }

        [HttpPost]
        public ActionResult<Book> Create([FromBody] Book book)
        {
            var validationError = Validate(book);
            if (validationError is not null)
            {
                return BadRequest(validationError);
            }

            book.Id = Books.Count == 0 ? 1 : Books.Max(b => b.Id) + 1;
            Books.Add(book);

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Book> Update(int id, [FromBody] Book updatedBook)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                return NotFound("Book not found");
            }

            var validationError = Validate(updatedBook);
            if (validationError is not null)
            {
                return BadRequest(validationError);
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return Ok(book);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book is null)
            {
                return NotFound("Book not found");
            }

            Books.Remove(book);

            return NoContent();
        }

        private static string? Validate(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return "Поле Title є обов'язковим";
            }

            if (string.IsNullOrWhiteSpace(book.Author))
            {
                return "Поле Author є обов'язковим";
            }

            if (book.Year < 1800)
            {
                return "Поле Year має бути не менше 1800";
            }

            return null;
        }
    }
}
