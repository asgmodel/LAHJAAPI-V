using APILAHJA.LAHJAAPI.Utilities;
using AutoGenerator.Notifications;
using LAHJAAPI.Services2;
using AutoMapper;
using LAHJAAPI.Data;
using LAHJAAPI.Utilities;
using Microsoft.Extensions.Options;

namespace LAHJAAPI.V1.Validators.Conditions
{
    public class TFactoryInjector : ITFactoryInjector
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        // يمكنك حقن اي طبقة
        public TFactoryInjector(
            IMapper mapper,
            DataContext context,
            IUserClaimsHelper userClaims,
            TokenService tokenService,
            IOptions<AppSettings> appSettings,
            IAutoNotifier notifier)
        {
            _mapper = mapper;
            _context = context;
            UserClaims = userClaims;
            Notifier = notifier;
            TokenService = tokenService;
            AppSettings = appSettings.Value;
        }

        public IMapper Mapper => _mapper;
        public DataContext Context => _context;
        public IUserClaimsHelper UserClaims { get; }

        public IAutoNotifier Notifier { get; }

        public TokenService TokenService { get; }

        public AppSettings AppSettings { get; }
    }
}