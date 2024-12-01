namespace BackendLaboratory.Data.Entities
{
    public class AsAddrObj
    {
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public Guid ObjectGuid { get; set; }

        public required string Name { get; set; }

        public required string Typename { get; set; }

        public required string Level { get; set; }

        public long? Parentobjid { get; set; }
    }
}
