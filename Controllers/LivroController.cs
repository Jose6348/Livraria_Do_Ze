using Livraria_Do_Zé.Models;
using Microsoft.AspNetCore.Mvc;

namespace Livraria_Do_Zé.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        // --- Simulação do Banco de Dados em Memória ---
        private static List<Livro> _livros = new List<Livro>
        {
            new Livro { Id = 1, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", Genero = "Fantasia", Preco = 59.90m, QuantidadeEmEstoque = 10 },
            new Livro { Id = 2, Titulo = "1984", Autor = "George Orwell", Genero = "Ficção Científica", Preco = 35.50m, QuantidadeEmEstoque = 5 },
            new Livro { Id = 3, Titulo = "O Grande Gatsby", Autor = "F. Scott Fitzgerald", Genero = "Romance", Preco = 42.00m, QuantidadeEmEstoque = 8 }
        };
        private static int _proximoId = 4;
        // ------------------------------------------------

        /// <summary>
        /// Endpoint para criar um novo livro.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CriarLivro([FromBody] Livro novoLivro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            novoLivro.Id = _proximoId++;
            _livros.Add(novoLivro);

            // Retorna o status 201 Created com o local do novo recurso e o próprio objeto criado.
            return CreatedAtAction(nameof(ObterLivroPorId), new { id = novoLivro.Id }, novoLivro);
        }

        /// <summary>
        /// Endpoint para visualizar todos os livros cadastrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ObterTodosOsLivros()
        {
            return Ok(_livros);
        }

        /// <summary>
        /// Endpoint para buscar um livro específico pelo seu Id.
        /// (Este não foi pedido, mas é útil para os outros endpoints e boas práticas)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObterLivroPorId(int id)
        {
            var livro = _livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
            {
                return NotFound($"Livro com Id {id} não encontrado."); // Status 404
            }
            return Ok(livro); // Status 200
        }

        /// <summary>
        /// Endpoint para editar as informações de um livro existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditarLivro(int id, [FromBody] Livro livroAtualizado)
        {
            if (id != livroAtualizado.Id)
            {
                return BadRequest("O Id da URL não corresponde ao Id do corpo da requisição.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var livroExistente = _livros.FirstOrDefault(l => l.Id == id);
            if (livroExistente == null)
            {
                return NotFound($"Livro com Id {id} não encontrado.");
            }

            // Atualiza as propriedades do livro existente
            livroExistente.Titulo = livroAtualizado.Titulo;
            livroExistente.Autor = livroAtualizado.Autor;
            livroExistente.Genero = livroAtualizado.Genero;
            livroExistente.Preco = livroAtualizado.Preco;
            livroExistente.QuantidadeEmEstoque = livroAtualizado.QuantidadeEmEstoque;

            return NoContent(); // Status 204 indica sucesso sem conteúdo para retornar.
        }

        /// <summary>
        /// Endpoint para excluir um livro.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ExcluirLivro(int id)
        {
            var livro = _livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
            {
                return NotFound($"Livro com Id {id} não encontrado."); // Status 404
            }

            _livros.Remove(livro);

            return NoContent(); // Status 204
        }
    }
}
