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
using System.DirectoryServices;

namespace Poslovanje.forme
{
    public partial class frmFirme : Form
    {

        OdbcDataAdapter updtelapt; OdbcDataAdapter updbrcdapt; OdbcCommand cSqlTIn; OdbcCommand cSqlTUp; OdbcCommand cSqlTDel;
        DataSet upteldataset; 
        OdbcCommandBuilder cmd;
        DataTable tbTelefoni = new DataTable(); DataTable tbziroracuni = new DataTable();
        public OdbcDataAdapter ad=new OdbcDataAdapter();
        public DataTable table=new DataTable();

#region "REGIJA otvaranje i pretraga uobicajeno na svakoj formi sa minimalnim promjenama ";
        public static string cIzabranaSifra;
        OdbcDataAdapter sqlDataAdapter;
        //Dim sqlDataAdapterNaziv As OdbcDataAdapter
        public static DataSet dataset;
        public static string cNaziv;
        public static string cKolona;
        //Public dw As DataView
        //Public dwTrenutniRed As DataRowView
        public static DataSet dss;
        public bool bLookUpForma;
        int SortColumn;
        bool bUkljuciDetalje;
        public static string cDbTabela;
        public bool Dragging ;
        public int mousex;
        public int mousey;
        public static int cRecordCount;
        public bool bPrviPut = true;
        public static bool bIzSetupaLoco;

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }
        public static modus_rada modus;

        public frmFirme()
        {
            bIzSetupaLoco = true;//moduli.variable.bIzSetupa;
            InitializeComponent();
            txtSearch.Focus();

        }

        private void frmFirme_KeyDown(object sender, KeyEventArgs e)
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
                else { Dispose(); }
            }

            if (e.KeyCode == Keys.Insert) { if (modus == modus_rada.pregled) { btnDodavanje_Click(this, e); } }
            if (e.KeyCode == Keys.End) { if (modus == modus_rada.pregled) { btnIzmjena_Click(this, e); } }
            if (e.KeyCode == Keys.Delete) { if (modus == modus_rada.pregled) { btnBrisanje_Click(this, e); } }
            if (e.KeyCode == Keys.Home) { if (modus == modus_rada.pregled) { btnDetalji_Click(this, e); } }
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) grid1.Focus();
            if (e.KeyCode == Keys.Enter) grid1.Focus();
        }

        private void frmFirme_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void frmFirme_Load(object sender, EventArgs e)
        {
            
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead,"");  //čitam posljednju poziciju

            cDbTabela = "firme";
            SortColumn = 1;
            cKolona = grid1.Columns[SortColumn].Name;
            bUkljuciDetalje = false;
            ukljucidetalje(bUkljuciDetalje);
            modus = modus_rada.pregled;

            //PrikaziSliku(pnlSlika,this);
            moduli.kon.osnovnakonektuj();

            cmbPosJedinica.Text = "1";
            cmbGodina.Text = DateTime.Now.Year.ToString();

            popunigrid("", "", "order by rb ASC");
            kreirajtmptelefone();
            resize();
            DetaljiBox.Enabled = false;

            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.txtImeBaze, "Ako je baza u Accessu onda ime baze treba biti Godina_dbSifra.mdb ... npr. 2013_db00001.mdb, u protivnom program neće funkcionisati.");

        }

        private void frmFirme_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cText;
                

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
                    cKolona = cKolona = grid1.Columns[SortColumn].Name;
                    //switch (SortColumn)
                    //{
                    //    case 0:
                    //        cKolona = "sifra";
                    //        break;
                    //    case 1:
                    //        cKolona = "naziv";
                    //        break;
                    //}
                    //dss = pretraga("where " & cKolona & " like '%" & cNaziv & "%'")

                }
                popunigrid(cKolona, cNaziv, "order by rb ASC");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Greška u odabiru karaktera !");
                txtSearch.Text = "";
            }

        }

        private void popunigrid(string cKolona, string cNaziv, string cOrder)
        {
            try
            {
                DataSet dss = new DataSet(); //ili dss.Dispose()
                if (cNaziv == "")
                {
                    dss = pretraga(cNaziv, cOrder);
                }
                else
                {
                    dss = pretraga("where " + cKolona + " like '%" + cNaziv + "%'", cOrder);
                }


                cRecordCount = dss.Tables[cDbTabela].Rows.Count;
                this.grid1.DataSource = dss.Tables[cDbTabela];

                ispraznikontrole();
                displaydata();
                displaytelefone(lblMoveDetalje.Visible == false ? false : true,"");
                displayziroracune(lblMoveDetalje.Visible == false ? false : true, "");

                grid1.AutoGenerateColumns = false;
                int i;

                for (i = 2; i < this.grid1.Columns.Count - 1; i++)
                {
                    //grid1.Columns.RemoveAt(2);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), "Parametri nisu pravilno uneseni !");
            }
        }

        public static DataSet pretraga(string uslov, string cOrder)
        {
            try
            {
                OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter("select * from " + cDbTabela + " " + uslov + " " + cOrder + " ", moduli.variable.mKonekcijaOsnovna);
                dataset = new DataSet();
                sqlDataAdapter.Fill(dataset, cDbTabela);
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
            //ukljucidetalje(bUkljuciDetalje);
            //pronadjired(sender);
            displaytelefone(lblMoveDetalje.Visible == false ? false : true,"");
            displayziroracune(lblMoveDetalje.Visible == false ? false : true, "");
        }

        private void ukljucidetalje(bool bUkljuciDetalje)
        {
            if (bUkljuciDetalje == false)
            {

                lblMoveDetalje.Visible = false;
            }
            else
            {
                try
                {
                    if (modus != modus_rada.dodavanje)
                    {

                    }

                    lblMoveDetalje.Visible = true;
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
 
        private void blokirajkontrole(Boolean upali){
        switch (upali){
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
        
        private void txtSifraFirme_Leave(object sender, EventArgs e) //nisam naso lost fcous
        {
            
            if (modus == modus_rada.izmjena) {return;}
            if (modus == modus_rada.pregled) {return;}
            string a;
            a = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna, cDbTabela, "sifra", "sifra='" + txtSifraFirme.Text + "'");
            if (a != "nema trazene vrijednosti")
            {
                MessageBox.Show("U bazi postoji vrijednost sa ovom šifrom !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifraFirme.Text = "";
                txtSifraFirme.Focus();
            }

            if (txtSifraFirme.Text == "")
            {
                MessageBox.Show("Vrijednost ne može biti prazna !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifraFirme.Text = "";
                txtSifraFirme.Focus();
            }
            if (txtSifraFirme.Text != "") {
                cIzabranaSifra = txtSifraFirme.Text;

                string cCreateSql = "";
                OdbcCommand cCreateSqlComm = new OdbcCommand(cCreateSql, moduli.variable.mKonekcijaOsnovnaKomandna);

                cCreateSql = "insert into telefoni (sifra,telefon,vrsta) " + 
                            "values " +
                            "('" + cIzabranaSifra.Replace( "'", "") + "','--','0')";

                try
                {
                    moduli.kon.osnovnakomandnakonektuj();
                    cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();
                    moduli.variable.mKonekcijaOsnovnaKomandna.Close();
                }
                catch (Exception exx) { MessageBox.Show("Greška : " + exx.ToString()); }


                displaytelefone(true, cIzabranaSifra);
        }
        }
   
        private void btnDodavanje_Click(object sender, EventArgs e)
        {
            modus = modus_rada.dodavanje;
            cIzabranaSifra = "";
            ispraznikontrole();
            blokirajkontrole(true);
            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            displaytelefone(false,"");
            txtSifraFirme.Focus();
        }

        private void btnIzmjena_Click(object sender, EventArgs e)
        {
            if (grid1.Rows.Count == 0) return;
            modus = modus_rada.izmjena;
            blokirajkontrole(true);
            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            displaytelefone(lblMoveDetalje.Visible == false ? false : true,"");
            displayziroracune(lblMoveDetalje.Visible == false ? false : true, "");
            omogucisifre(false);
            txtImeFirme.Focus();
        }

        private void btnOdustaniOdDetalja_Click(object sender, EventArgs e)
        {
            switch (modus){
            case modus_rada.izmjena:
                modus = modus_rada.pregled;
                popunigrid("", "", "order by rb ASC");
                omogucisifre(true);
                break;
            case modus_rada.dodavanje:
                modus = modus_rada.pregled;
                popunigrid("", "", "order by rb ASC");
                break;
                }
                
                blokirajkontrole(false);

                bUkljuciDetalje = true;
                btnDetalji_Click(this, e);

        }

        private void btnUpisi_Click(object sender, EventArgs e)
        {
                string cCreateSql = "";
                OdbcCommand cCreateSqlComm = new OdbcCommand(cCreateSql,moduli.variable.mKonekcijaOsnovnaKomandna);
        
                string cSifFirme=txtSifraFirme.Text;
                string cImeFirme=txtImeFirme.Text;
                string cAdresa=txtAdresa.Text;
                string cPoreskiBr=txtPoreskiBroj.Text;
                string cOpstina=txtOpstina.Text;
                string cIdBroj=txtIdBroj.Text;
                string cSud=txtSud.Text;
                string cPdvBr=txtPDvBroj.Text;
                string cLogo=txtLogo.Text;
                string cSablon=txtSabloni.Text;
                string cIOSA=txtIOSA.Text;
                string cPutanjaAccBaza = txtPutanjaDoBaze.Text;
                string cImeAccBAze = txtImeBaze.Text;



                string cPrinter = txtPrinter.Text;
                string cPort = txtPort.Text;
                string cLokFisFajl = txtLokFisFajl.Text;
                int cDelay = int.Parse ( txtFisDelay.Text);

                switch (modus) {
                    case modus_rada.dodavanje:
                        updatetelefone();
                        updateziroracuni();
                        insertUIpodesenja(cSifFirme.Replace("'", ""));
                        int cBr = moduli.funkcije.RetNextValue("rb", cDbTabela, "", moduli.variable.mKonekcijaOsnovna);
                        if (cBr == 0) break;
                        cCreateSql = "insert into " + cDbTabela + " (sifra,naziv,prodavnica,nazivprod,poreskibroj,opstina," +
                            "idbroj,sudskibroj,pdvbroj,pdvobaveza,logo,sabloni,fiskalnakasa,iosa,fiskalnafakturaproizvoda,arhivirajdnevnik,rb," +
                            "printer,port,lokfisfajla,fdelay,fdatecs,ftremol,ftremolesencijalno,fmikroelektronika,minusstanje,putanja,baza,fiskalnafakturagk )" +
                            "values " +
                            "('" + cSifFirme.Replace("'", "") + "','" + cImeFirme.Replace("'", "") + "'," +
                            "" + cmbPosJedinica.Text + ",'" + cAdresa.Replace("'", "") + "'," +
                            "'" + cPoreskiBr.Replace("'", "") + "','" + cOpstina.Replace("'", "") + "'," +
                            "'" + cIdBroj.Replace("'", "") + "','" + cSud.Replace("'", "") + "'," +
                            "'" + cPdvBr.Replace("'", "") + "'," + (chkPdvObaveza.Checked == true ? 1 : 0) + "," +
                            "'" + cLogo.Replace("'", "") + "','" + cSablon.Replace("'", "") + "'," +
                            "" + (chkFiskalnaKasa.Checked == true ? 1 : 0) + ",'" + cIOSA.Replace("'", "") + "'," +
                            "" + (chkFisFakturisanje.Checked == true ? 1 : 0) + "," + (chkFakArhDnevnik.Checked == true ? 1 : 0) + "," + cBr + "," +
                            "'" + cPrinter.Replace("'", "") + "','" + cPort.Replace("'", "") + "','" + cLokFisFajl.Replace("'", "") + "'," + cDelay + "," +
                            "" + (this.chkFiskalDatecs.Checked == true ? 1 : 0) + "," + (this.chkTremol.Checked == true ? 1 : 0) + "," +
                            "" + (this.chkTremolEsencijalan.Checked == true ? 1 : 0) + "," + (this.chkMikroelektronika.Checked == true ? 1 : 0) + "," + (this.chkMinusStanje.Checked == true ? 1 : 0) + "," +
                            "'" + cPutanjaAccBaza.Replace("'", "") + "','" + cImeAccBAze.Replace("'", "") + "'," + (this.chkFisFakturaGK.Checked == true ? 1 : 0) + ")";

                        try
                        {
                            moduli.kon.osnovnakomandnakonektuj();
                            cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;
                            cCreateSqlComm.CommandText = cCreateSql;
                            cCreateSqlComm.ExecuteNonQuery();
                            moduli.variable.mKonekcijaOsnovnaKomandna.Close();
                            // grid1.Item(0, 4).Selected = True;
                            modus = modus_rada.pregled;
                            popunigrid("", "", "order by rb DESC");
                        }
                        catch (Exception a) { MessageBox.Show(a.ToString()); }
                        break;

               
                    case modus_rada.izmjena:
                        updatetelefone();
                        updateziroracuni();
                        cCreateSql = "update " + cDbTabela + " set naziv='" + cImeFirme.Replace( "'", "") + "'," +
                            "prodavnica=" + cmbPosJedinica.Text + ",nazivprod='" + cAdresa.Replace ("'", "") + "'," +
                            "poreskibroj='" + cPoreskiBr.Replace("'", "") + "',opstina='" + cOpstina.Replace("'", "") + "'," +
                            "idbroj='" + cIdBroj.Replace("'", "") + "',sudskibroj='" + cSud.Replace("'", "") + "'," +
                            "pdvbroj='" + cPdvBr.Replace("'", "") + "',pdvobaveza=" + (chkPdvObaveza.Checked == true ? 1 : 0) + "," +
                            "logo='" + cLogo.Replace("'", "") + "',sabloni='" + cSablon.Replace("'", "") + "'," +
                            "fiskalnakasa=" + (chkFiskalnaKasa.Checked == true ? 1 : 0) + ",iosa='" + cIOSA.Replace ("'", "") + "'," +
                            "fiskalnafakturaproizvoda=" + (chkFisFakturisanje.Checked ==true ? 1 : 0) + ",arhivirajdnevnik=" + (chkFakArhDnevnik.Checked == true ? 1 : 0) + "," +
                            "printer='" + cPrinter.Replace("'", "") + "',port='" + cPort.Replace("'", "") + "',lokfisfajla='" + cLokFisFajl.Replace("'", "") + "'," +
                            "fdelay=" + cDelay + ",fdatecs=" + (this.chkFiskalDatecs.Checked == true ? 1 : 0) + ",ftremol=" + (this.chkTremol.Checked == true ? 1 : 0) + "," +
                            "ftremolesencijalno=" + (this.chkTremolEsencijalan.Checked == true ? 1 : 0) + ",fmikroelektronika=" + (this.chkMikroelektronika.Checked == true ? 1 : 0) + ",minusstanje=" + (this.chkMinusStanje.Checked == true ? 1 : 0) + ", " +
                            "putanja='" + cPutanjaAccBaza.Replace("'", "") + "',baza='" + cImeAccBAze.Replace("'", "") + "',fiskalnafakturagk=" + (this.chkFisFakturaGK.Checked == true ? 1 : 0) + " " + 
                            "where sifra='" + cSifFirme.Replace("'", "") + "'";

                        try
                        {
                            moduli.kon.osnovnakomandnakonektuj();
                            cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;
                            cCreateSqlComm.CommandText = cCreateSql;
                            cCreateSqlComm.ExecuteNonQuery();
                            moduli.variable.mKonekcijaOsnovnaKomandna.Close();
                            
                            moduli.variable.cEditVrijednostString = txtSifraFirme.Text;
                            cIzabranaSifra = moduli.variable.cEditVrijednostString; 
                            modus = modus_rada.pregled;
                            omogucisifre(true);
                            //popunigrid("", "", "order by rb ASC");

                            //int cred;
                            //DataSet dsp = new DataSet();
                            
                            //dsp = pretraga("", "order by rb ASC");
                            //for (cred = 0; cred < dsp.Tables[cDbTabela].Rows.Count; cred++)
                            //{
                            //    string cDspSif = dsp.Tables[cDbTabela].Rows[cred][0].ToString();
                            //    if (cDspSif == moduli.variable.cEditVrijednostString) 
                            //    {
                            //        grid1.Rows[cred].Selected=true;
                            //        DataRow crow;
                            //        crow = dsp.Tables[cDbTabela].Rows[cred];
                            //        displaydata(crow); 
                            //        break; //pošto je našao row neka izadje
                            //    }
                            //}
                            ispraznikontrole(); displaydata(); grid1.Refresh();
                            cIzabranaSifra = moduli.variable.cEditVrijednostString;  // ovo mi sjebe izabranu šifru kod brisanja pa sam odradio sljedeće
                            //pronadjired(grid1);
                            displaytelefone(true,"");
                            displayziroracune(true, "");
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
        }
  

        private void btnBrisanje_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Da li zaista želite obrisati stavku !", "Upozorenje !", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {

                if (grid1.Rows.Count == 0) return;
                try
                {


                    string cCreateSql = "";
                    OdbcCommand cCreateSqlComm = new OdbcCommand();
                    OdbcCommand cCreateSqlCommTel = new OdbcCommand();
                    string cSifFirme = txtSifraFirme.Text;
                    cCreateSql = "delete from " + cDbTabela + " where sifra='" + cSifFirme + "' ";

                    moduli.kon.osnovnakomandnakonektuj();
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;

                    cCreateSqlComm.ExecuteNonQuery();


                    cCreateSql = "delete from telefoni where sifra='" + cSifFirme + "' ";
                    cCreateSqlCommTel.CommandText = cCreateSql;
                    cCreateSqlCommTel.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;

                    cCreateSqlCommTel.ExecuteNonQuery();

                    cCreateSql = "delete from ziroracuni where sifra='" + cSifFirme + "' ";
                    cCreateSqlCommTel.CommandText = cCreateSql;
                    cCreateSqlCommTel.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;

                    cCreateSqlCommTel.ExecuteNonQuery();

                    cCreateSql = "delete from uipodesenja where sifra='" + cSifFirme + "' ";
                    cCreateSqlCommTel.CommandText = cCreateSql;
                    cCreateSqlCommTel.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;

                    cCreateSqlCommTel.ExecuteNonQuery();

                    moduli.variable.mKonekcijaOsnovnaKomandna.Close();
                    popunigrid("", "", "order by rb ASC");
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
                grid1.Focus();
            }
            else
            {
                bUkljuciDetalje = true;
            }
            ukljucidetalje(bUkljuciDetalje);
            displaytelefone(lblMoveDetalje.Visible == false ? false : true,"");
            displayziroracune(lblMoveDetalje.Visible == false ? false : true, "");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            frmSetup a = new frmSetup();
            a.Show();
            /*DialogResult dlgResult = MessageBox.Show("Da li da štampa bude iz text fielda ?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes) { frmStampaFirme a = new frmStampaFirme("Txt"); a.Show(this); }
            else { frmStampaFirme a = new frmStampaFirme("DataSet"); a.Show(this); }
            */


        }

 
        private void btnOdustani_Click(object sender, EventArgs e)
        {
            //obrisitmptelefone();
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
            Dispose();
        }

        private void lblMoveDetalje_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
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
          if (Dragging) {
                //move control to new position
                Point MPosition= new Point();
                MPosition = PointToClient(MousePosition);
                MPosition.Offset(mousex, mousey);
                //ensure control cannot leave container
                lblMoveDetalje.Location = MPosition;
          }
        }

        private void lblMoveDetalje_MouseUp(object sender, MouseEventArgs e)
        {
            if (Dragging) {
                //end the dragging
                Dragging = false;
                Cursor.Clip = new Rectangle();

                lblMoveDetalje.Invalidate();
            }
        }

        private void btnCloseDetalje_Click(object sender, EventArgs e)
        {
            btnOdustaniOdDetalja_Click(this, e);
            ukljucidetalje(false); 
            bUkljuciDetalje = true;
            btnDetalji_Click(this, e);

            
        }

        #region "REGIJA LookUp opština"

        private void txtOpstina_TextChanged(object sender, EventArgs e)
        {
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == false ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcijaOsnovna, "opstine", "naziv", "sifra='" + txtOpstina.Text + "'");
            
            if (modus != modus_rada.pregled)
            {
                if (a != "nema trazene vrijednosti")
                { txtOpstinaIme.Text = a; }
                else
                {
                    txtOpstinaIme.Text = "";
                }
            }
            else {txtOpstinaIme.Text = a; }
        }
       
        private void btnOpstinaLookUp_Click(object sender, EventArgs e)
        {
            moduli.variable.bIzSetupa = true;
            frmOpstine a = new frmOpstine(true,bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUp();
            //'frmOpstine.bLookUpForma = False
            moduli.variable.bIzSetupa = true;
            txtOpstina.Focus();
        }

        private void  popuniLookUp() {

            if (moduli.variable.cLookUpSifra != "") {
                txtOpstina.Text = moduli.variable.cLookUpSifra;
                txtOpstinaIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                if (txtOpstina.Text == "") {
                    txtOpstina.Text = "*";
                }
            }

           }

        private void txtOpstina_Validated(object sender, EventArgs e)
        {
            if (txtOpstina.Text == "" || txtOpstinaIme.Text == "")
            {
              btnOpstinaLookUp_Click(this, e);
          }
        }

        #endregion

#endregion


        #region "telefoni"

        private void kreirajtmptelefone() { 
            return;
        //string cCreateSql="" ;
 
        //cCreateSql = "CREATE TABLE  " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." :"" ) + "`tmptiptel` (" +
        //            "`tip` INT NOT NULL ," +
        //            "`ime` VARCHAR( 100 ) NOT NULL" +
        //            ") " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? "ENGINE = MYISAM ;" : "") + "";
      
        //OdbcCommand cCreateSqlComm = new OdbcCommand(cCreateSql);
        
        //moduli.kon.osnovnakomandnakonektuj();
        //cCreateSqlComm.Connection=moduli.variable.mKonekcijaOsnovnaKomandna;
        //try{
     
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();

        //    cCreateSql = "INSERT INTO   " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "`tmptiptel` " +
        //                 "(`tip` ,`ime`) VALUES ('0',  'Telefon');";
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();


        //    cCreateSql = "INSERT INTO   " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "`tmptiptel` " +
        //                 "(`tip` ,`ime`) VALUES  ('1',  'Mobitel');";
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();


        //    moduli.variable.mKonekcijaOsnovnaKomandna.Close();
        //} catch (Exception e) {
        //    cCreateSql = "DROP TABLE " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "tmptiptel";
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();

        //    cCreateSql = "CREATE TABLE  " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "`tmptiptel` (" +
        //                "`tip` INT NOT NULL ," +
        //                "`ime` VARCHAR( 100 ) NOT NULL" +
        //                ") " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? "ENGINE = MYISAM ;" : "") + "";

        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();

        //    cCreateSql = "INSERT INTO " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "`tmptiptel` (" +
        //                 "`tip` ,`ime`)VALUES ('0',  'Telefon');";
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();

        //    cCreateSql = "INSERT INTO " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "`tmptiptel` (" +
        //     "`tip` ,`ime`)VALUES ('1',  'Mobitel');";
        //    cCreateSqlComm.CommandText = cCreateSql;
        //    cCreateSqlComm.ExecuteNonQuery();

            
        //    moduli.variable.mKonekcijaOsnovnaKomandna.Close();

       // }
        }

        private void obrisitmptelefone() {
            return;
            //try{
            //    moduli.kon.osnovnakomandnakonektuj();
            //    string cCreateSql = "DROP TABLE `tmptiptel`";
            //    OdbcCommand cCreateSqlComm = new OdbcCommand(cCreateSql);
            //    cCreateSqlComm.Connection=moduli.variable.mKonekcijaOsnovnaKomandna;
            //    cCreateSqlComm.CommandText = cCreateSql;
            //    cCreateSqlComm.ExecuteNonQuery();
            //    moduli.variable.mKonekcijaOsnovnaKomandna.Close();
            //    }catch (Exception e) {} 
        
        }

        private void displaytelefone(bool prikazi,string cNovaSifra) {
            if (prikazi)
            {
                updtelapt = null;
                cmd = null;
                tbTelefoni = null;
                try
                {
                    string cIzabranaSifra="";
                    string aa = cNovaSifra =="" ? cIzabranaSifra = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString():cIzabranaSifra = cNovaSifra;
                    updtelapt = new OdbcDataAdapter("select sifra ,telefon,vrsta,rb from telefoni where sifra= '" + cIzabranaSifra + "'", moduli.variable.mKonekcijaOsnovna);
                    cmd = new OdbcCommandBuilder(updtelapt);
                    tbTelefoni = new DataTable();

                    updtelapt.Fill(tbTelefoni);
                    gridTelefoni.DataSource = null;

                    gridTelefoni.DataSource = tbTelefoni;


                    try
                    {

                        string CSQL = "select * from tmptiptel ";
                        AddComboBoxColumns(2, "ime", "tip", "vrsta", CSQL);
                        gridTelefoni.Columns.Remove("vrsta");
                        gridTelefoni.Columns["sifra"].Visible = false;
                        gridTelefoni.Columns["rb"].Visible = false;
                        gridTelefoni.ForeColor = Color.Black;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                catch (Exception e) { }
            }
            else {gridTelefoni.DataSource = null;}
                
            
            
    }
       
        private void AddComboBoxColumns(int addAt , string cName , string cID , string cKolonaKojuPretvaramUCombo , string CQSL )
        {
            DataGridViewComboBoxColumn cboCol;
            cboCol = CreateComboBoxColumn(cName, cKolonaKojuPretvaramUCombo);
            SetAlternateChoicesUsingDataSource(cboCol, cName, cID, CQSL);
            cboCol.HeaderText = cName;
            gridTelefoni.Columns.Insert(addAt, cboCol);
        }
        
        private DataGridViewComboBoxColumn CreateComboBoxColumn(string cName , string sID ) 
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn() {DataPropertyName = sID,HeaderText = cName,DropDownWidth = 160,Width = 90,MaxDropDownItems = 5,FlatStyle = FlatStyle.Flat};
            /*
             * col.DataPropertyName = sID;
                col.HeaderText = cName;
                col.DropDownWidth = 160;
                col.Width = 90;
                col.MaxDropDownItems = 5;
                col.FlatStyle = FlatStyle.Flat;
            */
            
            return col;
        }

        private void SetAlternateChoicesUsingDataSource(DataGridViewComboBoxColumn cboColumn, string cName, string cID , string qry )
        {
            cboColumn.DataSource = null;
            cboColumn.DataSource = Populate(qry);
            cboColumn.DisplayMember = cName;
            cboColumn.ValueMember = cID;
        }
       
        private DataTable Populate(string sqlCommand) 
        {
            OdbcCommand command = new OdbcCommand(sqlCommand);
            command.Connection=moduli.variable.mKonekcijaOsnovna;


            table =  new DataTable();
            try{
                ad.SelectCommand = command;
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;

                ad.Fill(table);
            }catch (Exception ex){
                MessageBox.Show(ex.ToString());
            }
            return table;
        }
 
        private void updatetelefone()
        {
            DataTable tbEdit = (DataTable)(tbTelefoni.GetChanges(DataRowState.Modified));
            DataTable tbAdded  = (DataTable)(tbTelefoni.GetChanges(DataRowState.Added));
            DataTable tbDel = (DataTable)(tbTelefoni.GetChanges(DataRowState.Deleted));
            try
            {
                if (tbEdit != null) { updtelapt.Update(tbEdit); }
                if (tbAdded != null) { updtelapt.Update(tbAdded); }
                if (tbDel != null) { updtelapt.Update(tbDel); }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

        }

        private void gridTelefoni_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (modus ==modus_rada.pregled) return;
            if (modus == modus_rada.dodavanje) {
                if (txtSifraFirme.Text == "" ) return;
            }
            gridTelefoni.Rows[gridTelefoni.CurrentCell.RowIndex].Cells[0].Value = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
        }
 
        private void gridTelefoni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                int index = this.gridTelefoni.CurrentCell.RowIndex;
                gridTelefoni.Rows.Remove(gridTelefoni.CurrentRow);

            }

                
        }
   

        #endregion


        #region "ziroracuni"

        private void displayziroracune(bool prikazi,string cNovaSifra)
        {
          if (prikazi)
                {
                updbrcdapt = null;
                cmd = null;
                tbziroracuni = null;
                try
                {
                    string cIzabranaSifra = "";
                    string aa = cNovaSifra == "" ? cIzabranaSifra = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString() : cIzabranaSifra = cNovaSifra;
                    updbrcdapt = new OdbcDataAdapter("select  sifra,ziroracun,banka,rb from " + (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql ? moduli.variable.cNazivOsnovneBaze + "." : "") + "ziroracuni where sifra= '" + cIzabranaSifra + "'", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    cmd = new OdbcCommandBuilder(updbrcdapt);
                    tbziroracuni = new DataTable();

                    updbrcdapt.Fill(tbziroracuni);
                    gridziroracuni.DataSource = null;

                        gridziroracuni.DataSource = tbziroracuni;
               

                    try
                    {

                        //string CSQL = "select * from tmptiptel ";

                        //gridziroracuni.Columns.Remove("rb");
                        gridziroracuni.Columns["sifra"].Visible = false;
                        gridziroracuni.Columns["rb"].Visible = false;
                        gridziroracuni.ForeColor = Color.Black;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }

                catch (Exception e) {  }
             }
          else { gridziroracuni.DataSource = null; }
        }

        private void updateziroracuni()
        {
            DataTable tbEdit = (DataTable)(tbziroracuni.GetChanges(DataRowState.Modified));
            DataTable tbAdded = (DataTable)(tbziroracuni.GetChanges(DataRowState.Added));
            DataTable tbDel = (DataTable)(tbziroracuni.GetChanges(DataRowState.Deleted));
            try
            {
                if (tbEdit != null) { updbrcdapt.Update(tbEdit); }
                if (tbAdded != null) { updbrcdapt.Update(tbAdded); }
                if (tbDel != null) { updbrcdapt.Update(tbDel); }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

        }

        private void gridziroracuni_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (modus == modus_rada.pregled) return;

            if (txtSifraFirme.Text == "") return;

            gridziroracuni.Rows[gridziroracuni.CurrentCell.RowIndex].Cells[0].Value = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
        }

        private void gridziroracuni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                int index = this.gridziroracuni.CurrentCell.RowIndex;
                gridziroracuni.Rows.Remove(gridziroracuni.CurrentRow);

            }


        }

        #endregion

        #region "LOGO"

        private void btnLogoLookUp_Click(object sender, EventArgs e)
        {
            string c = Application.ExecutablePath + "/slike/";
            dlg.InitialDirectory = c;
            dlg.Filter = "jpeg (*.jpg)|*.jpg|gif(*.gif)|*.gif|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtLogo.Text = dlg.FileName.Replace ((char)92,(char)47);
                //MessageBox.Show(dlg.FileName)

            }

        } 

        #endregion


        private void insertUIpodesenja(string cFirma) {

            string cSQLstring = "insert into uipodesenja (sifra, formaslikazaglavlje, sirinaredatabele) values ('" + cFirma + "','-',18)";
            OdbcCommand cCreateSqlComm = new OdbcCommand();
            moduli.kon.osnovnakomandnakonektuj();
            cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna;
            cCreateSqlComm.CommandText = cSQLstring;
            cCreateSqlComm.ExecuteNonQuery();
            moduli.variable.mKonekcijaOsnovnaKomandna.Close();

        
        
        }


        private void btnOdaberi_Click(object sender, EventArgs e)
        {
            Odaberi();
        }


        private void grid1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Odaberi();

        }

        private void Odaberi()
        {
            if (cmbGodina.Text == "")
            {
                MessageBox.Show("Morate odrediti poslovnu godinu na koju se želite konektovati!");
                cmbGodina.Focus();
                return;
            }
            moduli.variable.cSifraFirme = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            moduli.variable.cNazivBaze = cmbGodina.Text + "_db" + grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString() ;
            popunivariable();
            if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Access) {
                 
                string cRetPut = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,"Firme","putanja","sifra='" + moduli.variable.cSifraFirme + "'" );
                moduli.variable.cPutanjaDoBaze  = cRetPut;
                moduli.variable.cNazivBaze = moduli.variable.cNazivBaze + ".mdb" ;
                
            }
            
            moduli.kon.konektuj();
            if (moduli.variable.cGreskaKonekcije == false)
            {

                frmLogin a = new frmLogin(false,false,"MALOPRODAJA  Godina " + cmbGodina.Text + " firma " + grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString() + " - " + grid1.Rows[grid1.CurrentCell.RowIndex].Cells[1].Value.ToString());
                moduli.variable.cGodina = cmbGodina.Text;
                               
                //a.Text = "MALOPRODAJA  Godina " + cmbGodina.Text + " firma " + grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString() + " - " + grid1.Rows[grid1.CurrentCell.RowIndex].Cells[1].Value.ToString();
                a.Show(); this.Hide();
            }
            else { MessageBox.Show("Postoji greška u kontektovanju na izabranu bazu.Provjerite godinu i firmu."); }
 

        }


        private void button1_Click(object sender, EventArgs e)
        {
            frmSetup a = new frmSetup();
            a.Show();

        }


        #region "############################################################################posebno na svakoj formi"

        private void omogucisifre(bool Upali)
        {
            txtSifraFirme.Enabled = false;
            if (Upali == true) { txtSifraFirme.Enabled = true; }

        }

        public void popunivariable (){
            
  
           try {

                //---------isprazni variable
                //moduli.variable.cSifraFirme = "";
                //moduli.variable.cNazivBaze = "";

                moduli.variable.nPdv = false;
                moduli.variable.cProdavnica = "";
                moduli.variable.cAdresa ="";
                moduli.variable.cPoreskiBroj = "";
                moduli.variable.cSudskiBroj = "";
                moduli.variable.cPdvBroj = "";
                moduli.variable.cIdBroj = "";
                moduli.variable.cOpstina = "";

                moduli.variable.cPrinterPort = "";
                moduli.variable.cPrinterName = "";

                moduli.variable.cIOSA = "";
                moduli.variable.cFiskalnaKasa =  false;
                moduli.variable.nFiskalnaFakturaProizvoda =  false;//proizvodnja
                moduli.variable.nFiskalnaFakturaGK = false;//glavna knjiga
                moduli.variable.nArhivirajDnevnik =  false;//arhiviranje dnevnika iz faktura proizvodnje

                moduli.variable.cFiskalDelay =0;//vrijeme odlaganja dobivanja odgovora od fis.printera
                moduli.variable.cPutanjaDoFiskalFajlova = "";
                moduli.variable.nDatecsTxt =  false;
                moduli.variable.nTremolXml = false;
                moduli.variable.nEsencijalniFisRacun =false;//samo esencijalni fiskalni račun
                moduli.variable.nMikroelektronikaInp = false;

                moduli.variable.bObavjestOminusuRobe = false;//obavijest o dopuštenom minusu
       
                //---------popuni variable
                //moduli.variable.cSifraFirme= row["sifra"].ToString();
                //moduli.variable.cNazivBaze= row["naziv"].ToString();

                moduli.variable.nPdv = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "pdvobaveza", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;// row["pdvobaveza"].ToString() == "1" ? true : false;
                moduli.variable.cProdavnica = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "prodavnica", "sifra='" + moduli.variable.cSifraFirme + "'");//row["prodavnica"].ToString();
                moduli.variable.cAdresa= moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "nazivprod", "sifra='" + moduli.variable.cSifraFirme + "'");//row["nazivprod"].ToString();
                moduli.variable.cPoreskiBroj = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "poreskibroj", "sifra='" + moduli.variable.cSifraFirme + "'");//row["poreskibroj"].ToString();
                moduli.variable.cSudskiBroj = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "sudksibroj", "sifra='" + moduli.variable.cSifraFirme + "'");//row["sudskibroj"].ToString();
                moduli.variable.cPdvBroj = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "pdvbroj", "sifra='" + moduli.variable.cSifraFirme + "'");//row["pdvbroj"].ToString();
                moduli.variable.cIdBroj = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "idbroj", "sifra='" + moduli.variable.cSifraFirme + "'");// row["idbroj"].ToString();
                moduli.variable.cOpstina = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "opstina", "sifra='" + moduli.variable.cSifraFirme + "'");//row["opstina"].ToString(); 

                moduli.variable.cPrinterPort = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "port", "sifra='" + moduli.variable.cSifraFirme + "'");//row["port"].ToString();
                moduli.variable.cPrinterName = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "printer", "sifra='" + moduli.variable.cSifraFirme + "'");//row["printer"].ToString();

                moduli.variable.cIOSA = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "iosa", "sifra='" + moduli.variable.cSifraFirme + "'");//row["iosa"].ToString();
                moduli.variable.cFiskalnaKasa = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "fiskalnakasa", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;///row["fiskalnakasa"].ToString()=="1"? true : false;
                moduli.variable.nFiskalnaFakturaProizvoda = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "fiskalnafakturaproizvoda", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;///row["fiskalnafakturaproizvoda"].ToString() == "1" ? true : false;//proizvodnja
                moduli.variable.nArhivirajDnevnik = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "arhivirajdnevnik", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//row["arhivirajdnevnik"].ToString() == "1" ? true : false;//arhiviranje dnevnika iz faktura proizvodnje

                moduli.variable.nFiskalnaFakturaGK = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                     cDbTabela, "fiskalnafakturagk", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//faktura glavne knjige

                moduli.variable.cFiskalDelay = long.Parse(moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "fdelay", "sifra='" + moduli.variable.cSifraFirme + "'")); //long.Parse (row["fdelay"].ToString());//vrijeme odlaganja dobivanja odgovora od fis.printera
                string cPutanja = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "lokfisfajla", "sifra='" + moduli.variable.cSifraFirme + "'");//row["lokfisfajla"].ToString();
                moduli.variable.cPutanjaDoFiskalFajlova = cPutanja.Replace("/","\\");
                moduli.variable.nDatecsTxt = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "fdatecs", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//row["fdatecs"].ToString() == "1" ? true : false;
                moduli.variable.nTremolXml = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "ftremol", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//row["ftremol"].ToString() == "1" ? true : false;
                moduli.variable.nEsencijalniFisRacun = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "ftremolesencijalno", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//row["ftremolesencijalno"].ToString() == "1" ? true : false;//samo esencijalni fiskalni račun
                moduli.variable.nMikroelektronikaInp = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "fmikroelektronika", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;//row["fmikrolektronika"].ToString() == "1" ? true : false;

                moduli.variable.bObavjestOminusuRobe = moduli.funkcije.RetFieldValue("string",moduli.variable.mKonekcijaOsnovna,
                                    cDbTabela, "minusstanje", "sifra='" + moduli.variable.cSifraFirme + "'") == "1" ? true : false;// row["minusstanje"].ToString() == "1" ? true : false;//obavijest o dopuštenom minusu
  
           }catch {}                       
        }

        private void displaydata()
        {
            try
            {

                txtSifraFirme.DataBindings.Add("Text", grid1.DataSource, "sifra", false, DataSourceUpdateMode.OnPropertyChanged);
                txtImeFirme.DataBindings.Add("Text", grid1.DataSource, "naziv", false, DataSourceUpdateMode.OnPropertyChanged);
                cmbPosJedinica.DataBindings.Add("Text", grid1.DataSource, "prodavnica", false, DataSourceUpdateMode.OnPropertyChanged);
                txtAdresa.DataBindings.Add("Text", grid1.DataSource, "nazivprod", false, DataSourceUpdateMode.OnPropertyChanged);
                txtPoreskiBroj.DataBindings.Add("Text", grid1.DataSource, "poreskibroj", false, DataSourceUpdateMode.OnPropertyChanged);
                txtOpstina.DataBindings.Add("Text", grid1.DataSource, "opstina", false, DataSourceUpdateMode.OnPropertyChanged);
                txtIdBroj.DataBindings.Add("Text", grid1.DataSource, "idbroj", false, DataSourceUpdateMode.OnPropertyChanged);
                txtSud.DataBindings.Add("Text", grid1.DataSource, "sudskibroj", false, DataSourceUpdateMode.OnPropertyChanged);
                txtPDvBroj.DataBindings.Add("Text", grid1.DataSource, "pdvbroj", false, DataSourceUpdateMode.OnPropertyChanged);
                txtLogo.DataBindings.Add("Text", grid1.DataSource, "logo", false, DataSourceUpdateMode.OnPropertyChanged);
                txtSabloni.DataBindings.Add("Text", grid1.DataSource, "sabloni", false, DataSourceUpdateMode.OnPropertyChanged);
                txtIOSA.DataBindings.Add("Text", grid1.DataSource, "iosa", false, DataSourceUpdateMode.OnPropertyChanged);
                txtPrinter.DataBindings.Add("Text", grid1.DataSource, "printer", false, DataSourceUpdateMode.OnPropertyChanged);
                txtPort.DataBindings.Add("Text", grid1.DataSource, "port", false, DataSourceUpdateMode.OnPropertyChanged);
                txtLokFisFajl.DataBindings.Add("Text", grid1.DataSource, "lokfisfajla", false, DataSourceUpdateMode.OnPropertyChanged);
                txtFisDelay.DataBindings.Add("Text", grid1.DataSource, "fdelay", false, DataSourceUpdateMode.OnPropertyChanged);
                txtImeBaze.DataBindings.Add("Text", grid1.DataSource, "baza", false, DataSourceUpdateMode.OnPropertyChanged);
                txtPutanjaDoBaze.DataBindings.Add("Text", grid1.DataSource, "putanja", false, DataSourceUpdateMode.OnPropertyChanged);
                chkPdvObaveza.DataBindings.Add("Checked", grid1.DataSource, "pdvobaveza", false, DataSourceUpdateMode.OnPropertyChanged);
                chkFiskalnaKasa.DataBindings.Add("Checked", grid1.DataSource, "fiskalnakasa", false, DataSourceUpdateMode.OnPropertyChanged);
                chkFisFakturisanje.DataBindings.Add("Checked", grid1.DataSource, "fiskalnafakturaproizvoda", false, DataSourceUpdateMode.OnPropertyChanged);
                chkFakArhDnevnik.DataBindings.Add("Checked", grid1.DataSource, "arhivirajdnevnik", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkFiskalDatecs.DataBindings.Add("Checked", grid1.DataSource, "fdatecs", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkTremol.DataBindings.Add("Checked", grid1.DataSource, "ftremol", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkTremolEsencijalan.DataBindings.Add("Checked", grid1.DataSource, "ftremolesencijalno", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkMikroelektronika.DataBindings.Add("Checked", grid1.DataSource, "fmikroelektronika", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkMinusStanje.DataBindings.Add("Checked", grid1.DataSource, "minusstanje", false, DataSourceUpdateMode.OnPropertyChanged);
                this.chkFisFakturaGK.DataBindings.Add("Checked", grid1.DataSource, "fiskalnafakturagk", false, DataSourceUpdateMode.OnPropertyChanged);
             
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString)
            }



        }

        private void ispraznikontrole()
        {
            txtSifraFirme.DataBindings.Clear();
            txtImeFirme.DataBindings.Clear();
            cmbPosJedinica.DataBindings.Clear();
            txtAdresa.DataBindings.Clear();
            txtPoreskiBroj.DataBindings.Clear();
            txtOpstina.DataBindings.Clear();
            txtIdBroj.DataBindings.Clear();
            txtSud.DataBindings.Clear();
            txtPDvBroj.DataBindings.Clear();
            txtLogo.DataBindings.Clear();
            txtSabloni.DataBindings.Clear();
            txtIOSA.DataBindings.Clear();
            txtPrinter.DataBindings.Clear();
            txtPort.DataBindings.Clear();
            txtLokFisFajl.DataBindings.Clear();
            txtFisDelay.DataBindings.Clear();
            txtImeBaze.DataBindings.Clear();
            txtPutanjaDoBaze.DataBindings.Clear();
            chkPdvObaveza.DataBindings.Clear();
            chkFiskalnaKasa.DataBindings.Clear();
            chkFisFakturisanje.DataBindings.Clear();
            chkFakArhDnevnik.DataBindings.Clear();
            this.chkFiskalDatecs.DataBindings.Clear();
            this.chkTremol.DataBindings.Clear();
            this.chkTremolEsencijalan.DataBindings.Clear();
            this.chkMikroelektronika.DataBindings.Clear();
            this.chkMinusStanje.DataBindings.Clear();
            this.chkFisFakturaGK.DataBindings.Clear();


            txtSifraFirme.Text = "";
            txtImeFirme.Text = "";
            cmbPosJedinica.Text = "";
            txtAdresa.Text = "";
            txtPoreskiBroj.Text = "";
            txtOpstina.Text = "";
            txtIdBroj.Text = "";
            txtSud.Text = "";
            txtPDvBroj.Text = "";
            txtLogo.Text = "";
            txtSabloni.Text = "";
            txtIOSA.Text = "";
            txtImeBaze.Text = "";
            txtPutanjaDoBaze.Text = "";
            chkPdvObaveza.Checked = false;
            chkFiskalnaKasa.Checked = false;
            chkFisFakturisanje.Checked = false;
            chkFakArhDnevnik.Checked = false;
            this.chkFiskalDatecs.Checked = false;
            this.chkTremol.Checked = false;
            this.chkTremolEsencijalan.Checked = false;
            this.chkMikroelektronika.Checked = false;            
            this.chkMinusStanje.Checked = false;
            this.chkFisFakturaGK.Checked = false;
        }
     


        #endregion

 
        private void frmFirme_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    // perform necessary action
                    e.Handled = true;
                    break;
            }
        }

        private void btnSabloniLookUp_Click(object sender, EventArgs e)
        {
            string c = Application.ExecutablePath + "/";
            dlg.InitialDirectory = c;
            dlg.Filter = "doc (*.doc)|*.doc|docx (*.docx)|*.docx|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.txtSabloni.Text = dlg.FileName.Replace((char)92, (char)47);
                //MessageBox.Show(dlg.FileName)

            }
        }

        private void btnLokFisFajlLookUp_Click(object sender, EventArgs e)
        {
            
            if (dlgDirektorij.ShowDialog() == DialogResult.OK)
            {
                string cPut;
                cPut= dlgDirektorij.SelectedPath + "\\";
                cPut= cPut.Replace((char)92, (char)47);
                this.txtLokFisFajl.Text = cPut; 
                

            }
        }
    


        private void btnPutanjaBazaLookUp_Click(object sender, EventArgs e)
        {
            if (dlgDirektorij.ShowDialog() == DialogResult.OK)
            {
                this.txtPutanjaDoBaze.Text = dlgDirektorij.SelectedPath + "/";

                this.txtPutanjaDoBaze.Text =txtPutanjaDoBaze.Text.Replace((char)92, (char)47);

                lstCMP.Clear();
                //FIND ALL FILES IN FOLDER 
                int i; i = 0;
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(this.txtPutanjaDoBaze.Text);
                foreach (System.IO.FileInfo f in dir.GetFiles("*.*"))
                {
                    //LOAD FILES 
                    ListViewItem lSingleItem = lstCMP.Items.Add(f.Name);
                    lstCMP.Items[i].ImageIndex = 0;
                    //SUB ITEMS 
                    lSingleItem.SubItems.Add(Convert.ToString(f.Length));
                    lSingleItem.SubItems.Add(f.Extension);
                    i = i + 1;

                }

            }
        }

        private void lstCMP_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.txtImeBaze.Text = lstCMP.FocusedItem.Text;
        }

        private void btnKreirajBazu_Click(object sender, EventArgs e)
        {
            string cFajlBaza =@"" + txtPutanjaDoBaze.Text + txtImeBaze.Text ;
            if (System.IO.File.Exists(cFajlBaza))
            {
                MessageBox.Show("Baza sa tim nazivom postoji!");
                return;

            }
            else 
            {
                try
                {
                    string cIniPut = AppDomain.CurrentDomain.BaseDirectory + @"AccConfig\Master.mdb";
                    System.IO.File.Copy(cIniPut, cFajlBaza, true);
                    MessageBox.Show("Baza je uspješno kreirana.");
                }
                catch (Exception es) { MessageBox.Show("Problem sa kopiranjem master baze! " + es.ToString()); }
                
            }
        }

        private void txtPutanjaDoBaze_TextChanged(object sender, EventArgs e)
        {

        }



    


    }

 
}
