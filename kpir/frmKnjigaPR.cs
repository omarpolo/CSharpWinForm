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
    public partial class frmKnjigaPR : Form
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

        static string cSQLStampa = "";

        string[] cPolja = new string[2]; 

        string LGrid1 = Application.StartupPath + @"\frmKnjigaPRgrid1.txt";
        string LGrid2 = Application.StartupPath + @"\frmKnjigaPRgrid2.txt";

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }

        public static modus_rada modus;

        public frmKnjigaPR(bool bLookUpFormaOut, bool bIzSetupaOut)
        {
            bIzSetupaLoco = bIzSetupaOut;//moduli.variable.bIzSetupa;
            bLookUpForma = bLookUpFormaOut;
            InitializeComponent();
            txtSearch.Focus();

        }

        private void frmKnjigaPR_KeyDown(object sender, KeyEventArgs e)
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

        private void frmKnjigaPR_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    // perform necessary action OVIM GASIM BEEP NA TEXT KONTROLAMA
                    e.Handled = true;
                    break;
            }
        }

        private void frmKnjigaPR_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void frmKnjigaPR_Load(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead, "");  //čitam posljednju poziciju
            cDbTabela = "gk_promet";
            SortColumn = 1;
            //cKolona = grid1.Columns[SortColumn].Name;
            cKolona = grid1.Columns[SortColumn].DataField.ToString();
            bUkljuciDetalje = false;
            grid1.SaveLayout(LGrid1);


            modus = modus_rada.pregled;

            // PrikaziSliku(pnlSlika,this);
            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakonektuj(); }
            else { moduli.kon.konektuj(); }
            otvorikonta();
            txtDatOd.Value = DateTime.Today.AddDays(-15);

            optKIF.Checked = true;
            iKIFKUF = 0;
            
            resize();



            //***********

        }

        private void frmKnjigaPR_FormClosing(object sender, FormClosingEventArgs e)
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

                    if (grid1.Columns.IndexOf(grid1.Columns[SortColumn]) > 3) {
                        cNaziv = "";
                    }



                }
                popunigrid(cKolona, cNaziv, "");

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

                
                string cP = cPolja[0];
                string cU = cPolja[1];


                DataSet dss = new DataSet(); //ili dss.Dispose()
                if (cNaziv == "")
                {
                    dss = pretraga(cNaziv , txtDatOd.Value, txtDatDo.Value, cOrder, cP,cU);
                }
                else
                {
                    dss = pretraga(" and " + cKolona + " like '%" + cNaziv + "%'" , txtDatOd.Value, txtDatDo.Value, cOrder,cP,cU);
                }


                //--------- kreiranje datatabele

                DataTable tbCh = new DataTable();

                //--------- kreiranje kolona


                for (int i = 0; i <= dss.Tables[0].Columns.Count - 1; i++)
                {
                    switch (dss.Tables[0].Columns[i].ColumnName.ToString())
                    {
                        case "Datum":
                            tbCh.Columns.Add(dss.Tables[0].Columns[i].ColumnName.ToString(), typeof(DateTime));
                            break;
                        case "Konto":
                            tbCh.Columns.Add(dss.Tables[0].Columns[i].ColumnName.ToString(), typeof(String));
                            break;
                        case "VrstaDok":
                            tbCh.Columns.Add(dss.Tables[0].Columns[i].ColumnName.ToString(), typeof(int));
                            break;
                        case "Naziv":
                            tbCh.Columns.Add(dss.Tables[0].Columns[i].ColumnName.ToString(), typeof(string));
                            break ;
                        default :
                            tbCh.Columns.Add(dss.Tables[0].Columns[i].ColumnName.ToString(), typeof(double));
                            break ;
                    }
                    
                }

                //---------- kreriranje redova
                DataRow dr = null;
                for (int j = 0; j <= dss.Tables[0].Rows.Count - 1; j++)
                {
                    dr = tbCh.NewRow();
                    for (int ij = 0; ij <= dss.Tables[0].Columns.Count - 1; ij++)
                    {
                        dr[ij] = dss.Tables[0].Rows[j][ij].ToString();//punim kolone u svakom redu
                    }
                    tbCh.Rows.Add(dr);
                
                }


                dss.Tables.Add(tbCh);

                grid1.DataSource = dss.Tables[1];

                int cK = grid1.Columns.Count; double cZbir = 0; int a;
                int cBroj = 0; string cNas = "";
                for (int s = 0; s <= cK-1; s++)
                {
                    if (s!=cK-1) grid1.Splits[0].DisplayColumns[s].FooterDivider = false;
                    grid1.Splits[0].DisplayColumns[s].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Near;
                    if (s > 3)
                    {
                        grid1.Columns[s].NumberFormat = "Fixed";
                        grid1.Splits[0].DisplayColumns[s].Width = 70;
                        grid1.Splits[0].ColumnCaptionHeight = 40;
                        grid1.Splits[0].DisplayColumns[s].HeadingStyle.HorizontalAlignment =  C1.Win.C1TrueDBGrid.AlignHorzEnum.Center;
                        grid1.Splits[0].DisplayColumns[s].Style.HorizontalAlignment = C1.Win.C1TrueDBGrid.AlignHorzEnum.Far;

                        // --- caption kolona start ----
                        string cIME =grid1.Columns[s].Caption.ToString();//dsxl.Tables[0].Columns[cc2].ColumnName.ToString();
                        if (cIME.Substring(0, cIME.Length - 1) == "UKUPNO")
                        {

                        }
                        else
                        {
                            if (cIME.Substring(0, 1) == "P") //vrsta 0
                            {
                                cBroj = int.Parse(cIME.Substring(1, cIME.Length - 1)) + 1;
                                cNas = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "gk_podesenja", "Opis", "dp=0 and Kol=" + cBroj + "");
                            }
                            else
                            {
                                cBroj = int.Parse(cIME.Substring(1, cIME.Length - 1)) + 1;
                                cNas = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                    "gk_podesenja", "Opis", "dp=1 and Kol=" + cBroj + "");
                            }
                            grid1.Columns[s].Caption = cNas;
                        }
                        // --- caption kolona kraj ----

                       

                    }




                    cZbir =0;
                    for (int sr = 0; sr <= grid1.RowCount - 1; sr++)
                    {
                        if (s == 7) {
                            a = 1;
                        }
                        if (s > 3)
                        {
                            cZbir = cZbir + double.Parse(grid1[sr, s].ToString());
                        }

                        
                    }

                    grid1.Columns[s].FooterText = string.Format("{0:0,0.##}", cZbir.ToString());
                    

                }

                //grid1.LoadLayout(LGrid1);

            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), "Parametri nisu pravilno uneseni !");
            }
        }

        public static DataSet pretraga(string uslov, DateTime datum1, DateTime datum2, string cOrder,string polja,string cuslov)
        {

            try
            {

                string cSQL = "SELECT gk_promet.Datum, gk_promet.Konto, gk_promet.VrstaDok,gk_vrstedok.Naziv " + polja + " " +
                " From " + cDbTabela  + " gk_promet  " +
                " LEFT JOIN gk_vrstedok ON gk_promet.VrstaDok = gk_vrstedok.sifra " +
                " Where gk_promet.Datum Between " + moduli.funkcije.SQLDate(datum1) + " And " +
                ""+ moduli.funkcije.SQLDate(datum2)  + "  AND " + cuslov + " " + uslov  + " Order By gk_promet.Datum ";

                cSQLStampa = "SELECT gk_promet.Datum, gk_vrstedok.Naziv " + polja + " " +
                " From " + cDbTabela + " gk_promet  " +
                " LEFT JOIN gk_vrstedok ON gk_promet.VrstaDok = gk_vrstedok.sifra " +
                " Where gk_promet.Datum Between " + moduli.funkcije.SQLDate(datum1) + " And " +
                "" + moduli.funkcije.SQLDate(datum2) + "  AND " + cuslov + " " + uslov + " Order By gk_promet.Datum ";

                OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                dataset = new DataSet();
                sqlDataAdapter.Fill(dataset, cDbTabela);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Parametri nisu pravilno uneseni !");

            }

            return dataset;
        }

        private string [] otvorikonta()  {

            
            string cUslovSQL = null; string sUKUPNOsql = "0 ";
            string cSQL = null; string sOsimSQL = null; string sWhere = null;
            string []a = new string [20];
            try
            {
                cSQL = "select * from gk_podesenja where vrsta =1 and dp=0 order by Kol ASC";
                OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet dsK = new DataSet();
                sqlDataAdapter.Fill(dsK, "gk_podesenja");

                if (dsK.Tables[0].Rows.Count != 0)
                {
                    int cBrojRedova = dsK.Tables[0].Rows.Count;
                    for (int i = 0; i <= cBrojRedova-1; i++)
                    {
                        //---- konta duguje
                        string cP = dsK.Tables[0].Rows[i]["Konto"].ToString();
                        a = cP.Split(new Char[] { ';' });

                        if (a[0] != "")
                        {
                            cUslovSQL = "IIF( ";
                            cUslovSQL = cUslovSQL + " Left(gk_promet.Konto," + a[0].Length + ")='" + a[0].ToString() + "'";
                        }

                        for (int j = 1; j <= 20; j++)
                        {
                            try
                            {
                                if (a[j] != "" )
                                {
                                    //nKonta(i) = nKonta(i) & " OR Left(gk_promet.Konto," & Len(a(j)) & ")='" & a(j) & "'"  '& a(j)
                                    cUslovSQL = cUslovSQL + " OR Left(gk_promet.Konto," + a[j].Length + ")='" + a[j].ToString() + "'";
                                }
                                else { break; }
                            }
                            catch (Exception e) {break;}

                        }
                        //--- osim konta duguje
                        cP = null; a = null;
                        cP = dsK.Tables[0].Rows[i]["Kontoosim"].ToString();
                        a = cP.Split(new Char[] { ';' });

                        if (a[0] != "" )
                        {
                            if (a[0] != "-")
                            cUslovSQL = cUslovSQL + " AND Left(gk_promet.Konto," + a[0].Length + ")<>'" + a[0].ToString() + "'";
                            for (int j = 1; j <= 20; j++)
                            {
                                try
                                {
                                    if (a[j] != "")
                                    {
                                        if (a[j] !="-")
                                        cUslovSQL = cUslovSQL + " AND Left(gk_promet.Konto," + a[j].Length + ")<>'" + a[j].ToString() + "'";
                                    }
                                    else { break; }
                                }
                                catch (Exception e) { break; }
                            }



                            sUKUPNOsql = sUKUPNOsql + " + " + cUslovSQL + " ,[gk_promet]![Potrazuje],0)";
                            cUslovSQL = cUslovSQL + " ,[gk_promet]![Potrazuje],0)" + " AS P" + i;
                            sOsimSQL = sOsimSQL + "," + cUslovSQL;
                        }
                    }
                    sOsimSQL = sOsimSQL + ", " + sUKUPNOsql + " AS UKUPNO1";
                    sWhere = "(" + sUKUPNOsql + ") <> 0 ";
                    sUKUPNOsql = "0 ";

                }
                else
                { MessageBox.Show("Provjerite podešenja za glavnu knjigu u bazi! Tabela gk_podesenja je prazna."); }

            }
            catch (Exception e) { MessageBox.Show(e.ToString ()); }
            //--------------------------------------- POTRAZNI DIO ---------------------------------------------------------//
            a = null;
            try
            {
                cSQL = "select * from gk_podesenja where vrsta =1 and dp=1 order by Kol  ASC";
                OdbcDataAdapter sqlDataAdapter1 = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet dsK1 = new DataSet();
                sqlDataAdapter1.Fill(dsK1, "gk_podesenja");


                if (dsK1.Tables[0].Rows.Count != 0)
                {
                    int cBrojRedova = dsK1.Tables[0].Rows.Count;
                    for (int i = 0; i <= cBrojRedova-1; i++)
                    {
                        //---- konta potrazuje
                        string cP = dsK1.Tables[0].Rows[i]["Konto"].ToString();
                        a = cP.Split(new Char[] { ';' });

                        if (a[0] != "")
                        {
                            cUslovSQL = "IIF( ";
                            cUslovSQL = cUslovSQL + " Left(gk_promet.Konto," + a[0].Length + ")='" + a[0].ToString() + "'";
                        }

                        for (int j = 1; j <= 20; j++)
                        {
                            try
                            {
                                if (a[j] != "")
                                {
                                    //nKonta(i) = nKonta(i) & " OR Left(gk_promet.Konto," & Len(a(j)) & ")='" & a(j) & "'"  '& a(j)
                                    cUslovSQL = cUslovSQL + " OR Left(gk_promet.Konto," + a[j].Length + ")='" + a[j].ToString() + "'";
                                }
                                else { break; }
                            }
                            catch (Exception e) { break; }
                        }
                        //--- osim konta potrazuje
                        cP = dsK1.Tables[0].Rows[i]["Kontoosim"].ToString();
                        a = cP.Split(new Char[] { ';' });

                        if (a[0] != "")
                        {
                            if (a[0] != "-")
                            cUslovSQL = cUslovSQL + " AND Left(gk_promet.Konto," + a[0].Length + ")<>'" + a[0].ToString() + "'";
                            for (int j = 1; j <= 20; j++)
                            {
                                try
                                {
                                    if (a[j] != "")
                                    {
                                        if (a[j] != "-")
                                            cUslovSQL = cUslovSQL + " AND Left(gk_promet.Konto," + a[j].Length + ")<>'" + a[j].ToString() + "'";
                                    }
                                    else { break; }
                                }
                                catch (Exception e) { break; }
                            }

                        }
                        sUKUPNOsql = sUKUPNOsql + " + " + cUslovSQL + " ,[gk_promet]![Duguje],0)";
                        cUslovSQL = cUslovSQL + " ,[gk_promet]![Duguje],0)" + " AS D" + i;
                        sOsimSQL = sOsimSQL + "," + cUslovSQL;
                    }
                    sOsimSQL = sOsimSQL + ", " + sUKUPNOsql + " AS UKUPNO2";
                    sWhere = "(" + sUKUPNOsql + ") <> 0 ";
                }
                else
                { MessageBox.Show("Provjerite podešenja za glavnu knjigu u bazi! Tabela gk_podesenja je prazna."); }
              }
            catch (Exception e) { MessageBox.Show(e.ToString ()); }

            cPolja[0] = sOsimSQL;
            cPolja[1] = sWhere;
            return cPolja;
        
        
        }

        private void kreirajpolja()
        { 
        
        
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show();
            Point pt = grid1.PointToScreen(btnPrint.Location);
            contextMenuStrip1.Show(pt);


        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
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

        private int brojKolona(int UI)
        {
            int cDP=0;
            string cSQL = "select * from gk_podesenja where dp=" + UI  + "";
            OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds, "gk_podesenja");
            cDP = ds.Tables[0].Rows.Count;
            return cDP;
            
        }

        private void PrintToExcell(string cSQL, string sNaslov) 
        {


            int nColCount = grid1.Columns.Count;
            if (nColCount == 0) return;

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlBook = null;
            Microsoft.Office.Interop.Excel.Worksheet xlSheet = null;

            xlBook = xlApp.Workbooks.Add();
            xlSheet = xlBook.Worksheets.Add();

            Microsoft.Office.Interop.Excel.Range xlRange = xlSheet.UsedRange;

            xlBook.ActiveSheet.PageSetup.PrintTitleRows = "";
            xlBook.ActiveSheet.PageSetup.PrintTitleColumns = "";
            xlBook.ActiveSheet.PageSetup.PrintArea = "";

            xlBook.ActiveSheet.PageSetup.LeftHeader = "";
            xlBook.ActiveSheet.PageSetup.CenterHeader = "";
            xlBook.ActiveSheet.PageSetup.RightHeader = "";
            xlBook.ActiveSheet.PageSetup.LeftFooter = "";
            xlBook.ActiveSheet.PageSetup.CenterFooter = "";
            xlBook.ActiveSheet.PageSetup.RightFooter = "";
            xlBook.ActiveSheet.PageSetup.LeftMargin = xlApp.InchesToPoints(0.393700787401575);
            xlBook.ActiveSheet.PageSetup.RightMargin = xlApp.InchesToPoints(0.393700787401575);
            xlBook.ActiveSheet.PageSetup.TopMargin = xlApp.InchesToPoints(0.78740157480315);
            xlBook.ActiveSheet.PageSetup.BottomMargin = xlApp.InchesToPoints(0.78740157480315);
            xlBook.ActiveSheet.PageSetup.HeaderMargin = xlApp.InchesToPoints(0.511811023622047);
            xlBook.ActiveSheet.PageSetup.FooterMargin = xlApp.InchesToPoints(0.511811023622047);
            xlBook.ActiveSheet.PageSetup.PrintHeadings = false;
            xlBook.ActiveSheet.PageSetup.PrintGridlines = false;
            xlBook.ActiveSheet.PageSetup.PrintComments = Microsoft.Office.Interop.Excel.XlPrintLocation.xlPrintSheetEnd;
            xlBook.ActiveSheet.PageSetup.PrintQuality = 100;
            xlBook.ActiveSheet.PageSetup.CenterHorizontally = false;
            xlBook.ActiveSheet.PageSetup.CenterVertically = false;
            xlBook.ActiveSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            xlBook.ActiveSheet.PageSetup.Draft = false;
            xlBook.ActiveSheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
            xlBook.ActiveSheet.PageSetup.FirstPageNumber = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            xlBook.ActiveSheet.PageSetup.Order = Microsoft.Office.Interop.Excel.XlOrder.xlDownThenOver;
            xlBook.ActiveSheet.PageSetup.BlackAndWhite = false;
            //.PrintErrors = xlPrintErrorsDisplayed
            xlBook.ActiveSheet.PageSetup.Zoom = 100;

            //xlApp.Visible = true;

            //moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ?
            //                     moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, cDbTabela,
            //                     "sifra", "sifra=" + txtSifra.Text + "");
            
            xlSheet.Cells[1, 1] = moduli.funkcije.RetFieldValue("string",
                                  moduli.variable.mKonekcijaOsnovna ,"firme", "naziv", "sifra='" + moduli.variable.cSifraFirme  + "'");  //aFirma("Naziv_Firme").Key;

            xlSheet.Cells[2, 1] = moduli.variable.cAdresa; //aFirma("Adresa").Key;
	        xlSheet.Cells[3, 1] = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna ,
                                      "opstine", "naziv", "sifra='" + moduli.variable.cOpstina + "'");
	        xlSheet.Cells[4, 1] = "Datum: " + String.Format( "Short date",DateTime.Now);
	        xlSheet.Cells[6, 1] = sNaslov;
	        xlSheet.Range[xlSheet.Cells[6, 1], xlSheet.Cells[6, 1]].Font.Bold = true;
            xlSheet.Range[xlSheet.Cells[6, 1], xlSheet.Cells[6, 1]].Font.Size = 14;

            xlSheet.Cells[7, 1] = "RB";
            xlSheet.Range["A7:A8"].Select();
            xlSheet.Range["A7:A8"].Merge();

            xlSheet.Cells[7, 2] = "Datum";
            xlSheet.Range["B7:B8"].Select();
            xlSheet.Range["B7:B8"].Merge();

            xlSheet.Cells[7, 3] = "Opis";
            xlSheet.Range["C7:C8"].Select();
            xlSheet.Range["C7:C8"].Merge();

            xlSheet.Range["A8:C8"].Select();
            xlSheet.Cells[7, 4] = "Naplaćeni Prihodi";
            xlSheet.Cells[7, 3 + brojKolona(0) + 2] = "Troškovi poslovanja - Rashodi";


            //ODAVDE SE POCINJE DEFINISATI BORDER

          
            xlSheet.Range["A7:C8"].Select();
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown].LineStyle =Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
 
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            xlBook.Application.Selection.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;


            xlSheet.Range["A7:N8"].Select();
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;

  
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
 
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
 
            xlSheet.Range["D7:G7"].Select();
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;


            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;


            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlSheet.Range["H7:N7"].Select();
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;

 
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;
            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlBook.Application.Selection.Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = Microsoft.Office.Interop.Excel.Constants.xlNone;
            xlSheet.Range["A7:N8"].Select();


            xlBook.Application.Selection.Font.Name = "Arial";
            xlBook.Application.Selection.Font.Size = 8;
            xlBook.Application.Selection.Font.Strikethrough = false;
            xlBook.Application.Selection.Font.Superscript = false;
            xlBook.Application.Selection.Font.Subscript = false;
            xlBook.Application.Selection.Font.OutlineFont = false;
            xlBook.Application.Selection.Font.Shadow = false;
            xlBook.Application.Selection.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleNone;
            xlBook.Application.Selection.Font.ColorIndex = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

            xlSheet.Range["D8:N8"].Select();


            xlBook.Application.Selection.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlGeneral;
            xlBook.Application.Selection.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlBottom;
            xlBook.Application.Selection.WrapText = true;
            xlBook.Application.Selection.Orientation = 0;
            xlBook.Application.Selection.AddIndent = false;
            xlBook.Application.Selection.IndentLevel = 0;
            xlBook.Application.Selection.ShrinkToFit = false;
            xlBook.Application.Selection.ReadingOrder = Microsoft.Office.Interop.Excel.Constants.xlContext;
            xlBook.Application.Selection.MergeCells = false;

            xlBook.Application.Selection.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            xlBook.Application.Selection.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlBottom;
            xlBook.Application.Selection.WrapText = true;
            xlBook.Application.Selection.Orientation = 0;
            xlBook.Application.Selection.AddIndent = false;
            xlBook.Application.Selection.IndentLevel = 0;
            xlBook.Application.Selection.ShrinkToFit = false;
            xlBook.Application.Selection.ReadingOrder = Microsoft.Office.Interop.Excel.Constants.xlContext;
            xlBook.Application.Selection.MergeCells = false;

            xlBook.Application.Columns["C:C"].ColumnWidth = 14.57;
            xlBook.Application.Columns["E:E"].ColumnWidth = 10.71;
            xlBook.Application.Columns["H:H"].ColumnWidth = 9.57;
            xlBook.Application.Columns["L:L"].ColumnWidth = 9.86;
            xlBook.Application.Columns["F:F"].ColumnWidth = 9.43;
            xlBook.Application.Columns["F:F"].ColumnWidth = 10;
            xlBook.Application.Rows["8:8"].RowHeight = 23.25;
            xlBook.Application.Rows["8:8"].RowHeight = 25.5;
            xlSheet.Range["E7"].Select();
            //OVDJE JE KRAJ BORDERA


            int n = 10; int dPrenos = 37; int dDonos = 9;
            bool bNovaStrana = false; bool isZag = false;

            OdbcDataAdapter xlAD = new OdbcDataAdapter(cSQL , bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            DataSet dsxl = new DataSet();
            xlAD.Fill(dsxl, cDbTabela);

            nColCount = dsxl.Tables[0].Columns.Count;

            try
            {

                int cBroj = 0; string cNas = "";
                for (int cc2 =2; cc2 <= nColCount - 1; cc2++)
                {
                    string cIME=dsxl.Tables[0].Columns[cc2].ColumnName.ToString();
                    if (cIME.Substring(0, cIME.Length - 1) == "UKUPNO")
                    {
                        xlSheet.Cells[8, cc2 + 2] = cIME;
                    }
                    else 
                    {
                        if (cIME.Substring(0, 1) == "P") //vrsta 0
                        {
                            cBroj = int.Parse ( cIME.Substring(1, cIME.Length - 1)) + 1; 
                            cNas = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                  "gk_podesenja", "Opis", "dp=0 and Kol=" + cBroj + "");
                        }
                        else
                        {
                            cBroj = int.Parse(cIME.Substring(1, cIME.Length - 1)) + 1;
                            cNas = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                "gk_podesenja", "Opis", "dp=1 and Kol=" + cBroj  + "");
                        }
                        xlSheet.Cells[8, cc2 + 2] = cNas;
                    }
                    
                    
                }


                bool PrekidExporta = false; int xc = 0; string c = ""; int K = 0;
                for (int b = 0; b <= dsxl.Tables[0].Rows.Count - 1; b++)
                {
                    xc = xc + 1;
                    if (PrekidExporta)
                    {
                        PrekidExporta = false;
                        if (MessageBox.Show("Prekid eksporta", "Upozorenje", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            xlApp.Quit();
                            xlSheet = null;
                            xlBook = null;
                            xlApp = null;
                            break;
                        }

                    }
                    c = "";



                    double c1 = 0; double c2 = 0; double c3 = 0;
                    string  cs1 = ""; string cs2 = ""; string cs3 = "";
                    int ii = 2;
                    for (int i = 1; i <= nColCount; i++)
                    {

                        if (n == dPrenos)
                        {
                            dPrenos = dPrenos + 37;
                            if (n == 36)
                            {
                                dDonos = dPrenos - 33;
                            }
                            else
                            {
                                dDonos = dPrenos - 33;
                            }
                            n = n + 5;
                            bNovaStrana = true;
                            isZag = true;
                        }

                        xlBook.Application.Cells[n, 1] =  xc;
                        string cRed=dsxl.Tables[0].Rows[b][i - 1].ToString();
                        xlBook.Application.Cells[n, i + 1] = cRed;//grid1.Columns[i - 1].Value;
                        if (i >= 3)
                        {
                            if (bNovaStrana)
                            {
                                //xlSheet.Cells[dPrenos, i + 1] = double.Parse(xlSheet.Cells[dPrenos - 37, i + 1]) + 
                                //            double.Parse(xlSheet.Cells[dPrenos, i + 1]) + grid1.Columns[i - 1].Value;
                                c1 = 0; c2 = 0; c3 = 0;

                                try { cs1 = (string)xlSheet.Cells[dPrenos - 37, i + 1].Value2; }
                                catch { cs1 = xlSheet.Cells[dPrenos - 37, i + 1].Value2.ToString(); }

                                c1 = double.Parse(cs1 != null && cs1 != "" ? cs1.ToString() : "0");

                                try { cs2 = (string)xlSheet.Cells[dPrenos, i + 1].Value2; }
                                catch { cs2 = xlSheet.Cells[dPrenos, i + 1].Value2.ToString(); }

                                c2 = double.Parse(cs2 != null && cs1 != "" ? cs2.ToString() : "0");

                                c3 = double.Parse(dsxl.Tables[0].Rows[b][i - 1].ToString());

                                xlSheet.Cells[dPrenos, i + 1] = c1 + c2 + c3;

                                //c1 = 0;
                                //c1 = double.Parse(xlSheet.Cells[dPrenos - 37, i + 1]);
                                xlSheet.Cells[dDonos, i + 1] = c1;
                                xlSheet.Cells[dPrenos, 3] = "PRENOS";
                                xlSheet.Cells[dDonos, 3] = "DONOS";
                                if (isZag == true)
                                {
                                    //KOPIRAM ZAGLAVLJE
                                    xlSheet.Range["A7:N8"].Select();
                                    xlBook.Application.Selection.Copy();
                                    xlBook.Application.Range["A" + (dDonos - 2)].Select();
                                    xlBook.Application.ActiveSheet.Paste();
                                }
                                isZag = false;
                            }
                            else
                            {
                                c1 = 0; c2 = 0;


    
                                //cs1 = (string)(xlRange.Cells[dPrenos, i + 1]).Value2;
                                try
                                {
                                    cs1 = (string)xlSheet.Cells[dPrenos, i + 1].Value2;
                                }
                                catch {
                                    cs1 = xlSheet.Cells[dPrenos, i + 1].Value2.ToString();
                                }
                                c1 = double.Parse(cs1 != null && cs1 != "" ? cs1.ToString() : "0");
                                try
                                {
                                    cs2 = dsxl.Tables[0].Rows[b][i-1].ToString();
                                }
                                catch
                                {
                                    MessageBox.Show(dsxl.Tables[0].Rows[b][i-1].ToString());
                                }
                                c2 = double.Parse(cs2 != null && cs2 != "" ? cs2.ToString() : "0");

                                xlSheet.Cells[dPrenos, i + 1] = c1 + c2;//Me.TDBGrid1.Columns(i - 1).Value;
                                xlSheet.Cells[dPrenos, 3] = "PRENOS";
                            }
                            if (n < 36)
                            {
                                xlSheet.Cells[dDonos, i + 1] = 0;
                                xlSheet.Cells[dDonos, 3] = "DONOS";
                            }

                        }

                        ii++;

                    }
                    bNovaStrana = false;
                    xlSheet.Range[xlSheet.Cells[dDonos, 1], xlSheet.Cells[dPrenos, nColCount + 1]].Font.Size = 8;
                    xlSheet.Range[xlSheet.Cells[dDonos, 1], xlSheet.Cells[dPrenos, nColCount + 1]].Font.Size = 8;
                    xlSheet.Range[xlSheet.Cells[dDonos, 1], xlSheet.Cells[dPrenos, nColCount + 1]].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = 2;
                    //rs.MoveNext
                    //Dim a1 As Double
                    //Dim b1 As Long
                    //' a1 = 368 * 100 'xc * 100 / rs.RecordCount
                    //'frm.pb1.FloodPercent = xc * 100 / rs.RecordCount
                    //frm.pb1.FloodPercent = Round(K * 100 / rs.RecordCount, 0)
                    //frm.pb1.Refresh

                    K = K + 1;
                    n = n + 1;
                }

                //If rs.RecordCount > 100 Then
                //Clipboard.SetText s
                //End If
                xlSheet.Range[xlSheet.Cells[9, 1], xlSheet.Cells[9, 1]].Select();
                if (dsxl.Tables[0].Rows.Count - 1 > 100)
                {
                    //xlSheet.Paste();
                }
                //TDBGrid1.Visible = True
                MessageBox.Show("PREGLED KREIRAN U EXCEL FORMATU!", "Obavještenje");
                //Unload frm
                //Set frm = Nothing
                //'    xlBook.xlSheet.Cells(1, 1).Select
                xlBook.Saved = true;
                xlApp.Visible = true;
                xlApp.Windows[1].Visible = true;
                xlApp = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                xlApp.Quit();
                xlSheet = null;
                xlBook = null;
                xlApp = null;

 
            }
        }

        private void štampaBrutoBilansaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            PrintToExcell(cSQLStampa, "KNJIGA PRIHODA I RASHODA za period " + txtDatOd.Text  + " " + txtDatDo.Text  + "");

        }

        private void štampaBrutoBilansaNazivKontaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }




        #endregion










    }
}
