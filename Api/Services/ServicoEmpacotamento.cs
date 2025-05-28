using System.Collections.Generic;
using System.Linq;
using api.DTOs;
using api.Models;

namespace api.Services
{
    public class ServicoEmpacotamento
    {
        private readonly List<(string Tipo, Dimensao Tamanho)> _tiposCaixas = new()
        {
            ("Caixa 1", new Dimensao(30, 40, 80)),
            ("Caixa 2", new Dimensao(80, 50, 40)),
            ("Caixa 3", new Dimensao(50, 80, 60)),
        };

        public List<ResultadoEmpacotamentoDto> ProcessarPedidos(List<PedidoInputDto> pedidos)
        {
            var resultado = new List<ResultadoEmpacotamentoDto>();

            foreach (var pedido in pedidos)
            {
                var produtos = pedido.produtos
                    .Where(p => p.dimensoes.Altura > 0 && p.dimensoes.Largura > 0 && p.dimensoes.Comprimento > 0)
                    .Select(p => new
                    {
                        Nome = p.produto_id,
                        Dimensao = new Dimensao(p.dimensoes.Altura, p.dimensoes.Largura, p.dimensoes.Comprimento)
                    })
                    .ToList();

                var caixas = new List<CaixaDto>();

                while (produtos.Any())
                {
                    var primeiro = produtos.First();
                    var caixaInfo = _tiposCaixas.FirstOrDefault(c => CabeNaCaixa(primeiro.Dimensao, c.Tamanho));
                    if (caixaInfo == default)
                    {
                        caixas.Add(new CaixaDto
                        {
                            caixa_id = null,
                            produtos = new List<string> { primeiro.Nome },
                            observacao = "Produto não cabe em nenhuma caixa disponível."
                        });
                        produtos.RemoveAt(0);
                        continue;
                    }

                    var tipoCaixa = caixaInfo.Tipo;
                    var capacidade = caixaInfo.Tamanho.Volume;
                    var volAtual = 0;
                    var embalados = new List<string>();

                    foreach (var p in produtos.ToList())
                    {
                        if (CabeNaCaixa(p.Dimensao, caixaInfo.Tamanho) && volAtual + p.Dimensao.Volume <= capacidade)
                        {
                            embalados.Add(p.Nome);
                            volAtual += p.Dimensao.Volume;
                        }
                        else
                        {
                            break;
                        }
                    }

                    caixas.Add(new CaixaDto
                    {
                        caixa_id = tipoCaixa,
                        produtos = embalados
                    });

                    produtos.RemoveAll(p => embalados.Contains(p.Nome));
                }

                caixas = caixas
                    .OrderByDescending(c => c.caixa_id == null ? -1 : _tiposCaixas.First(t => t.Tipo == c.caixa_id).Tamanho.Volume)
                    .ToList();

                resultado.Add(new ResultadoEmpacotamentoDto
                {
                    pedido_id = pedido.pedido_id,
                    caixas = caixas
                });
            }

            return resultado;
        }

        private bool CabeNaCaixa(Dimensao item, Dimensao caixa)
        {
            if (item.Altura > caixa.Altura) return false;

            var normal = item.Largura <= caixa.Largura && item.Comprimento <= caixa.Comprimento;
            var rotacao = item.Largura <= caixa.Comprimento && item.Comprimento <= caixa.Largura;

            return normal || rotacao;
        }
    }
}
