using AutoMapper;

namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodMappingProfile : Profile {

        public PaymentMethodMappingProfile() {
            CreateMap<PaymentMethod, PaymentMethodListVM>();
            CreateMap<PaymentMethod, PaymentMethodBrowserVM>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.DescriptionEn));
            CreateMap<PaymentMethod, PaymentMethodReadDto>();
            CreateMap<PaymentMethodWriteDto, PaymentMethod>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.DescriptionEn, x => x.MapFrom(x => x.DescriptionEn.Trim()));
        }

    }

}