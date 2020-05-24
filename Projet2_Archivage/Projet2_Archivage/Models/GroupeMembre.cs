using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class GroupeMembre
    {
        [Key]
        public int id_gm { get; set; }

        [ForeignKey("Groupe")]
        public Nullable<int> grp_id { get; set; }
        public virtual Groupe Groupe { get; set; }

        [ForeignKey("Etudiant")]
        public Nullable<int> id_et { get; set; }
        public virtual Etudiant Etudiant { get; set; }
    }
}