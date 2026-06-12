using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Parameters {

    public class ParameterReadDto : IMetadata {

        public Guid Id { get; set; }
        public bool EmailInvoicesIsActive { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}