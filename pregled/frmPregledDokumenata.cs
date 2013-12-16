using System;
using System.Collections.Generic;
using System.Collections;
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
    public partial class frmPregledDokumenata : Form
    {
        public bool bLookUpForma;
        public bool bIzSetupaLoco;
        OdbcDataAdapter sqlDataAdapter;
        //Dim sqlDataAdapterNaziv As OdbcDataAdapter
        public static DataSet dataset;
        public static DataTable datatab;
        OdbcDataAdapter sqlDataAdapterDetalji;
        public static DataTable datatabDetalji;
        public static string cDbTabela;
        public static int mIzbor;
        public static ArrayList selectedRow;
        public static string  mNacinPlacanja;
        public bool Dragging;
        public int mousex;
        public int mousey;

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }
        public static modus_rada modus;

        public static bool bPristoDaBrisePrethodneRacune;

        public frmPregledDokumenata(bool bIzSetupa, bool bLookUpForma)
        {
            InitializeComponent();
            bIzSetupaLoco = bIzSetupa;

        }

        private void frmPregledDokumenata_Load(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead, "");  //čitam posljednju poziciju
            cDbTabela = "mp_dnevnik";
            optDokumentiIzlaza.Checked = true;
            mIzbor = 1;
            popunitree(0); popunicombo(false); lblMoveDetalje.Visible = false;
            modus = modus_rada.pregled;
        }

        private void frmPregledDokumenata_KeyDown(object sender, KeyEventArgs e)
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
                if (lblMoveDetalje.Visible == true) { btnOdustaniOdDetalja_Click(this, e); ukljucidetalje(false); grid1.Focus(); }
                else { Dispose(); }
            }


            //if (e.KeyCode == Keys.Insert) { if (modus == modus_rada.pregled) { btnDodavanje_Click(this, e); } }
            //if (e.KeyCode == Keys.End) { if (modus == modus_rada.pregled) { btnIzmjena_Click(this, e); } }
            if (e.KeyCode == Keys.Delete) btnBrisanje_Click(this, e); 
            //if (e.KeyCode == Keys.Home) { if (modus == modus_rada.pregled) { btnDetalji_Click(this, e); } }
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

        private void resize()
        {
            gridDetalji.Width = Width - 300;
            gridDetalji.Height = (Height - 145);
            //grid1.Width = Width -464;
            grid1.Height = (Height - 145);
            pnlSlika.Width = Width - 8;
            panel1.Width = Width - 8;

        }

        private void popunitree(int cIzborChekova)
        {
            // PrikaziSliku(pnlSlika,this);
            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakonektuj(); }
            else { moduli.kon.konektuj(); }
            
            string cSQL ="";
            
            if (moduli.variable.DbTipBaze== moduli.variable.DbTip.Access ) {
                cSQL = "SELECT " + cIzborChekova + " as n,LasT(mp_dnevnik.BrojDok) as racun,LasT(mp_dnevnik.BrojRacuna) as br,LasT(mp_dnevnik.fiskalniracun) as fis,LasT(mp_dnevnik.zakljucen) as zak From mp_dnevnik " +
                "Where mp_dnevnik.VrstaDok = " + mIzbor + "  GROUP BY mp_dnevnik.BrojDok Order by mp_dnevnik.BrojDok;";
            }else{
                cSQL = "SELECT " + cIzborChekova + " as n, mp_dnevnik.BrojDok as racun,mp_dnevnik.BrojRacuna as br,mp_dnevnik.fiskalniracun as fis,mp_dnevnik.zakljucen as zak From mp_dnevnik " +
                "Where mp_dnevnik.VrstaDok = " + mIzbor + " " +
                "GROUP BY mp_dnevnik.BrojDok Order by mp_dnevnik.BrojDok;";}
             
            OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            datatab = new DataTable();
            sqlDataAdapter.Fill(datatab);
            grid1.AutoGenerateColumns = false;
            
            grid1.DataSource = datatab;
            selectedRow = new ArrayList();
        }

        private void grid1_SelectionChanged(object sender, EventArgs e)
        {
            pronadjired(sender);
        }

        private void pronadjired(object sender)
        {

            try
            {
                DataGridView thisGrid = (DataGridView)sender; //ovde je problem što izgubim sendera ako uključim detalje sa dugmeta detalji znači isporbaj da napraviš neovisnu funkciju pronadji red
                DataTable table = (DataTable)thisGrid.DataSource;
                DataRow row = table.Rows[thisGrid.CurrentCell.RowIndex];


                //MessageBox.Show(currentRow["Sifra"].ToString());
                //cIzabranaSifra = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();


                popunidetalje(row);

            }
            catch (Exception e) { }

        }

        private void popunidetalje(DataRow row) {

            string cSQL = "select * from mp_dnevnik where vrstadok=" + mIzbor + " and brojdok=" + row["racun"].ToString() + "";

            OdbcDataAdapter sqlDataAdapterDetalji = new OdbcDataAdapter(cSQL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            datatabDetalji = new DataTable();
            sqlDataAdapterDetalji.Fill(datatabDetalji);
            gridDetalji.AutoGenerateColumns = false;
            gridDetalji.Columns["kol"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridDetalji.Columns["kol"].DefaultCellStyle.Format="######0.00";
            gridDetalji.Columns["cijena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridDetalji.Columns["cijena"].DefaultCellStyle.Format = "######0.00";
            gridDetalji.Columns["iznos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridDetalji.Columns["iznos"].DefaultCellStyle.Format = "######0.00";
            gridDetalji.DataSource = datatabDetalji;
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

        private void btnBrisanje_Click(object sender, EventArgs e)
        {
            if (modus != modus_rada.pregled) return;
            if (modus == modus_rada.izmjena) return;
            brisanje();
        }

        private void brisanje() {

            if (moduli.variable.nFiskalnaFakturaProizvoda == true) { MessageBox.Show("Program radi u modu proizvodnje i brisanje nije moguće !"); return; }

            DialogResult dlgResult = MessageBox.Show("Da li želite obrisati označene dokumente ?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.No) return;

            string nSerijaReklamacija = "";
            string nSerijaDokumenata = "";
            string nSerijaRacuna = "";
            bPristoDaBrisePrethodneRacune = true;

            foreach (DataRow dr in datatab.Rows)
            {
                //if (dr["n"].ToString() == "1") { MessageBox.Show("rpow je selektovan " + dr["br"].ToString()); }
                if (dr["n"].ToString() == "1")
                {
                    string csqL = "select * from mp_fiskalnereklamacije where kasaracun=" + dr["br"].ToString() + "";
                    OdbcDataAdapter adRekRac = new OdbcDataAdapter(csqL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet dsRekRac = new DataSet();
                    adRekRac.Fill(dsRekRac, "mp_fiskalnereklamacije");
                    if (dsRekRac.Tables["mp_fiskalnereklamacije"].Rows.Count > 0)
                    {
                        if (bPristoDaBrisePrethodneRacune == true)
                        {
                            DialogResult dlgResult1 = MessageBox.Show("Da li želite obrisati i račune koji su reklamirani ?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dlgResult1 == DialogResult.Yes)
                            {

                                if (nSerijaReklamacija == "")
                                { nSerijaReklamacija = dsRekRac.Tables["mp_fiskalnereklamacije"].Rows[0]["kasareklamiraniracun"].ToString(); }
                                else { nSerijaReklamacija = nSerijaReklamacija + "," + dsRekRac.Tables["mp_fiskalnereklamacije"].Rows[0]["kasareklamiraniracun"].ToString(); }
                            }
                            else
                            {
                                bPristoDaBrisePrethodneRacune = false;
                            }

                        }
                        else
                        {
                            nSerijaReklamacija = "";
                        }

                    }
                    else
                    {//select * from mp_fiskalnereklamacije where kasareklamiraniracun=" 
                        string csqL1 = "select * from mp_fiskalnereklamacije where kasareklamiraniracun=" + dr["br"].ToString() + "";
                        OdbcDataAdapter adRekRac1 = new OdbcDataAdapter(csqL1, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                        DataSet dsRekRac1 = new DataSet();
                        adRekRac1.Fill(dsRekRac1, "mp_fiskalnereklamacije");
                        if (dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows.Count > 0)
                        {
                            if (nSerijaReklamacija == "")
                            { nSerijaReklamacija = dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows[0]["kasaracun"].ToString(); }
                            else { nSerijaReklamacija = nSerijaReklamacija + "," + dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows[0]["kasaracun"].ToString(); }
                        }
                        else
                        { nSerijaReklamacija = ""; }
                        adRekRac1 = null;
                        dsRekRac1 = null;
                    }
                    adRekRac = null;
                    dsRekRac = null;




                    if (nSerijaDokumenata == "")
                    {
                        nSerijaDokumenata = dr["racun"].ToString();
                        nSerijaRacuna = dr["br"].ToString();
                    }
                    else
                    {
                        nSerijaDokumenata = nSerijaDokumenata + "," + dr["racun"].ToString();
                        nSerijaRacuna = nSerijaRacuna + "," + dr["br"].ToString();
                    }
                }

            }



            if (nSerijaDokumenata.Length != 0)
            {
                if (mIzbor == 1)
                {

                    //******************************************* FISKALNO *********************************************************************************
                    if (moduli.variable.cFiskalnaKasa == true)
                    {
                        string csqL2 = "select * from mp_dnevnik where vrstadok=1 and brojdok in (" + nSerijaDokumenata + ") and fiskalniracun <>0";
                        OdbcDataAdapter adRekRac2 = new OdbcDataAdapter(csqL2, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                        DataSet dsRekRac2 = new DataSet();
                        adRekRac2.Fill(dsRekRac2, "mp_dnevnik");
                        if (dsRekRac2.Tables["mp_dnevnik"].Rows.Count > 0)
                        {
                            MessageBox.Show("Postoje računi koji imaju fiskalni broj.Brisanje je obustavljeno!");
                            adRekRac2 = null;
                            dsRekRac2 = null;
                            return;

                        }
                        adRekRac2 = null;
                        dsRekRac2 = null;

                    }
                    //**************************************************************************************************************************************            
                    //cCreateSqlComm.Connection = (bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovnaKomandna : moduli.variable.mKonekcijaKomandna);

                    string cCreateSql = "";
                    OdbcCommand cCreateSqlComm = new OdbcCommand();

                    if (bIzSetupaLoco == true)
                    { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                    else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                    cCreateSql = "delete from mp_dnevnik where vrstadok=1 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_dnevnik where vrstadok=3 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_tmpdnevnik where vrstadok=3 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_kupci_promet where racun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_fiskalnereklamacije where kasaracun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_fiskalnereklamacije where kasareklamiraniracun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    if (nSerijaReklamacija != "")
                    {
                        cCreateSql = "delete from mp_dnevnik where brojracuna in (" + nSerijaReklamacija + ") and vrstadok=1";
                        cCreateSqlComm.CommandText = cCreateSql;
                        cCreateSqlComm.ExecuteNonQuery();

                        cCreateSql = "delete from mp_dnevnik where brojracuna in (" + nSerijaReklamacija + ") and vrstadok=3";
                        cCreateSqlComm.CommandText = cCreateSql;
                        cCreateSqlComm.ExecuteNonQuery();


                    }

                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                    else { moduli.variable.mKonekcijaKomandna.Close(); }


                }
                else
                {
                    string cCreateSql = "";
                    OdbcCommand cCreateSqlComm = new OdbcCommand();

                    if (bIzSetupaLoco == true)
                    { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                    else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                    cCreateSql = "delete from mp_dnevnik where vrstadok=0 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_dnevnik where vrstadok=2 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_tmpdnevnik where vrstadok=2 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                    else { moduli.variable.mKonekcijaKomandna.Close(); }


                }
            }
            else
            {
                MessageBox.Show("Morate odrediti dokumente za brisanje!");
            }

            popunitree(0);
        
        }

        private void pojedinacnobrisanje()
        {

            if (moduli.variable.nFiskalnaFakturaProizvoda == true) { MessageBox.Show("Program radi u modu proizvodnje i brisanje nije moguće !"); return; }

            DialogResult dlgResult = MessageBox.Show("Da li želite obrisati označene dokumente ?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.No) return;

            string nSerijaReklamacija = "";
            string nSerijaDokumenata = "";
            string nSerijaRacuna = "";
            bPristoDaBrisePrethodneRacune = true;

            long cUkRac = long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString());
            long cDneRac = long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["racun"].Value.ToString());
           //foreach (DataRow dr in datatab.Rows)
           // {
                
                //if (dr["n"].ToString() == "1")
                //{

                    string csqL = "select * from mp_fiskalnereklamacije where kasaracun=" + cUkRac + "";
                    OdbcDataAdapter adRekRac = new OdbcDataAdapter(csqL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet dsRekRac = new DataSet();
                    adRekRac.Fill(dsRekRac, "mp_fiskalnereklamacije");
                    if (dsRekRac.Tables["mp_fiskalnereklamacije"].Rows.Count > 0)
                    {
                        if (bPristoDaBrisePrethodneRacune == true)
                        {
                            DialogResult dlgResult1 = MessageBox.Show("Da li želite obrisati i račune koji su reklamirani ?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dlgResult1 == DialogResult.Yes)
                            {

                                if (nSerijaReklamacija == "")
                                { nSerijaReklamacija = dsRekRac.Tables["mp_fiskalnereklamacije"].Rows[0]["kasareklamiraniracun"].ToString(); }
                                else { nSerijaReklamacija = nSerijaReklamacija + "," + dsRekRac.Tables["mp_fiskalnereklamacije"].Rows[0]["kasareklamiraniracun"].ToString(); }
                            }
                            else
                            {
                                bPristoDaBrisePrethodneRacune = false;
                            }

                        }
                        else
                        {
                            nSerijaReklamacija = "";
                        }

                    }
                    else
                    {//select * from mp_fiskalnereklamacije where kasareklamiraniracun=" 
                        string csqL1 = "select * from mp_fiskalnereklamacije where kasareklamiraniracun=" + cUkRac + "";
                        OdbcDataAdapter adRekRac1 = new OdbcDataAdapter(csqL1, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                        DataSet dsRekRac1 = new DataSet();
                        adRekRac1.Fill(dsRekRac1, "mp_fiskalnereklamacije");
                        if (dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows.Count > 0)
                        {
                            if (nSerijaReklamacija == "")
                            { nSerijaReklamacija = dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows[0]["kasaracun"].ToString(); }
                            else { nSerijaReklamacija = nSerijaReklamacija + "," + dsRekRac1.Tables["mp_fiskalnereklamacije"].Rows[0]["kasaracun"].ToString(); }
                        }
                        else
                        { nSerijaReklamacija = ""; }
                        adRekRac1 = null;
                        dsRekRac1 = null;
                    }
                    adRekRac = null;
                    dsRekRac = null;




                    if (nSerijaDokumenata == "")
                    {
                        nSerijaDokumenata ="" +  cDneRac;
                        nSerijaRacuna ="" + cUkRac;
                    }
                    else
                    {
                        nSerijaDokumenata = nSerijaDokumenata + "," + cDneRac;
                        nSerijaRacuna = nSerijaRacuna + "," + cUkRac;
                    }
                //}

            //}



            if (nSerijaDokumenata.Length != 0)
            {
                if (mIzbor == 1)
                {

                    //******************************************* FISKALNO *********************************************************************************
                    if (moduli.variable.cFiskalnaKasa == true)
                    {
                        string csqL2 = "select * from mp_dnevnik where vrstadok=1 and brojdok in (" + nSerijaDokumenata + ") and fiskalniracun <>";
                        OdbcDataAdapter adRekRac2 = new OdbcDataAdapter(csqL2, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                        DataSet dsRekRac2 = new DataSet();
                        adRekRac2.Fill(dsRekRac2, "mp_dnevnik");
                        if (dsRekRac2.Tables["mp_dnevnik"].Rows.Count > 0)
                        {
                            MessageBox.Show("Postoje računi koji imaju fiskalni broj.Brisanje je obustavljeno!");
                            adRekRac2 = null;
                            dsRekRac2 = null;
                            return;

                        }
                        adRekRac2 = null;
                        dsRekRac2 = null;

                    }
                    //**************************************************************************************************************************************            
                    //cCreateSqlComm.Connection = (bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovnaKomandna : moduli.variable.mKonekcijaKomandna);

                    string cCreateSql = "";
                    OdbcCommand cCreateSqlComm = new OdbcCommand();

                    if (bIzSetupaLoco == true)
                    { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                    else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                    cCreateSql = "delete from mp_dnevnik where vrstadok=1 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_dnevnik where vrstadok=3 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_tmpdnevnik where vrstadok=3 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_kupci_promet where racun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_fiskalnereklamacije where kasaracun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_fiskalnereklamacije where kasareklamiraniracun in (" + nSerijaRacuna + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    if (nSerijaReklamacija != "")
                    {
                        cCreateSql = "delete from mp_dnevnik where brojracuna in (" + nSerijaReklamacija + ") and vrstadok=1";
                        cCreateSqlComm.CommandText = cCreateSql;
                        cCreateSqlComm.ExecuteNonQuery();

                        cCreateSql = "delete from mp_dnevnik where brojracuna in (" + nSerijaReklamacija + ") and vrstadok=3";
                        cCreateSqlComm.CommandText = cCreateSql;
                        cCreateSqlComm.ExecuteNonQuery();


                    }

                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                    else { moduli.variable.mKonekcijaKomandna.Close(); }


                }
                else
                {
                    string cCreateSql = "";
                    OdbcCommand cCreateSqlComm = new OdbcCommand();

                    if (bIzSetupaLoco == true)
                    { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                    else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                    cCreateSql = "delete from mp_dnevnik where vrstadok=0 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_dnevnik where vrstadok=2 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    cCreateSql = "delete from mp_tmpdnevnik where vrstadok=2 and brojdok in (" + nSerijaDokumenata + ")";
                    cCreateSqlComm.CommandText = cCreateSql;
                    cCreateSqlComm.ExecuteNonQuery();

                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                    else { moduli.variable.mKonekcijaKomandna.Close(); }


                }
            }
            else
            {
                MessageBox.Show("Morate odrediti dokumente za brisanje!");
            }

            popunitree(0);

        }

        private void frmPregledDokumenata_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void optDokumentiIzlaza_Click(object sender, EventArgs e)
        {
            optDokumentiUlaza.Checked = false;
            chkSve.Checked = false;
            mIzbor = 1;
            popunitree(0);
        }

        private void optDokumentiUlaza_Click(object sender, EventArgs e)
        {
            optDokumentiIzlaza.Checked = false;
            chkSve.Checked = false;
            mIzbor = 0;
            popunitree(0);
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void frmPregledDokumenata_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsWrite, "");
        }

        private void chkSve_Click(object sender, EventArgs e)
        {
           
            if (chkSve.Checked == true)
            { popunitree(1); }
            else { popunitree(0); }
            
        }

        private void grid1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = grid1.PointToScreen(e.Location);
                contextMenuStrip1.Show(pt);
            }
        }

        private void pojedinačnoBrisanjeDokumenataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pojedinacnobrisanje();
        }

        private void btnReklamacija_Click(object sender, EventArgs e)
        {
            if (moduli.variable.nFiskalnaFakturaProizvoda == true) { MessageBox.Show("Program radi u modu proizvodnje i brisanje nije moguće !"); return; }

            DialogResult dlgResult = MessageBox.Show("Da li želite reklamitati označene dokumente ?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.No) return;


            double cUkupnoParaKasa;
            double cUkupnoParaRacun;
            long nNaredniRekRacun ;
            long nNaredniRekSmjenaRacun;

            foreach (DataRow dr in datatab.Rows)
            {
                if (dr["n"].ToString() == "1")
                {
                   long nRegularniRacun =long.Parse( dr["br"].ToString());
                   long nFiskalniRacun =long.Parse( dr["fis"].ToString());
                    if (moduli.variable.cFiskalnaKasa == true) 
                    {
                        try
                        {
                            string csqL = "SELECT Sum(" + moduli.funkcije.SQLuslov() + "(mp_dnevnik.VrstaDok=3,-1*(mp_dnevnik.Cijena *  " +
                            "mp_dnevnik.Kol-mp_dnevnik.Iznos),mp_dnevnik.Iznos)) AS Ukupno " +
                            "From mp_dnevnik mp_dnevnik Where mp_dnevnik.VrstaDok <> 0 And mp_dnevnik.VrstaDok <> 2 " +
                            "And mp_dnevnik.Zakljucen= 1 and mp_dnevnik.Placeno=1 GROUP BY mp_dnevnik.TarBr ORDER BY mp_dnevnik.TarBr";
                            OdbcDataAdapter adRekRac = new OdbcDataAdapter(csqL, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                            DataSet dsRekRac = new DataSet();
                            adRekRac.Fill(dsRekRac, "mp_dnevnik");
                            if (dsRekRac.Tables["mp_dnevnik"].Rows.Count == 0)
                            {
                                DialogResult dlgResult1 = MessageBox.Show("U kasi nema dovoljno novaca za reklamaciju računa.Da li želite nastaviti proces jer bi printer mogao zaglaviti?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dlgResult1 == DialogResult.No) return;
                                cUkupnoParaKasa = 0;
                            }
                            else
                            {
                                cUkupnoParaKasa = Convert.ToDouble(dsRekRac.Tables["mp_dnevnik"].Rows[0]["Ukupno"].ToString());
                            }


                            string csqL1 = "SELECT Sum(" + moduli.funkcije.SQLuslov() + "(mp_dnevnik.VrstaDok=3,-1*(mp_dnevnik.Cijena *  " +
                            "mp_dnevnik.Kol-mp_dnevnik.Iznos),mp_dnevnik.Iznos)) AS Ukupno " +
                            "From mp_dnevnik mp_dnevnik Where mp_dnevnik.VrstaDok <> 0 And mp_dnevnik.VrstaDok <> 2 " +
                            "And mp_dnevnik.Zakljucen= 1 and mp_dnevnik.brojracuna=" + nRegularniRacun + " " +
                            "GROUP BY mp_dnevnik.TarBr ORDER BY mp_dnevnik.TarBr";
                            OdbcDataAdapter adRekRac1 = new OdbcDataAdapter(csqL1, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                            DataSet dsRekRac1 = new DataSet();
                            adRekRac1.Fill(dsRekRac1, "mp_dnevnik");
                            if (dsRekRac1.Tables["mp_dnevnik"].Rows.Count == 0)
                            {
                                cUkupnoParaRacun = 0;
                            }
                            else
                            {
                                cUkupnoParaRacun = Convert.ToDouble(dsRekRac1.Tables["mp_dnevnik"].Rows[0]["Ukupno"].ToString());
                            }

                            if (cUkupnoParaRacun < cUkupnoParaKasa)
                            {
                                DialogResult dlgResult2 = MessageBox.Show("U kasi nema dovoljno novaca za reklamaciju računa.Da li želite nastaviti proces jer bi printer mogao zaglaviti?", "Upozorenje ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dlgResult2 == DialogResult.No) return;

                            }
                        }
                        
                        catch (Exception a) { MessageBox.Show(a.ToString()); }
                    
                    }



                    string cSQLKontrolaNiv = "SELECT mp_dnevnik.BrojRacuna, mp_dnevnik.SifraRobe, roba.Sifra,roba.Naziv, mp_dnevnik.Cijena," +
                    "roba.Cijena as a FROM mp_dnevnik LEFT JOIN roba ON mp_dnevnik.SifraRobe = roba.Sifra " +
                    "WHERE mp_dnevnik.BrojRacuna=" + nRegularniRacun + " AND (mp_dnevnik.Cijena<>roba.Cijena);";
                    OdbcDataAdapter adKontrolaNiv = new OdbcDataAdapter(cSQLKontrolaNiv, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet dsKontrolaNiv = new DataSet();
                    adKontrolaNiv.Fill(dsKontrolaNiv, "mp_dnevnik");
                    if (dsKontrolaNiv.Tables["mp_dnevnik"].Rows.Count != 0)
                    {
                        MessageBox.Show("U računu je roba kojoj je u medjuvremenu nivelirana cijena.Nivelirajte cijenu robe " + dsKontrolaNiv.Tables["mp_dnevnik"].Rows[0]["sifra"].ToString() + "  " + dsKontrolaNiv.Tables["mp_dnevnik"].Rows[0]["naziv"].ToString() + " sa vrijednosti " + dsKontrolaNiv.Tables["mp_dnevnik"].Rows[0]["a"].ToString() + " na vrijednost  " + dsKontrolaNiv.Tables["mp_dnevnik"].Rows[0]["cijena"].ToString() + "  koja je bila u računu.Proces je obustavljen.");
                        return;
                    }

                    // ---------------------------------------------- provjera reklamacija
                    string csqLKontRek = "select * from mp_fiskalnereklamacije where kasaracun=" + nRegularniRacun + "";
                    OdbcDataAdapter adKontRek = new OdbcDataAdapter(csqLKontRek, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet dsKontRek = new DataSet();
                    adKontRek.Fill(dsKontRek, "mp_fiskalnereklamacije");
                    if (dsKontRek.Tables["mp_fiskalnereklamacije"].Rows.Count != 0)
                    {
                        MessageBox.Show("Račun broj " + nRegularniRacun + " je reklamacija računa broj " + moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "mp_fiskalnereklamacije", "kasareklamiraniracun", "kasaracun=" + nRegularniRacun + "") + " !");
                    }
                    else
                    {

                        string csqLKontRek1 = "select * from mp_fiskalnereklamacije where kasareklamiraniracun=" + nRegularniRacun +"";
                        OdbcDataAdapter adKontRek1 = new OdbcDataAdapter(csqLKontRek1, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                        DataSet dsKontRek1 = new DataSet();
                        adKontRek1.Fill(dsKontRek1, "mp_fiskalnereklamacije");
                        if (dsKontRek1.Tables["mp_fiskalnereklamacije"].Rows.Count != 0)
                        {
                            MessageBox.Show("Račun broj " + nRegularniRacun + " je reklamiran računom broj " + moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "mp_fiskalnereklamacije", "kasaracun", "kasareklamiraniracun=" + nRegularniRacun + "") + " !");
                        }  
                        else 
                        {
                                nNaredniRekRacun = moduli.funkcije.RetNextValuelng(bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "mp_dnevnik", "brojracuna", "vrstadok=1");
                                nNaredniRekSmjenaRacun = moduli.funkcije.RetNextValuelng(bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "mp_dnevnik", "brojdok", "vrstadok=1");

                                
                                string csqL3 = "select * from mp_dnevnik where brojracuna=" + nRegularniRacun + "";
                                OdbcCommand adRekRacComm = new OdbcCommand(csqL3,bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                                OdbcDataReader myReader;
                                myReader = adRekRacComm.ExecuteReader();

                                if (myReader.HasRows)
                                {
                              
                                        while (myReader.Read())
                                        {
                                            OdbcCommand cCreateSqlComm = new OdbcCommand();
                                            double cKol=Convert.ToDouble (myReader["kol"]) * -1;
                                            double cCijena = Convert.ToDouble(myReader["cijena"]);
                                            double cPorezI=Convert.ToDouble( myReader["porezi"]) * -1;
                                            double cIznos=Convert.ToDouble(myReader["iznos"]) * -1;
                                            string cCreateSql= "INSERT INTO mp_dnevnik (prodavnica,login,datum,brojracuna,vrstadok,brojdok,rednibroj," +
                                            "sifrarobe,nazivrobe,tarbr,jm,porezp,kol,cijena,iznos,porezi,zakljucen,placeno,fiskalniracun) VALUES " +
                                            "(" + myReader["prodavnica"] + ",'" +  myReader["login"]  + "'," + moduli.funkcije.SQLDate(DateTime.Parse( myReader["datum"].ToString())) + "," +
                                            "" + nNaredniRekRacun + "," +
                                            "" +  myReader["vrstadok"] + "," + nNaredniRekSmjenaRacun + "," + myReader["rednibroj"] + "," +
                                            "'" +  myReader["sifrarobe"] + "','" +  myReader["nazivrobe"] + "','" +  myReader["tarbr"] + "'," + 
                                            "'" +  myReader["jm"] + "'," + moduli.funkcije.MySqlBr( myReader["porezp"].ToString()) + "," + moduli.funkcije.MySqlBr(Convert.ToString(cKol)) + "," +
                                            "" + moduli.funkcije.MySqlBr(cCijena.ToString()) + "," + moduli.funkcije.MySqlBr(cIznos.ToString()) + "," + moduli.funkcije.MySqlBr(Convert.ToString(cPorezI)) + "," + 
                                            "" + myReader["zakljucen"] + "," + myReader["placeno"] + "," + myReader["fiskalniracun"] + ")";

                                            if (bIzSetupaLoco == true)
                                            { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                                            else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                                            cCreateSqlComm.CommandText = cCreateSql;
                                            cCreateSqlComm.ExecuteNonQuery();

                                            if (bIzSetupaLoco == true)
                                            { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                                            else { moduli.variable.mKonekcijaKomandna.Close(); }

                                        }
                                        myReader.Close();

                                            OdbcCommand cCreateSqlComm1 = new OdbcCommand();
                                            string cCreateSql1= "insert into mp_fiskalnereklamacije (kasaracun,fiskalniracun,kasareklamiraniracun) " +
                                            "values (" + nNaredniRekRacun + "," + nFiskalniRacun + "," + nRegularniRacun + ")";

                                            if (bIzSetupaLoco == true)
                                            { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm1.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                                            else { moduli.kon.komandnakonektuj(); cCreateSqlComm1.Connection = moduli.variable.mKonekcijaKomandna; }

                                            cCreateSqlComm1.CommandText = cCreateSql1;
                                            cCreateSqlComm1.ExecuteNonQuery();

                                            if (bIzSetupaLoco == true)
                                            { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                                            else { moduli.variable.mKonekcijaKomandna.Close(); }

                                            //************************** FISKALNO ******************************************
                                            moduli.fiskal.fiskalnareklamacija(nRegularniRacun, nFiskalniRacun, "mp_dnevnik","");
                                            if (moduli.variable.bFiskalnaGreska == true)  moduli.variable.bFiskalnaGreska = false;
                                    
                                            //******************************************************************************
                                    }
                            }
                    }

                }
            }
        popunitree(0);
        
        }

        private void otkjlučajRačunToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (this.optDokumentiUlaza.Checked  == true) return;
            DialogResult dlgResult = MessageBox.Show("Da li ste sigurni da želite otključati račun i time poništiti fiskalni broj ukoliko ga račun ima ?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.No) {return; } 
            else
            { 
                    OdbcCommand cCreateSqlComm1 = new OdbcCommand();
                    string cCreateSql1 = "update mp_dnevnik set zakljucen=0,fiskalniracun=0 where brojracuna=" + grid1.Rows[grid1.CurrentCell.RowIndex ].Cells["br"].Value  +  "";

                    if (bIzSetupaLoco == true)
                    { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm1.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                    else { moduli.kon.komandnakonektuj(); cCreateSqlComm1.Connection = moduli.variable.mKonekcijaKomandna; }

                    cCreateSqlComm1.CommandText = cCreateSql1;
                    cCreateSqlComm1.ExecuteNonQuery();

                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                    else { moduli.variable.mKonekcijaKomandna.Close(); }
                    popunitree(0);
                    //baza.Execute "update mp_dnevnik set zakljucen=0,fiskalniracun=0 where brojracuna=" & Me.TDBGrid2.Columns(2).Value & ""

            }
        }

        private void zaključiRačunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (optDokumentiUlaza.Checked == true) return;

            if (grid1.Rows[grid1.CurrentCell.RowIndex].Cells["zak"].Value.ToString() == "1") { MessageBox.Show("Račun je već zaključen !"); return; }

            DialogResult dlg = MessageBox.Show("Da li želite zaključiti zaključiti račun?", "Upozorenje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.No) return;

            //************************************** FISKALNO *********************************************************
            if (moduli.variable.cFiskalnaKasa == true)
            {
                moduli.fiskal.fiskalsinhronizirajrobuizracunaDatecsTxt(long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString()),"mp_dnevnik");
            }
            //*********************************************************************************************************
            blokirajkontrole(true);
            ukljucidetalje(true);
            modus = modus_rada.izmjena;
           
      
        }


        #region "popunicombo"

        private void popunicombo(bool IzBaze)
        {
            if (IzBaze == true)
            { }
            else
            {

                var dataSource   = new List<placanje>();
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
            mNacinPlacanja = cmbPlacanje.SelectedValue.ToString();
            Console.WriteLine(cmbPlacanje.SelectedValue.ToString());
        }

    #endregion

        #region "REGIJA LookUp kupac"

        private void txtKupac_TextChanged(object sender, EventArgs e)
        {
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "kupci", "naziv", "sifra='" + txtKupac.Text + "'");
            if (a != "nema trazene vrijednosti")
            { txtKupacIme.Text = a; }
            else
            {
                txtKupacIme.Text = "";
            }
            /*
            if (modus != modus_rada.pregled)
            {
                if (a != "nema trazene vrijednosti")
                { txtKupacIme.Text = a; }
                else
                {
                    txtKupacIme.Text = "";
                }
            }
            else { txtKupacIme.Text = a; }
             */
        }

        private void btnKupacLookUp_Click(object sender, EventArgs e)
        {
            moduli.variable.bIzSetupa = true;
            frmKupci a = new frmKupci(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUp();
            //'frmOpstine.bLookUpForma = False
            moduli.variable.bIzSetupa = true;
            txtKupac.Focus();
        }

        private void popuniLookUp()
        {

            if (String.IsNullOrEmpty(moduli.variable.cLookUpSifra )==false )
            {
                txtKupac.Text = moduli.variable.cLookUpSifra;
                txtKupacIme.Text = moduli.variable.cLookUpNaziv;
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                /*
                if (txtKupac.Text == "")
                {
                    txtKupac.Text = "*";
                }*/
                txtKupac.Text = "*";
            }

        }

        private void txtKupac_Validated(object sender, EventArgs e)
        {
            if (txtKupac.Text == "" )
            {
                btnKupacLookUp_Click(this, e);
            }
        }

        #endregion

        private void ukljucidetalje(bool bUkljuciDetalje)
        {
            if (bUkljuciDetalje == false)
            {

                //ispraznikontrole();
                //displaytelefone(false);
                lblMoveDetalje.Visible = false;
            }
            else
            {
                try
                {
                    /*if (modus != modus_rada.dodavanje)
                    {
                        //pronadjired();
                    }*/
                    
                    lblMoveDetalje.Visible = true;
                }
                catch (Exception gr) { }

            }

        }

        private void btnOdustaniOdDetalja_Click(object sender, EventArgs e)
        {
            //bUkljuciDetalje = true;
            txtKupac.Text = "";
            ukljucidetalje(false);
            blokirajkontrole(false);
            modus = modus_rada.pregled;
        }

        private void btnCloseDetalje_Click(object sender, EventArgs e)
        {
            txtKupac.Text = "";
            ukljucidetalje(false);
            blokirajkontrole(false);
            modus = modus_rada.pregled;
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

        private void btnUpisi_Enter(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtKupac.Text)) return;
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

        private void btnUpisi_Click(object sender, EventArgs e)
        {

            string cCreateSql = "";
            OdbcCommand cCreateSqlComm = new OdbcCommand();

            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
            else { moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

            cCreateSql = "delete from mp_kupci_promet where racun=" + long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString ()) +"";
            cCreateSqlComm.CommandText = cCreateSql;
            cCreateSqlComm.ExecuteNonQuery();

            if (string.IsNullOrEmpty(txtKupacIme.Text) != false)
            {
                cCreateSql = "INSERT INTO mp_kupci_promet (racun,kupac,ino) values "+
                "(" + long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString()) + ",'" + txtKupac.Text + "',0)";
                cCreateSqlComm.CommandText = cCreateSql;
                cCreateSqlComm.ExecuteNonQuery();
            }

            cCreateSql = "update mp_dnevnik set zakljucen=1,placeno=" + int.Parse(mNacinPlacanja) + " " +
            "where brojracuna=" + long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString()) + " and (vrstadok=1 or vrstadok=3)";
            cCreateSqlComm.CommandText = cCreateSql;
            cCreateSqlComm.ExecuteNonQuery();

            if (bIzSetupaLoco == true)
            { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
            else { moduli.variable.mKonekcijaKomandna.Close(); }

            if (moduli.variable.cFiskalnaKasa == true) 
            {
                DialogResult dlg = MessageBox.Show("Da li želite izraditi fiskalni račun=?", "Upozorenje", MessageBoxButtons.YesNo);
                if (dlg == DialogResult.Yes) 
                {
                    moduli.fiskal.fiskalniracun(long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString()), "mp_dnevnik","");
                }

                popunitree(0);
                txtKupac.Text = "";
                btnOdustaniOdDetalja_Click(this, e); ukljucidetalje(false); grid1.Focus();
            }


        }

        private void štampaMaloprodajnogRačunaA4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cSQL="";string cRadnaTabela="";string cSQLDetalj="";string cSQLTB="";string cSQLKupac="";
            long mReportRacunBroj =long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["racun"].Value.ToString ());
            long mReportGenerisaniBroj = long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["br"].Value.ToString ());
            long mReportFiskalniRacun=long.Parse(grid1.Rows[grid1.CurrentCell.RowIndex].Cells["fis"].Value.ToString ());
            moduli.variable.bArhiviraniRacuni = false;//radi se iz dnevnika

            cRadnaTabela = moduli.variable.bArhiviraniRacuni==false ? "mp_dnevnik" : "mp_promet";

            string CpubKupac = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    "mp_kupci_promet", "kupac", "racun=" + mReportGenerisaniBroj + "");

            cSQLKupac = "select * from kupci where sifra='" + CpubKupac  + "'";
            
            /* vrijednosti za print u zaglavlju fakture*************************************************/
            DateTime mReportDatum = DateTime.Parse(moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                cRadnaTabela, "datum", "brojracuna=" + mReportGenerisaniBroj + ""));

            moduli.variable.mReportDatum = mReportDatum.ToShortDateString();
            moduli.variable.mReportBrojFiskalnogRacuna = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                            cRadnaTabela, "fiskalniracun", "brojracuna=" + mReportGenerisaniBroj + "");

            moduli.variable.mReportBrojRacuna = mReportGenerisaniBroj.ToString();
            
            /********************************************************************************************/


            cSQL="select * from " + cRadnaTabela  + " where  " +
            "vrstadok = 3 and brojracuna=" + mReportGenerisaniBroj + ""; //provjeravam da li je bilo popusta
            OdbcDataAdapter AdPopust =new OdbcDataAdapter(cSQL,bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            DataSet DsPopust = new DataSet();AdPopust.Fill(DsPopust,cRadnaTabela);
            if (DsPopust.Tables[cRadnaTabela].Rows.Count != 0){
                //ako je postojao popust
                cSQLDetalj = "select sifrarobe,nazivrobe ,jm,kol,cijena,(kol * cijena) as iznos,(iznos-porezi) as osnovica,porezi,porezp " +
                "from " + cRadnaTabela  + " where vrstadok = 3 and brojracuna=" + mReportGenerisaniBroj + "";
                    
                if (moduli.variable.DbTipBaze==moduli.variable.DbTip.Mysql){
                    cSQLTB = "SELECT tarifnibroj.naziv as opis,dnevnik.TarBr as TarBr,dnevnik.Porezp AS Porezp," +
                    "sum(dnevnik.iznos-dnevnik.porezi)as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                    "From " + cRadnaTabela  + " dnevnik left join tarifnibroj on dnevnik.tarbr=tarifnibroj.sifra " +
                    "Where dnevnik.VrstaDok = 3 And dnevnik.brojracuna=" + mReportGenerisaniBroj + " GROUP BY dnevnik.TarBr ";
                }else{
                    cSQLTB = "SELECT FIRST(tarifnibroj.naziv) as opis,FIRST(dnevnik.TarBr) as TarBr,FIRST(dnevnik.Porezp) AS Porezp," +
                    "sum(dnevnik.iznos-dnevnik.porezi)as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                    "From " + cRadnaTabela  + " dnevnik left join tarifnibroj on dnevnik.tarbr=tarifnibroj.sifra " +
                    "Where dnevnik.VrstaDok = 3 And dnevnik.brojracuna=" + mReportGenerisaniBroj + " GROUP BY dnevnik.TarBr ";
                }
                
            }else{
                //ako nije postojao popust
                cSQLDetalj = "select sifrarobe,nazivrobe ,jm,kol,cijena,(kol * cijena) as iznos,(iznos-porezi) as osnovica,porezi,porezp " +
                "from " + cRadnaTabela  + " where vrstadok = 1 and brojracuna=" + mReportGenerisaniBroj + "";
                    
                if (moduli.variable.DbTipBaze==moduli.variable.DbTip.Mysql){
                    cSQLTB = "SELECT tarifnibroj.naziv as opis,dnevnik.TarBr as TarBr,dnevnik.Porezp AS Porezp," +
                    "sum(dnevnik.iznos-dnevnik.porezi)as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                    "From " + cRadnaTabela  + " dnevnik left join tarifnibroj on dnevnik.tarbr=tarifnibroj.sifra " +
                    "Where dnevnik.VrstaDok = 1 And dnevnik.brojracuna=" + mReportGenerisaniBroj + " GROUP BY dnevnik.TarBr ";
                }else{
                    cSQLTB = "SELECT FIRST(tarifnibroj.naziv) as opis,FIRST(dnevnik.TarBr) as TarBr,FIRST(dnevnik.Porezp) AS Porezp," +
                    "sum(dnevnik.iznos-dnevnik.porezi)as osnovica, Sum(dnevnik.PorezI) AS PorezI " +
                    "From " + cRadnaTabela  + " dnevnik left join tarifnibroj on dnevnik.tarbr=tarifnibroj.sifra " +
                    "Where dnevnik.VrstaDok = 1 And dnevnik.brojracuna=" + mReportGenerisaniBroj + " GROUP BY dnevnik.TarBr ";
                }                
                
            }


            if (optDokumentiUlaza.Checked == true) return;
    
            DialogResult dlg = MessageBox.Show("Da li želite štampu u Wordu?", "Upozorenje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.No) {

                cSQLKupac = "select kupci.sifra,kupci.naziv,kupci.adresa,kupci.id, kupci.pdvbroj AS pdv, opstine.naziv AS grad from kupci left join opstine " +
                    "on kupci.opstina=opstine.sifra where kupci.sifra='" + CpubKupac + "'";
                frmStampaFirme a = new frmStampaFirme("Txt","FakturaMP",cSQLDetalj,cRadnaTabela,cSQLTB,"tarifnibroj",cSQLKupac ,"kupci",false );
                a.Show();
            
            
            }

            else
            {
                

                moduli.funkcije.PrintToMsWord(cSQLDetalj,cRadnaTabela,cSQLTB,"FAKTURA",CpubKupac,mReportGenerisaniBroj,mReportFiskalniRacun,mReportDatum,false );
            
            }
            

        }



   
            
    }
}
