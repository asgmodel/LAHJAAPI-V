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
    /// AdvertisementTab class property for BuilderRepository.
    /// </summary>
     //
    public class AdvertisementTabBuilderRepository : BaseBuilderRepository<AdvertisementTab, AdvertisementTabRequestBuildDto, AdvertisementTabResponseBuildDto>, IAdvertisementTabBuilderRepository<AdvertisementTabRequestBuildDto, AdvertisementTabResponseBuildDto>
    {
        /// <summary>
        /// Constructor for AdvertisementTabBuilderRepository.
        /// </summary>
        public AdvertisementTabBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
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