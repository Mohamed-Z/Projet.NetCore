using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Projet2_Archivage.Models
{
    public class ArchiveContext : DbContext
    {
        public ArchiveContext (DbContextOptions<ArchiveContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> admins { get; set; }
        public DbSet<Enseignant> enseignants { get; set; }
        public DbSet<Etudiant> etudiants { get; set; }
        public DbSet<Filiere> filieres { get; set; }
        public DbSet<Calendrier> calendriers { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<Groupe> groupes { get; set; }
        public DbSet<GroupeMembre> groupeMembres { get; set; }
        public DbSet<Type_file> type_Files { get; set; }
        public DbSet<Societe> societes { get; set; }
    }
}
