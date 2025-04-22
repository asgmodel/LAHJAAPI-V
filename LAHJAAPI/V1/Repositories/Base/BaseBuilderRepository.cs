using AutoGenerator;
using AutoGenerator.Repositories.Builder;
using AutoMapper;
using LAHJAAPI.Data;

namespace V1.Repositories.Base
{
    /// <summary>
    /// BaseRepository class for ShareRepository.
    /// </summary>
    public abstract class BaseBuilderRepository<TModel, TBuildRequestDto, TBuildResponseDto> :
        TBaseBuilderRepository<TModel, TBuildRequestDto, TBuildResponseDto>,
        IBaseBuilderRepository<TBuildRequestDto, TBuildResponseDto>,
        ITBuildRepository where TModel : class where TBuildRequestDto : class where TBuildResponseDto : class
    {
        private readonly BaseRepository<TModel> _baseRepository;

        public BaseBuilderRepository(DataContext context, IMapper mapper, ILogger logger)
            : base(new BaseRepository<TModel>(context, logger), mapper, logger)
        {
            _baseRepository = new BaseRepository<TModel>(context, logger);
        }

        public async Task<bool> ExecuteTransactionAsync(Func<Task<bool>> operation)
        {
            return await _baseRepository.ExecuteTransactionAsync(operation);
        }



    }
}