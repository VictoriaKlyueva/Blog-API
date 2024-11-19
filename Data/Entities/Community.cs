using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class Community
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsClosed { get; set; } = false;

        [Required]
        public int SubscribersCount { get; set; } = 0;

        public List<User> Users { get; set; } = new List<User>();

        public List<CommunityUser> CommunityUsers { get; set; } = new List<CommunityUser>();
    }
}
