﻿using System.ComponentModel.DataAnnotations;

namespace inventory_control_of_dep_api.Models.Room
{
    public class UpdateRoomRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Number { get; set; }
    }
}
