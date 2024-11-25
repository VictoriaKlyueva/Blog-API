using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Data.DTO
{
    public class PostPagedListDto
    {
        public List<PostDto>? Posts { get; set; }
        public required PageInfoModel Pagination { get; set; }
    }
}