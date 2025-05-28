namespace api.DTOs
{
    public class PedidoInputDto
    {
        public int pedido_id { get; set; }
        public List<ProdutoInputDto> produtos { get; set; }
    }

    public class ProdutoInputDto
    {
        public string produto_id { get; set; }
        public DimensoesDto dimensoes { get; set; }
    }

    public class DimensoesDto
    {
        public int Altura { get; set; }
        public int Largura { get; set; }
        public int Comprimento { get; set; }
    }
}
