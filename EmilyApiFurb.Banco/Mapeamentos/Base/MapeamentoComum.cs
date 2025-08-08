using EmilyApiFurb.Dominio.Modelos.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestApiFurb.Infra.Mapeamentos.Base;

public static class MapeamentoComum
{
    public static void MapearComum<T>(EntityTypeBuilder<T> builder) where T : ModeloBase
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
    }
}