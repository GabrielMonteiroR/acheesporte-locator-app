using acheesporte_locator_app.Dtos.CepDtos;
using System.Threading.Tasks;

namespace acheesporte_locator_app.Interfaces;

public interface ICepService
{
    Task<CepResponseDto> GetAddressByCepAsync(string cep);
}
