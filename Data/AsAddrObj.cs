namespace BackendLaboratory.Data
{
    public class AsAddrObj
    {
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public Guid Objectguid { get; set; }

        public required string Name { get; set; }

        public required string Typename { get; set; }

        public required string Level { get; set; }

        public long Parentobjid { get; set; }

        public required string Path { get; set; }
    }
}
