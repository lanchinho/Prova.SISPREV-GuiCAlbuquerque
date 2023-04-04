using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using System;
using System.Diagnostics;
using System.Reflection;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data
{
    public class SistemaCompraContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public SistemaCompraContext(DbContextOptions options) : base(options) { }
        public DbSet<ProdutoAgg.Produto> Produtos { get; set; }
        public DbSet<SolicitacaoAgg.SolicitacaoCompra> SolicitacaoCompras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Debugger.Launch();

            var seedProduto = new ProdutoAgg.Produto("Produto01", "Descricao01", "Madeira", 100);
            var seedMoney = new Money(100m);

            modelBuilder.Entity<ProdutoAgg.Produto>()
                .HasData(seedProduto);

            modelBuilder.Entity<ProdutoAgg.Produto>().OwnsOne(x => x.Preco)
                .HasData(new {
                    ProdutoId = seedProduto.Id,
                    seedMoney.Value
                });

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory)  
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Server= DESKTOP-GUILHER\SQLEXPRESS;Database=SistemaCompraDb;Trusted_Connection=True;MultipleActiveResultSets=true");
                //.UseSqlServer(@"Server=localhost\SQLEXPRESS01;Database=SistemaCompraDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
