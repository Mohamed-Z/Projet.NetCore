using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2_Archivage.Models
{
    public class ListePfeViewModel
    {
        public int Groupe_id { get; set; }
        public List<Etudiant> Membres { get; set; }
        public String Encadrant_interne { get; set; }
        public String Encadrant_externe { get; set; }
        public String Societe { get; set; }
        public String Ville { get; set; }
        public String Sujet { get; set; }
        public File Descriptif { get; set; }
        public List<File> Rapports_avancement { get; set; }
        public File Rapport_final { get; set; }
        public String Date_soutenance { get; set; }
    }
}
