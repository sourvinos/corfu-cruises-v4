using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Parameters {

    public interface ISaleParameterValidation : IRepository<SaleParameter> {

        int IsValid(SaleParameter x, ParameterWriteDto parameter);

    }

}