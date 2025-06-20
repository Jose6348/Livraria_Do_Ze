using System.ComponentModel.DataAnnotations;

namespace Livraria_Do_Zé.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório.")]
        public string? Autor { get; set; }

        public string? Genero { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
        public int QuantidadeEmEstoque { get; set; }
    }
}
