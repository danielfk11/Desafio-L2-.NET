using api.Models;

public class Pedido
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public List<Produto> Produtos { get; set; }
    public List<CaixaUsada> CaixasUsadas { get; set; }
}
