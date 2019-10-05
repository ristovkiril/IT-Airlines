﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Airlines.Models.Entities
{
    public class Flight
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Origin")]
        public virtual Airport AirportFrom { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public virtual Airport AirportTo { get; set; }

        [Required]
        public virtual Airplane Airplane { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TakeOffTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LandingTime { get; set; }

        [Required]
        public float BasePrice { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}-{2}", Id, AirportFrom.City, AirportTo.City);
        }
    }
}