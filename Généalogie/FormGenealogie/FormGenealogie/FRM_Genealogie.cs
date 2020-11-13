using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FormGenealogie
{
    public partial class FRM_Genealogie : Form
    {
        internal FRM_Genealogie(Requetes requete) { InitializeComponent(); rq = requete; }
        private static OleDbDataReader oDr;
        private static OleDbDataReader oDr2;
        private static OleDbDataReader oDr3;
        private static OleDbDataReader oDr4;
        private static OleDbDataReader oDr5;
        private static OleDbDataReader oDr6;
        private static OleDbDataReader oDr7;
        private static OleDbDataReader oDr8;
        private static OleDbDataReader oDr9a;
        private static OleDbDataReader oDr9b;
        private static OleDbDataReader oDr10a;
        private static OleDbDataReader oDr10b;
        private static OleDbDataReader oDr11;
        private static OleDbDataReader oDr12;
        private static OleDbDataReader oDrListView1;
        private static OleDbDataReader oDrListView2;
        private static OleDbDataReader oDrListView3;
        private static OleDbDataReader oDrListViewPere;
        private static OleDbDataReader oDrListViewMere;
        private static OleDbDataReader oDrListViewConjoint;
        private OleDbConnection connection = new OleDbConnection(); 
        private OleDbCommand command = new OleDbCommand();
        private static Requetes rq = new Requetes();
        const int LB_GETITEMDATA = 0x0199;
        const int LB_SETITEMDATA = 0x019A;
        bool valide = false;
        public List<Object> dataList = new List<Object>();
        
        public FRM_Genealogie()
        {
            
            InitializeComponent();
            rq.oconn.Open();            
        }
       
        private void LB_Individu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllTXT();
            LB_NomMere.Items.Clear();
            LB_NomPere.Items.Clear();
            LB_Anniversaires.Items.Clear();
            LB_OnclesTantes.Items.Clear();
            LB_Enfants.Items.Clear();
            LB_FreresSoeurs.Items.Clear();
            LB_Conjoint.Items.Clear();
            LBL_MoisAnniv.Text = "";
            const int LB_GETITEMDATA = 0x0199;
            int idliste = LB_Individu.SelectedIndex, idtable = Program.SendMessage(LB_Individu.Handle, LB_GETITEMDATA, idliste, 0);
            oDr = rq.Extraction("SELECT * FROM individu WHERE id_individu=" + idtable, "select");
            oDr2 = rq.Extraction("SELECT * FROM commune INNER JOIN individu ON commune.commune_id = individu.ldn_individu WHERE id_individu=" + idtable, "select");
            oDr3 = rq.Extraction("SELECT * FROM commune INNER JOIN individu ON commune.commune_id = individu.ldd_individu WHERE id_individu=" + idtable, "select");
            oDr4 = rq.Extraction("SELECT * FROM commune INNER JOIN individu ON commune.commune_id = individu.id_commune_individu WHERE id_individu=" + idtable, "select");
            oDr5 = rq.Extraction("SELECT * FROM individu, (SELECT id_pere FROM individu WHERE id_individu = " + idtable + ") AS pechopere WHERE pechopere.id_pere=individu.id_individu", "select");           
            oDr6 = rq.Extraction("SELECT * FROM individu, (SELECT id_mere FROM individu WHERE id_individu = " + idtable + ") AS pechomere WHERE pechomere.id_mere = individu.id_individu", "select");           
            oDr7 = rq.Extraction("SELECT * FROM individu, (SELECT id_pere, id_mere FROM individu WHERE id_individu = " + idtable + ") AS takefrere WHERE takefrere.id_pere=individu.id_pere and takefrere.id_mere = individu.id_mere and individu.id_individu !=" + idtable, "select");
            oDr8 = rq.Extraction("SELECT * FROM individu WHERE id_pere = " + idtable + " OR id_mere =" + idtable, "select");
            oDr10a = rq.Extraction("SELECT id_pere FROM individu WHERE id_individu =" + idtable, "select");
            oDr10b = rq.Extraction("SELECT id_mere FROM individu WHERE id_individu =" + idtable, "select");
            oDr11 = rq.Extraction("SELECT * FROM individu, (SELECT id_conjoint FROM individu WHERE id_individu = " + idtable + ") AS pechoconjoint WHERE pechoconjoint.id_conjoint=individu.id_individu", "select");
            
            //if (oDr != null)
            //{
            if (oDr.Read())
                {
                    //oDr

                    //Sexe
                    if (!DBNull.Value.Equals(oDr["sexe_individu"]))
                    {
                        if ((string)oDr["sexe_individu"] == "1")
                        {
                            TXT_Sexe.Text = "Homme";
                        }
                        else
                        {
                            TXT_Sexe.Text = "Femme";
                        }
                    }else
                    {
                        TXT_Sexe.Text = "Pas de données";
                    }
                    //EtatCivil
                    TXT_IDEtatCivil.Text = ((int)oDr["id_individu"]).ToString();

                    //Nom
                    if (!DBNull.Value.Equals(oDr["nom_individu"]))
                    {
                        TXT_Nom.Text = (string)oDr["nom_individu"];
                    }else
                    {
                        TXT_Nom.Text = "Pas de données";
                    }

                    //Prenom
                    if (!DBNull.Value.Equals(oDr["prenomusage_individu"]))
                    {
                        TXT_Prenoms.Text = (string)oDr["prenomusage_individu"];
                    }else
                    {
                        TXT_Prenoms.Text = "Pas de données";
                    }

                    //Autres prenoms
                    if (!DBNull.Value.Equals(oDr["prenoms_individu"]))
                    {
                        TXT_MultiPrenoms.Text = (string)oDr["prenoms_individu"];
                    }else
                    {
                        TXT_MultiPrenoms.Text = "Pas de données";
                    }

                    //Lieu de naissance (id)
                    if (!DBNull.Value.Equals(oDr["ldn_individu"]))
                    {
                        TXT_IDNaissance.Text = ((int)oDr["ldn_individu"]).ToString();
                    }else
                    {
                        TXT_IDNaissance.Text = "Pas de données";
                    }

                    //Date de naissance
                    if (!DBNull.Value.Equals(oDr["ddn_individue"]))
                    {
                        TXT_DateNaissance.Text = (string)oDr["ddn_individue"];
                    }else
                    {
                        TXT_DateNaissance.Text = "Pas de données";
                    }

                    //Lieu de deces (id)
                    if (!DBNull.Value.Equals(oDr["ldd_individu"]))
                    {
                        TXT_IDCommuneDeces.Text = ((int)oDr["ldd_individu"]).ToString();
                    }else
                    {
                        TXT_IDCommuneDeces.Text = "Pas de données";
                    }

                    //Date de deces
                    if (!DBNull.Value.Equals(oDr["ddd_individu"]))
                    {
                        TXT_DateDeces.Text = (string)oDr["ddd_individu"];
                    }else
                    {
                        TXT_DateDeces.Text = "Pas de données";
                    }

                    //Commune (id)
                    if (!DBNull.Value.Equals(oDr["id_commune_individu"]))
                    {
                        TXT_IDCommune.Text = ((int)oDr["id_commune_individu"]).ToString();
                    }else
                    {
                        TXT_IDCommune.Text = "Pas de données";
                    }

                    //Numero de rue
                    if (!DBNull.Value.Equals(oDr["num_rue_individu"]))
                    {
                        TXT_NumRue.Text = ((int)oDr["num_rue_individu"]).ToString();
                    }else
                    {
                        TXT_NumRue.Text = "Pas de données";
                    }

                    //Nom de rue
                    if (!DBNull.Value.Equals(oDr["nom_rue_individu"]))
                    {
                        TXT_NomRue.Text = (string)oDr["nom_rue_individu"];
                    }else
                    {
                        TXT_NomRue.Text = "Pas de données";
                    }

                    //Telephone
                    if (!DBNull.Value.Equals(oDr["telephone_individu"]))
                    {
                        TXT_Telephone.Text = (string)oDr["telephone_individu"];
                    }else
                    {
                        TXT_Telephone.Text = "Pas de données";
                    }

                    //Pere (id)
                    if (!DBNull.Value.Equals(oDr["id_pere"]))
                    {
                        TXT_IDPere.Text = ((int)oDr["id_pere"]).ToString();
                    }else
                    {
                        TXT_IDPere.Text = "Pas de données";
                    }

                    //Mere (id)
                    if (!DBNull.Value.Equals(oDr["id_mere"]))
                    {
                        TXT_IDMere.Text = ((int)oDr["id_mere"]).ToString();
                    }else
                    {
                        TXT_IDMere.Text = "Pas de données";
                    }                                         
                }
                oDr.Close();

               
                    //oDr2
                    //Commune de naissance
                    if (oDr2.Read())
                    {
                        TXT_CommuneNaissance.Text = (string)oDr2["commune_nom_reel"];
                    }else
                    {
                        TXT_CommuneNaissance.Text = "Pas de données";
                    }                
                    oDr2.Close();

                    //oDr3
                    //Commune de deces
                    if (oDr3.Read())
                    {
                        TXT_CommuneDeces.Text = (string)oDr3["commune_nom_reel"];
                    }else
                    {
                        TXT_CommuneDeces.Text = "Pas de données";
                    }
                    oDr3.Close();

                
                    //oDr4
                    //Commune
                    //Code postal
                    if (oDr4.Read())
                    {
                        TXT_Commune.Text = (string)oDr4["commune_nom_reel"];
                        TXT_CP.Text = (string)oDr4["commune_code_postal"];
                    }
                    else
                    {
                        TXT_Commune.Text = "Pas de données";
                        TXT_CP.Text = "Pas de données";
                    }
                    oDr4.Close();
                                                   

                    //Nom et prénom pere
                    if (oDr5.Read())
                    {
                    //oDr5                    
                        LB_NomPere.Items.Clear();
                        idtable = (int)oDr5["id_individu"];
                        idliste = LB_NomPere.Items.Add((string)oDr5["nom_individu"] + " " + (string)oDr5["prenomusage_individu"]);
                        Program.SendMessage(LB_NomPere.Handle, LB_SETITEMDATA, idliste, idtable);
                    }else
                    {
                        idliste = LB_NomPere.Items.Add("Pas de données");
                        Program.SendMessage(LB_NomPere.Handle, LB_SETITEMDATA, idliste, idtable);
                    }                                    
                    oDr5.Close();

               
         


                     //Nom et prénom mere
                     if (oDr6.Read())
                     {
                     //oDr6
                        LB_NomMere.Items.Clear();
                        idtable = (int)oDr6["id_individu"];
                        idliste = LB_NomMere.Items.Add((string)oDr6["nom_individu"] + " " + (string)oDr6["prenomusage_individu"]);
                        Program.SendMessage(LB_NomMere.Handle, LB_SETITEMDATA, idliste, idtable);
                     }else
                     {
                        idliste = LB_NomMere.Items.Add("Pas de données");
                        Program.SendMessage(LB_NomMere.Handle, LB_SETITEMDATA, idliste, idtable);
                     }                                   
                     oDr6.Close();
                 
                     //Nom et prénom enfants  
                     LB_Enfants.Items.Clear();                                 
                     while (oDr8.Read())
                     {
                     //oDr8                        
                        idtable = (int)oDr8["id_individu"];
                        idliste = LB_Enfants.Items.Add(((string)oDr8["nom_individu"]) + " " + (string)oDr8["prenomusage_individu"]);
                        Program.SendMessage(LB_Enfants.Handle, LB_SETITEMDATA, idliste, idtable);                   
                     }                                   
                     oDr8.Close();

                     //Nom et prénom frères et soeurs    
                     LB_FreresSoeurs.Items.Clear(); 
                     while (oDr7.Read())
                     {
                     //oDr7                        
                        idtable = (int)oDr7["id_individu"];
                        idliste = LB_FreresSoeurs.Items.Add((string)oDr7["nom_individu"] + " " + (string)oDr7["prenomusage_individu"]);
                        Program.SendMessage(LB_FreresSoeurs.Handle, LB_SETITEMDATA, idliste, idtable);
                     }                               
                     oDr7.Close();
                        
                     //Nom et prénom conjoint
                    if (oDr11.Read())
                    {
                    //oDr11                    
                        LB_Conjoint.Items.Clear();
                        idtable = (int)oDr11["id_individu"];
                        idliste = LB_Conjoint.Items.Add((string)oDr11["nom_individu"] + " " + (string)oDr11["prenomusage_individu"]);
                        Program.SendMessage(LB_Conjoint.Handle, LB_SETITEMDATA, idliste, idtable);
                    }
                    else
                    {
                        idliste = LB_Conjoint.Items.Add("Pas de données");
                        Program.SendMessage(LB_Conjoint.Handle, LB_SETITEMDATA, idliste, idtable);
                    }
                    oDr11.Close();

                    string recherche = null;
                    
                    recherche = TXT_DateNaissance.Text.Substring(2, 4);
                    oDr12 = rq.Extraction("SELECT * FROM individu WHERE ddn_individue LIKE '%" + recherche + "%'", "select");

                    if(recherche == "/01/") { LBL_MoisAnniv.Text = "Janvier"; } if(recherche == "/02/") { LBL_MoisAnniv.Text = "Février"; } if(recherche == "/03/") { LBL_MoisAnniv.Text = "Mars"; } if(recherche == "/04/") { LBL_MoisAnniv.Text = "Avril"; }if(recherche == "/05/") { LBL_MoisAnniv.Text = "Mai"; }if(recherche == "/06/") { LBL_MoisAnniv.Text = "Juin"; }if(recherche == "/07/") { LBL_MoisAnniv.Text = "Juillet"; }if(recherche == "/08/") { LBL_MoisAnniv.Text = "Août"; } if(recherche == "/09/") { LBL_MoisAnniv.Text = "Septembre"; }if(recherche == "/10/") { LBL_MoisAnniv.Text = "Octobre"; }if(recherche == "/11/") { LBL_MoisAnniv.Text = "Novembre"; }if(recherche == "/12/") { LBL_MoisAnniv.Text = "Décembre"; }
                    
                    
                    

                    

            //Nom et prénom anniv
            
            if (oDr12 != null )
            {
                LB_Anniversaires.Items.Clear();
                while (oDr12.Read())
                {
                    //oDr12                                               
                    idtable = (int)oDr12["id_individu"];
                    idliste = LB_Anniversaires.Items.Add((string)oDr12["nom_individu"] + " " + (string)oDr12["prenomusage_individu"]);
                    Program.SendMessage(LB_Anniversaires.Handle, LB_SETITEMDATA, idliste, idtable);
                }
               
            }
            else
            {
                LB_Anniversaires.Items.Clear();
                idliste = LB_Anniversaires.Items.Add("Pas de données");
                Program.SendMessage(LB_Anniversaires.Handle, LB_SETITEMDATA, idliste, idtable);
            }
            oDr12.Close();

                     //Nom et prénom oncles et tantes
                     int a=0,b=0;
                     if (oDr10a.Read())
                     {
                        a = (int)oDr10a["id_pere"];
                     }
                     oDr10a.Close();

                     if (oDr10b.Read())
                     {
                        b = (int)oDr10b["id_mere"];
                     }
                     oDr10b.Close();

                     oDr9a = rq.Extraction("SELECT * FROM individu, (SELECT id_pere, id_mere FROM individu WHERE id_individu = " + a + ") AS takepere WHERE (takepere.id_pere=individu.id_pere or takepere.id_mere = individu.id_mere) and individu.id_individu !=" + a, "select");
                     oDr9b = rq.Extraction("SELECT * FROM individu, (SELECT id_pere, id_mere FROM individu WHERE id_individu = " + b + ") AS takepere WHERE (takepere.id_pere=individu.id_pere or takepere.id_mere = individu.id_mere) and individu.id_individu !=" + b, "select");

                     LB_OnclesTantes.Items.Clear();
                     while (oDr9a.Read())
                     {
                     //oDr9a
                        idtable = (int)oDr9a["id_individu"];
                        idliste = LB_OnclesTantes.Items.Add((string)oDr9a["nom_individu"] + " " + (string)oDr9a["prenomusage_individu"]);
                        Program.SendMessage(LB_OnclesTantes.Handle, LB_SETITEMDATA, idliste, idtable);
                     }
                     oDr9a.Close();
                    
                     while (oDr9b.Read())
                     { 
                     //oDr9b
                        idtable = (int)oDr9b["id_individu"];
                        idliste = LB_OnclesTantes.Items.Add((string)oDr9b["nom_individu"] + " " + (string)oDr9b["prenomusage_individu"]);
                        Program.SendMessage(LB_OnclesTantes.Handle, LB_SETITEMDATA, idliste, idtable);
                     }
                     oDr9b.Close();

                    
            
                    
                    
            //}
        }

        private void Initialisation()
        {
            int idliste, idtable;

            oDr = rq.Extraction("SELECT * FROM individu", "select");
            
            if (oDr != null)

            {

                LB_Individu.Items.Clear();

                while (oDr.Read())

                {
                   
                        string nom, prenom;
                        idtable = (int)oDr["id_individu"];

                        if (DBNull.Value.Equals(oDr["prenomusage_individu"]))
                        {
                            prenom = "Inconnu";
                            idliste = LB_Individu.Items.Add((string)oDr["nom_individu"] + " " + prenom);
                            Program.SendMessage(LB_Individu.Handle, LB_SETITEMDATA, idliste, idtable);

                        }

                        if (DBNull.Value.Equals(oDr["nom_individu"]))
                        {
                            nom = "Inconnu";
                            idliste = LB_Individu.Items.Add(nom + " " + (string)oDr["prenomusage_individu"]);
                            Program.SendMessage(LB_Individu.Handle, LB_SETITEMDATA, idliste, idtable);


                        }
                        if(!DBNull.Value.Equals(oDr["nom_individu"]) && !DBNull.Value.Equals(oDr["prenomusage_individu"]))
                        {
                            idliste = LB_Individu.Items.Add((string)oDr["nom_individu"] + " " + (string)oDr["prenomusage_individu"]);
                            Program.SendMessage(LB_Individu.Handle, LB_SETITEMDATA, idliste, idtable);
                        }



                    
                }

                oDr.Close();

                

                oDrListView1 = rq.Extraction("SELECT * FROM Commune ORDER BY commune_code_postal", "select");
                oDrListView2 = rq.Extraction("SELECT * FROM Commune ORDER BY commune_code_postal", "select");
                oDrListView3 = rq.Extraction("SELECT * FROM Commune ORDER BY commune_code_postal", "select");
                oDrListViewPere = rq.Extraction("SELECT * FROM individu", "select");
                oDrListViewMere = rq.Extraction("SELECT * FROM individu", "select");
                oDrListViewConjoint = rq.Extraction("SELECT * FROM individu", "select");

                ListViewItem items1 = new ListViewItem();
                if(oDrListView1 != null)
                {
                    while (oDrListView1.Read())
                    {
                        items1 = new ListViewItem(new string[] { oDrListView1[5].ToString(), oDrListView1[8].ToString(), oDrListView1[0].ToString()});
                        LV_CommuneCoordonnees.Items.Add(items1);
                    }
                }
                ListViewItem items2 = new ListViewItem();
                if (oDrListView2 != null)
                {
                    while (oDrListView2.Read())
                    {
                        items2 = new ListViewItem(new string[] { oDrListView2[5].ToString(), oDrListView2[8].ToString(), oDrListView2[0].ToString() });
                        LV_CommuneDeces.Items.Add(items2);
                    }
                }
                ListViewItem items3 = new ListViewItem();
                if (oDrListView3 != null)
                {
                    while (oDrListView3.Read())
                    {
                        items3 = new ListViewItem(new string[] { oDrListView3[5].ToString(), oDrListView3[8].ToString(), oDrListView3[0].ToString() });
                        LV_CommuneNaissance.Items.Add(items3);
                    }
                }
                ListViewItem itemsPere = new ListViewItem();
                if (oDrListViewPere != null)
                {
                    while (oDrListViewPere.Read())
                    {
                        itemsPere = new ListViewItem(new string[] { oDrListViewPere[1].ToString(), oDrListViewPere[3].ToString(), oDrListViewPere[0].ToString() });
                        LV_AjoutPere.Items.Add(itemsPere);
                    }
                }
                ListViewItem itemsMere = new ListViewItem();
                if (oDrListViewMere != null)
                {
                    while (oDrListViewMere.Read())
                    {
                        itemsMere = new ListViewItem(new string[] { oDrListViewMere[1].ToString(), oDrListViewMere[3].ToString(), oDrListViewMere[0].ToString() });
                        LV_AjoutMere.Items.Add(itemsMere);
                    }
                }
                ListViewItem itemsConjoint = new ListViewItem();
                if (oDrListViewConjoint != null)
                {
                    while (oDrListViewConjoint.Read())
                    {
                        itemsConjoint = new ListViewItem(new string[] { oDrListViewConjoint[1].ToString(), oDrListViewConjoint[3].ToString(), oDrListViewConjoint[0].ToString() });
                        LV_AjoutConjoint.Items.Add(itemsConjoint);
                    }
                }

            }
        }

        private void FRM_Genealogie_Load(object sender, EventArgs e)
        {
            Initialisation();
        }

        private void ClearAllLB()
        {
            LB_ActeDeces.Items.Clear();
            LB_ActeDivorce.Items.Clear();
            LB_ActeNaissance.Items.Clear();
            LB_Anniversaires.Items.Clear();
            LB_Enfants.Items.Clear();
            LB_FreresSoeurs.Items.Clear();
            LB_NomMere.Items.Clear();
            LB_NomPere.Items.Clear();
            LB_OnclesTantes.Items.Clear();
            LB_Mariage.Items.Clear();
            LB_Conjoint.Items.Clear();
        }
        private void ClearAllTXT()
        {
            TXT_Commune.Clear();
            TXT_CommuneDeces.Clear();
            TXT_CommuneNaissance.Clear();
            TXT_CP.Clear();
            TXT_DateDeces.Clear();
            TXT_DateNaissance.Clear();
            TXT_IDCommune.Clear();
            TXT_IDCommuneDeces.Clear();
            TXT_IDEtatCivil.Clear();
            TXT_IDList.Clear();
            TXT_IDMere.Clear();
            TXT_IDNaissance.Clear();
            TXT_IDPere.Clear();
            TXT_MultiPrenoms.Clear();
            TXT_Nom.Clear();
            TXT_NomRue.Clear();
            TXT_NumRue.Clear();
            TXT_Prenoms.Clear();
            TXT_Sexe.Clear();
            TXT_Telephone.Clear();
            TXT_IDConjoint.Clear();
            RAD_Femme.Checked = false;
            RAD_Homme.Checked = false;
        }

        private void AffichePerso (ListBox lb)
        {
            object RecupEnfant = (int)oDr8["id_individu"];
            oDr = rq.Extraction("SELECT * FROM individu WHERE id_individu=" + RecupEnfant, "select");                       
        }
        private void LB_FreresSoeurs_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_FreresSoeurs);
            AffichePerso(LB_FreresSoeurs);
            
        }

        private void LB_Enfants_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_Enfants);
            AffichePerso(LB_Enfants);
            
        }

        private void LB_OnclesTantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_OnclesTantes);
            AffichePerso(LB_OnclesTantes);
            
        }

        private void B_CommuneNaissance_Click(object sender, EventArgs e)
        {
            valide = !valide;
            LV_CommuneNaissance.Visible = valide;
            LV_CommuneCoordonnees.Visible = false;
            LV_CommuneDeces.Visible = false;
            LBL_RechercheCoordonnees.Visible = false;
            TXT_RechercheCoordonnees.Visible = false;
            LBL_RechercheDeces.Visible = false;
            TXT_RechercheDeces.Visible = false;
            LBL_RechercheMere.Visible = false;
            TXT_RechercheMere.Visible = false;
            LBL_RecherchePere.Visible = false;
            TXT_RecherchePere.Visible = false;
            TXT_RechercheNaissance.Visible = valide;
            LBL_RechercheNaissance.Visible = valide;
            LBL_RechercheConjoint.Visible = false;
            TXT_RechercheConjoint.Visible = false;
        }

        private void B_CommuneCoordonnees_Click(object sender, EventArgs e)
        {
            valide = !valide;
            LV_CommuneCoordonnees.Visible = valide;
            LV_CommuneDeces.Visible = false;
            LV_CommuneNaissance.Visible = false;
            LBL_RechercheCoordonnees.Visible = valide;
            TXT_RechercheCoordonnees.Visible = valide;
            LBL_RechercheDeces.Visible = false;
            TXT_RechercheDeces.Visible = false;
            LBL_RechercheMere.Visible = false;
            TXT_RechercheMere.Visible = false;
            LBL_RecherchePere.Visible = false;
            TXT_RecherchePere.Visible = false;
            TXT_RechercheNaissance.Visible = false;
            LBL_RechercheNaissance.Visible = false;
            LBL_RechercheConjoint.Visible = false;
            TXT_RechercheConjoint.Visible = false;
        }

        private void B_CommuneDeces_Click(object sender, EventArgs e)
        {
            valide = !valide;
            LV_CommuneDeces.Visible = valide;
            LV_CommuneNaissance.Visible = false;
            LV_AjoutConjoint.Visible = false;
            LV_CommuneCoordonnees.Visible = false;
            LBL_RechercheCoordonnees.Visible = false;
            TXT_RechercheCoordonnees.Visible = false;
            LBL_RechercheDeces.Visible = valide;
            TXT_RechercheDeces.Visible = valide;
            LBL_RechercheMere.Visible = false;
            TXT_RechercheMere.Visible = false;
            LBL_RecherchePere.Visible = false;
            TXT_RecherchePere.Visible = false;
            TXT_RechercheNaissance.Visible = false;
            LBL_RechercheNaissance.Visible = false;
            LBL_RechercheConjoint.Visible = false;
            TXT_RechercheConjoint.Visible = false;
        }

        private void B_AjoutMere_Click(object sender, EventArgs e)
        {           
            valide = !valide;
            LV_AjoutMere.Visible = valide;
            LV_AjoutPere.Visible = false;
            LV_AjoutConjoint.Visible = false;
            LBL_RechercheCoordonnees.Visible = false;
            TXT_RechercheCoordonnees.Visible = false;
            LBL_RechercheDeces.Visible = false;
            TXT_RechercheDeces.Visible = false;
            LBL_RechercheMere.Visible = valide;
            TXT_RechercheMere.Visible = valide;
            LBL_RecherchePere.Visible = false;
            TXT_RecherchePere.Visible = false;
            TXT_RechercheNaissance.Visible = false;
            LBL_RechercheNaissance.Visible = false;
            LBL_RechercheConjoint.Visible = false;
            TXT_RechercheConjoint.Visible = false;
        }

        private void B_AjoutPere_Click(object sender, EventArgs e)
        {   
            valide = !valide;
            LV_AjoutPere.Visible = valide;
            LV_AjoutMere.Visible = false;
            LV_AjoutConjoint.Visible = false;
            LBL_RechercheCoordonnees.Visible = false;
            TXT_RechercheCoordonnees.Visible = false;
            LBL_RechercheDeces.Visible = false;
            TXT_RechercheDeces.Visible = false;
            LBL_RechercheMere.Visible = false;
            TXT_RechercheMere.Visible = false;
            LBL_RecherchePere.Visible = valide;
            TXT_RecherchePere.Visible = valide;
            TXT_RechercheNaissance.Visible = false;
            LBL_RechercheNaissance.Visible = false;
            LBL_RechercheConjoint.Visible = false;
            TXT_RechercheConjoint.Visible = false;
        }
        private void B_AjoutConjoint_Click(object sender, EventArgs e)
        {
            valide = !valide;
            LV_AjoutPere.Visible = false;
            LV_AjoutMere.Visible = false;
            LV_AjoutConjoint.Visible = valide;
            LBL_RechercheCoordonnees.Visible = false;
            TXT_RechercheCoordonnees.Visible = false;
            LBL_RechercheDeces.Visible = false;
            TXT_RechercheDeces.Visible = false;
            LBL_RechercheMere.Visible = false;
            TXT_RechercheMere.Visible = false;
            LBL_RecherchePere.Visible = false;
            TXT_RecherchePere.Visible = false;
            TXT_RechercheNaissance.Visible = false;
            LBL_RechercheNaissance.Visible = false;
            LBL_RechercheConjoint.Visible = valide;
            TXT_RechercheConjoint.Visible = valide;
        }

        private void LV_CommuneNaissance_SelectedIndexChanged(object sender, EventArgs e)
        {
            string commune, id;
            commune = LV_CommuneNaissance.SelectedItems[0].Text;
            id = LV_CommuneNaissance.SelectedItems[0].SubItems[2].Text;
            TXT_CommuneNaissance.Text = commune;
            TXT_IDNaissance.Text = id;
        }

        private void LV_CommuneDeces_SelectedIndexChanged(object sender, EventArgs e)
        {
            string commune, id;
            commune = LV_CommuneDeces.SelectedItems[0].Text;
            id = LV_CommuneDeces.SelectedItems[0].SubItems[2].Text;
            TXT_CommuneDeces.Text = commune;
            TXT_IDCommuneDeces.Text = id;
        }

        private void LV_CommuneCoordonnees_SelectedIndexChanged(object sender, EventArgs e)
        {
            string commune, code_postal, id;
            commune = LV_CommuneCoordonnees.SelectedItems[0].Text;
            TXT_Commune.Text = commune;
            code_postal = LV_CommuneCoordonnees.SelectedItems[0].SubItems[1].Text;
            TXT_CP.Text = code_postal;
            id = LV_CommuneCoordonnees.SelectedItems[0].SubItems[2].Text;
            TXT_IDCommune.Text = id;
        }

        private void LV_AjoutPere_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nom, id, prenom;
            nom = LV_AjoutPere.SelectedItems[0].Text;
            prenom = LV_AjoutPere.SelectedItems[0].SubItems[1].Text;
            LB_NomPere.Items.Clear();
            LB_NomPere.Items.Add(nom + " " + prenom);
            id = LV_AjoutPere.SelectedItems[0].SubItems[2].Text;
            TXT_IDPere.Text = id;
        }

        private void LV_AjoutMere_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string id, nom, prenom;
            nom = LV_AjoutMere.SelectedItems[0].Text;
            prenom = LV_AjoutMere.SelectedItems[0].SubItems[1].Text;
            LB_NomMere.Items.Clear();
            LB_NomMere.Items.Add(nom + " " + prenom);
            id = LV_AjoutMere.SelectedItems[0].SubItems[2].Text;
            TXT_IDMere.Text = id;
        }
        private void LV_AjoutConjoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id, nom, prenom;
            nom = LV_AjoutConjoint.SelectedItems[0].Text;
            prenom = LV_AjoutConjoint.SelectedItems[0].SubItems[1].Text;
            LB_Conjoint.Items.Clear();
            LB_Conjoint.Items.Add(nom + " " + prenom);
            id = LV_AjoutConjoint.SelectedItems[0].SubItems[2].Text;
            TXT_IDConjoint.Text = id;
        }
        private void VerifyNull()
        {
            if (string.IsNullOrEmpty(TXT_DateNaissance.Text) || TXT_DateNaissance.Text == "Pas de données") { TXT_DateNaissance.Text = "NULL"; } else { TXT_DateNaissance.Text = "'" + TXT_DateNaissance.Text + "'"; }if (string.IsNullOrEmpty(TXT_DateDeces.Text) || TXT_DateDeces.Text == "Pas de données") { TXT_DateDeces.Text = "NULL"; } else { TXT_DateDeces.Text = "'" + TXT_DateDeces.Text + "'"; }if (string.IsNullOrEmpty(TXT_Commune.Text) || TXT_Commune.Text == "Pas de données") { TXT_Commune.Text = "NULL"; } else { TXT_Commune.Text = "'" + TXT_Commune.Text + "'"; } if (string.IsNullOrEmpty(TXT_CommuneDeces.Text) || TXT_CommuneDeces.Text == "Pas de données") { TXT_CommuneDeces.Text = "NULL"; } else { TXT_CommuneDeces.Text = "'" + TXT_CommuneDeces + "'"; } if (string.IsNullOrEmpty(TXT_CommuneNaissance.Text) || TXT_CommuneNaissance.Text == "Pas de données") { TXT_CommuneNaissance.Text = "NULL"; } else { TXT_CommuneNaissance.Text = "'" + TXT_CommuneNaissance.Text + "'"; }  if (string.IsNullOrEmpty(TXT_CP.Text) || TXT_CP.Text == "Pas de données") { TXT_CP.Text = "NULL"; } else { TXT_CP.Text = "'" + TXT_CP.Text + "'"; }   if (string.IsNullOrEmpty(TXT_MultiPrenoms.Text) || TXT_MultiPrenoms.Text == "Pas de données") { TXT_MultiPrenoms.Text = "NULL"; } else { TXT_MultiPrenoms.Text = "'" + TXT_MultiPrenoms.Text + "'"; } if (string.IsNullOrEmpty(TXT_Nom.Text) || TXT_Nom.Text == "Pas de données") { TXT_Nom.Text = "NULL"; } else { TXT_Nom.Text = "'" + TXT_Nom.Text + "'"; } if (string.IsNullOrEmpty(TXT_NomRue.Text) || TXT_NomRue.Text == "Pas de données") { TXT_NomRue.Text = "NULL"; } else { TXT_NomRue.Text = "'" + TXT_NomRue.Text + "'"; } if (string.IsNullOrEmpty(TXT_NumRue.Text) || TXT_NumRue.Text == "Pas de données") { TXT_NumRue.Text = "NULL"; } else { TXT_NumRue.Text = "'" + TXT_NumRue.Text + "'"; } if (string.IsNullOrEmpty(TXT_Prenoms.Text) || TXT_Prenoms.Text == "Pas de données") { TXT_Prenoms.Text = "NULL"; } else { TXT_Prenoms.Text = "'" + TXT_Prenoms.Text + "'"; } if (string.IsNullOrEmpty(TXT_Sexe.Text) || TXT_Sexe.Text == "Pas de données") { TXT_Sexe.Text = "NULL"; } else { TXT_Sexe.Text = "'" + TXT_Sexe.Text + "'"; } if(string.IsNullOrEmpty(TXT_Telephone.Text) || TXT_Telephone.Text == "Pas de données") { TXT_Telephone.Text = "NULL"; } else { TXT_Telephone.Text = "'" + TXT_Telephone.Text + "'"; }
        }
        private void B_Ajouter_Click(object sender, EventArgs e)
        {
            object idnaissance, iddeces, idmere, idpere, idcommune, idetatcivil, idconjoint;
            if (string.IsNullOrEmpty(TXT_IDCommune.Text) || TXT_IDCommune.Text == "Pas de données") { idcommune = System.Data.SqlTypes.SqlInt32.Null; } else { idcommune = "'" + TXT_IDCommune.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDCommuneDeces.Text) || TXT_IDCommuneDeces.Text == "Pas de données") { iddeces = System.Data.SqlTypes.SqlInt32.Null; } else { iddeces = "'" + TXT_IDCommuneDeces.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDConjoint.Text) || TXT_IDConjoint.Text == "Pas de données") { idconjoint = System.Data.SqlTypes.SqlInt32.Null; } else { idconjoint = "'" + TXT_IDConjoint.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDEtatCivil.Text) || TXT_IDEtatCivil.Text == "Pas de données") { idetatcivil = System.Data.SqlTypes.SqlInt32.Null; } else { idetatcivil = "'" + TXT_IDEtatCivil.Text + "'"; } if(string.IsNullOrEmpty(TXT_IDMere.Text) || TXT_IDMere.Text == "Pas de données") { idmere = System.Data.SqlTypes.SqlInt32.Null; } else { idmere = "'" + TXT_IDMere.Text + "'"; }if (string.IsNullOrEmpty(TXT_IDNaissance.Text) || TXT_IDNaissance.Text == "Pas de données") { idnaissance = System.Data.SqlTypes.SqlInt32.Null; } else { idnaissance = "'" + TXT_IDNaissance.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDPere.Text) || TXT_IDPere.Text == "Pas de données") {idpere = System.Data.SqlTypes.SqlInt32.Null; } else { idpere = "'" + TXT_IDPere.Text + "'"; }
            string sexeajout;
            if (TXT_Sexe.Text == "Homme")
            {
                sexeajout = "'1'";
            }
            else if (TXT_Sexe.Text == "Femme")
            {
                sexeajout = "'0'";
            }
            else
            {
                sexeajout = "NULL";
            }
            VerifyNull();
            const int LB_SETITEMDATA = 0x019A;
            int idliste = rq.ExtractionSimple("SELECT count(*) FROM individu","Select count", "B_Count_Click"), 
            idtable = idliste +1;
            rq.Insertion("INSERT INTO individu (nom_individu, prenoms_individu, prenomusage_individu, sexe_individu, ddn_individue, ldn_individu, ddd_individu, num_rue_individu, nom_rue_individu, telephone_individu, id_pere, id_mere, id_conjoint, id_commune_individu, ldd_individu) VALUES (" + "" + TXT_Nom.Text + ", " + TXT_MultiPrenoms.Text + ", " + TXT_Prenoms.Text + " , " + sexeajout + " , " + TXT_DateNaissance.Text + " , " + idnaissance + " , " + TXT_DateDeces.Text + " , " + TXT_NumRue.Text + ", " + TXT_NomRue.Text + ", " + TXT_Telephone.Text + ", " + idpere + " , " + idmere + ", " + idconjoint + ", " +idcommune + ", " + iddeces + ")", "Insert", "B_Insertion_Click");
            
            

            Program.SendMessage(LB_Individu.Handle, LB_SETITEMDATA, idliste, idtable);
            ClearAllTXT();
            ClearAllLB();
            Initialisation();
        }

        private void RAD_Homme_CheckedChanged(object sender, EventArgs e)
        {
            if(RAD_Homme.Checked == true)
            {
                TXT_Sexe.Text = "Homme";
                RAD_Femme.Checked = false;
            }
        }
        private void RAD_Femme_CheckedChanged(object sender, EventArgs e)
        {
            if (RAD_Femme.Checked == true)
            {
                TXT_Sexe.Text = "Femme";
                RAD_Homme.Checked = false;
            }
        }

        private void LB_NomPere_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_NomPere);
        }

        private void LB_NomMere_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_NomMere);
        }
        private void SelectLB (ListBox lb)
        {
            string item = lb.SelectedItem.ToString();
            int index = LB_Individu.FindString(item);
            if (index == -1)
                MessageBox.Show("Personne inconnue");
            else
                LB_Individu.SetSelected(index, true);
        }

        private void LB_Conjoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_Conjoint);
        }

        private void GPX_Mere_Enter(object sender, EventArgs e)
        {

        }

        private void B_Clear_Click(object sender, EventArgs e)
        {
            ClearAllTXT();
            ClearAllLB();
        }

        private void B_Supprimer_Click(object sender, EventArgs e)
        {
            try
            {
                int idliste = LB_Individu.SelectedIndex;
                int idtable = Program.SendMessage(LB_Individu.Handle, LB_GETITEMDATA, idliste, 0);
                rq.Suppression("DELETE FROM individu WHERE id_individu =" + idtable);
                Initialisation();
                ClearAllLB();
                ClearAllTXT();
            }
            catch(Exception)
            {
                MessageBox.Show("Erreur");
            }
           
        }

        private void B_Modifier_Click(object sender, EventArgs e)
        {
            string sexe;
            object idnaissance, iddeces, idmere, idpere, idcommune, idetatcivil, idconjoint;
            if (string.IsNullOrEmpty(TXT_IDCommune.Text) || TXT_IDCommune.Text == "Pas de données") { idcommune = System.Data.SqlTypes.SqlInt32.Null; } else { idcommune = "'" + TXT_IDCommune.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDCommuneDeces.Text) || TXT_IDCommuneDeces.Text == "Pas de données") { iddeces = System.Data.SqlTypes.SqlInt32.Null; } else { iddeces = "'" + TXT_IDCommuneDeces.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDConjoint.Text) || TXT_IDConjoint.Text == "Pas de données") { idconjoint = System.Data.SqlTypes.SqlInt32.Null; } else { idconjoint = "'" + TXT_IDConjoint.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDEtatCivil.Text) || TXT_IDEtatCivil.Text == "Pas de données") { idetatcivil = System.Data.SqlTypes.SqlInt32.Null; } else { idetatcivil = "'" + TXT_IDEtatCivil.Text + "'"; } if(string.IsNullOrEmpty(TXT_IDMere.Text) || TXT_IDMere.Text == "Pas de données") { idmere = System.Data.SqlTypes.SqlInt32.Null; } else { idmere = "'" + TXT_IDMere.Text + "'"; }if (string.IsNullOrEmpty(TXT_IDNaissance.Text) || TXT_IDNaissance.Text == "Pas de données") { idnaissance = System.Data.SqlTypes.SqlInt32.Null; } else { idnaissance = "'" + TXT_IDNaissance.Text + "'"; } if (string.IsNullOrEmpty(TXT_IDPere.Text) || TXT_IDPere.Text == "Pas de données") {idpere = System.Data.SqlTypes.SqlInt32.Null; } else { idpere = "'" + TXT_IDPere.Text + "'"; }
            if (TXT_Sexe.Text == "Femme")
            {
                sexe = "'0'";
            }
            else if(TXT_Sexe.Text == "Homme")
            {
                sexe = "'1'";
            }
            else
            {
                sexe = "NULL";
            }




            VerifyNull();
            int idliste = LB_Individu.SelectedIndex;
            int idtable = Program.SendMessage(LB_Individu.Handle, LB_GETITEMDATA, idliste, 0);
            rq.Modification("UPDATE individu SET nom_individu = " + TXT_Nom.Text + ", prenoms_individu= " + TXT_MultiPrenoms.Text + ", prenomusage_individu =" + TXT_Prenoms.Text +", sexe_individu=" + sexe +", ddn_individue= " + TXT_DateNaissance.Text + ", ldn_individu=" + idnaissance + ", ddd_individu=" + TXT_DateDeces.Text + ", num_rue_individu=" + TXT_NumRue.Text + ", nom_rue_individu=" + TXT_NomRue.Text + ", telephone_individu="+TXT_Telephone.Text+",id_pere= "+ idpere + ", id_mere="+idmere+", id_conjoint=" + idconjoint +", id_commune_individu=" +idcommune + ", ldd_individu=" + iddeces +" WHERE id_individu="+idtable);
            ClearAllLB();
            ClearAllTXT();
        }

        private void TXT_RechercheDeces_TextChanged(object sender, EventArgs e)
        {

            LV_CommuneDeces.View = View.Details;
            TXT_RechercheDeces.TextChanged += new EventHandler(TXT_RechercheDeces_TextChanged);
            ListViewItem foundItem =LV_CommuneDeces.FindItemWithText(TXT_RechercheDeces.Text, false, 0, true);
            if (foundItem != null)
            {
                LV_CommuneDeces.TopItem = foundItem;

            }
        }

        private void TXT_RechercheNaissance_TextChanged(object sender, EventArgs e)
        {

            LV_CommuneNaissance.View = View.Details;
            TXT_RechercheNaissance.TextChanged += new EventHandler(TXT_RechercheNaissance_TextChanged);
            ListViewItem foundItem = LV_CommuneNaissance.FindItemWithText(TXT_RechercheNaissance.Text, false, 0, true);
            if (foundItem != null)
            {
                LV_CommuneNaissance.TopItem = foundItem;

            }
        }

        private void TXT_RechercheCoordonnees_TextChanged(object sender, EventArgs e)
        {
            LV_CommuneCoordonnees.View = View.Details;
            TXT_RechercheCoordonnees.TextChanged += new EventHandler(TXT_RechercheCoordonnees_TextChanged);
            ListViewItem foundItem = LV_CommuneCoordonnees.FindItemWithText(TXT_RechercheCoordonnees.Text, false, 0, true);
            if (foundItem != null)
            {
                LV_CommuneCoordonnees.TopItem = foundItem;

            }
        }

        private void TXT_RecherchePere_TextChanged(object sender, EventArgs e)
        {
            LV_AjoutPere.View = View.Details;
            TXT_RecherchePere.TextChanged += new EventHandler(TXT_RecherchePere_TextChanged);
            ListViewItem foundItem = LV_AjoutPere.FindItemWithText(TXT_RecherchePere.Text, false, 0, true);
            if (foundItem != null)
            {
                LV_AjoutPere.TopItem = foundItem;

            }
        }

        private void TXT_RechercheMere_TextChanged(object sender, EventArgs e)
        {
            LV_AjoutMere.View = View.Details;
            TXT_RechercheMere.TextChanged += new EventHandler(TXT_RechercheMere_TextChanged);
            ListViewItem foundItem = LV_AjoutMere.FindItemWithText(TXT_RechercheMere.Text, false, 0, true);
            if (foundItem != null)
            {
                LV_AjoutMere.TopItem = foundItem;

            }
        }

        private void LB_Anniversaires_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectLB(LB_Anniversaires);
        }

        private void B_AfficherArbre_Click(object sender, EventArgs e)
        {

            FRM_Arbre arbre = new FRM_Arbre();

            this.Hide();
            arbre.ShowDialog();
        }
    }
}
