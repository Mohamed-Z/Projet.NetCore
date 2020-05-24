using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Type { get; set; }

        public int Length { get; set; }

        public byte[] Content { get; set; }

        public string date_disp { get; set; }

        [ForeignKey("Groupe")]
        public Nullable<int> groupe_Id { get; set; }
        public virtual Groupe Groupe { get; set; }

        [ForeignKey("Type_file")]
        public Nullable<int> id_tp { get; set; }
        public virtual Type_file Type_file { get; set; }

    }
}