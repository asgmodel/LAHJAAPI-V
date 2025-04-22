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
    /// CategoryModel class property for BuilderRepository.
    /// </summary>
     //
    public class CategoryModelBuilderRepository : BaseBuilderRepository<CategoryModel, CategoryModelRequestBuildDto, CategoryModelResponseBuildDto>, ICategoryModelBuilderRepository<CategoryModelRequestBuildDto, CategoryModelResponseBuildDto>
    {
        /// <summary>
        /// Constructor for CategoryModelBuilderRepository.
        /// </summary>
        public CategoryModelBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
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