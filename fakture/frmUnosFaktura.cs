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
    public partial class frmUnosFaktura : Form
    {
        #region "REGIJA otvaranje i pretraga uobicajeno na svakoj formi sa minimalnim promjenama ";
        public static string cIzabranaSifra;
        OdbcDataAdapter sqlDataAdapter;

        public static DataSet dataset;
        public static string cNaziv;
        public static string cKolona = "";
        public static DataSet dss;
        public bool bLookUpForma;
        int SortColumn;
        bool bUkljuciDetalje;
        public static string cDbTabela;
        public static string cDbTabela1;
        public static string cDbTabelaP;
        public bool Dragging;
        public int mousex;
        public int mousey;
        public static int cRecordCount;
        public bool bPrviPut = true;
        public static bool bIzSetupaLoco;
        static int iKIFKUF;
        static string sFilterDatum;
        static int cBROJFAKTURE = 0;
        static bool bMODUSFORMEDODAVANJE = false;
        static bool bMODUSFORMEREFRESHZATVARANJEDODAVANJE = false;
        static double CTotalRabat = 0;
        static int bRedosljed = 0;
        static int mNacinPlacanja = 0;

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }

        public static modus_rada modus;

        public frmUnosFaktura(bool bLookUpFormaOut, bool bIzSetupaOut, bool modusadodavanje,int brojFakture)
        {
            bIzSetupaLoco = bIzSetupaOut;//moduli.variable.bIzSetupa;
            bLookUpForma = bLookUpFormaOut;
            cBROJFAKTURE = brojFakture;
            bMODUSFORMEDODAVANJE = modusadodavanje;
            bMODUSFORMEREFRESHZATVARANJEDODAVANJE = modusadodavanje;
            InitializeComponent();
            txtSearch.Focus();



        }

        private void frmUnosFaktura_KeyDown(object sender, KeyEventArgs e)
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
                if (lblMoveDetalje.Visible == true) { btnOdustaniOdDetalja_Click(this, e); ukljucidetalje(false); bUkljuciDetalje = false; grid1.Focus(); }
                else { refreshMaticnuFormu(bMODUSFORMEREFRESHZATVARANJEDODAVANJE); Dispose(); }
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                if (grid1.Rows.Count == 0) return;
                btnKnjizi_Click(this, e);

            }

            if (e.KeyCode == Keys.Insert) { if (modus == modus_rada.pregled) { btnDodavanje_Click(this, e); } }
            if (e.KeyCode == Keys.End) { if (modus == modus_rada.pregled) { btnIzmjena_Click(this, e); } }
            if (e.KeyCode == Keys.Delete) 
            { 
                if (modus == modus_rada.pregled) 
                {
                    bool grpZag = false;
                    foreach (TextBox t in  this.groupBox2.Controls.OfType<TextBox>())
                    {
                        if (t.Focused == true)
                        {
                            grpZag = true;
                            break;
                        }
                    }
                    if (grpZag != true)btnBrisanje_Click(this, e); 
                } 
            }
            if (e.KeyCode == Keys.Home) { if (modus == modus_rada.pregled) { btnDetalji_Click(this, e); } }

            //if (modus != modus_rada.pregled)
            //{
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
            //}
        }

        private void frmUnosFaktura_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    // perform necessary action OVIM GASIM BEEP NA TEXT KONTROLAMA
                    e.Handled = true;
                    break;
            }
        }

        private void frmUnosFaktura_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void frmUnosFaktura_Load(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead, "");  //čitam posljednju poziciju

            cDbTabela = "fak_fakzag";
            cDbTabela1 = "fak_fakstav";


            SortColumn = 4;
            cKolona = grid1.Columns[SortColumn].Name;
            txtFaktura.Enabled = false;
            bUkljuciDetalje = false;
            popunicombo(false);
            ukljucidetalje(bUkljuciDetalje);
            modus = modus_rada.pregled;

            // PrikaziSliku(pnlSlika,this);
            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakonektuj(); }
            else { moduli.kon.konektuj(); }


            popunizag(cBROJFAKTURE , bMODUSFORMEDODAVANJE);
            displaycombo(true);
            //popunigrid("", "", "order by rbsort ASC");


            resize();
            resizeKnjizenje(false);
            DetaljiBox.Enabled = false;
            pokaziFiskal(false);
            txtVrsta.Focus();

            //***********

        }

        private void frmUnosFaktura_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
        }

        // ----------- disebliranje Close buttona -----------
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        // --------------------------------------------------

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cText;


                cText = txtSearch.Text.Replace("'", "");
                if (cText == "")
                {
                    cNaziv = "";
                }
                else
                {
                    cNaziv = cText.ToString();
                    cKolona = grid1.Columns[SortColumn].Name;
                    //--spec.slucaj kod join querija
                    switch (cKolona)
                    {
                        case "naziv":
                            cKolona = "R.Naziv";
                            break;
                    }

                }
                popunigrid(cKolona, cNaziv, "order by  fak_fakstav.Broj, fak_fakstav.StavID ASC");
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

        private void popunizag(int cBroj,bool bMODUSFORMEDODAVANJEL)
        {
            if (bMODUSFORMEDODAVANJEL)
            {
                cBroj = moduli.funkcije.RetNextValue("Broj", cDbTabela, "", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                txtFiskalniBroj.Text = "0";
            }

            string cSQL = "select * from " + cDbTabela + " where Broj=" + cBroj + "";
            OdbcDataAdapter adZag = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            DataSet dsZag = new DataSet();
            adZag.Fill(dsZag, cDbTabela);
            if (dsZag.Tables[cDbTabela].Rows.Count != 0)
            { displayzag(dsZag,cDbTabela); }
            else
            { txtFaktura.Text = cBroj.ToString();
                txtBazaKurs.Text = "1";
            }
            popunigrid("", "", "order by fak_fakstav.Broj, fak_fakstav.StavID ASC");
        }

        private void popunigrid(string cKolona, string cNaziv, string cOrder)
        {
            try
            {

                string cOsnovniUslov = " Where fak_fakstav.Broj=" + txtFaktura.Text + "";
                DataSet dss = new DataSet(); //ili dss.Dispose()
                if (cNaziv == "")
                {
                    dss = pretraga(cNaziv, cOrder, cOsnovniUslov);
                }
                else
                {
                    dss = pretraga("and " + cKolona + " like '%" + cNaziv + "%'", cOrder,cOsnovniUslov);
                }


                cRecordCount = dss.Tables[cDbTabela].Rows.Count;
                grid1.AutoGenerateColumns = false;

                this.grid1.DataSource = dss.Tables[cDbTabela];

               

                int i;

                for (i = 0; i <= this.grid1.Columns.Count - 1; i++)
                {
                    if (grid1.Columns.IndexOf(grid1.Columns[i]) > 2)
                    {
                        grid1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        grid1.Columns[i].DefaultCellStyle.Format = "######0.00";
                    }
                }
                grid1.Columns["Rb"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid1.Columns["Roba"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ;
                grid1.Columns["Naziv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;




                ispraznikontrole();
                displaydata();
                popuniTotal("Iznos", "PorezI", "IznosF", "RabatI");

            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), "Parametri nisu pravilno uneseni !");
            }
        }

        public static DataSet pretraga(string uslov, string cOrder,string cOsnovniUslov)
        {
            try
            {
                string cSQL = "";

                cSQL = "select fak_fakstav.*, R.Naziv from " + cDbTabela1 + " fak_fakstav " +
                "LEFT JOIN roba R ON fak_fakstav.Roba=R.sifra " +
                ""+cOsnovniUslov+" "+uslov+" "+cOrder+"";
                

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

        private void popuniTotal(string cKolona1, string cKolona2, string cKolona3, string cUslov)
        {
            //btnKnjizi.Enabled = false;
            try
            {
                OdbcDataAdapter adTotal = new OdbcDataAdapter(
                    "select sum(" + cKolona1 + ") as jedan, sum(" + cKolona2 + ") as dva,sum(" + cKolona3 + ") as tri," +
                    "sum("+cUslov+") as cet from " + cDbTabela1 + " " +
                                    "WHERE " + cDbTabela1 + ".Broj="+txtFaktura.Text+"",
                    bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet dsTotal = new DataSet();
                adTotal.Fill(dsTotal, cDbTabela);

                if (dsTotal.Tables[cDbTabela].Rows.Count != 0)
                {
                    this.txtUkupnoTotal.Text = dsTotal.Tables[cDbTabela].Rows[0]["tri"].ToString();
                    this.txtPDVTotal.Text = dsTotal.Tables[cDbTabela].Rows[0]["dva"].ToString();
                    this.txtOsnovicaTotal.Text = dsTotal.Tables[cDbTabela].Rows[0]["jedan"].ToString();
                    CTotalRabat  = double.Parse (dsTotal.Tables[cDbTabela].Rows[0]["cet"].ToString());
                }
                else
                {
                    txtOsnovicaTotal.Text = "0,00"; txtPDVTotal.Text = "0,00"; 
                    this.txtUkupnoTotal.Text = "0,00"; CTotalRabat = 0;
                }

            }
            catch (Exception e) { }
        }

        private void resize()
        {
            grid1.Width = Width - 8;
            grid1.Height = (Height - 250) - 35;
            pnlSlika.Width = Width - 8;
            panel1.Width = Width - 8;
            resizeTotal();

        }

        private void resizeTotal()
        {
            lblTotal.Size = new System.Drawing.Size(773, 35);
            lblTotal.Top = grid1.Bottom - 1;
            lblTotal.Left = grid1.Left;

        }

        private void resizeKnjizenje(bool bPokazi)
        {
            lblKnjizenje.Visible = false;
            lblTotal.Enabled = true;
            lblTotal.Visible = true;
            if (bPokazi)
            {
                resizeTotal();
                lblTotal.Enabled = false;
                ukljucidetalje(false);
                lblKnjizenje.Top = grid1.Bottom - 1;
                lblKnjizenje.Left = grid1.Left;
                lblTotal.Visible = false;
                lblKnjizenje.Visible = true;
            }

        }

        private void pokaziFiskal(bool bPokazi)
        {
            lblUnosFiskalnogBroja.Visible = false;

            if (bPokazi)
                lblUnosFiskalnogBroja.Visible = true ;

            lblUnosFiskalnogBroja.Top = grid1.Top + 60;
            lblUnosFiskalnogBroja.Left = grid1.Left + 260;
        
        }

        private void grid1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //omoguciknjizenje(grid1.CurrentCell.RowIndex);
                if (bUkljuciDetalje == false) return;
            }
            catch (Exception a) { }

        }

        private void omoguciknjizenje(int cKred)
        {
            this.btnKnjizi.Enabled = false;
            //string cs = grid1["zakljuceno", cKred].Value.ToString();
            if (cKred  == 0)
            {
                this.btnKnjizi.Enabled = true;
            }
        }

        private void ukljucidetalje(bool bUkljuciDetalje)
        {
            if (bUkljuciDetalje == false)
            {
                lblMoveDetalje.Visible = false;
                groupBox2.Enabled = true;
            }
            else
            {
                try
                {
                    if (modus != modus_rada.dodavanje)
                    {
                    }

                    lblMoveDetalje.Visible = true;
                    groupBox2.Enabled = false;
                }
                catch (Exception gr) { }

            }

        }

        private void grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortColumn = e.ColumnIndex;
        
            txtSearch.Text = "";
            txtSearch.Focus();
        }

        private void blokirajkontrole(Boolean upali)
        {
            switch (upali)
            {
                case true:
                    DetaljiBox.Enabled = true;
                    panel1.Enabled = false;
                    grid1.Enabled = false;
                    break;
                case false:
                    DetaljiBox.Enabled = false;
                    panel1.Enabled = true;
                    grid1.Enabled = true;
                    break;
            }
        }

        private void txtRBsort_Leave(object sender, EventArgs e) //nisam naso lost fcous
        {

            if (modus == modus_rada.izmjena) { return; }
            if (modus == modus_rada.pregled) { return; }
          

            //string cUser = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
            //    "gk_dnevnik", "korisnik", "nalog=" + txtRBsort.Text  + "");

            //if (cUser != moduli.variable.cUser)
            //{
            //    MessageBox.Show("Ovaj nalog je radio drugi korisnik!");

            //}

            //popunirednibroj();


        }

        private void txtRb_Leave(object sender, EventArgs e)
        {
            //string a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
            //    "gk_dnevnik", "korisnik", "nalog=" + txtRBsort.Text + " and rb=" + txtRb.Text + "");
            //if (a != "nema trazene vrijednosti")
            //{
            //    MessageBox.Show("Redni broj: " + txtRb.Text + " već postoji.Program će dati naredni slobodni broj.");
            //    int cBr = moduli.funkcije.RetNextValue("rb", cDbTabela, " where nalog=" + txtRBsort.Text + "", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            //    if (cBr == 0) return;
            //    this.txtRb.Text = Convert.ToString(cBr);

            //}

        }

        private void btnDodavanje_Click(object sender, EventArgs e)
        {
            if (bMODUSFORMEDODAVANJE) return;
            modus = modus_rada.dodavanje;
            cIzabranaSifra = "";
            ispraznikontrole();
            blokirajkontrole(true);

            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            resizeTotal();
            popunirednibroj();
            txtRabatP.Text = txtRabat.Text;
            txtRoba.Focus();
        }

        private void btnIzmjena_Click(object sender, EventArgs e)
        {
            if (grid1.Rows.Count == 0) return;
            modus = modus_rada.izmjena;
            blokirajkontrole(true);
            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            resizeTotal();
            omogucisifre(false);
            txtFaktura.Focus();
        }

        private void btnOdustaniOdDetalja_Click(object sender, EventArgs e)
        {
            switch (modus)
            {
                case modus_rada.izmjena:
                    modus = modus_rada.pregled;
                    popunigrid("", "", "order by fak_fakstav.Broj, fak_fakstav.StavID ASC");
                    omogucisifre(true);
                    break;
                case modus_rada.dodavanje:
                    modus = modus_rada.pregled;
                    popunigrid("", "", "order by fak_fakstav.Broj, fak_fakstav.StavID ASC");
                    break;
            }

            blokirajkontrole(false);

            bUkljuciDetalje = true;
            btnDetalji_Click(this, e);
        }

        private bool bPostojiDok(string cDokument, string cTarifa, int cID, int cOrgDio)
        {

            string a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                cDbTabela, "id", "dokument='" + cDokument + "' and tarifa='" + cTarifa + "' and orgdio=" + cOrgDio + " and id <> " + cID + "");

            if (a != "nema trazene vrijednosti")
            { return true; }
            else
            { return false; }

        }

        private void btnUpisi_Click(object sender, EventArgs e)
        {
            if (chkZakljuceno.Checked == true)
            {
                MessageBox.Show("Faktura je zaključena. Promjene nisu dozvoljene.", "Upozorenje !");
                return;
            }
            string cCreateSql = "";

            if (modus == modus_rada.dodavanje) popunirednibroj();

            string cNalog = txtFaktura.Text;

            int cBroj = int.Parse(txtFaktura.Text);
            int cRb = int.Parse(txtRb.Text);
            string cRoba = txtRoba.Text;
            string cKol =moduli.funkcije.MySqlBr(txtKolicina.Text);
            string cCijenaBP = moduli.funkcije.MySqlBr(txtCijenaBP.Text);
            string cCijena = moduli.funkcije.MySqlBr(txtCijena.Text);
            string cPorezP = moduli.funkcije.MySqlBr(txtTBIme.Text);
            string cPorezI = moduli.funkcije.MySqlBr(txtIznosPdv.Text);
            string cRabatP = moduli.funkcije.MySqlBr(txtRabatP.Text);
            string cRabatI = moduli.funkcije.MySqlBr(txtRabatI.Text);
            string cIznosBP = moduli.funkcije.MySqlBr(txtOsnovica.Text);
            string cUkupno = moduli.funkcije.MySqlBr(txtIznos.Text);

            string cOperator = moduli.variable.cUserProgram;


            switch (modus)
            {
                case modus_rada.dodavanje:

                    cCreateSql = "insert into " + cDbTabela1 + " ( Broj, StavID, Roba, CijenaBP, Cijena, Kolicina, " +
                    "Iznos, RabatP, RabatI, Tarifa, PorezP, PorezI, IznosF) Values "+
                    "("+cBroj+","+cRb+",'"+cRoba+"',"+cCijenaBP+","+cCijena+","+cKol+","+cIznosBP+","+
                    ""+cRabatP+","+cRabatI+",'"+txtTB.Text+"',"+cPorezP+","+cPorezI+","+cUkupno+")";

                    try
                    {

                        moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql);
                        modus = modus_rada.pregled;
                        popunigrid("", "", "order by  fak_fakstav.Broj, fak_fakstav.StavID DESC");
                    }
                    catch (Exception a) { }
                    break;


                case modus_rada.izmjena:

                    cCreateSql = "update " + cDbTabela1 + " set  Roba='" + cRoba + "', CijenaBP=" + cCijenaBP + ", Cijena=" + cCijena + "," +
                        "Kolicina=" + cKol + " , Iznos=" + cIznosBP + ", RabatP=" + cRabatP + ", RabatI=" + cRabatI + "," +
                        "Tarifa='" + txtTB.Text + "', PorezP=" + cPorezP + ", PorezI=" + cPorezI + ", IznosF=" + cUkupno + " "+
                        "where Broj=" + cBroj + " and StavID=" + cRb + "";

                    try
                    {
                        moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql);
                        cIzabranaSifra = moduli.variable.cEditVrijednostString;
                        modus = modus_rada.pregled;
                        omogucisifre(true);
                        ispraznikontrole(); displaydata(); grid1.Refresh();
                        cIzabranaSifra = moduli.variable.cEditVrijednostString;
                        //pronadjired(grid1);

                        moduli.variable.cEditVrijednostString = "";

                    }
                    catch (Exception a) { MessageBox.Show(a.ToString()); }
                    break;


            }


            btnCloseDetalje_Click(this, e);
            modus = modus_rada.pregled;
            blokirajkontrole(false);
            grid1.Focus();


        }

        private void btnUpisi_Enter(object sender, EventArgs e)
        {


            //http://stackoverflow.com/questions/9450222/checking-multiple-textbox-if-theyre-null-or-whitespace
            // linq query - metoda (minimalno vs2008 i framework 3,5)
            var emptyTextboxes = from tb in DetaljiBox.Controls.OfType<TextBox>()
                                 where string.IsNullOrEmpty(tb.Text)
                                 select tb;

 


            if (emptyTextboxes.Any())
            {
                foreach (var textBox in emptyTextboxes)
                {
                    MessageBox.Show("Vrijednost je obavezna za unos. Popunite polje " + textBox.Name + " !");
                    textBox.Focus();
                    break;
                }
            }


            //var emptyTextboxes1 = from tb in groupBox3.Controls.OfType<TextBox>()
            //                      where string.IsNullOrEmpty(tb.Text)
            //                      select tb;

            //if (emptyTextboxes1.Any())
            //{
            //    foreach (var textBox1 in emptyTextboxes1)
            //    {
            //        MessageBox.Show("Vrijednost je obavezna za unos. Popunite polje " + textBox1.Name + " !");
            //        textBox1.Focus();
            //        break;
            //    }
            //}

            //var emptyTextboxes2 = from tb in groupBox2.Controls.OfType<TextBox>()
            //                      where string.IsNullOrEmpty(tb.Text)
            //                      select tb;

            //if (emptyTextboxes2.Any())
            //{
            //    foreach (var textBox2 in emptyTextboxes2)
            //    {
            //        if (textBox2.Name != "txtID")
            //        {
            //            MessageBox.Show("Vrijednost je obavezna za unos. Popunite polje " + textBox2.Name + " !");
            //            textBox2.Focus();
            //            break;
            //        }
            //    }
            //}

        }

        private void btnBrisanje_Click(object sender, EventArgs e)
        {
            if (chkZakljuceno.Checked == true)
            {
                MessageBox.Show("Faktura je zaključena. Promjene nisu dozvoljene.", "Upozorenje !");
                return;
            }
            
            if (MessageBox.Show("Da li zaista želite obrisati stavku !", "Upozorenje !", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (grid1.Rows.Count == 0) return;
                try
                {

                    string cCreateSql = "";

                    int cRb = int.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString());
                    int cBroj = int.Parse(txtFaktura.Text);
                    cCreateSql = "delete from " + cDbTabela1 + " where  Broj=" + cBroj + " and StavID=" + cRb + "";
                    moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql);

                    popunigrid("", "", "order by  fak_fakstav.Broj, fak_fakstav.StavID ASC");
                    modus = modus_rada.pregled;
                    grid1.Focus();
                }
                catch (Exception ex) { MessageBox.Show(" Greška : " + ex.ToString()); }

            }
        }

        private void btnDetalji_Click(object sender, EventArgs e)
        {
            if (bUkljuciDetalje)
            {
                bUkljuciDetalje = false;
                resizeTotal();
                grid1.Focus();
            }
            else
            {
                bUkljuciDetalje = true;
                resizeTotal();
            }
            ukljucidetalje(bUkljuciDetalje);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            //DialogResult dlgResult = MessageBox.Show("Da li da štampa bude iz text fielda ?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dlgResult == DialogResult.Yes) { frmStampaFirme a = new frmStampaFirme("Txt"); a.Show(this); }
            //else { frmStampaFirme a = new frmStampaFirme("DataSet"); a.Show(this); }

            int cBrojFak = int.Parse(txtFaktura.Text);

            string cSQLDetalj = "select fs.roba as sifrarobe,r.naziv as nazivrobe , r.jm, fs.kolicina as kol," +
                "fs.cijena, (fs.Cijena * fs.Kolicina) as iznos,fs.iznos as osnovica, fs.porezi, fs.porezp " +
                "from " + cDbTabela1 + " fs left join roba r on fs.roba=r.sifra where fs.broj=" + cBrojFak + "";

            string cSQLTB = null;
            if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql)
            {
                cSQLTB = "SELECT tarifnibroj.naziv as opis,dnevnik.tarifa as TarBr,dnevnik.Porezp AS Porezp," +
                "Sum(dnevnik.iznos) as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                "From " + cDbTabela1 + " dnevnik left join tarifnibroj on dnevnik.Tarifa=tarifnibroj.sifra " +
                "Where dnevnik.broj =" + cBrojFak + " GROUP BY dnevnik.Tarifa ";
            }
            else
            {
                cSQLTB = "SELECT FIRST(tarifnibroj.naziv) as opis,FIRST(dnevnik.tarifa) as TarBr,FIRST(dnevnik.Porezp) AS Porezp," +
                "sum(dnevnik.iznos)as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                "From " + cDbTabela1 + " dnevnik left join tarifnibroj on dnevnik.Tarifa=tarifnibroj.sifra " +
                "Where dnevnik.broj =" + cBrojFak + " GROUP BY dnevnik.Tarifa  ";
            }

            string CpubKupac = txtPartner.Text;

            string cSQLKupac = "select kupci.sifra,kupci.naziv,kupci.adresa,kupci.id, kupci.pdvbroj AS pdv, opstine.naziv AS grad from kupci left join opstine " +
            "on kupci.opstina=opstine.sifra where kupci.sifra='" + CpubKupac + "'";

            DialogResult dlg = MessageBox.Show("Da li želite štampu u Wordu?", "Upozorenje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.No)
            {
                frmStampaFirme a = new frmStampaFirme("Txt", "FakturaMP", cSQLDetalj, cDbTabela1, cSQLTB, "tarifnibroj", cSQLKupac, "kupci", false);
                a.Show();
            }
            else
            {
                int mReportFiskalniRacun =int.Parse (moduli.funkcije.RetFieldValue("int",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                cDbTabela1, "FiskalniRacun", "Broj=" + cBrojFak + ""));

                DateTime  mReportDatum = DateTime.Parse ( txtDatum.Text);
                moduli.funkcije.PrintToMsWord(cSQLDetalj, cDbTabela1, cSQLTB, "FAKTURA", CpubKupac, cBrojFak, mReportFiskalniRacun, mReportDatum, false);
            }

            

        }

        private void btnOdaberi_Click(object sender, EventArgs e)
        {
            if (chkZakljuceno.Checked == false )
            { 
                
                MessageBox.Show ("Račun mora biti zaključen da bi mu se unio fiskalni broj.");
                return; 
            }

            int cBroj = int.Parse(txtFaktura.Text);

            int cRetFis = int.Parse (moduli.funkcije.RetFieldValue("int", bIzSetupaLoco == true ?
                moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                cDbTabela, "FiskalniRacun", "Broj=" + cBroj + ""));

            if (cRetFis != 0)
            {
                MessageBox.Show("Faktura već ima fiskalni broj.");
                txtUnesiFiskalniBroj.Text = cRetFis.ToString();
            }

            pokaziFiskal(true);
            txtUnesiFiskalniBroj.Focus();

        }

        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Odaberi();

        }

        private void grid1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter) Odaberi();
        }

        private void Odaberi()
        {
            try
            {

                if (bLookUpForma == false) return;

                Dispose();

            }
            catch (Exception e) { MessageBox.Show("Morate odabrati vrijednost!"); txtSearch.Text = ""; }
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            refreshMaticnuFormu(bMODUSFORMEREFRESHZATVARANJEDODAVANJE);
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
            //obrisitmptelefone();
            Dispose();
        }

        private void lblMoveDetalje_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                mousex = -e.X;
                mousey = -e.Y;
                int clipleft = PointToClient(MousePosition).X - lblMoveDetalje.Location.X;
                int cliptop = PointToClient(MousePosition).Y - lblMoveDetalje.Location.Y;
                int clipwidth = ClientSize.Width - (lblMoveDetalje.Width - clipleft);
                int clipheight = ClientSize.Height - (lblMoveDetalje.Height - cliptop);
                Cursor.Clip = RectangleToScreen(new Rectangle(clipleft, cliptop, clipwidth, clipheight));
                lblMoveDetalje.Invalidate();
            }
        }

        private void lblMoveDetalje_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //move control to new position
                Point MPosition = new Point();
                MPosition = PointToClient(MousePosition);
                MPosition.Offset(mousex, mousey);
                //ensure control cannot leave container
                lblMoveDetalje.Location = MPosition;
            }
        }

        private void lblMoveDetalje_MouseUp(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //end the dragging
                Dragging = false;
                Cursor.Clip = new Rectangle();

                lblMoveDetalje.Invalidate();
            }
        }

        private void btnCloseDetalje_Click(object sender, EventArgs e)
        {
            btnOdustaniOdDetalja_Click(this, e);
            bUkljuciDetalje = true;
            btnDetalji_Click(this, e);
        }

        private void lblTotal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                mousex = -e.X;
                mousey = -e.Y;
                int clipleft = PointToClient(MousePosition).X - lblTotal.Location.X;
                int cliptop = PointToClient(MousePosition).Y - lblTotal.Location.Y;
                int clipwidth = ClientSize.Width - (lblTotal.Width - clipleft);
                int clipheight = ClientSize.Height - (lblTotal.Height - cliptop);
                Cursor.Clip = RectangleToScreen(new Rectangle(clipleft, cliptop, clipwidth, clipheight));
                lblTotal.Invalidate();
            }
        }

        private void lblTotal_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //move control to new position
                Point MPosition = new Point();
                MPosition = PointToClient(MousePosition);
                MPosition.Offset(mousex, mousey);
                //ensure control cannot leave container
                lblTotal.Location = MPosition;
            }
        }

        private void lblTotal_MouseUp(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //end the dragging
                Dragging = false;
                Cursor.Clip = new Rectangle();

                lblTotal.Invalidate();
            }
        }

        private void lblKnjizenje_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                mousex = -e.X;
                mousey = -e.Y;
                int clipleft = PointToClient(MousePosition).X - lblKnjizenje.Location.X;
                int cliptop = PointToClient(MousePosition).Y - lblKnjizenje.Location.Y;
                int clipwidth = ClientSize.Width - (lblKnjizenje.Width - clipleft);
                int clipheight = ClientSize.Height - (lblKnjizenje.Height - cliptop);
                Cursor.Clip = RectangleToScreen(new Rectangle(clipleft, cliptop, clipwidth, clipheight));
                lblKnjizenje.Invalidate();
            }
        }

        private void lblKnjizenje_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //move control to new position
                Point MPosition = new Point();
                MPosition = PointToClient(MousePosition);
                MPosition.Offset(mousex, mousey);
                //ensure control cannot leave container
                lblKnjizenje.Location = MPosition;
            }
        }

        private void lblKnjizenje_MouseUp(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //end the dragging
                Dragging = false;
                Cursor.Clip = new Rectangle();

                lblKnjizenje.Invalidate();
            }
        }

        private void lblUnosFiskalnogBroja_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragging = true;
                mousex = -e.X;
                mousey = -e.Y;
                int clipleft = PointToClient(MousePosition).X - lblUnosFiskalnogBroja.Location.X;
                int cliptop = PointToClient(MousePosition).Y - lblUnosFiskalnogBroja.Location.Y;
                int clipwidth = ClientSize.Width - (lblUnosFiskalnogBroja.Width - clipleft);
                int clipheight = ClientSize.Height - (lblUnosFiskalnogBroja.Height - cliptop);
                Cursor.Clip = RectangleToScreen(new Rectangle(clipleft, cliptop, clipwidth, clipheight));
                lblUnosFiskalnogBroja.Invalidate();
            }
        }

        private void lblUnosFiskalnogBroja_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //move control to new position
                Point MPosition = new Point();
                MPosition = PointToClient(MousePosition);
                MPosition.Offset(mousex, mousey);
                //ensure control cannot leave container
                lblUnosFiskalnogBroja.Location = MPosition;
            }
        }

        private void lblUnosFiskalnogBroja_MouseUp(object sender, MouseEventArgs e)
        {
            if (Dragging)
            {
                //end the dragging
                Dragging = false;
                Cursor.Clip = new Rectangle();

                lblUnosFiskalnogBroja.Invalidate();
            }
        }

        #endregion

        #region "############################################################posebno na svakoj formi"

        private void omogucisifre(bool Upali)
        {
            txtRb.Enabled = false;
            txtDokument.Enabled = false; txtVrsta.Enabled = false;
            if (Upali == true) {txtRb.Enabled = true; txtDokument.Enabled = true; txtVrsta.Enabled = true; }

        }

        private void txtRBsort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtRb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void displayzag(DataSet dsZagsrc,string Tabela)
        {
            
            try {

                    txtDatum.DataBindings.Add("Text", dsZagsrc.Tables[Tabela] , "Datum", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtPartner.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Partner", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtFaktura.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Broj", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtDokument.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "BrojOtp", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtVrsta.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Vrsta", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtValuta.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Valuta", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtRokPlacanja.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Rok", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtUgovor.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Ugovor", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtNarudzba.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Narudzba", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtRabat.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "RabatDef", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtBazaKurs.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "Kurs", false, DataSourceUpdateMode.OnPropertyChanged);
                    txtFiskalniBroj.DataBindings.Add("Text", dsZagsrc.Tables[Tabela], "FiskalniRacun", false, DataSourceUpdateMode.OnPropertyChanged);

                    int cZak=int.Parse (moduli.funkcije.RetFieldValue("int",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    cDbTabela, "Zakljuceno", "Broj=" + txtFaktura.Text + ""));

                    chkZakljuceno.Checked = false;
                    if (cZak == 1) chkZakljuceno.Checked = true;
                    omoguciknjizenje(cZak);
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        
        }

        private void displaydata()
        {

            try
            {

                txtRb.DataBindings.Add("Text", grid1.DataSource, "StavID", false, DataSourceUpdateMode.OnPropertyChanged);
                txtRoba.DataBindings.Add("Text", grid1.DataSource, "Roba", false, DataSourceUpdateMode.OnPropertyChanged);
                txtTB.DataBindings.Add("Text", grid1.DataSource, "Tarifa", false, DataSourceUpdateMode.OnPropertyChanged);
                txtTBIme.DataBindings.Add("Text", grid1.DataSource, "PorezP", false, DataSourceUpdateMode.OnPropertyChanged);
                txtRabatP.DataBindings.Add("Text", grid1.DataSource, "RabatP", false, DataSourceUpdateMode.OnPropertyChanged);
                txtRabatI.DataBindings.Add("Text", grid1.DataSource, "RabatI", false, DataSourceUpdateMode.OnPropertyChanged);
                txtKolicina.DataBindings.Add("Text", grid1.DataSource, "Kolicina", false, DataSourceUpdateMode.OnPropertyChanged);
                txtCijenaBP.DataBindings.Add("Text", grid1.DataSource, "CijenaBP", false, DataSourceUpdateMode.OnPropertyChanged);
                txtCijena.DataBindings.Add("Text", grid1.DataSource, "Cijena", false, DataSourceUpdateMode.OnPropertyChanged);
                txtIznosPdv.DataBindings.Add("Text", grid1.DataSource, "PorezI", false, DataSourceUpdateMode.OnPropertyChanged);
                txtIznos.DataBindings.Add("Text", grid1.DataSource, "IznosF", false, DataSourceUpdateMode.OnPropertyChanged);
                txtOsnovica.DataBindings.Add("Text", grid1.DataSource, "Nabavna", false, DataSourceUpdateMode.OnPropertyChanged);
                

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString)
            }



        }

        private void ispraznikontrole()
        {

            txtRb.DataBindings.Clear();
            txtRoba.DataBindings.Clear();
            txtTB.DataBindings.Clear();
            txtTBIme.DataBindings.Clear();
            txtRabatP.DataBindings.Clear();
            txtRabatI.DataBindings.Clear();
            txtKolicina.DataBindings.Clear();
            txtCijenaBP.DataBindings.Clear();
            txtCijena.DataBindings.Clear();
            txtIznosPdv.DataBindings.Clear();
            txtIznos.DataBindings.Clear();
            txtOsnovica.DataBindings.Clear();
            

            txtRb.Text = "";
            txtRoba.Text = "";
            txtTB.Text = "";
            txtTBIme.Text = "";
            txtRabatP.Text = "";
            txtRabatI.Text = "";
            txtKolicina.Text = "";
            txtCijenaBP.Text = "";
            txtCijena.Text = "";
            txtIznosPdv.Text = "";
            txtIznos.Text = "";
            txtOsnovica.Text = "";
            
  

        }

        private void popunirednibroj()
        {
            txtRb.Text = "1";
            //int corg = txtVrsta.Text == "" ? 0 : int.Parse(txtVrsta.Text);

            //string cRB = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
            //    cDbTabela1, "StavId", "Broj=" + txtFaktura.Text + "");
            int cRB = moduli.funkcije.RetNextValue("StavID", cDbTabela1, " where Broj=" + txtFaktura.Text + "",
                bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            if (cRB.ToString() != "nema trazene vrijednosti")
            {
                txtRb.Text = cRB.ToString();


            }



            return;

        }


        #region "REGIJA LookUp kursnalista"

        private void txtValuta_TextChanged(object sender, EventArgs e)
        {
            txtValuta.Text = txtValuta.Text.ToUpper(); this.txtValuta.SelectionStart = this.txtValuta.Text.Length;
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                "kursnalista", "kurs", "Datum=(SELECT MAX(Datum) AS D FROM kursnalista WHERE Valuta='" + txtValuta.Text + "')");

            //if (modus != modus_rada.pregled)
            //{
                if (a != "nema trazene vrijednosti")
                {
                    txtValutaIme.Text = a;

                }
                else
                {
                    txtValutaIme.Text = "";
                }
                txtValuta.Focus();
            //}
            //else { txtValutaIme.Text = a; }


        }

        private void btnValutaLookUp_Click(object sender, EventArgs e)
        {
            //moduli.variable.bIzSetupa = true;
            frmKursnaLista a = new frmKursnaLista(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUpValuta();
            //'frmOpstine.bLookUpForma = False
            //moduli.variable.bIzSetupa = true;
            txtValuta.Focus();
        }

        private void popuniLookUpValuta()
        {

            if (moduli.variable.cLookUpSifra != "")
            {
                txtValuta.Text = moduli.variable.cLookUpSifra;
                txtValutaIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                if (txtValuta.Text == "")
                {
                    txtValuta.Text = "*";
                }
            }

        }

        private void txtValuta_Validated(object sender, EventArgs e)
        {
            if (txtValuta.Text == "" || txtValutaIme.Text == "")
            {
                btnValutaLookUp_Click(this, e);
            }
        }

        #endregion

        #region "REGIJA LookUp Vrsta"

        private void txtVrsta_TextChanged(object sender, EventArgs e)
        {
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "fak_vrstedok", "naziv", "sifra=" + txtVrsta.Text + "");

            //if (modus != modus_rada.pregled)
            //{
                if (a != "nema trazene vrijednosti")
                { txtVrstaIme.Text = a; }
                else
                {
                    txtVrstaIme.Text = "";
                }
            //}
            //else { txtVrstaIme.Text = a; }
        }

        private void btnVrstaLookUp_Click(object sender, EventArgs e)
        {
            frmVrsteDokumenataFak a = new frmVrsteDokumenataFak(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUpVrsta();
            txtVrsta.Focus();
        }

        private void popuniLookUpVrsta()
        {

            if (moduli.variable.cLookUpSifra != "")
            {
                txtVrsta.Text = moduli.variable.cLookUpSifra;
                txtVrstaIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                if (txtVrsta.Text == "")
                {
                    txtVrsta.Text = "*";
                }
            }

        }

        private void txtVrsta_Validated(object sender, EventArgs e)
        {
            if (txtVrsta.Text == "" || txtVrstaIme.Text == "")
            {
                btnVrstaLookUp_Click(this, e);
            }
        }

        #endregion

        #region "REGIJA LookUp partner"

        private void txtPartner_TextChanged(object sender, EventArgs e)
        {
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "kupci", "naziv", "sifra='" + txtPartner.Text + "'");

            //if (modus != modus_rada.pregled)
            //{
                if (a != "nema trazene vrijednosti")
                { txtPartnerIme.Text = a; }
                else
                {
                    txtPartnerIme.Text = "";
                }
            //}
            //else { txtPartnerIme.Text = a; }

        }

        private void btnPartnerLookUp_Click(object sender, EventArgs e)
        {
            //moduli.variable.bIzSetupa = true;
            frmKupci a = new frmKupci(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUpPartner();
            //'frmOpstine.bLookUpForma = False
            //moduli.variable.bIzSetupa = true;
            txtPartner.Focus();
        }

        private void popuniLookUpPartner()
        {

            if (moduli.variable.cLookUpSifra != "")
            {
                txtPartner.Text = moduli.variable.cLookUpSifra;
                txtPartnerIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                if (txtPartner.Text == "")
                {
                    txtPartner.Text = "*";
                }
            }

        }

        private void txtPartner_Validated(object sender, EventArgs e)
        {
            if (txtPartner.Text == "" || txtPartnerIme.Text == "")
            {
                btnPartnerLookUp_Click(this, e);
            }
        }

        #endregion

        #region "REGIJA LookUp Roba"

        private void txtRoba_TextChanged(object sender, EventArgs e)
        {

            string a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "naziv", "sifra='" + txtRoba.Text + "'");
            string atb = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "TB", "sifra='" + txtRoba.Text + "'");
            string aPP = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "Porez", "sifra='" + txtRoba.Text + "'");
            string aCnBp = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "CijenaBP", "sifra='" + txtRoba.Text + "'");
            string aCn = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "Cijena", "sifra='" + txtRoba.Text + "'");
            if (modus != modus_rada.pregled)
            {
                if (a != "nema trazene vrijednosti")
                { 
                    txtRobaIme.Text = a;
                    txtTB.Text = atb;
                    txtTBIme.Text = aPP;
                    txtCijenaBP.Text = aCnBp;
                    txtCijena.Text = aCn;
                }
                else
                {
                    txtRobaIme.Text = "";
                    txtTB.Text = "";
                    txtTBIme.Text = "";
                    txtCijenaBP.Text = "";
                    txtCijena.Text = "";

                }
            }
            else
            {
                txtRobaIme.Text = a;
                txtTB.Text = atb;
                txtTBIme.Text = aPP;
                txtCijenaBP.Text = aCnBp;
                txtCijena.Text = aCn;

            }
        }

        private void btnRobaLookUp_Click(object sender, EventArgs e)
        {
            frmRoba a = new frmRoba(true, bIzSetupaLoco); a.ShowDialog();


            popuniLookUpRoba();
            txtRoba.Focus();
        }

        private void popuniLookUpRoba()
        {

            if (moduli.variable.cLookUpSifra != "")
            {
                txtRoba.Text = moduli.variable.cLookUpSifra;
                txtRobaIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
                string atb = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "TB", "sifra='" + txtRoba.Text + "'");
                string aPP = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "Porez", "sifra='" + txtRoba.Text + "'");
                string aCnBp = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "CijenaBP", "sifra='" + txtRoba.Text + "'");
                string aCn = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "roba", "Cijena", "sifra='" + txtRoba.Text + "'");

                txtTB.Text = atb;
                txtTBIme.Text = aPP;
                txtCijenaBP.Text = aCnBp;
                txtCijena.Text = aCn;
            }
            else
            {
                if (txtRoba.Text == "")
                {
                    txtRoba.Text = "*";
                    txtTB.Text = "";
                    txtTBIme.Text = "";
                    txtCijenaBP.Text = "";
                    txtCijena.Text = "";

                }
            }

        }

        private void txtRoba_Validated(object sender, EventArgs e)
        {
            if (txtRoba.Text == "" || txtRobaIme.Text == "")
            {
                btnRobaLookUp_Click(this, e);
            }
        }

        private void txtRoba_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        private void btnUpisiZag_Click(object sender, EventArgs e)
        {
            if (chkZakljuceno.Checked == true)
            {
                MessageBox.Show("Faktura je zaključena. Promjene nisu dozvoljene.", "Upozorenje !");
                return;
            }

            double cStariRabat = double.Parse(moduli.funkcije.RetFieldValue("double",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                cDbTabela, "RabatDef", "Broj=" + txtFaktura.Text + ""));

            double cNoviRabat=double.Parse(txtRabat.Text);
            if (grid1.Rows.Count != 0)
            {
                if (cNoviRabat != cStariRabat)
                {
                    if (MessageBox.Show("Da li želite izmjeniti rabat za sve stavke dokumenta? Ako ne želite, stari rabat" +
                        "biće vraćen.", "Upozorenje!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {



                    }
                }
            }

            int cFaktura = int.Parse(txtFaktura.Text);
            string cDokument = txtDokument.Text;
            int cVrsta = int.Parse(txtVrsta.Text);
            DateTime cDatumFak = txtDatum.Value;

            string cValuta = txtValuta.Text;
            string cKurs = moduli.funkcije.MySqlBr(txtValutaIme.Text);
            int cRok = int.Parse(txtRokPlacanja.Text);
            string cUgovor = txtUgovor.Text;
            string cNarudzba = txtNarudzba.Text;
            string cRabat = moduli.funkcije.MySqlBr(txtRabat.Text);
            string cPartner = txtPartner.Text;
            int cPdvStatus = int.Parse (moduli.funkcije.RetFieldValue("int",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
            "kupci", "pdv", "sifra='" + cPartner + "'"));
            int cPravno = int.Parse (moduli.funkcije.RetFieldValue("int",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
            "kupci", "pravno", "sifra='" + cPartner + "'"));
            string cOperator = moduli.variable.cUserProgram;
            mNacinPlacanja = int.Parse(cmbPlacanje.SelectedValue.ToString());

            if (bMODUSFORMEDODAVANJE == true)
            {

                string cSQL="insert into "+cDbTabela+" (Broj,Vrsta,Brojotp,Lice,Datum,Partner,Valuta,Kurs,Operator,Rabatdef,RjPart,"+
                "Ugovor,Narudzba,Rok,Pdv_status,Placeno) Values "+
                "(" + cFaktura + "," + cVrsta + ",'" + cDokument.Replace("'", "") + "'," + cPravno + "," + moduli.funkcije.SQLDate(cDatumFak) + ",'" + cPartner + "','" + cValuta + "'," +
                "" + cKurs + ",'" + cOperator.Replace("'", "") + "'," + cRabat + ",1,'" + cUgovor.Replace("'", "") + "','" + cNarudzba.Replace("'", "") + "'," + cRok + "," + cPdvStatus + "," + mNacinPlacanja + ")";
                moduli.funkcije.SQLExecute(false,cSQL);

            }
            else
            {
                string cSQL = "update  " + cDbTabela + " Set Vrsta=" + cVrsta + ",Brojotp='" + cDokument.Replace("'", "") + "',Lice=" + cPravno + "," +
                "Datum=" + moduli.funkcije.SQLDate(cDatumFak) + ",Partner='" + cPartner + "',Valuta='" + cValuta.Replace("'", "") + "'," +
                "Kurs=" + cKurs + ",Operator='" + cOperator.Replace("'", "") + "',Rabatdef=" + cRabat + ",RjPart=1," +
                "Ugovor='" + cUgovor.Replace("'", "") + "',Narudzba='" + cNarudzba.Replace("'", "") + "',Rok=" + cRok + ",Pdv_status=" + cPdvStatus + ",Placeno=" + mNacinPlacanja + " " +
                "Where Broj =" + cFaktura + "";
                 moduli.funkcije.SQLExecute(false, cSQL);
            }

            string csqlupd = "UPDATE " + cDbTabela1 + " SET " +
            "RabatP =" + cRabat + "  where Broj=" + txtFaktura.Text + ";";
            moduli.funkcije.SQLExecute(false, csqlupd);
           

            csqlupd = "UPDATE " + cDbTabela1 + " SET " +
            "RabatI = Round(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina])*([fak_fakstav]![RabatP]/100),2), " +
            "IznosF = Round( " +
            "([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) - " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) * ([fak_fakstav]![RabatP]/100)),2), " +
            "Iznos = Round ( " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) - " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) * ([fak_fakstav]![RabatP]/100))) / " +
            "(1 + ([fak_fakstav]![PorezP]/100)),2)," +
            "PorezI = Round (  " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) - " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) * ([fak_fakstav]![RabatP]/100))) - " +
            "((([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) - " +
            "(([fak_fakstav]![Cijena]*[fak_fakstav]![Kolicina]) * ([fak_fakstav]![RabatP]/100))) / " +
            "(1 + ([fak_fakstav]![PorezP]/100))),2) " +
            "where Broj=" + txtFaktura.Text + ";";

            moduli.funkcije.SQLExecute(false,csqlupd);

            popunigrid("", "", "order by  fak_fakstav.Broj, fak_fakstav.StavID DESC");

            bMODUSFORMEDODAVANJE=false ;
            
            txtSearch.Focus();

        }

        private void refreshMaticnuFormu(bool bMODUSFORMEREFRESHZATVARANJEDODAVANJEl)
        {
            frmFakture.Instance.popunigrid("", "", "");
            if (bMODUSFORMEREFRESHZATVARANJEDODAVANJEl==false) //radi pozicioniranje samo ako je edit
            {
                frmFakture.Instance.pozicionirajRed(int.Parse(txtFaktura.Text));
            }
            
        
        }

        private void btnUpisiZag_Enter(object sender, EventArgs e)
        {
            //foreach control - metoda
            //TextBox tbkontrola;
            //bool isIncomplete = false;
            //foreach (Control control in this.DetaljiBox.Controls)
            //{
            //    if (control is TextBox)
            //    {
            //        TextBox tb = control as TextBox;
            //        if (string.IsNullOrEmpty(tb.Text))
            //        {
            //            tbkontrola = tb;
            //            tbkontrola.Name=tb.Name;
            //            isIncomplete = true;
            //            MessageBox.Show("Vrijednost mora biti popunjena !");
            //            tb.Focus();
            //            break;
            //        }
            //    }
            //}


            //http://stackoverflow.com/questions/9450222/checking-multiple-textbox-if-theyre-null-or-whitespace
            // linq query - metoda (minimalno vs2008 i framework 3,5)


            var emptyTextboxes2 = from tb in groupBox2.Controls.OfType<TextBox>()
                                  where string.IsNullOrEmpty(tb.Text)
                                  select tb;

            if (emptyTextboxes2.Any())
            {
                foreach (var textBox2 in emptyTextboxes2)
                {
                    if (textBox2.Name != "txtID")
                    {
                        MessageBox.Show("Vrijednost je obavezna za unos. Popunite polje " + textBox2.Name + " !");
                        textBox2.Focus();
                        break;
                    }
                }
            }
        }

        private void preracunaj() {
            double cKol=0;
            double cCijenaBP = 0; double cCijena = 0;
            double cPorezP = 0; double cPorezI = 0;
            double cRabatP = 0; double cRabatI = 0;
            double cIznosBP = 0; double cUkupno = 0;

            cKol =double.Parse (txtKolicina.Text);
            cCijenaBP=double.Parse (txtCijenaBP.Text);
            cCijena = double.Parse(txtCijena.Text);
            cPorezP = double.Parse(txtTBIme.Text);
            cPorezI = double.Parse(txtIznosPdv.Text);
            cRabatP = double.Parse(txtRabatP.Text);
            cRabatI = double.Parse(txtRabatI.Text);
            cIznosBP = double.Parse(txtOsnovica.Text);
            cUkupno = double.Parse(txtIznos.Text);



            cRabatI = Math.Round((cCijena * cKol) * (cRabatP / 100), 2);
            //cPorezI = Math.Round(((cCijena * cKol) / (1 + (cPorezP / 100))) - cRabatI, 2);
            cUkupno = Math.Round((cCijena * cKol) - ((cCijena * cKol) * (cRabatP / 100)), 2);
            cIznosBP = Math.Round(cUkupno / (1 + (cPorezP / 100)), 2);
            cPorezI = cUkupno - cIznosBP;
            //cUkupno = Math.Round(cIznosBP + cPorezI, 2);

            txtKolicina.Text = cKol.ToString();
            txtIznosPdv.Text = cPorezI.ToString();
            txtRabatI.Text = cRabatI.ToString();
            txtOsnovica.Text = cIznosBP.ToString();
            txtIznos.Text = cUkupno.ToString();
        }

        private void txtCijenaBP_TextChanged(object sender, EventArgs e)
        {
            if (txtCijenaBP.Focused)
            {
                double cCijenaBP = double.Parse(txtCijenaBP.Text);
                double cPorezP = double.Parse(txtTBIme.Text);
                double cCijena = Math.Round(cCijenaBP * (1+(cPorezP /100)),2);
                txtCijena.Text = cCijena.ToString("#######0.00");
                preracunaj();
            }
        }

        private void txtCijena_TextChanged(object sender, EventArgs e)
        {
            if (txtCijena.Focused)
            {
                double cCijena = double.Parse(txtCijena.Text);
                double cPorezP = double.Parse(txtTBIme.Text);
                double cCijenaBP = Math.Round(cCijena/ (1 + (cPorezP / 100)), 2);
                txtCijenaBP.Text = cCijenaBP.ToString("#######0.00");
                preracunaj();
            }
        }

        private void txtKolicina_TextChanged(object sender, EventArgs e)
        {
            preracunaj();
        }
  
        private void txtDokument_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtVrsta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnProknjizi_Click(object sender, EventArgs e)
        {

        }

        private void btnKnjizi_Click(object sender, EventArgs e)
        {
            if (chkZakljuceno.Checked == true)
            {
                MessageBox.Show("Faktura je zaključena. Promjene nisu dozvoljene.", "Upozorenje !");
                return; 
            }

            if (grid1.Rows.Count == 0)
            { 
                MessageBox.Show ("Ne možete zaključiti fakturu bez stavki. Operacija je obustavljena.");
                return;
            }
            if (MessageBox.Show("Da li zaista želite zaključiti fakturu !", "Upozorenje !", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Knjizenje();
                // ************************************** FISKALNO *********************************************************
                if (moduli.variable.cFiskalnaKasa == true)
                {
                    moduli.variable.bFiskalnaGreska = false;
                    moduli.variable.bFiskalnaGreskaNijeVratioBroj  = false;

                    moduli.fiskal.fiskalsinhronizirajrobuizracunaDatecsTxt(long.Parse(txtFaktura.Text),cDbTabela1);
                    moduli.fiskal.fiskalniracun(long.Parse(txtFaktura.Text), cDbTabela1,cDbTabela);

                    if (moduli.variable.bFiskalnaGreska == true)
                    {
                        moduli.variable.bFiskalnaGreska = false;
                        if (moduli.variable.bFiskalnaGreskaNijeVratioBroj == true)
                        {
                            moduli.variable.bFiskalnaGreskaNijeVratioBroj = false;
                            MessageBox.Show("Odgovor printera nije pronadjen. Unesite broj fiskalnog računa. Ukoliko odustanete" + 
                            " račun neće imati parametar koji ga povezuje sa fiskalnim računom.","Upozorenje!");

                            pokaziFiskal(true);
                            txtUnesiFiskalniBroj.Focus();

                        }


                    }
                }
                // *********************************************************************************************************
            
            }
        }

        private void Knjizenje()
        {
            string cOsn = moduli.funkcije.MySqlBr(txtOsnovicaTotal.Text);
            string cPor = moduli.funkcije.MySqlBr(txtPDVTotal.Text);
            string cRab = moduli.funkcije.MySqlBr(CTotalRabat.ToString("######0.00"));
            string cFak = moduli.funkcije.MySqlBr(txtUkupnoTotal.Text);

            string cKom = "update " + cDbTabela + " set Zakljuceno=1, Iznos=" + cOsn + ",Porezi=" + cPor + "," +
                "Rabati=" + cRab + ",Iznosf=" + cFak + " where Broj=" + txtFaktura.Text + "";

            moduli.funkcije.SQLExecute(false, cKom);
            chkZakljuceno.Checked = true;
        
        }

        #region "popunicombo"

        private void displaycombo(bool prikazi) //pozovi sa loada
        {
            if (prikazi)
            {
                try
                {
                    int cCmbIzbor = int.Parse (txtFaktura.Text);
                    string cCm = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, cDbTabela, "PLACENO", "Broj=" + cCmbIzbor + "");
                    if (cCm == "nema trazene vrijednosti" || cCm =="0")
                    { cmbPlacanje.SelectedValue = "1"; }
                    else { cmbPlacanje.SelectedValue = cCm; }
                }
                catch (Exception e) { }
            }

        }

        private void popunicombo(bool IzBaze)
        {
            if (IzBaze == true)
            { }
            else
            {

                var dataSource = new List<placanje>();
                dataSource.Add(new placanje() { Name = "Gotovina", Value = "1" });
                dataSource.Add(new placanje() { Name = "Kartica", Value = "2" });
                dataSource.Add(new placanje() { Name = "Kredit", Value = "3" });
                dataSource.Add(new placanje() { Name = "Žiralno", Value = "4" });
                dataSource.Add(new placanje() { Name = "Faktura", Value = "5" });

                this.cmbPlacanje.DataSource = dataSource;
                this.cmbPlacanje.DisplayMember = "Name";
                this.cmbPlacanje.ValueMember = "Value";
                this.cmbPlacanje.DropDownStyle = ComboBoxStyle.DropDownList;
            }

        }

        public class placanje
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private void cmbPlacanje_SelectedValueChanged(object sender, EventArgs e)
        {
            mNacinPlacanja = int.Parse (cmbPlacanje.SelectedValue.ToString());
            Console.WriteLine(cmbPlacanje.SelectedValue.ToString());
        }

        #endregion

        #region " ------ unos fiskalnog broja ------ "

        private void button3_Click(object sender, EventArgs e)
        {
            txtUnesiFiskalniBroj.Text = "";
            pokaziFiskal(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtUnesiFiskalniBroj.Text = "";
            pokaziFiskal(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtUnesiFiskalniBroj.Text =="")return ;
            int cFBroj = int.Parse(txtUnesiFiskalniBroj.Text);

            string cInfo = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? 
                moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, 
                cDbTabela , "Broj", "FiskalniRacun=" + cFBroj + "");

            if (cInfo != "nema trazene vrijednosti")
            {
                if (cInfo != "")
                {
                    MessageBox.Show("Faktura broj "+cInfo +" ima ovaj fiskalni broj. Promijenite broj fiskalnog računa.");
                    txtUnesiFiskalniBroj.Text = ""; txtUnesiFiskalniBroj.Focus();
                    return;
                }
            }
            else
            {
                string cc = "update "+cDbTabela+" set FiskalniRacun ="+cFBroj+" where broj="+ int.Parse(txtFaktura.Text)+"";
                moduli.funkcije.SQLExecute(false,cc);
                pokaziFiskal(false); SendKeys.Send("{ESC}");
            }

        }
        
        private void txtUnesiFiskalniBroj_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        


        #endregion







        #endregion




    }

}

