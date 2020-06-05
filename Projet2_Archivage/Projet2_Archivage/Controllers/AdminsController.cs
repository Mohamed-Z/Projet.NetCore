using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projet2_Archivage.Models;
using System.Data.OleDb;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ClosedXML.Excel;

namespace Projet2_Archivage.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ArchiveContext db;
        private readonly IConfiguration _configuration;
        OleDbConnection Econ;
        private IHostingEnvironment _environment;
        SqlConnection con;

        //calendrier
        Calendrier cad1 = new Calendrier();
        Calendrier cad2 = new Calendrier();
        Calendrier cad3 = new Calendrier();
        Calendrier cad4 = new Calendrier();
        Calendrier cad5 = new Calendrier();
        Calendrier cad6 = new Calendrier();
        Calendrier cad7 = new Calendrier();
        Calendrier cad8 = new Calendrier();
        Calendrier cad9 = new Calendrier();
        Calendrier cad10 = new Calendrier();
        Calendrier cad11 = new Calendrier();
        Calendrier cad12 = new Calendrier();
        Calendrier cad13 = new Calendrier();
        //date pour tester sur l input (null==date ou pas)
        DateTime date = new DateTime(0001, 01, 01);
        //endcalendrier

        public AdminsController(ArchiveContext context, IConfiguration config, IHostingEnvironment envir)
        {
            db = context;
            _configuration = config;
            _environment = envir;
            string str = _configuration.GetConnectionString("ArchiveContext");
            con = new SqlConnection(str);
        }

        private string GenerateJSONWebToken(Admin adminInfo)
        {
            var admin = db.admins.Where(x => x.email == adminInfo.email && x.password == adminInfo.password).SingleOrDefault();
            if (admin == null)
            {
                return null;
            }
            var signingKey = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var expiryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("adminid",admin.Id.ToString()),
                    new Claim("nom",admin.nom),
                    new Claim("prenom",admin.prenom),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }

        [HttpGet]
        public IActionResult Connexion()
        {
            ViewBag.erreur = "";
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public IActionResult Connexion(string email, string password)
        {
            /*
            Admin admin = new Admin{ email = email, password = password };

            var jwtToken = GenerateJSONWebToken(admin);

            if (jwtToken != null)
            {
                HttpContext.Session.SetString("alerts", "true");
                HttpContext.Session.SetString("JWToken", jwtToken);
                return RedirectToAction("EspaceAdmin");
            }
            */

            var x = db.admins.Where(y => y.email == email && y.password == password);
            foreach (Admin a in x)
            {
                if (a.email == email && a.password == password)
                {
                    HttpContext.Session.SetString("alerts", "true");
                    HttpContext.Session.SetInt32("admin_id", a.Id);
                    return RedirectToAction("Importation");
                }
            }
            ViewBag.erreur = "is-invalid";
            ViewBag.msg = "Email ou mot de passe incorrect !";
            return View();
        }

        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inscription(Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.admins.Add(admin);
                db.SaveChanges();
                HttpContext.Session.SetString("alerts", "true");
                /*
                var jwtToken = GenerateJSONWebToken(admin);
                HttpContext.Session.SetString("JWToken", jwtToken);
                */
                HttpContext.Session.SetInt32("admin_id", admin.Id);
                return RedirectToAction("EspaceAdmin");
            }
            return View();
        }


        public IActionResult EspaceAdmin()
        {
            /*
            var adminNom = HttpContext.User.Claims.Where(x => x.Type == "nom").SingleOrDefault();
            var adminPrenom = HttpContext.User.Claims.Where(x => x.Type == "prenom").SingleOrDefault();

            Admin admin = new Admin { nom = adminNom.Value, prenom = adminPrenom.Value };
            */
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.search = "clicked";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "";
            return View(admin);
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Connexion");
        }

        [HttpGet]
        public IActionResult Modifier()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "clicked";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "";
            return View(admin);
        }

        [HttpPost]
        public IActionResult Modifier(Admin admin)
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin a = db.admins.Find(id);
            if (ModelState.IsValid)
            {
                a.nom = admin.nom;
                a.prenom = admin.prenom;
                a.email = admin.email;
                a.password = admin.password;
                a.confirmation = admin.confirmation;
                db.SaveChanges();
            }
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "clicked";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "";
            return View(admin);
        }


        public PartialViewResult AfficherDetailsAdmin(string search,string rech)
        {
            if (search == null)
            {
                search = "00000000000000";
            }
            if (rech == null)
            {
                rech = "description";
            }

            SearchModelAdmin sm = new SearchModelAdmin(db);

            sm.searchBy(rech, search);

            return PartialView("_AfficherDetailsAdmin", sm);
        }
        
        public FileResult Get(int id)
        {
            Models.File file = db.files.Find(id);

            return File(file.Content, "application/pdf",file.Name);
        }
        

        public IActionResult Importation()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            if (HttpContext.Session.GetString("succes") == null)
            {
                HttpContext.Session.SetString("succes", "null");
            }
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "clicked";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "";
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> ImportEnseignantAsyn(IFormFile file)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/ExcelFolder/" + filename;
            var fullpath = this._environment.ContentRootPath + "/ExcelFolder/" + filename;
            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            InsertExcelData(filepath, filename);
            HttpContext.Session.SetString("succes", "enseignant");
            return RedirectToAction("Importation");
        }

        [HttpPost]
        public async Task<IActionResult> ImportEtudiantAsyn(IFormFile file)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/ExcelFolder/" + filename;
            var fullpath = this._environment.ContentRootPath + "/ExcelFolder/" + filename;
            using (var stream = new FileStream(fullpath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            InsertExcelData2(filepath, filename);
            HttpContext.Session.SetString("succes", "etudiant");
            return RedirectToAction("Importation");
        }

        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }

        private void InsertExcelData(string filepath, string filename)
        {
            string fullpath = this._environment.ContentRootPath + "/ExcelFolder/" + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Feuil1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();

            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            objbulk.DestinationTableName = "enseignants";
            objbulk.ColumnMappings.Add("nom", "nom");
            objbulk.ColumnMappings.Add("prenom", "prenom");
            objbulk.ColumnMappings.Add("email", "email");
            objbulk.ColumnMappings.Add("fil_id", "fil_id");
            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }

        private void InsertExcelData2(string filepath, string filename)
        {
            string fullpath = this._environment.ContentRootPath + "/ExcelFolder/" + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Feuil1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();

            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            objbulk.DestinationTableName = "etudiants";
            objbulk.ColumnMappings.Add("cne", "cne");
            objbulk.ColumnMappings.Add("nom", "nom");
            objbulk.ColumnMappings.Add("prenom", "prenom");
            objbulk.ColumnMappings.Add("email", "email");
            objbulk.ColumnMappings.Add("tel", "tel");
            objbulk.ColumnMappings.Add("cin", "cin");
            objbulk.ColumnMappings.Add("id_fil", "id_fil");
            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }

        [HttpGet]
        public IActionResult Affectation()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.e = new SelectList(db.filieres, "Id_filiere", "Nom_filiere");
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "";
            ViewBag.affect = "clicked";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "";
            return View(admin);
        }

        [HttpPost]
        public PartialViewResult AfficherListeAffectation(int id)
        {
            var x = db.groupes.Where(g => g.id_filiere == id).ToList();
            List<int> list_grp = new List<int>();
            List<List<Etudiant>> list_etuds = new List<List<Etudiant>>();
            List<string> list_societes = new List<string>();
            List<SelectList> listsl = new List<SelectList>();
            List<string> list_valid = new List<string>();

            foreach (Groupe g in x)
            {
                list_grp.Add(g.id_grp);
                Societe s = db.societes.Find(g.id_soc);
                list_societes.Add(s.nom);
                var y = (from m in db.groupeMembres
                         join e in db.etudiants on m.id_et equals e.cne
                         where m.grp_id==g.id_grp
                         select new
                         {
                            et=e
                         }).ToList();
                List<Etudiant> list_e = new List<Etudiant>();
                foreach(var i in y)
                {
                    Etudiant e = i.et;
                    list_e.Add(e);
                }
                list_etuds.Add(list_e);
                var z = db.enseignants.Where(es => es.fil_id == g.id_filiere);
                SelectList sl;
                if (g.id_ens != null)
                {
                    sl = new SelectList(z, "Id", "nom",g.id_ens);
                    list_valid.Add("is-valid");
                }
                else
                {
                    sl = new SelectList(z, "Id", "nom");
                    list_valid.Add("");
                }
                
                listsl.Add(sl);
            }
            ViewBag.grps = list_grp;
            ViewBag.names = list_etuds;
            ViewBag.sos = list_societes;
            ViewBag.sl = listsl;
            ViewBag.valid = list_valid;
            return PartialView("_AfficherListeAffectation");
        }

        public IActionResult AffectEncadrants()
        {
            List<int> list = new List<int>();
            var x = db.groupes.ToList();
            foreach(Groupe g in x)
            {
                int id = g.id_grp;
                if (Request.Form["y[" + id + "]"].Count()!=0)
                {
                    int val=Int32.Parse(Request.Form["y[" + id + "]"]);
                    g.id_ens = val;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Affectation");
        }

        public IActionResult InsertionDates()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "clicked";
            ViewBag.note = "";
            ViewBag.pln = "";
            var list = db.calendriers.ToList();
            ViewBag.liste = list;
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Inserer(DateTime ouv_enrg, DateTime ferm_enrg, DateTime attr_encd, DateTime del_sujet, DateTime del_rpavc1,
            DateTime del_rpavc2, DateTime del_rpavc3, DateTime del_rpavc4, DateTime del_rpfin, DateTime dep_rpfin, DateTime dep_rpfincrg,
            DateTime aff_plan, DateTime dat_stnc)

        {
            var list = db.calendriers.ToList();
            ViewBag.liste = list;

            /*if (ModelState.IsValid)
            {*/
            //date 1

            cad1.Description = "Ouverture_d'enregistrement";
            cad1.Date = ouv_enrg.ToString();
            if (ouv_enrg.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Ouverture_d'enregistrement"))
                {
                    db.Add(cad1);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Ouverture_d'enregistrement").FirstOrDefault();
                    cal.Date = ouv_enrg.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }

            }
            //date 2

            cad2.Description = "fermuture_d'enregistrement";
            cad2.Date = ferm_enrg.ToString();
            if (ferm_enrg.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "fermuture_d'enregistrement"))
                {
                    db.Add(cad2);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "fermuture_d'enregistrement").FirstOrDefault();
                    cal.Date = ferm_enrg.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 3

            cad3.Description = "Attribution_encadrants";
            cad3.Date = attr_encd.ToString();
            if (attr_encd.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Attribution_encadrants"))
                {
                    db.Add(cad3);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Attribution_encadrants").FirstOrDefault();
                    cal.Date = attr_encd.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 4

            cad4.Description = "Dernier_délai_sujet";
            cad4.Date = del_sujet.ToString();
            if (del_sujet.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_sujet"))
                {
                    db.Add(cad4);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_sujet").FirstOrDefault();
                    cal.Date = del_sujet.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 5

            cad5.Description = "Dernier_délai_rapport_avanc1";
            cad5.Date = del_rpavc1.ToString();
            if (del_rpavc1.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_rapport_avanc1"))
                {
                    db.Add(cad5);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_rapport_avanc1").FirstOrDefault();
                    cal.Date = del_rpavc1.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 6

            cad6.Description = "Dernier_délai_rapport_avanc2";
            cad6.Date = del_rpavc2.ToString();
            if (del_rpavc2.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_rapport_avanc2"))
                {
                    db.Add(cad6);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_rapport_avanc2").FirstOrDefault();
                    cal.Date = del_rpavc2.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 7

            cad7.Description = "Dernier_délai_rapport_avanc3";
            cad7.Date = del_rpavc3.ToString();
            if (del_rpavc3.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_rapport_avanc3"))
                {
                    db.Add(cad7);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_rapport_avanc3").FirstOrDefault();
                    cal.Date = del_rpavc3.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 8

            cad8.Description = "Dernier_délai_rapport_avanc4";
            cad8.Date = del_rpavc4.ToString();
            if (del_rpavc4.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_rapport_avanc4"))
                {
                    db.Add(cad8);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_rapport_avanc4").FirstOrDefault();
                    cal.Date = del_rpavc4.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 9

            cad9.Description = "Dernier_délai_rapport_final";
            cad9.Date = del_rpfin.ToString();
            if (del_rpfin.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_rapport_final"))
                {
                    db.Add(cad9);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_rapport_final").FirstOrDefault();
                    cal.Date = del_rpfin.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 10

            cad10.Description = "Dernier_délai_depot_rapport_final";
            cad10.Date = dep_rpfin.ToString();
            if (dep_rpfin.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_depot_rapport_final"))
                {
                    db.Add(cad10);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_depot_rapport_final").FirstOrDefault();
                    cal.Date = dep_rpfin.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 11

            cad11.Description = "Dernier_délai_depot_rapport_final_corrigé";
            cad11.Date = dep_rpfincrg.ToString();
            if (dep_rpfincrg.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dernier_délai_depot_rapport_final_corrigé"))
                {
                    db.Add(cad11);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dernier_délai_depot_rapport_final_corrigé").FirstOrDefault();
                    cal.Date = dep_rpfincrg.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 12

            cad12.Description = "Affichage_planning";
            cad12.Date = aff_plan.ToString();
            if (aff_plan.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Affichage_planning"))
                {
                    db.Add(cad12);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Affichage_planning").FirstOrDefault();
                    cal.Date = aff_plan.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //date 13

            cad13.Description = "Dates_soutenances";
            cad13.Date = dat_stnc.ToString();
            if (dat_stnc.Date.CompareTo(date) != 0)
            {
                if (!list.Exists(x => x.Description == "Dates_soutenances"))
                {
                    db.Add(cad13);
                    db.SaveChanges();
                }
                else
                {
                    var cal = db.calendriers.Where(x => x.Description == "Dates_soutenances").FirstOrDefault();
                    cal.Date = dat_stnc.Date.ToString();
                    db.Update(cal);
                    db.SaveChanges();
                }
            }
            //return RedirectToAction(nameof(InsertionDates));
            /* }*/
            return RedirectToAction("InsertionDates");
        }

        //liste des notes
        public IActionResult Notes()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "clicked";
            ViewBag.pln = "";
            List<Filiere> listf = db.filieres.ToList();
            ViewBag.listf = listf;
            return View(admin);
        }

        [HttpPost]
        public PartialViewResult AfficherNotes(int id)
        {
            List<Etudiant> liste = db.etudiants.Where(x => x.id_fil == id).ToList();
            ViewBag.liste = liste;
            ViewBag.ide = id;
            return PartialView("_AfficherNotes");
        }

        

        public IActionResult NoteExcel(int id)
        {
            var liste = db.etudiants.Where(x => x.id_fil == id).ToList();
            Filiere f = db.filieres.Find(id);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Notes_"+f.Nom_filiere);
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "CNE";
                worksheet.Cell(currentRow, 2).Value = "NOM";
                worksheet.Cell(currentRow, 3).Value = "PRENOM";
                worksheet.Cell(currentRow, 4).Value = "NOTE";
                foreach (var etud in liste)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = etud.cne;
                    worksheet.Cell(currentRow, 2).Value = etud.nom;
                    worksheet.Cell(currentRow, 3).Value = etud.prenom;
                    worksheet.Cell(currentRow, 4).Value = etud.note;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Notes "+f.Nom_filiere+".xlsx");
                }
            }

        }

        [HttpGet]
        public IActionResult Planning()
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);
            ViewBag.search = "";
            ViewBag.deconnect = "";
            ViewBag.edit = "";
            ViewBag.import = "";
            ViewBag.affect = "";
            ViewBag.date = "";
            ViewBag.note = "";
            ViewBag.pln = "clicked";
            HttpContext.Session.SetString("succes_pln", "no");
            return View(admin);
        }

        [HttpPost]
        public IActionResult Planning(IFormFile file)
        {
            int? id = HttpContext.Session.GetInt32("admin_id");
            Admin admin = db.admins.Find(id);

            Models.File sujet = new Models.File();

            Models.File fichier = db.files.Where(f => f.id_tp == 7).SingleOrDefault();

            if (fichier != null)
            {
                db.files.Remove(fichier);
                db.SaveChanges();
            }

            if (file != null && file.Length > 0)
            {
                //new file
                sujet.Name = file.FileName;
                sujet.id_tp = 7;
                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                sujet.Content = memoryStream.ToArray();
                sujet.Type = Request.ContentType;
                sujet.Length = (int)file.Length;
                DateTime localDate = DateTime.Now;
                sujet.date_disp = Convert.ToString(localDate);
            }
            db.files.Add(sujet);
            db.SaveChanges();
            HttpContext.Session.SetString("succes_pln", "yes");
            return View(admin);
        }
    }
}
