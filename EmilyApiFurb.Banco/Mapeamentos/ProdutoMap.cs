using EmilyApiFurb.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    private const string NOME_TABELA = "PRODUTO";

    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.ComandaId).HasColumnName("COMANDA_FK").IsRequired();
        builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
        builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
        builder.Property(x => x.Preco).HasColumnName("PRECO").HasPrecision(10,2).IsRequired();
    }
}