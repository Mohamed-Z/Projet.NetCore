using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2_Archivage.Models
{
    public class Enseignant
    {
        public virtual ICollection<Groupe> groupes { get; set; }

        public Enseignant()
        {
            this.groupes = new HashSet<Groupe>();
        }

        [Key]
        public int Id { get; set; }

        public string nom { get; set; }

        public string prenom { get; set; }

        public string email { get; set; }

    }
}
