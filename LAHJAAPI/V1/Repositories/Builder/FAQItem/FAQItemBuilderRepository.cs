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
    /// FAQItem class property for BuilderRepository.
    /// </summary>
     //
    public class FAQItemBuilderRepository : BaseBuilderRepository<FAQItem, FAQItemRequestBuildDto, FAQItemResponseBuildDto>, IFAQItemBuilderRepository<FAQItemRequestBuildDto, FAQItemResponseBuildDto>
    {
        /// <summary>
        /// Constructor for FAQItemBuilderRepository.
        /// </summary>
        public FAQItemBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
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