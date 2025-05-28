using api.Models;

public class ProdutoNaCaixa
{
    public int Id { get; set; }

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }

    public int CaixaUsadaId { get; set; }
    public CaixaUsada CaixaUsada { get; set; }
}
