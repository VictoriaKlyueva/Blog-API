﻿using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class CommunityFullDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Name { get; set; }

        public required string Description { get; set; }

        [Required]
        public bool IsClosed { get; set; } = false;

        [Required]
        public int SubscribersCount { get; set; } = 0;

        [Required]
        public required List<UserDto> Administrators { get; set; }
    }
}
