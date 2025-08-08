using EmilyApiFurb.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ComandaMap : IEntityTypeConfiguration<Comanda>
{
    private const string NOME_TABELA = "COMANDA";

    public void Configure(EntityTypeBuilder<Comanda> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.UsuarioId).HasColumnName("USUARIO_ID").IsRequired();
        builder.Property(x => x.NomeUsuario).HasColumnName("NOME_USUARIO").IsRequired().HasMaxLength(100);
        builder.Property(x => x.TelefoneUsuario).HasColumnName("TELEFONE_USUARIO").IsRequired().HasMaxLength(15)
        ;

        builder.HasMany(c => c.Produtos)
            .WithOne()
            .HasForeignKey(pc => pc.ComandaId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}