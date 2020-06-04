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

namespace Projet2_Archivage.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ArchiveContext db;
        private readonly IConfiguration _configuration;
        OleDbConnection Econ;
        private IHostingEnvironment _environment;
        SqlConnection con;

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
                    return RedirectToAction("EspaceAdmin");
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

            //If file exists....

            /*MemoryStream ms = new MemoryStream(file.Content, 0, 0, true, true);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "inline;filename=" + file.Name);
            Response.Clear();
            using (var sw = new StreamWriter(Response.Body))
            {
                char[] chars = Encoding.ASCII.GetChars(ms.GetBuffer());
                sw.Write(chars, 0, ms.GetBuffer().Length);
                sw.Flush();
            }
            //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            //Response.OutputStream.Flush();*/
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
    }
}
