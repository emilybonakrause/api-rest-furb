using EmilyApiFurb.Api.ViewModels;
using EmilyApiFurb.Dominio.Modelos;

namespace EmilyApiFurb.Api.Conversores;

public interface IComandaConversor
{
    ComandaViewModel ConverterParaViewModel(Comanda comanda);
}