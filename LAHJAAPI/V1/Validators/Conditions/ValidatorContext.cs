using AutoGenerator.Conditions;
using LAHJAAPI.Models;
using LAHJAAPI.V1.Validators.Conditions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using V1.DyModels.Dso.ResponseFilters;

namespace ApiCore.Validators.Conditions
{
   

   
    public abstract class ValidatorContext<TContext, EValidator> : BaseValidatorContext<TContext, EValidator>, ITValidator
       where TContext : class
       where EValidator : Enum
       
    {
        protected readonly ITFactoryInjector _injector;


        public ValidatorContext(IConditionChecker checker) : base(checker)
        {
            _injector= checker.Injector;
        }
         
        
        protected virtual async Task<TContext?>  FinModel(string? id)
        {
            

            var _model = await _injector.Context.Set<TContext>().FindAsync(id);
            return _model;



        }


        protected override Task<TContext?> GetModel(string? id)
        {
            
            return FinModel(id);
        }


         
    }
}