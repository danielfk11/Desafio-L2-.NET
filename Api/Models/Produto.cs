namespace api.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        public Dimensao Dimensao { get; set; }

        public List<ProdutoNaCaixa> ProdutoNaCaixa { get; set; }
    }
}
