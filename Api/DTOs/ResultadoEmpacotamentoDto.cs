using System.Text.Json.Serialization;

namespace api.DTOs
{
    public class ResultadoEmpacotamentoDto
    {
        public int pedido_id { get; set; }
        public List<CaixaDto> caixas { get; set; }
    }

    public class CaixaDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? caixa_id { get; set; }
        public List<string> produtos { get; set; }
        public string? observacao { get; set; }
    }
}
