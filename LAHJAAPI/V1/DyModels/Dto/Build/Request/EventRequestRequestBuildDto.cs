using AutoGenerator;

namespace V1.DyModels.Dto.Build.Requests
{
    public class EventRequestRequestBuildDto : ITBuildDto
    {
        /// <summary>
        /// Id property for DTO.
        /// </summary>
        public string Id { get; set; } = $"event_{Guid.NewGuid():N}";
        /// <summary>
        /// Status property for DTO.
        /// </summary>
        public String? Status { get; set; }
        /// <summary>
        /// Details property for DTO.
        /// </summary>
        public String? Details { get; set; }
        /// <summary>
        /// CreatedAt property for DTO.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// RequestId property for DTO.
        /// </summary>
        public String? RequestId { get; set; }
        public RequestRequestBuildDto? Request { get; set; }
    }
}