namespace BackendLaboratory.Data.Entities
{
    public class AsHouse
    {
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public Guid Objectguid { get; set; }

        public required string Housenum { get; set; }

        public required string Addnum1 { get; set; }

        public required string Addnum2 { get; set; }

        public long Parentobjid { get; set; }

        public required string Path { get; set; }
    }
}
