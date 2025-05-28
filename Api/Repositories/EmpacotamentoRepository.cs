using Microsoft.Data.SqlClient;
using api.DTOs;
using api.Extensions;
using api.Models;

namespace api.Repositories
{
    public class EmpacotamentoRepository
    {
        private readonly string _connectionString;

        public EmpacotamentoRepository(IConfiguration config)
        {
            _connectionString = "CONNECTION_STRING".ObterVariavel();
        }

        public async Task SalvarResultadoAsync(ResultadoEmpacotamentoDto resultado, Dictionary<string, Dimensao> dimensoesDosProdutos)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var insertPedido = "INSERT INTO Pedidos (DataCriacao) OUTPUT INSERTED.Id VALUES (@DataCriacao)";
                var cmd = new SqlCommand(insertPedido, connection, transaction);
                cmd.Parameters.AddWithValue("@DataCriacao", DateTime.UtcNow);
                var result = await cmd.ExecuteScalarAsync();
                if (result == null || result == DBNull.Value)
                    throw new Exception("Falha ao inserir pedido.");
                int pedidoId = Convert.ToInt32(result);

                var produtoIdMap = new Dictionary<string, int>();

                foreach (var caixa in resultado.caixas)
                {
                    int? caixaId = null;

                    if (!string.IsNullOrEmpty(caixa.caixa_id))
                    {
                        var insertCaixa = "INSERT INTO CaixasUsadas (TipoCaixa, PedidoId) OUTPUT INSERTED.Id VALUES (@TipoCaixa, @PedidoId)";
                        using var cmdCaixa = new SqlCommand(insertCaixa, connection, transaction);
                        cmdCaixa.Parameters.AddWithValue("@TipoCaixa", caixa.caixa_id);
                        cmdCaixa.Parameters.AddWithValue("@PedidoId", pedidoId);
                        var res = await cmdCaixa.ExecuteScalarAsync();
                        caixaId = res != null && res != DBNull.Value ? Convert.ToInt32(res) : throw new Exception("Erro ao salvar CaixaUsada.");
                    }

                    foreach (var produtoNome in caixa.produtos)
                    {
                        if (!produtoIdMap.TryGetValue(produtoNome, out var produtoId))
                        {
                            if (!dimensoesDosProdutos.TryGetValue(produtoNome, out var dim))
                                throw new Exception($"Dimensões não encontradas para o produto '{produtoNome}'.");

                            var insertDimensao = "INSERT INTO Dimensoes (Altura, Largura, Comprimento) OUTPUT INSERTED.Id VALUES (@Altura, @Largura, @Comprimento)";
                            using var cmdDim = new SqlCommand(insertDimensao, connection, transaction);
                            cmdDim.Parameters.AddWithValue("@Altura", dim.Altura);
                            cmdDim.Parameters.AddWithValue("@Largura", dim.Largura);
                            cmdDim.Parameters.AddWithValue("@Comprimento", dim.Comprimento);
                            var resDim = await cmdDim.ExecuteScalarAsync();
                            int dimensaoId = resDim != null && resDim != DBNull.Value ? Convert.ToInt32(resDim) : throw new Exception("Erro ao salvar Dimensão.");

                            var insertProduto = "INSERT INTO Produtos (Nome, PedidoId, DimensaoId) OUTPUT INSERTED.Id VALUES (@Nome, @PedidoId, @DimensaoId)";
                            using var cmdProd = new SqlCommand(insertProduto, connection, transaction);
                            cmdProd.Parameters.AddWithValue("@Nome", produtoNome);
                            cmdProd.Parameters.AddWithValue("@PedidoId", pedidoId);
                            cmdProd.Parameters.AddWithValue("@DimensaoId", dimensaoId);
                            var resProd = await cmdProd.ExecuteScalarAsync();
                            produtoId = resProd != null && resProd != DBNull.Value ? Convert.ToInt32(resProd) : throw new Exception("Erro ao salvar Produto.");

                            produtoIdMap[produtoNome] = produtoId;
                        }

                        if (caixaId.HasValue)
                        {
                            var insertRelacao = "INSERT INTO ProdutosNaCaixa (ProdutoId, CaixaUsadaId) VALUES (@ProdutoId, @CaixaUsadaId)";
                            using var cmdRel = new SqlCommand(insertRelacao, connection, transaction);
                            cmdRel.Parameters.AddWithValue("@ProdutoId", produtoId);
                            cmdRel.Parameters.AddWithValue("@CaixaUsadaId", caixaId.Value);
                            await cmdRel.ExecuteNonQueryAsync();
                        }
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
