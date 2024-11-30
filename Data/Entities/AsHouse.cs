namespace BackendLaboratory.Data.Entities
{
    public class AsHouse
    {
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public Guid Objectguid { get; set; }

        public required string Housenum { get; set; }

        public string? Addnum1 { get; set; }

        public string? Addnum2 { get; set; }

        public int Housetype { get; set; }

        public int? Addtype1 { get; set; }

        public int? Addtype2 { get; set; }

        public long Parentobjid { get; set; }

        public required string Path { get; set; }
    }
}
