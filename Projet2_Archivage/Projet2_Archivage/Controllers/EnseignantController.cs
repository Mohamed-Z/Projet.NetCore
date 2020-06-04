using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2_Archivage.Models;

namespace Projet2_Archivage.Controllers
{
    public class EnseignantController : Controller
    {
        private readonly ArchiveContext _context;

        public EnseignantController(ArchiveContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Connexion()
        {
            if (HttpContext.Session.GetInt32("enseignant") != null)
            {
                
                return View("Espace_Enseignant");

            }
            return View();
        }

        [HttpPost]
        public IActionResult Connexion(string username, string password)
        {

            var x = _context.enseignants.ToList();
            foreach (Enseignant e in x)
            {
                if (username == (e.nom + e.prenom) && password == (e.nom + e.prenom))
                {
                    HttpContext.Session.SetInt32("enseignant", e.Id);
                    return RedirectToAction("Espace_Enseignant");
                }
            }
            
            ViewBag.msg = "Email ou mot de passe incorrect !";
            return View("Connexion");
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Remove("enseignant");
            return View("Connexion");
             
        }

        public IActionResult Espace_Enseignant()
        {
            return View(); 
        }

        
      

        //methodes concernant la page de l'information
        public IActionResult PageInformations()
        {
            var liste_calendrier = _context.calendriers.ToList();
            ViewBag.liste = liste_calendrier;
            ViewBag.Type_page = "calendrier";
            return View();
        }

        public IActionResult PageCalendrier()
        {
            var liste_calendrier = _context.calendriers.ToList();
            ViewBag.liste = liste_calendrier;
            ViewBag.Type_page = "calendrier";
            return View("PageInformations");
        }

        public IActionResult PageListeInfo()
        {
            List<ListePfeViewModel> liste_pfe_info = new List<ListePfeViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<Groupe> liste_groupes = _context.groupes.Where(g => g.id_filiere == 1).ToList();

            List<Etudiant> liste_etudiants_2 = new List<Etudiant>();
            foreach (var i in liste_groupes)
            {
                List<Etudiant> liste_etudiants = new List<Etudiant>();
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i.id_grp).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    liste_etudiants.Add(et);
                }
                //liste_etudiants.Add(liste_etudiants_2.ToList());
                ListePfeViewModel pfe = new ListePfeViewModel
                {
                    Groupe_id = i.id_grp,
                    Membres = liste_etudiants,
                    Encadrant_interne = _context.enseignants.Where(s => s.Id == i.id_ens).Select(s => s.nom).FirstOrDefault(),
                    Encadrant_externe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom_enc).FirstOrDefault(),
                    Societe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom).FirstOrDefault(),
                    Ville = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.ville).FirstOrDefault(),
                    Sujet = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.sujet).FirstOrDefault(),
                    Descriptif = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 7).FirstOrDefault(),
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 1 || s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 5).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_info.Add(pfe);
                //Vider la liste etudiants
                liste_etudiants = null;
                //liste_etudiants.Clear();

            }
            ViewBag.liste = liste_pfe_info;
            ViewBag.Type_page = "Linfo";
            return View("PageInformations");
        }

        public IActionResult PageListeGtr()
        {
            List<ListePfeViewModel> liste_pfe_gtr = new List<ListePfeViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<Groupe> liste_groupes = _context.groupes.Where(g => g.id_filiere == 2).ToList();
            List<Etudiant> liste_etudiants = new List<Etudiant>();
            foreach (var i in liste_groupes)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i.id_grp).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    liste_etudiants.Add(et);
                }
                ListePfeViewModel pfe = new ListePfeViewModel
                {
                    Groupe_id = i.id_grp,
                    Membres = liste_etudiants,
                    Encadrant_interne = _context.enseignants.Where(s => s.Id == i.id_ens).Select(s => s.nom).FirstOrDefault(),
                    Encadrant_externe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom_enc).FirstOrDefault(),
                    Societe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom).FirstOrDefault(),
                    Ville = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.ville).FirstOrDefault(),
                    Sujet = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.sujet).FirstOrDefault(),
                    Descriptif = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 7).FirstOrDefault(),
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 1 || s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 5).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_gtr.Add(pfe);
            }
            ViewBag.liste = liste_pfe_gtr;
            ViewBag.Type_page = "Lgtr";
            return View("PageInformations");
        }

        public IActionResult PageListeIndus()
        {
            List<ListePfeViewModel> liste_pfe_indus = new List<ListePfeViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<Groupe> liste_groupes = _context.groupes.Where(g => g.id_filiere == 3).ToList();
            List<Etudiant> liste_etudiants = new List<Etudiant>();
            foreach (var i in liste_groupes)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i.id_grp).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    liste_etudiants.Add(et);
                }
                ListePfeViewModel pfe = new ListePfeViewModel
                {
                    Groupe_id = i.id_grp,
                    Membres = liste_etudiants,
                    Encadrant_interne = _context.enseignants.Where(s => s.Id == i.id_ens).Select(s => s.nom).FirstOrDefault(),
                    Encadrant_externe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom_enc).FirstOrDefault(),
                    Societe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom).FirstOrDefault(),
                    Ville = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.ville).FirstOrDefault(),
                    Sujet = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.sujet).FirstOrDefault(),
                    Descriptif = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 7).FirstOrDefault(),
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 1 || s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 5).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_indus.Add(pfe);
            }
            ViewBag.liste = liste_pfe_indus;
            ViewBag.Type_page = "Lindus";
            return View("PageInformations");
        }

        public IActionResult PageListeGpmc()
        {
            List<ListePfeViewModel> liste_pfe_gpmc = new List<ListePfeViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<Groupe> liste_groupes = _context.groupes.Where(g => g.id_filiere == 4).ToList();
            List<Etudiant> liste_etudiants = new List<Etudiant>();
            foreach (var i in liste_groupes)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i.id_grp).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    liste_etudiants.Add(et);
                }
                ListePfeViewModel pfe = new ListePfeViewModel
                {
                    Groupe_id = i.id_grp,
                    Membres = liste_etudiants,
                    Encadrant_interne = _context.enseignants.Where(s => s.Id == i.id_ens).Select(s => s.nom).FirstOrDefault(),
                    Encadrant_externe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom_enc).FirstOrDefault(),
                    Societe = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.nom).FirstOrDefault(),
                    Ville = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.ville).FirstOrDefault(),
                    Sujet = _context.societes.Where(s => s.Id == i.id_soc).Select(s => s.sujet).FirstOrDefault(),
                    Descriptif = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 7).FirstOrDefault(),
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 1 || s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 5).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_gpmc.Add(pfe);
            }
            ViewBag.liste = liste_pfe_gpmc;
            ViewBag.Type_page = "Lgpmc";
            return View("PageInformations");
        }

        public IActionResult PagePlanning()
        {
            ViewBag.Type_page = "planning";
            return View("PageInformations");
        }
    }
}
