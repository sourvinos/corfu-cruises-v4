using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Parameters {

    public interface ISaleParametersRepository : IRepository<SaleParameter> {

        Task<SaleParameter> GetAsync();

    }

}