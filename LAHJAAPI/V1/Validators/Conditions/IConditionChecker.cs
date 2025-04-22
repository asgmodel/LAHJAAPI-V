using AutoGenerator.Conditions;

namespace LAHJAAPI.V1.Validators.Conditions
{
    public interface IConditionChecker : IBaseConditionChecker
    {
        public ITFactoryInjector Injector { get; }
    }
}