using api.DTOs;
using api.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Api.Tests
{
    public class ServicoEmpacotamentoTests
    {
        [Fact]
        public void Deve_Empacotar_Produto_Que_Cabe_Em_Caixa()
        {
            var servico = new ServicoEmpacotamento();

            var pedidos = new List<PedidoInputDto>
            {
                new PedidoInputDto
                {
                    pedido_id = 1,
                    produtos = new List<ProdutoInputDto>
                    {
                        new ProdutoInputDto
                        {
                            produto_id = "Mouse",
                            dimensoes = new DimensoesDto { Altura = 5, Largura = 10, Comprimento = 10 }
                        }
                    }
                }
            };

            var resultado = servico.ProcessarPedidos(pedidos);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(1);
            resultado[0].caixas.Should().HaveCount(1);
            resultado[0].caixas[0].produtos.Should().Contain("Mouse");
        }

        [Fact]
        public void Deve_Marcar_Produto_Que_Nao_Cabe_Em_Nenhuma_Caixa()
        {
            var servico = new ServicoEmpacotamento();

            var pedidos = new List<PedidoInputDto>
            {
                new PedidoInputDto
                {
                    pedido_id = 99,
                    produtos = new List<ProdutoInputDto>
                    {
                        new ProdutoInputDto
                        {
                            produto_id = "TV",
                            dimensoes = new DimensoesDto { Altura = 200, Largura = 200, Comprimento = 200 }
                        }
                    }
                }
            };

            var resultado = servico.ProcessarPedidos(pedidos);

            resultado[0].caixas.Should().ContainSingle(c => c.caixa_id == null && c.observacao != null);
        }

        [Fact]
        public void Deve_Distribuir_Produtos_Em_Multiplas_Caixas_Quando_Necessario()
        {
            var servico = new ServicoEmpacotamento();

            var pedidos = new List<PedidoInputDto>
            {
                new PedidoInputDto
                {
                    pedido_id = 2,
                    produtos = new List<ProdutoInputDto>
                    {
                        new ProdutoInputDto { produto_id = "Produto1", dimensoes = new DimensoesDto { Altura = 50, Largura = 50, Comprimento = 50 } },
                        new ProdutoInputDto { produto_id = "Produto2", dimensoes = new DimensoesDto { Altura = 60, Largura = 60, Comprimento = 60 } }
                    }
                }
            };

            var resultado = servico.ProcessarPedidos(pedidos);

            resultado[0].caixas.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public void Deve_Ignorar_Produto_Com_Dimensoes_Invalidas()
        {
            var servico = new ServicoEmpacotamento();

            var pedidos = new List<PedidoInputDto>
            {
                new PedidoInputDto
                {
                    pedido_id = 3,
                    produtos = new List<ProdutoInputDto>
                    {
                        new ProdutoInputDto
                        {
                            produto_id = "Invalido",
                            dimensoes = new DimensoesDto { Altura = -10, Largura = 20, Comprimento = 30 }
                        }
                    }
                }
            };

            var resultado = servico.ProcessarPedidos(pedidos);

            resultado[0].caixas.Should().BeEmpty();
        }

        [Fact]
        public void Deve_Empacotar_Produtos_Que_Cabem_Juntos_Na_Mesma_Caixa()
        {
            var servico = new ServicoEmpacotamento();

            var pedidos = new List<PedidoInputDto>
            {
                new PedidoInputDto
                {
                    pedido_id = 4,
                    produtos = new List<ProdutoInputDto>
                    {
                        new ProdutoInputDto { produto_id = "Item1", dimensoes = new DimensoesDto { Altura = 10, Largura = 10, Comprimento = 10 } },
                        new ProdutoInputDto { produto_id = "Item2", dimensoes = new DimensoesDto { Altura = 10, Largura = 10, Comprimento = 10 } }
                    }
                }
            };

            var resultado = servico.ProcessarPedidos(pedidos);

            resultado[0].caixas.Should().HaveCount(1);
            resultado[0].caixas[0].produtos.Should().Contain(new[] { "Item1", "Item2" });
        }
    }
}
