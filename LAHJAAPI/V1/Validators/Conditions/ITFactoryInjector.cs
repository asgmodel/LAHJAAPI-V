using APILAHJA.LAHJAAPI.Utilities;
using AutoGenerator.Conditions;
using LAHJAAPI.Services2;
using LAHJAAPI.Data;
using LAHJAAPI.Utilities;

namespace LAHJAAPI.V1.Validators.Conditions
{
    public interface ITFactoryInjector : ITBaseFactoryInjector
    {
        public DataContext Context { get; }
        public IUserClaimsHelper UserClaims { get; }
        public TokenService TokenService { get; }
        public AppSettings AppSettings { get; }
    }
}