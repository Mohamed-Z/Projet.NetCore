using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projet2_Archivage.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string nom { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire")]
        public string prenom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Format de l'email est incorrect")]
        public string email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mot de passe faible")]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "Les mots de passe saisis ne sont pas identiques")]
        public string confirmation { get; set; }
    }
}