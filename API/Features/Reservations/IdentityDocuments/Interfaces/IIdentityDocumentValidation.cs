using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.IdentityDocuments {

    public interface IIdentityDocumentValidation : IRepository<IdentityDocument> {

        int IsValid(IdentityDocument x, IdentityDocumentWriteDto identityDocument);

    }

}