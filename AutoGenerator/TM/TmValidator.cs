using AutoGenerator.ApiFolder;

namespace AutoGenerator.TM
{

    public class TmValidators
    {


        public static string GetTmConditionChecker(string classNameServiceTM, TmOptions options = null)
        {
            return @$"
 public class ConditionChecker :BaseConditionChecker, IConditionChecker
    {{
        private readonly ITFactoryInjector _injector;

        public ITFactoryInjector Injector => _injector;
        public ConditionChecker(ITFactoryInjector injector) : base()
        {{
        }}

        // الدوال السابقة تبقى كما هي

     
    }}

";



        }


        public static string GetTmIConditionChecker(string classNameServiceTM, TmOptions options = null)
        {
            return @$"
    public interface IConditionChecker: IBaseConditionChecker
    {{
   

        public ITFactoryInjector Injector {{ get; }}



    }}

";



        }


        public static string GetTmITFactoryInjector(string classNameServiceTM, TmOptions options = null)
        {
            return @$"
     public interface ITFactoryInjector: ITBaseFactoryInjector
    {{

  
    public  {ApiFolderInfo.TypeContext.Name} Context {{ get; }}


    }}
";



        }


        public static string GetTmTFactoryInjector(string classNameServiceTM, TmOptions options = null)
        {
            return @$"
    public class TFactoryInjector : ITFactoryInjector
    {{

         
 
        private  readonly IMapper _mapper;

        private readonly {ApiFolderInfo.TypeContext.Name} _context;

        // يمكنك حقن اي طبقة


        public TFactoryInjector(
           
            IMapper mapper,
            {ApiFolderInfo.TypeContext.Name} context



            )

        {{



          
            _mapper = mapper;
            _context = context;
           


        }}

       
        public IMapper Mapper => _mapper;

        public {ApiFolderInfo.TypeContext.Name} Context => _context;
    }}

";



        }


        public static string GetTmConfigValidator(string classNameServiceTM, TmOptions options = null)
        {
            return @$"
     public  static class ConfigValidator
    {{
        public static void AddAutoValidator(this IServiceCollection serviceCollection)
        {{


            Assembly? assembly =Assembly.GetExecutingAssembly();

            serviceCollection.AddScoped<ITFactoryInjector, TFactoryInjector>();
            serviceCollection.AddScoped<IConditionChecker, ConditionChecker>(pro =>
            {{
                var injctor = pro.GetRequiredService<ITFactoryInjector>();

                var checker= new ConditionChecker(injctor);


                BaseConfigValidator.Register(checker, assembly);

                return checker;

            }});




        }}
     
    }}

";



        }

        public static string GetTmValidator(string classNameValidatorTM, TmOptions options = null)
        {
            return @$"
    

    public class {classNameValidatorTM}Validator : BaseValidator<{classNameValidatorTM}ResponseFilterDso, {classNameValidatorTM}ValidatorStates>, ITValidator
    {{

    
        
        public {classNameValidatorTM}Validator(IConditionChecker checker) : base(checker)
        {{

           
        }}
        protected override void InitializeConditions()
        {{
            _provider.Register(
                {classNameValidatorTM}ValidatorStates.IsActive,
                new LambdaCondition<{classNameValidatorTM}ResponseFilterDso>(
                    nameof({classNameValidatorTM}ValidatorStates.IsActive),

                    context => IsActive(context),
                    ""{classNameValidatorTM} is not active""
                )
            );



            
        





        }}



        private bool IsActive({classNameValidatorTM}ResponseFilterDso context)
        {{
            if (context!=null){{
                return true;
            }}
            return false;
        }}

      

    }}

      //
       
     //  Base
     public enum {classNameValidatorTM}ValidatorStates //
    {{
        IsActive,
        IsFull,
        IsValid,
        
    //
    }}

 

";

  

        }



    }

}