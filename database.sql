/*remplissage de la table calendriers*/

INSERT INTO calendriers (Description)
 VALUES
 ('Ouverture_d''enregistrement'),
 ('fermuture_d''enregistrement'),
 ('Attribution_encadrants'),
 ('Dernier_délai_sujet'),
 ('Dernier_délai_rapport_avanc1'),
 ('Dernier_délai_rapport_avanc2'),
 ('Dernier_délai_rapport_avanc3'),
 ('Dernier_délai_rapport_avanc4'),
 ('Dernier_délai_rapport_final'),
 ('Dernier_délai_depot_rapport_final'),
 ('Dernier_délai_depot_rapport_final_corrigé'),
 ('Affichage_planning'),
 ('Dates_soutenances');

/*remplissage de la table type_Files*/

INSERT INTO type_Files(nom_type)
 VALUES
 ('desc'),
 ('rapport 1'),
 ('rapport 2'),
 ('rapport 3'),
 ('rapport 4'),
 ('final_rapport'),
 ('planning_stnc');

/*remplissage de la table filieres*/

INSERT INTO filieres(Nom_filiere)
 VALUES
 ('Génie Informatique'),
 ('Génie Telecom'),
 ('Génie Industriel'),
 ('Génie Procedes');

/*remplissage de la table admins*/

INSERT INTO admins(nom,prenom,email,password,confirmation)
 VALUES
 ('OUARRACHI','Maryem','maryam.ouarrachi@gmail.com','codedepasse1','codedepasse1');
