using System.Collections.Generic;

namespace api.DTOs
{
    public class RespostaEmpacotamentoDto
    {
        public List<ResultadoEmpacotamentoDto> pedidos { get; set; } = new();
    }
}
