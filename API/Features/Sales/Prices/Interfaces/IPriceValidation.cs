using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public interface IPriceValidation : IRepository<Price> {

        int IsValid(Price x, PriceWriteDto price);
    }

}