using AutoGenerator.Conditions;

namespace LAHJAAPI.V1.Validators.Conditions
{
    public class ConditionChecker : BaseConditionChecker, IConditionChecker
    {
        private readonly ITFactoryInjector _injector;
        public ITFactoryInjector Injector => _injector;

        public ConditionChecker(ITFactoryInjector injector) : base()
        {
            _injector = injector;
        }
        // الدوال السابقة تبقى كما هي
    }
}