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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Data.SqlClient;

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
        public ActionResult Connexion()
        {
            ViewBag.erreur = "";
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Connexion(string email, string password)
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
        public ActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inscription(Admin admin)
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


        public ActionResult EspaceAdmin()
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

        public ActionResult Deconnexion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Connexion");
        }

        [HttpGet]
        public ActionResult Modifier()
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
        public ActionResult Modifier(Admin admin)
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


        public PartialViewResult AfficherDetailsUser()
        {/*
            string word = Request.Form["search"];
            string rech = Request.Form["rech"];
            if (word == "")
            {
                word = "00000000000000";
            }
            if (rech == null)
            {
                rech = "description";
            }

            SearchModelUser sm = new SearchModelUser();

            sm.searchBy(rech, word);
            

            return PartialView("_AfficherDetailsUser", sm);*/
            return PartialView("_AfficherDetailsAdmin");
        }
        /*
        public ActionResult Get(int id)
        {
            Models.File file = db.files.Find(id);

            //If file exists....

            MemoryStream ms = new MemoryStream(file.Content, 0, 0, true, true);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "inline;filename=" + file.Name);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, "application/pdf");
        }
        */

        public ActionResult Importation()
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
        public async Task<ActionResult> ImportEnseignantAsyn(IFormFile file)
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
        public async Task<ActionResult> ImportEtudiantAsyn(IFormFile file)
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
    }
}
