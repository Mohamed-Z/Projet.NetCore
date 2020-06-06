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
                Filiere f = context.filieres.SingleOrDefault(e => e.Id_filiere == y.id_fil);
                ViewBag.filiere = f.Nom_filiere;ViewBag.soc = "vous avez deja initialiser societé et fichier ";
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
                var f = context.filieres;
                string k = "me";
                foreach (Filiere ff in f)
                {
                    if (y.id_fil == ff.Id_filiere)
                    {
                        k = ff.Nom_filiere;

                    }
                }
                ViewBag.filiere = k;
                return View("Espace_Etudiant", y);
            }


            Etudiant x = context.etudiants.SingleOrDefault(p => p.cne.Equals(a.cne) );

            if (x != null)
            {
                if (x.cne== Int32.Parse(Request.Form["password"]))
                {

                    HttpContext.Session.SetInt32("etudiant", x.cne);
                    var f = context.filieres;
                    string k = "me";
                    foreach (Filiere ff in f)
                    {
                        if (x.id_fil == ff.Id_filiere)
                        {
                            k = ff.Nom_filiere;

                        }
                    }
                    ViewBag.filiere = k;

                    return View("Espace_Etudiant",x);
                }
            }
            
            return View();
        }




        [HttpGet]
        public ActionResult Espace_Etudiant(Etudiant a) {

         


            if (HttpContext.Session.GetInt32("etudiant") != null)
            {
               
                var cne = HttpContext.Session.GetInt32("etudiant");
                Etudiant x = context.etudiants.SingleOrDefault(p => p.cne==cne);
                var f = context.filieres;
                string k = "me";
                foreach (Filiere ff in f)
                {
                    if (x.id_fil == ff.Id_filiere)
                    {
                        k = ff.Nom_filiere;

                    }
                }
                ViewBag.filiere = k;

                return View(x);
            }

            else
            {
               
                return View("connexion");
            }
        }
  
    
        public ActionResult Creer_Groupe()
        {
            var cne = HttpContext.Session.GetInt32("etudiant");
           
            Etudiant y = context.etudiants.SingleOrDefault(p => p.cne == cne);
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
            var listmembre = (from Etudiant in context.etudiants select Etudiant).Where(x => x.id_fil == y.id_fil).ToList();
            // ViewBag.e = new SelectList(context.etudiants.Where(x => x.Filiere.Id_filiere == e.id_fil ), "cne", "nom");
            ViewBag.e = listmembre;


            return View("InviterGroupe", list);

        }



        [HttpPost]
        public ActionResult Creer_Groupe(Groupe g)
        {
            Groupe grp = new Groupe();
            var cne = HttpContext.Session.GetInt32("etudiant");
            Etudiant e = context.etudiants.SingleOrDefault(p => p.cne == cne);
            grp.id_filiere = e.id_fil;
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
            var list = context.groupeMembres.Where(x => x.grp_id == this.idgrp).ToList();
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
            var listmembre =context.etudiants.ToList();
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
          
            if (currentGroupMembers.Count() >= 2)
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

            var cne = HttpContext.Session.GetInt32("etudiant");
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (membre == null)
            {
                ViewBag.soc = "vous devez êtres membre d'un groupe pour réaliser cette tache !";
                return View("erreur_stage");
            }

            this.idgrp = membre.grp_id;
            /*
            var groupe = context.files.Where(x => x.groupe_Id == this.idgrp );
            foreach(Models.File f in groupe){
                if (f.id_tp == 1) {
                 ViewBag.soc = "vous avez deja marquer ça";
                return View("erreur_stage"); }
            }
            
                ViewBag.soc = "vous avez pas encore charger vos information";
                */

            Groupe groupe = context.groupes.Find(idgrp);

            Societe societe = context.societes.Find(groupe.id_soc);
            if (societe != null)
            {
                ViewBag.soc = "vous avez deja marquer ça";
                return View("erreur_stage");
            }

            ViewBag.societe = null;
            return View("Information_De_stage");
        }

        public ActionResult erreur_stage()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Information_De_stage(Societe entreprise , IFormFile file)
        {
            
            var cne = HttpContext.Session.GetInt32("etudiant");
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }

            Groupe groupe = context.groupes.Find(idgrp);

            context.societes.Add(entreprise);
            context.SaveChanges();

            groupe.id_soc = entreprise.Id;
            context.SaveChanges();
            /*
           var groupe = context.files.Where(x => x.groupe_Id ==this.idgrp && x.id_tp == 1);
            if(groupe != null)
            {
                ViewBag.soc = "vous avez deja marquer ça";
                return View("erreur_stage");
            }
            */


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
                context.files.Add(sujet);
                context.SaveChanges();

                entreprise.id_f = sujet.Id;
                context.SaveChanges();
            }

            ViewBag.societe = entreprise;
            return View();
        }





        public ActionResult Rapport()
        {
            var cnee = HttpContext.Session.GetInt32("etudiant");

            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cnee);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }

            ViewBag.i = this.idgrp;
            var filemem = context.files.Where(x => x.groupe_Id == this.idgrp && x.id_tp != 1);
            if (filemem == null)
            {
                ViewBag.nbr = "1ere upload";

            }

            else
            {

                int numeroderapport = 2;
                foreach (Models.File ff in filemem)
                {
                    if (ff.id_tp > numeroderapport)
                    {
                        numeroderapport = (int)ff.id_tp;
                        numeroderapport = numeroderapport-2 ;
                        ViewBag.nbr = numeroderapport + "eme upload";
                    }
                    if (ff.id_tp == 5)
                    {

                        ViewBag.soc = "vous avez deja telecharger tous les rapports d'avancement ";
                        return View("erreur_stage");

                    }

                }

            }
            //recuperation des dates de depot
            List<String> liste_dates = new List<String>();
            DateTime date_rap1 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc1").Select(s=> s.Date).FirstOrDefault());
            DateTime date_rap2 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc2").Select(s => s.Date).FirstOrDefault());
            DateTime date_rap3 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc3").Select(s => s.Date).FirstOrDefault());
            DateTime date_rap4 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc4").Select(s => s.Date).FirstOrDefault());
            liste_dates.Add(date_rap1.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap2.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap3.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap4.ToString("MM-dd-yyyy"));
            ViewBag.liste_dates = liste_dates;
            return View();
          }
        

  [HttpPost]
        public ActionResult Rapport(IFormFile file)
        {
            var cnee = HttpContext.Session.GetInt32("etudiant");

            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cnee);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }
            Models.File Rapport = new Models.File();

            //var file = Request.Files[0];

            if (file != null && file.Length > 0)
            {




                int ? id = 2;
               
                //new file
                Rapport.Name = file.FileName;
                var filesgroupe = context.files.Where(x => x.groupe_Id == this.idgrp && x.id_tp != 1);
                int i = 2;
              
                
                if (filesgroupe != null) {

                    foreach (Models.File f in filesgroupe)
                    {
                        if (f.id_tp == i)
                        { 
                            
                            id =id+1;
                            i++;
                            

                        }
                        if (f.id_tp == 5)
                        {

                            ViewBag.nbr = "dernier rapport d'avancement  deja telecharger ";
                            ViewBag.soc = "vous avez deja telecharger tous les rapports d'avancement ";
                            return View("erreur_stage");
                        }
                    }
                }
              
              

                   
                Rapport.id_tp =id;

                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                Rapport.Content = memoryStream.ToArray();
                Rapport.Type = Request.ContentType;
                Rapport.Length = (int)file.Length;
                DateTime localDate = DateTime.Now;
                Rapport.date_disp = Convert.ToString(localDate);
                Rapport.groupe_Id = this.idgrp;}

                
                
               
            context.files.Add(Rapport);
            context.SaveChanges();

            var filemem = context.files.Where(x => x.groupe_Id == this.idgrp && x.id_tp != 1);
            if (filemem == null)
            {
                ViewBag.nbr = "1ere upload";

            }
          
            else
            {  int numeroderapport = 2;
                foreach(Models.File ff in filemem)
                {
                    if (ff.id_tp > numeroderapport)
                    {
                        numeroderapport =(int) ff.id_tp;
                    }
                }
                numeroderapport = numeroderapport - 1;
                ViewBag.nbr = numeroderapport+"eme upload";
            }
            
            //recuperation des dates de depot
            List<String> liste_dates = new List<String>();
            DateTime date_rap1 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc1").Select(s=> s.Date).FirstOrDefault());
            DateTime date_rap2 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc2").Select(s => s.Date).FirstOrDefault());
            DateTime date_rap3 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc3").Select(s => s.Date).FirstOrDefault());
            DateTime date_rap4 = Convert.ToDateTime(context.calendriers.Where(c => c.Description == "Dernier_délai_rapport_avanc4").Select(s => s.Date).FirstOrDefault());
            liste_dates.Add(date_rap1.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap2.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap3.ToString("MM-dd-yyyy"));
            liste_dates.Add(date_rap4.ToString("MM-dd-yyyy"));
            ViewBag.liste_dates = liste_dates;
            
            return View();
        }



        public ActionResult Rapport_final()
        {
            var cne = HttpContext.Session.GetInt32("etudiant");
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }

            var groupe = context.files.Where(x => x.groupe_Id == this.idgrp);
            foreach (Models.File f in groupe)
            {
                if (f.id_tp == 6)
                {
                    ViewBag.soc = "vous avez deja telechargé ça ";
                    return View("erreur_stage");
                }
            }

            ViewBag.alert = "vous n avez pas encore telechargé votre Rapport final";
            return View();



          


        }
        [HttpPost]
        public ActionResult Rapport_final(IFormFile file)
        {
            var cne = HttpContext.Session.GetInt32("etudiant");
            GroupeMembre membre = context.groupeMembres.SingleOrDefault(x => x.id_et == cne);
            if (membre != null)
            {
                this.idgrp = membre.grp_id;
            }

            var groupe = context.files.Where(x => x.groupe_Id == this.idgrp );
            foreach(Models.File f in groupe) { 
            if (f.id_tp == 6)
            {
                ViewBag.soc = "vous avez deja marquer ça";
                return View("erreur_stage");
            }
            }

            Models.File sujet = new Models.File();

            //var file = Request.Files[0];

            if (file != null && file.Length > 0)
            {






                //new file
                sujet.Name = file.FileName;
                sujet.id_tp = 6;
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
           
            context.SaveChanges();
            ViewBag.soc = "telecharger avec succes ";
            return View("erreur_stage");
        }
    } }
