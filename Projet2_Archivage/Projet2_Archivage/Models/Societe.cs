using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2_Archivage.Models
{
    public class Societe
    {
        public virtual ICollection<Groupe> groupes { get; set; }

        public Societe()
        {
            this.groupes = new HashSet<Groupe>();
        }

        [Key]
        public int Id { get; set; }

        public string nom { get; set; }

        public string tel { get; set; }

        public string ville { get; set; }

        public string nom_enc { get; set; }

        public string email_enc { get; set; }

        public string tel_enc { get; set; }

        public string sujet { get; set; }

        public string description { get; set; }


        [ForeignKey("File")]
        public int id_f { get; set; }
        public virtual File File { get; set; }
        
    }
}
