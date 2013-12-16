using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//****************************
using System.Data.Odbc;
using System.Data.SqlClient;
namespace Poslovanje.forme
{
    public partial class frmBrutoBilans : Form
    {
        #region "REGIJA otvaranje i pretraga uobicajeno na svakoj formi sa minimalnim promjenama ";
        public static string cIzabranaSifra;
        OdbcDataAdapter sqlDataAdapter;
        //Dim sqlDataAdapterNaziv As OdbcDataAdapter
        public static DataSet dataset;
        public static string cNaziv;
        public static string cKolona = "";

        //Public dw As DataView
        //Public dwTrenutniRed As DataRowView
        public static DataSet dss;
        public bool bLookUpForma;
        int SortColumn;
        bool bUkljuciDetalje;
        public static string cDbTabela;
        public bool Dragging;
        public int mousex;
        public int mousey;
        public static int cRecordCount;
        public bool bPrviPut = true;
        public static bool bIzSetupaLoco;
        static int iKIFKUF;

        string LGrid1 = Application.StartupPath + @"\frmBrutoBilansgrid1.txt";
        string LGrid2 = Application.StartupPath + @"\frmBrutoBilansgrid2.txt";

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }

        public static modus_rada modus;

        public frmBrutoBilans(bool bLookUpFormaOut, bool bIzSetupaOut)
        {
            bIzSetupaLoco = bIzSetupaOut;//moduli.variable.bIzSetupa;
            bLookUpForma = bLookUpFormaOut;
            InitializeComponent();
            txtSearch.Focus();

        }

        private void frmBrutoBilans_KeyDown(object sender, KeyEventArgs e)
        {

            if ((bLookUpForma == true) && (e.KeyCode == Keys.Enter))
            {
                if (ActiveControl.Name == "grid1")
                {
                    //btnOdaberi_Click(Me, e)
                    //MessageBox.Show("ohooohoo");
                }

            }

            if ((ActiveControl.Name == "txtSearch") && (e.KeyCode == Keys.Enter))
            {
                grid1.Focus();
            }

            if (e.KeyCode == Keys.Escape)
            {

                Dispose();
            }

            if (e.KeyCode == Keys.Insert) { if (modus == modus_rada.pregled) { } }
            if (e.KeyCode == Keys.End) { if (modus == modus_rada.pregled) { } }
            if (e.KeyCode == Keys.Delete) { if (modus == modus_rada.pregled) { } }
            if (e.KeyCode == Keys.Home) { if (modus == modus_rada.pregled) { } }
            if (modus != modus_rada.pregled)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        ProcessTabKey(true);
                        break;
                    case Keys.Up:
                        ProcessTabKey(false);
                        break;
                    case Keys.Down:
                        ProcessTabKey(true);
                        break;
                }
            }
        }

        private void frmBrutoBilans_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    // perform necessary action OVIM GASIM BEEP NA TEXT KONTROLAMA
                    e.Handled = true;
                    break;
            }
        }

        private void frmBrutoBilans_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void frmBrutoBilans_Load(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead, "");  //čitam posljednju poziciju
            cDbTabela = "gk_brutobilans";
            SortColumn = 1;
            //cKolona = grid1.Columns[SortColumn].Name;
            cKolona = grid1.Columns[SortColumn].DataField.ToString();
            bUkljuciDetalje = false;
            grid1.SaveLayout(LGrid1);
            grid2.SaveLayout(LGrid2);

            modus = modus_rada.pregled;

            // PrikaziSliku(pnlSlika,this);
            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakonektuj(); }
            else { moduli.kon.konektuj(); }

            txtDatOd.Value = DateTime.Today.AddDays(-15);

            optKIF.Checked = true;
            iKIFKUF = 0;

            resize();



            //***********

        }

        private void frmBrutoBilans_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cText;
                //cKolona = "";

                //dss = New DataSet 'ili dss.Dispose()

                cText = txtSearch.Text.Replace("'", "");
                if (cText == "")
                {
                    cNaziv = "";
                    //dss = pretraga(cNaziv)

                }
                else
                {
                    cNaziv = cText.ToString();
                    cKolona = grid1.Columns[SortColumn].DataField.ToString();
                    switch (cKolona)
                    {
                        case "Nazivi":
                            cKolona = "pe_programi.Naziv";
                            break;
                        case "NazivP":
                            cKolona = "partneri.Naziv";
                            break;
                    }


                }
                popunigrid(cKolona, cNaziv, "order by BrutoBilans. ASC");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Greška u odabiru karaktera !");
                txtSearch.Text = "";
            }


        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) grid1.Focus();
            if (e.KeyCode == Keys.Enter) grid1.Focus();

        }

        private void popunigrid(string cKolona, string cNaziv, string cOrder)
        {
            try
            {

                string cDatum1=moduli.funkcije.SQLDate(txtDatOd.Value);
                string cDatum2=moduli.funkcije.SQLDate(txtDatDo.Value);
                DateTime cD=txtDatOd.Value;
                string  cDat=moduli.funkcije.SQLDate(new DateTime (cD.Year ,1,1));

                string comSQL = "delete from " + cDbTabela + "";
                moduli.funkcije.SQLExecute(false, comSQL);

                comSQL = "INSERT INTO " + cDbTabela + "  " +
                "(Konto, DugujePS, PotrazujePS, DugujeTP, PotrazujeTP, DugujeUK, PotrazujeUK, SaldoDug, SaldoPot) " +
                "SELECT gk_promet.Konto, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Datum=" + cDat + ",gk_promet.Duguje,0) AS Expr1, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Datum=" + cDat + ",gk_promet.Potrazuje,0) AS Expr2, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Datum>=" + cDatum1 + " and gk_promet.Datum<=" + cDatum2 + "" +
                "and gk_promet.Datum<>" + cDat + ",gk_promet.Duguje,0) AS Expr3, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Datum>=" + cDatum1 + " and gk_promet.Datum<=" + cDatum2 + "" +
                "and gk_promet.Datum<>" + cDat + ",gk_promet.Potrazuje,0) AS Expr4, " +
                "gk_promet.Duguje, gk_promet.Potrazuje, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Duguje>=gk_promet.Potrazuje,gk_promet.Duguje-gk_promet.Potrazuje,0) AS Expr5, " +
                "" + moduli.funkcije.SQLuslov() + "" +
                "(gk_promet.Potrazuje>gk_promet.Duguje,gk_promet.Potrazuje-gk_promet.Duguje,0) AS Expr6 " +
                "FROM kontni_plan RIGHT JOIN gk_promet ON kontni_plan.Konto = gk_promet.Konto " +
                "WHERE gk_promet.Datum>=" + cDatum1 + " and gk_promet.DATUM<=" + cDatum2 + ";";

                moduli.funkcije.SQLExecute(false, comSQL);

                DataSet dss = new DataSet(); //ili dss.Dispose()
                if (cNaziv == "")
                {
                    dss = pretraga(cNaziv, txtDatOd.Value, txtDatDo.Value, cOrder);
                }
                else
                {
                    dss = pretraga(" and " + cKolona + " like '%" + cNaziv + "%'", txtDatOd.Value, txtDatDo.Value, cOrder);
                }


                BindingSource masterBindingSource = new BindingSource();

                BindingSource detailsBindingSource = new BindingSource();


                DataRelation relation = new DataRelation("relacija",
                 dss.Tables[cDbTabela].Columns["rb"], dss.Tables["BB"].Columns["rb"], false);
                //GREŠKA _ :This constraint cannot be enabled as not all values have corresponding parent values 
                //zato sam na kraj relacije stavio false

                dss.Relations.Add(relation);

                masterBindingSource.DataSource = dss;
                masterBindingSource.DataMember = cDbTabela;
                detailsBindingSource.DataSource = masterBindingSource;
                detailsBindingSource.DataMember = "relacija";


                cRecordCount = dss.Tables[cDbTabela].Rows.Count;



                grid1.DataSource = masterBindingSource;

                grid2.DataSource = detailsBindingSource;

                grid1.LoadLayout(LGrid1);
                grid2.LoadLayout(LGrid2);



                //grid1.Columns["DatumKnj"].NumberFormat = "Short Date";
                //grid1.Columns["DatumFak"].NumberFormat = "Short Date";
                //grid1.Columns["IznosOsn"].NumberFormat = "Fixed";
                //grid1.Columns["IznosPDVa"].NumberFormat = "Fixed";
                //grid1.Columns["IznosFakPDV"].NumberFormat = "Fixed";
                //grid1.Columns["OdbijenPDV"].NumberFormat = "Fixed";
                //grid1.Columns["NeodbijenPDV"].NumberFormat = "Fixed";

                //int i;
                //double suma1 = 0; double suma2 = 0; double suma3 = 0;
                //for (i = 0; i <= cRecordCount - 1; i++)
                //{
                //    suma1 = suma1 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosOsn"].ToString());
                //    suma2 = suma2 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosPDVa"].ToString());
                //    suma3 = suma3 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosFakPDV"].ToString());
                //}

                //grid1.Columns["IznosOsn"].FooterText = suma1.ToString();
                //grid1.Columns["IznosPDVa"].FooterText = suma2.ToString();
                //grid1.Columns["IznosFakPDV"].FooterText = suma3.ToString();



                //grid2.Columns["IznosOsn"].NumberFormat = "Fixed";
                //grid2.Columns["IznosPDVa"].NumberFormat = "Fixed";
                //grid2.Columns["IznosFakPDV"].NumberFormat = "Fixed";

                //for (i = 0; i <= cRecordCount - 1; i++)
                //{
                //    suma1 = suma1 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosOsn"].ToString());
                //    suma2 = suma2 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosPDVa"].ToString());
                //    suma3 = suma3 + double.Parse(dss.Tables[cDbTabela].Rows[i]["IznosFakPDV"].ToString());
                //}
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), "Parametri nisu pravilno uneseni !");
            }
        }

        public static DataSet pretraga(string uslov, DateTime datum1, DateTime datum2, string cOrder)
        {

            try
            {


                string cSQL = "SELECT  " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Access ? "First(BrutoBilans.rb) as rb,":"BrutoBilans.rb,") + "" +
                "Left(BrutoBilans.Konto,3) As Konto, Sum(BrutoBilans.DugujePS) AS DugujePS," +
                "Sum(BrutoBilans.PotrazujePS) AS PotrazujePS, " +
                "Sum(BrutoBilans.DugujeTP) AS DugujeTP, Sum(BrutoBilans.PotrazujeTP) AS PotrazujeTP, Sum(BrutoBilans.DugujeUK) AS DugujeUK, " +
                "Sum(BrutoBilans.PotrazujeUK) AS PotrazujeUK, " +
                ""  + moduli.funkcije.SQLuslov()  + "(Sum(BrutoBilans.SaldoDug)>= Sum(BrutoBilans.SaldoPot),Sum(BrutoBilans.SaldoDug)- Sum(BrutoBilans.SaldoPot),0) AS SaldoD, " +
                ""  + moduli.funkcije.SQLuslov()  + "(Sum(BrutoBilans.SaldoPot) > Sum(BrutoBilans.SaldoDug),Sum(BrutoBilans.SaldoPot) - Sum(BrutoBilans.SaldoDug),0) AS SaldoP " +
                "From " + cDbTabela + " BrutoBilans GROUP BY Left(BrutoBilans.Konto,3) " + cOrder + ";";

                OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                dataset = new DataSet();
                sqlDataAdapter.Fill(dataset, cDbTabela);

                string cChild = "SELECT  " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Access ? "First(BB.rb) as rb," : "BB.rb,") + "" +
                "BB.Konto, Sum(BB.DugujePS) AS DugujePS, Sum(BB.PotrazujePS) AS PotrazujePS, " +
                "Sum(BB.DugujeTP) AS DugujeTP, Sum(BB.PotrazujeTP) AS PotrazujeTP, Sum(BB.DugujeUK) AS DugujeUK, " +
                "Sum(BB.PotrazujeUK) AS PotrazujeUK, " +
                "" + moduli.funkcije.SQLuslov() + "(Sum(BB.SaldoDug) >= Sum(BB.SaldoPot),Sum(BB.SaldoDug)- Sum(BB.SaldoPot),0) AS SaldoD, " +
                "" + moduli.funkcije.SQLuslov() + "(Sum(BB.SaldoPot) > Sum(BB.SaldoDug),Sum(BB.SaldoPot)-Sum(BB.SaldoDug),0) AS SaldoP " +
                "From " + cDbTabela + " BB GROUP BY BB.Konto " + cOrder + ";";

                OdbcDataAdapter childAd = new OdbcDataAdapter(cChild, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                childAd.Fill(dataset, "BB");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Parametri nisu pravilno uneseni !");

            }

            return dataset;
        }
        
        private void resize()
        {
            grid1.Width = Width - 8;
            grid1.Height = (Height - 145);
            pnlSlika.Width = Width - 8;
            panel1.Width = Width - 8;

        }

        private void grid1_SelectionChanged(object sender, EventArgs e)
        {


            if (bUkljuciDetalje == false) return;
            //ukljucidetalje(bUkljuciDetalje);
            //pronadjired(sender);
        }

        //private void grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    SortColumn = e.ColumnIndex;
        //    txtSearch.Text = "";
        //    txtSearch.Focus();
        //}

        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show();
            Point pt = grid1.PointToScreen(btnPrint.Location );
            contextMenuStrip1.Show(pt);


        }



        private void btnOdustani_Click(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
            //obrisitmptelefone();
            Dispose();
        }



        #endregion





        #region "############################################################posebno na svakoj formi"

        private void txtDatOd_ValueChanged(object sender, EventArgs e)
        {
            popunigrid("", "", "");
        }

        private void txtDatDo_ValueChanged(object sender, EventArgs e)
        {
            popunigrid("", "", "");
        }


        private void grid1_HeadClick(object sender, C1.Win.C1TrueDBGrid.ColEventArgs e)
        {
            SortColumn = e.ColIndex;
            txtSearch.Text = "";
            txtSearch.Focus();
        }



        #endregion

        private void štampaBrutoBilansaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string cSQL1 = ""; string cDatumNaslov ="Zaključni list za period " + txtDatOd.Text + " - " + txtDatDo.Text  ;

            cSQL1 = "SELECT '" + moduli.variable.cSifraFirme + "', BrutoBilans.Konto, BrutoBilans.Konto as KontoN," +
            "Sum(BrutoBilans.DugujePS) AS DugujePS, Sum(BrutoBilans.PotrazujePS) AS PotrazujePS, " +
            "Sum(BrutoBilans.DugujeTP) AS DugujeTP, Sum(BrutoBilans.PotrazujeTP) AS PotrazujeTP, Sum(BrutoBilans.DugujeUK) AS DugujeUK, " +
            "Sum(BrutoBilans.PotrazujeUK) AS PotrazujeUK," +
            "" + moduli.funkcije.SQLUslov() + "" +
            "(Sum(BrutoBilans.SaldoDug) >= Sum(BrutoBilans.SaldoPot),Sum(BrutoBilans.SaldoDug)- Sum(BrutoBilans.SaldoPot),0) AS SaldoD, " +
            "" + moduli.funkcije.SQLUslov() + "" +
            "(Sum(BrutoBilans.SaldoPot) > Sum(BrutoBilans.SaldoDug),Sum(BrutoBilans.SaldoPot)-Sum(BrutoBilans.SaldoDug),0) AS SaldoP " +
            "From " + cDbTabela  + " BrutoBilans GROUP BY BrutoBilans.Konto;";


            frmStampaFirme a = new frmStampaFirme("Txt", "Brutobilans", cSQL1, cDbTabela, cDatumNaslov, "", "", "", false);
            a.Show();

        }

        private void štampaBrutoBilansaNazivKontaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cSQL1 = ""; string cDatumNaslov = "Zaključni list za period " + txtDatOd.Text + " - " + txtDatDo.Text;

            cSQL1 = "SELECT '" + moduli.variable.cSifraFirme + "', BrutoBilans.Konto,kontni_plan.KontoN," +
            "Sum(BrutoBilans.DugujePS) AS DugujePS, Sum(BrutoBilans.PotrazujePS) AS PotrazujePS, " +
            "Sum(BrutoBilans.DugujeTP) AS DugujeTP, Sum(BrutoBilans.PotrazujeTP) AS PotrazujeTP, Sum(BrutoBilans.DugujeUK) AS DugujeUK, " +
            "Sum(BrutoBilans.PotrazujeUK) AS PotrazujeUK," +
            "" + moduli.funkcije.SQLUslov() + "" +
            "(Sum(BrutoBilans.SaldoDug) >= Sum(BrutoBilans.SaldoPot),Sum(BrutoBilans.SaldoDug)- Sum(BrutoBilans.SaldoPot),0) AS SaldoD, " +
            "" + moduli.funkcije.SQLUslov() + "" +
            "(Sum(BrutoBilans.SaldoPot) > Sum(BrutoBilans.SaldoDug),Sum(BrutoBilans.SaldoPot)-Sum(BrutoBilans.SaldoDug),0) AS SaldoP " +
            "From " + cDbTabela + " BrutoBilans LEFT JOIN kontni_plan ON BrutoBilans.Konto = kontni_plan.Konto " +
            "GROUP BY BrutoBilans.Konto, kontni_plan.KontoN";

            frmStampaFirme a = new frmStampaFirme("Txt", "Brutobilansnaziv", cSQL1, cDbTabela, cDatumNaslov, "", "", "", false);
            a.Show();
        }











    }
}
