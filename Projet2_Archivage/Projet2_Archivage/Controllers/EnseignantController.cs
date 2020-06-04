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
                
                return View("PageInformations");

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
                    
                    return RedirectToAction("PageInformations");
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


        [HttpPost]
        public IActionResult ModifierProfil()
        {
            // recuperer l'enseignant
            Enseignant en = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            
                en.nom = Request.Form["nom"];
                en.prenom = Request.Form["prenom"];
                en.email = Request.Form["email"];
                _context.Update(en);
                _context.SaveChanges();
               ViewBag.enseign = en;
            return View("PageInformations"); 
        }

        

        //La page principale
        public IActionResult InsererNotes()
        {
            List<ListeNotesViewModel> liste_notes_info = new List<ListeNotesViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<int> liste_grp = _context.groupes.Where(g => g.id_ens == HttpContext.Session.GetInt32("enseignant") && g.id_filiere == 1).Select(g => g.id_grp).Cast<int>().ToList();

            foreach (var i in liste_grp)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    ListeNotesViewModel etudiantNote = new ListeNotesViewModel
                    {
                        Id_groupe = (int)j.grp_id,
                        Cne = et.cne,
                        Nom = et.nom,
                        Prenom = et.prenom,
                        Email = et.email,
                        Note = et.note
                    };
                    liste_notes_info.Add(etudiantNote);
                }
            }
            ViewBag.liste = liste_notes_info;
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes");
        }

        //les listes des notes selon la filière
        public IActionResult GinfNotes()
        {
            List<ListeNotesViewModel> liste_notes_info = new List<ListeNotesViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<int> liste_grp = _context.groupes.Where(g => g.id_ens == HttpContext.Session.GetInt32("enseignant") && g.id_filiere == 1).Select(g => g.id_grp).Cast<int>().ToList();

            foreach (var i in liste_grp)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    ListeNotesViewModel etudiantNote = new ListeNotesViewModel
                    {
                        Id_groupe = (int)j.grp_id,
                        Cne = et.cne,
                        Nom = et.nom,
                        Prenom = et.prenom,
                        Email = et.email,
                        Note = et.note
                    };
                    liste_notes_info.Add(etudiantNote);
                }
            }
            ViewBag.liste = liste_notes_info;
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes");
        }

        public IActionResult GgtrNotes()
        {
            List<ListeNotesViewModel> liste_notes = new List<ListeNotesViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<int> liste_grp = _context.groupes.Where(g => g.id_ens == HttpContext.Session.GetInt32("enseignant") && g.id_filiere == 2).Select(g => g.id_grp).Cast<int>().ToList();

            foreach (var i in liste_grp)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    ListeNotesViewModel etudiantNote = new ListeNotesViewModel
                    {
                        Id_groupe = (int)j.grp_id,
                        Cne = et.cne,
                        Nom = et.nom,
                        Prenom = et.prenom,
                        Email = et.email,
                        Note = et.note
                    };
                    liste_notes.Add(etudiantNote);
                }
            }
            ViewBag.liste = liste_notes;
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes");
        }

        public IActionResult GindusNotes()
        {
            List<ListeNotesViewModel> liste_notes = new List<ListeNotesViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<int> liste_grp = _context.groupes.Where(g => g.id_ens == HttpContext.Session.GetInt32("enseignant") && g.id_filiere == 3).Select(g => g.id_grp).Cast<int>().ToList();

            foreach (var i in liste_grp)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    ListeNotesViewModel etudiantNote = new ListeNotesViewModel
                    {
                        Id_groupe = (int)j.grp_id,
                        Cne = et.cne,
                        Nom = et.nom,
                        Prenom = et.prenom,
                        Email = et.email,
                        Note = et.note
                    };
                    liste_notes.Add(etudiantNote);
                }
            }
            ViewBag.liste = liste_notes;
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes");
        }

        public IActionResult GgpmcNotes()
        {
            List<ListeNotesViewModel> liste_notes = new List<ListeNotesViewModel>();
            List<GroupeMembre> liste_grp_mbre = new List<GroupeMembre>();
            List<int> liste_grp = _context.groupes.Where(g => g.id_ens == HttpContext.Session.GetInt32("enseignant") && g.id_filiere == 4).Select(g => g.id_grp).Cast<int>().ToList();

            foreach (var i in liste_grp)
            {
                liste_grp_mbre = _context.groupeMembres.Where(m => m.grp_id == i).ToList();
                foreach (var j in liste_grp_mbre)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == j.id_et).Single();
                    ListeNotesViewModel etudiantNote = new ListeNotesViewModel
                    {
                        Id_groupe = (int)j.grp_id,
                        Cne = et.cne,
                        Nom = et.nom,
                        Prenom = et.prenom,
                        Email = et.email,
                        Note = et.note
                    };
                    liste_notes.Add(etudiantNote);
                }
            }
            ViewBag.liste = liste_notes;
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes");
        }

        //possibilité de modifier ou ajouter une note
        [HttpPost]
        public IActionResult Ajouter()
        {

            foreach (string key in Request.Form.Keys)
            {
                if (key.Length < 20)
                {
                    Etudiant et = _context.etudiants.Where(e => e.cne == Convert.ToInt32(key)).FirstOrDefault();
                    if (Request.Form[key] != "")
                    {
                        et.note = Convert.ToDouble(Request.Form[key]);
                        _context.Update(et);
                        _context.SaveChanges();

                        ViewBag.note1 = Convert.ToDouble(Request.Form[key]);
                    }

                }
            }
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("InsererNotes"); ;
        }

        //methodes concernant la page de l'information
        public IActionResult PageInformations()
        {
            var liste_calendrier = _context.calendriers.ToList();
            ViewBag.liste = liste_calendrier;
            ViewBag.Type_page = "calendrier";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View();
        }

        public IActionResult PageCalendrier()
        {
            var liste_calendrier = _context.calendriers.ToList();
            ViewBag.liste = liste_calendrier;
            ViewBag.Type_page = "calendrier";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
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
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4 || s.id_tp == 5)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 6).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_info.Add(pfe);
                //Vider la liste etudiants
                liste_etudiants = null;
                //liste_etudiants.Clear();

            }
            ViewBag.liste = liste_pfe_info;
            ViewBag.Type_page = "Linfo";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
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
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4 || s.id_tp == 5)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 6).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_gtr.Add(pfe);
            }
            ViewBag.liste = liste_pfe_gtr;
            ViewBag.Type_page = "Lgtr";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
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
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4 || s.id_tp == 5)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 6).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_indus.Add(pfe);
            }
            ViewBag.liste = liste_pfe_indus;
            ViewBag.Type_page = "Lindus";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
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
                    Rapports_avancement = _context.files.Where(s => s.groupe_Id == i.id_grp && (s.id_tp == 2 || s.id_tp == 3 || s.id_tp == 4 || s.id_tp == 5)).ToList(),
                    Rapport_final = _context.files.Where(s => s.groupe_Id == i.id_grp && s.id_tp == 6).FirstOrDefault(),
                    Date_soutenance = i.date_stnc
                };
                liste_pfe_gpmc.Add(pfe);
            }
            ViewBag.liste = liste_pfe_gpmc;
            ViewBag.Type_page = "Lgpmc";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("PageInformations");
        }

        public IActionResult PagePlanning()
        {
            ViewBag.Type_page = "planning";
            //recuperer l'enseignant
            Enseignant enseign = _context.enseignants.Where(z => z.Id == HttpContext.Session.GetInt32("enseignant")).FirstOrDefault();
            ViewBag.enseign = enseign;
            return View("PageInformations");
        }
    }
}
