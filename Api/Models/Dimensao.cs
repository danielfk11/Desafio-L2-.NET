namespace api.Models
{
    public class Dimensao
    {
        public int Id { get; set; } 

        public int Altura { get; set; }
        public int Largura { get; set; }
        public int Comprimento { get; set; }

        public int Volume => Altura * Largura * Comprimento;

        public Dimensao() { }
        
        public Dimensao(int altura, int largura, int comprimento)
        {
            Altura = altura;
            Largura = largura;
            Comprimento = comprimento;
        }

        public bool CabeNaCaixa(Dimensao caixa)
        {
            var orientacoes = new List<(int a, int l, int c)>
            {
                (Altura, Largura, Comprimento),
                (Altura, Comprimento, Largura),
                (Largura, Altura, Comprimento),
                (Largura, Comprimento, Altura),
                (Comprimento, Altura, Largura),
                (Comprimento, Largura, Altura)
            };

            return orientacoes.Any(o =>
                o.a <= caixa.Altura &&
                o.l <= caixa.Largura &&
                o.c <= caixa.Comprimento
            );
        }
    }
}
