using AutoMapper;
using Stripe;

namespace StripeGateway
{
    public class CurrencyFormatter : IValueConverter<long, decimal>
    {
        public decimal Convert(long source, ResolutionContext context)
        {
            decimal d = source / 100m;
            return d;
        }
    }


    public class StripeMappingConfig : Profile
    {
        public StripeMappingConfig()
        {
            CreateMap<Subscription, StripeSubscriptionResponse>();
            CreateMap<Customer, CustomerResponse>();
            CreateMap<SetupIntent, StripeSetuptIntentResponse>();
            CreateMap<SetupIntentPaymentMethodOptions, SetupIntentPaymentMethodOptionsResponse>();
            CreateMap<PaymentMethod, PaymentMethodResponse>();
            CreateMap<PaymentMethodBillingDetails, PaymentMethodBillingDetailsResponse>();
            CreateMap<PaymentMethodCard, PaymentMethodCardResponse>();
            //CreateMap<Price, PriceResponse>()
            //    .ForMember(p => p.Interval, pp => pp.MapFrom(p => p.Recurring.Interval))
            //    .ForMember(p => p.UnitAmountDecimal, opt => opt.ConvertUsing(new CurrencyFormatter(), "UnitAmount"))
            //    .ReverseMap();

            //CreateMap<Product, ProductResponse>().ReverseMap();
            //CreateMap<Session, SessionResponse>().ReverseMap();
            //CreateMap<Stripe.BillingPortal.Session, SessionResponse>().ReverseMap();


            //CreateMap<ProductCreate, ProductCreateOptions>();
            //CreateMap<ProductUpdate, ProductUpdateOptions>();


        }
    }
}