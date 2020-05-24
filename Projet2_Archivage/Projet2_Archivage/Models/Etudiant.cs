using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class Etudiant
    {
        public virtual ICollection<GroupeMembre> GroupeMembres { get; set; }

        public Etudiant()
        {
            this.GroupeMembres = new HashSet<GroupeMembre>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cne { get; set; }

        public string nom { get; set; }

        public string prenom { get; set; }

        public string email { get; set; }

        public string tel { get; set; }

        public string cin { get; set; }

        [ForeignKey("Filiere")]
        public Nullable<int> id_fil { get; set; }
        public virtual Filiere Filiere { get; set; }
    }
}