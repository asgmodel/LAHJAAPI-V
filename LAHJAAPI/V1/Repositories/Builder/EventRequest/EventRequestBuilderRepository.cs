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
    /// EventRequest class property for BuilderRepository.
    /// </summary>
     //
    public class EventRequestBuilderRepository : BaseBuilderRepository<EventRequest, EventRequestRequestBuildDto, EventRequestResponseBuildDto>, IEventRequestBuilderRepository<EventRequestRequestBuildDto, EventRequestResponseBuildDto>
    {
        /// <summary>
        /// Constructor for EventRequestBuilderRepository.
        /// </summary>
        public EventRequestBuilderRepository(DataContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) // Initialize  constructor.
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