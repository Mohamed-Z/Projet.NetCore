using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class Filiere
    {
        public virtual ICollection<Etudiant> etudiants { get; set; }
        public virtual ICollection<Groupe> groupes { get; set; }
        public virtual ICollection<Enseignant> enseignants { get; set; }

        public Filiere()
        {
            this.etudiants = new HashSet<Etudiant>();
            this.groupes = new HashSet<Groupe>();
            this.enseignants = new HashSet<Enseignant>();
        }

        [Key]
        public int Id_filiere { get; set; }
        public string Nom_filiere { get; set; }

    }
}