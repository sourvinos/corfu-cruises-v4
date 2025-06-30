using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.DocumentTypes {

    public interface IDocumentTypeRepository : IRepository<DocumentType> {

        Task<IEnumerable<DocumentTypeListVM>> GetAsync();
        Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserAsync(int discriminatorId);
        Task<DocumentTypeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<DocumentType> GetByIdAsync(int id);
        Task<int> GetLastDocumentTypeNoAsync(int documentTypeId);

    }

}