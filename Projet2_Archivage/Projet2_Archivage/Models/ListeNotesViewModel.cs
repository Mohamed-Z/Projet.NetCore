using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Projet2_Archivage.Models
{
    public class ListeNotesViewModel
    {
        public int Id_groupe { get; set; }

        public int Cne { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Email { get; set; }

        public Nullable<double> Note { get; set; }

    }
}
