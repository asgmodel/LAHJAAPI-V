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
    /// CategoryTab interface property for BuilderRepository.
    /// </summary>
    public interface ICategoryTabBuilderRepository<TBuildRequestDto, TBuildResponseDto> : IBaseBuilderRepository<TBuildRequestDto, TBuildResponseDto> //
 where TBuildRequestDto : class //
 where TBuildResponseDto : class //
    {
    // Define methods or properties specific to the builder interface.
    }
}