using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2_Archivage.Models
{
    public class Calendrier
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }
    }
}
