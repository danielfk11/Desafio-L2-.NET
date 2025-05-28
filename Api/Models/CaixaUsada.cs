public class CaixaUsada
{
    public int Id { get; set; }
    public string TipoCaixa { get; set; }

    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; }

    public List<ProdutoNaCaixa> ProdutosNaCaixa { get; set; }
}
