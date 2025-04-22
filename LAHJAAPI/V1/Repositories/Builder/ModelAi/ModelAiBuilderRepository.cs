using AutoMapper;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using V1.Repositories.Base;
using AutoGenerator.Repositories.Builder;
using V1.DyModels.Dto.Build.Requests;
using V1.DyModels.Dto.Build.Responses;
using System;

namespace V1.Repositories.Builder
{
    /// <summary>
    /// ModelAi class property for BuilderRepository.
    /// </summary>
     //
    public class ModelAiBuilderRepository : BaseBuilderRepository<ModelAi, ModelAiRequestBuildDto, ModelAiResponseBuildDto>, IModelAiBuilderRepository<ModelAiRequestBuildDto, ModelAiResponseBuildDto>
    {
        /// <summary>
        /// Constructor for ModelAiBuilderRepository.
        /// </summary>
        public ModelAiBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
        {
        // Initialize necessary fields or call base constructor.
        ///
        /// 
         
        /// 
        }
    //
    // Add additional methods or properties as needed.
    }
}