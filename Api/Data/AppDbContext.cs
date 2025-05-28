using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<CaixaUsada> CaixasUsadas { get; set; }
        public DbSet<ProdutoNaCaixa> ProdutosNaCaixa { get; set; }
        public DbSet<Dimensao> Dimensoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProdutoNaCaixa>()
                .HasOne(p => p.Produto)
                .WithMany(p => p.ProdutoNaCaixa)
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProdutoNaCaixa>()
                .HasOne(p => p.CaixaUsada)
                .WithMany(c => c.ProdutosNaCaixa) 
                .HasForeignKey(p => p.CaixaUsadaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
