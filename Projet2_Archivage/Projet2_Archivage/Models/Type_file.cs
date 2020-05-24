using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class Type_file
    {
        public virtual ICollection<File> files { get; set; }

        public Type_file()
        {
            this.files = new HashSet<File>();
        }

        [Key]
        public int id_type { get; set; }

        public String nom_type { get; set; }
    }
}