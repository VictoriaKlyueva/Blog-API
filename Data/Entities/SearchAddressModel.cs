using BackendLaboratory.Data.Entities.Enums;

namespace BackendLaboratory.Data.Entities
{
    public class SearchAddressModel
    {
        public long ObjectId { get; set; }
        public Guid ObjectGuid { get; set; }
        public required string Text { get; set; }
        public ObjectLevel? ObjectLevel { get; set; }
        public required string ObjectLevelText { get; set; }
    }
}
