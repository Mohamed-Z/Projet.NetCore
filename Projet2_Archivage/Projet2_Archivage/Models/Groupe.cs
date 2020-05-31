using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class Groupe
    {
        public virtual ICollection<GroupeMembre> GroupeMembres { get; set; }
        public virtual ICollection<File> Files { get; set; }

        public Groupe()
        {
            this.GroupeMembres = new HashSet<GroupeMembre>();
            this.Files = new HashSet<File>();
        }

        [Key]
        public int id_grp { get; set; }

        [ForeignKey("Enseignant")]
        public Nullable<int> id_ens { get; set; }
        public virtual Enseignant Enseignant { get; set; }

        [ForeignKey("Filiere")]
        public Nullable<int> id_filiere { get; set; }
        public virtual Filiere Filiere { get; set; }

        [ForeignKey("Societe")]
        public Nullable<int> id_soc { get; set; }
        public virtual Societe Societe { get; set; }

        public string date_stnc { get; set; }
    }
}