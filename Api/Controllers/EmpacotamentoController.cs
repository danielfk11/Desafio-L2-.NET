using api.DTOs;
using api.Repositories;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/empacotamento")]
    public class EmpacotamentoController : ControllerBase
    {
        private readonly ServicoEmpacotamento _servico;
        private readonly EmpacotamentoRepository _repo;

        public EmpacotamentoController(ServicoEmpacotamento servico, EmpacotamentoRepository repo)
        {
            _servico = servico;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequisicaoEmpacotamentoDto requisicao)
        {
            var resultado = _servico.ProcessarPedidos(requisicao.pedidos);

            var dimensoes = requisicao.pedidos
                .SelectMany(p => p.produtos)
                .ToDictionary(
                    p => p.produto_id,
                    p => new Dimensao(p.dimensoes.Altura, p.dimensoes.Largura, p.dimensoes.Comprimento)
                );

            foreach (var pedidoEmpacotado in resultado)
            {
                await _repo.SalvarResultadoAsync(pedidoEmpacotado, dimensoes);
            }

            return Ok(new RespostaEmpacotamentoDto { pedidos = resultado });
        }
    }
}
