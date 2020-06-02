using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet2_Archivage.Models
{
    public class SearchModelAdmin
    {
        ArchiveContext db ;


        public List<List<Etudiant>> liste = new List<List<Etudiant>>();
        public List<string> listf = new List<string>();
        public List<string> listt = new List<string>();
        public List<string> lists = new List<string>();
        public List<string> listd = new List<string>();
        public List<string> listdt = new List<string>();
        public List<int> listr = new List<int>();
        public List<bool> listbool = new List<bool>();
        public List<string> listenc = new List<string>();

        public SearchModelAdmin(ArchiveContext context)
        {
            this.db = context;
            var x = (from g in db.groupes
                     join t in db.societes on g.id_soc equals t.Id
                     join ed in db.enseignants on g.id_ens equals ed.Id
                     select new
                     {
                         sos = t,
                         societe = t.nom,
                         grps = g.id_grp,
                         enc = ed
                     });

            foreach (var i in x)
            {
                listenc.Add(i.enc.nom + " " + i.enc.prenom);
                listt.Add(i.societe);
                listr.Add(i.grps);
                File f = db.files.Where(p => p.groupe_Id == i.grps).SingleOrDefault();
                Societe sos = i.sos;
                if (f != null)
                {
                    lists.Add(sos.sujet);
                    listdt.Add(f.date_disp);
                    listd.Add(sos.description);
                    listbool.Add(true);
                }
                else
                {
                    lists.Add("");
                    listdt.Add("");
                    listd.Add("");
                    listbool.Add(false);
                }
                List<Etudiant> listet = new List<Etudiant>();
                var y = (from e in db.etudiants
                         join m in db.groupeMembres on e.cne equals m.id_et
                         where m.grp_id == i.grps
                         select new
                         {
                             etudiant = e,
                         });
                foreach (var j in y)
                {
                    listet.Add(j.etudiant);
                }
                liste.Add(listet);
                listf.Add(listet[0].Filiere.Nom_filiere);
            }
        }

        internal void searchBy(string rech, string word)
        {
            if (rech.Equals("sujet"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(lists[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("description"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listd[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("type"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listt[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("filiere"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listf[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("etudiant"))
            {
                for (int i = 0; i < liste.Count; i++)
                {
                    string str = "";
                    foreach (Etudiant e in liste[i])
                    {
                        str += e.nom + " " + e.prenom + " ";
                    }
                    if (!(str.ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (rech.Equals("filiere"))
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    if (!(listenc[i].ToLower().Contains(word.ToLower())))
                    {
                        lists.RemoveAt(i);
                        listd.RemoveAt(i);
                        listt.RemoveAt(i);
                        listdt.RemoveAt(i);
                        liste.RemoveAt(i);
                        listr.RemoveAt(i);
                        listf.RemoveAt(i);
                        listbool.RemoveAt(i);
                        listenc.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
