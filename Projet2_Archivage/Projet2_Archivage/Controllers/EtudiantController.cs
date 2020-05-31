using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projet2_Archivage.Models;




namespace Projet2_Archivage.Controllers
{
    public class EtudiantController : Controller
    {
        ArchiveContext context;
        int ? idgrp;

       public EtudiantController(ArchiveContext context) {
            this.context = context;
            idgrp = null;



        }

        public IActionResult connexion()
        {
            if (HttpContext.Session.GetInt32("etudiant") != null)
            {
                var cne = HttpContext.Session.GetInt32("etudiant");

               
                Etudiant y = context.etudiants.SingleOrDefault(p => p.cne.Equals(cne));

                return View("Espace_Etudiant", y);
               
            }
            return View();
        }

       

        [HttpPost]
        public ActionResult connexion(Etudiant a)
        {
            if (HttpContext.Session.GetInt32("etudiant") != null)
            {
                var cne = HttpContext.Session.GetInt32("etudiant");

               
                Etudiant y = context.etudiants.SingleOrDefault(p => p.cne.Equals(cne));

                return View("Espace_Etudiant", y);
            }


            Etudiant x = context.etudiants.SingleOrDefault(p => p.cne.Equals(a.cne) );

           


            if (x != null)
            {
                if (x.cne== Int32.Parse(Request.Form["password"]))
                {

                    HttpContext.Session.SetInt32("etudiant", x.cne);
                    ViewBag.nom = HttpContext.Session.GetInt32("etudiant").ToString();
                    return View("Espace_Etudiant",x);
                }
            }
            
            
            return View();
            

        }




        [HttpGet]
        public ActionResult Espace_Etudiant(Etudiant a) {

            ViewBag.groupe = "vous etes pas dans un groupe pour creer un cliquer sur la gestion des groupe ";


            if (HttpContext.Session.GetInt32("etudiant") != null)
            {
                var cne = HttpContext.Session.GetInt32("etudiant");
                Etudiant x = context.etudiants.SingleOrDefault(p => p.cne==cne);

               

                return View(x);
            }

            else
            {
                ViewBag.nom = "ssesion null";
                return View("connexion");
            }
        }

        public ActionResult Creer_Groupe()
        {
            var cne = HttpContext.Session.GetInt32("etudiant");
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }

           if (this.idgrp == null)
            {
                return View();
            }
            
            var list = context.groupeMembres.Where(x => x.grp_id == idgrp).ToList();
            var listmembre = (from Etudiant in context.etudiants select Etudiant).ToList();
            // ViewBag.e = new SelectList(context.etudiants.Where(x => x.Filiere.Id_filiere == e.id_fil ), "cne", "nom");
            ViewBag.e = listmembre;


            return View("InviterGroupe",list);

        }



        [HttpPost]
        public ActionResult Creer_Groupe(Groupe g)
        {
            Groupe grp = new Groupe();
            var cne = HttpContext.Session.GetInt32("etudiant");
            Etudiant e = context.etudiants.SingleOrDefault(p => p.cne == cne);

            context.groupes.Add(grp);
            context.SaveChanges();
            this.idgrp = grp.id_grp;

           

            GroupeMembre createur = new GroupeMembre
            {
               grp_id = this.idgrp,
                id_et = cne,
               
            };
           
           



            context.groupeMembres.Add(createur);

            context.SaveChanges();
            ViewBag.grpmm = "fait";
            var list = context.groupeMembres.Where(x => x.grp_id == idgrp).ToList();
            var listmembre = context.etudiants.Where(x => x.Filiere.Id_filiere == e.Filiere.Id_filiere);
            ViewBag.e = listmembre;
            return    RedirectToAction("InviterGroupe",list);

        }

        [HttpGet]
        public ActionResult InviterGroupe(Groupe g)
        {
            var cne = HttpContext.Session.GetInt32("etudiant");
            Etudiant e = context.etudiants.SingleOrDefault(p => p.cne == cne);
            
            if (this.idgrp == null)
            {
                
                return RedirectToAction("Creer_Groupe");
            }

          

            var list = context.groupeMembres.Where(x => x.grp_id == this.idgrp).ToList();
            var listmembre =(from Etudiant in context.etudiants select Etudiant).ToList();
           // ViewBag.e = new SelectList(context.etudiants.Where(x => x.Filiere.Id_filiere == e.id_fil ), "cne", "nom");
            ViewBag.e = listmembre;
         
            return View(list);
            

          

        }





       

         

       








        [HttpPost]
        public ActionResult AddToGroup()
        {
            
            int cne = Int32.Parse(Request.Form["invitedEtudiant"]);
            ViewBag.cne = cne;
            var cnee = HttpContext.Session.GetInt32("etudiant");
            
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cnee);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }




            var currentGroupMembers = context.groupeMembres.Where(g => g.grp_id == this.idgrp);
            ViewBag.groupp = this.idgrp;
          
            if (currentGroupMembers.Count() > 4)
            {

           
                ViewBag.etat = "full";
                return RedirectToAction("InviterGroupe");
            }
            var deja = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (deja == null) { 
                GroupeMembre groupe = new GroupeMembre();
                groupe.grp_id = this.idgrp;
                groupe.id_et = cne;




                context.groupeMembres.Add(groupe);
                context.SaveChanges();

             
                ViewBag.etat = "added";
                return RedirectToAction("InviterGroupe");

}


        //    }

            ViewBag.etat = "deja";
           
           
            return RedirectToAction("InviterGroupe");


        }

        public ActionResult Deconnexion()
        {
            HttpContext.Session.Remove("etudiant");
            this.idgrp = null;

            return View("connexion");
        }


        public ActionResult Information_De_stage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Information_De_stage(Societe entreprise , IFormFile file)
        {
                    Models.File sujet = new Models.File();

            //var file = Request.Files[0];
          
                if (file != null && file.Length > 0)
                {






                    //new file
                    sujet.Name = file.FileName;
                    sujet.id_tp = 1;
                    var memoryStream = new MemoryStream();
                    file.CopyTo(memoryStream);
                    sujet.Content = memoryStream.ToArray();
                    sujet.Type = Request.ContentType;
                    sujet.Length = (int)file.Length;
                    DateTime localDate = DateTime.Now;
                    sujet.date_disp = Convert.ToString(localDate);
                    sujet.groupe_Id = this.idgrp;
                }
            context.files.Add(sujet);
            entreprise.id_f = sujet.Id;
            



            context.societes.Add(entreprise);
            context.SaveChanges();
            return View(entreprise);
        }

    }
}
