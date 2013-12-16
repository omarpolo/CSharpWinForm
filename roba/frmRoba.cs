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
    public partial class frmRoba : Form
    {

        OdbcDataAdapter updbrcdapt; OdbcCommand cSqlTIn; OdbcCommand cSqlTUp; OdbcCommand cSqlTDel;
        DataSet upteldataset;
        OdbcCommandBuilder cmd; 
        DataTable tbBarkodovi;

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

        public enum modus_rada
        {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
        }

        public static modus_rada modus;

        public frmRoba(bool bLookUpFormaOut, bool bIzSetupaOut)
        {
            bIzSetupaLoco = bIzSetupaOut;//moduli.variable.bIzSetupa;
            bLookUpForma = bLookUpFormaOut;
            InitializeComponent();
            txtSearch.Focus();

        }

        private void frmRoba_KeyDown(object sender, KeyEventArgs e)
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

        private void frmRoba_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    // perform necessary action OVIM GASIM BEEP NA TEXT KONTROLAMA
                    e.Handled = true;
                    break;
            }
        }

        private void frmRoba_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void frmRoba_Load(object sender, EventArgs e)
        {
            moduli.funkcije.FormLayout(this, moduli.variable.constRW.dsRead, "");  //čitam posljednju poziciju
            cDbTabela = "roba";
            SortColumn = 1;
            cKolona = grid1.Columns[SortColumn].Name;
            bUkljuciDetalje = false;
            ukljucidetalje(bUkljuciDetalje);
            modus = modus_rada.pregled;

            // PrikaziSliku(pnlSlika,this);
            if (bIzSetupaLoco == true)
            { moduli.kon.osnovnakonektuj(); }
            else { moduli.kon.konektuj(); }


            popunigrid("", "", "order by rb ASC");
            displaybarkodove(true);
            resize();
            DetaljiBox.Enabled = false;


            //***********

        }

        private void frmRoba_FormClosing(object sender, FormClosingEventArgs e)
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
                    switch (SortColumn)
                    {
                        case 0:
                            cKolona = grid1.Columns[SortColumn].Name;
                            //cKolona = "sifra";
                            break;
                        case 1:
                            cKolona = grid1.Columns[SortColumn].Name;
                            //cKolona = "naziv";
                            break;
                    }
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) grid1.Focus();
            if (e.KeyCode == Keys.Enter) grid1.Focus();
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
                grid1.AutoGenerateColumns = false;
                this.grid1.DataSource = dss.Tables[cDbTabela];
                ispraznikontrole();
                displaydata();
                displaybarkodove(lblMoveDetalje.Visible == false ? false : true);
                int i;

                for (i = 2; i < this.grid1.Columns.Count; i++)
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
                OdbcDataAdapter sqlDataAdapter = new OdbcDataAdapter("select * from " + cDbTabela + " " + uslov + " " + cOrder + " ", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
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
            if (bUkljuciDetalje ==false) return;
            displaybarkodove(lblMoveDetalje.Visible == false ? false : true );
            //ukljucidetalje(bUkljuciDetalje);
            //pronadjired(sender);
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

        private void txtSifra_Leave(object sender, EventArgs e) //nisam naso lost fcous
        {

            if (modus == modus_rada.izmjena) { return; }
            if (modus == modus_rada.pregled) { return; }
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, cDbTabela, "sifra", "sifra='" + txtSifra.Text + "'");
            if (a != "nema trazene vrijednosti")
            {
                MessageBox.Show("U bazi postoji vrijednost sa ovom šifrom !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifra.Text = "";
                txtSifra.Focus();
            }
            /*
            if (txtSifra.Text == "")
            {
                MessageBox.Show("Vrijednost ne može biti prazna !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifra.Text = "";
                txtSifra.Focus();
            }
            */
        }

        private void btnDodavanje_Click(object sender, EventArgs e)
        {
            modus = modus_rada.dodavanje;
            cIzabranaSifra = "";
            ispraznikontrole();
            blokirajkontrole(true);

            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            popunifiskalnusifru();
            txtSifra.Focus();
        }

        private void btnIzmjena_Click(object sender, EventArgs e)
        {
            if (grid1.Rows.Count == 0) return;
            modus = modus_rada.izmjena;
            blokirajkontrole(true);
            bUkljuciDetalje = true;
            ukljucidetalje(bUkljuciDetalje);
            displaybarkodove(lblMoveDetalje.Visible == false ? false : true);
            omogucisifre(false);
            txtNaziv.Focus();
        }

        private void btnOdustaniOdDetalja_Click(object sender, EventArgs e)
        {
            switch (modus)
            {
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
            
            string cSif = txtSifra.Text;
            string cNaziv = txtNaziv.Text;
            int cFsifra = int.Parse(txtFSifra.Text);
            string cJM = txtJM.Text;
            string cTB = txtTB.Text;
            string cPP = moduli.funkcije.MySqlBr(txtTBIme.Text);
            string cCijena = moduli.funkcije.MySqlBr(txtCijena.Text);
            string cCijenaBP = moduli.funkcije.MySqlBr(txtCijenaBP.Text);
            string cVrsta = txtVrsta.Text;
            string cStatus = txtVrsta.Text;


            switch (modus)
            {
                case modus_rada.dodavanje:
                    
                    //int cBr = moduli.funkcije.MaxBroj("rb", cDbTabela, "", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    //if (cBr == 0) break;
                    cCreateSql = "insert into " + cDbTabela + " (sifra,naziv,fiskalnasifra,jm,tb,porez,cijena,cijenabp,vrsta,status) " +
                        " values " +
                        "('" + cSif.Replace("'", "") + "','" + cNaziv.Replace("'", "") + "'," + cFsifra + ",'" + cJM.Replace("'", "") + "'," +
                        "'" + cTB.Replace("'", "") + "'," + cPP + "," +
                        "" + cCijena + "," + cCijenaBP + ",'" + cVrsta.Replace("'", "") + "'," + (chkStatus.Checked == true ? 1 : 0) + ")";

                    try
                    {
                        moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql); 
                        updatebarkodove();
                        // grid1.Item(0, 4).Selected = True;
                        modus = modus_rada.pregled;
                        popunigrid("", "", "order by rb DESC");
                    }
                    catch (Exception a) { }
                    break;


                case modus_rada.izmjena:


                    cCreateSql = "update " + cDbTabela + " set naziv='" + cNaziv.Replace("'", "") + "',jm='" + cJM.Replace("'", "") + "',TB='" + cTB.Replace("'", "") + "'," +
                        "porez=" + cPP + ",cijena=" + cCijena + ",cijenabp=" + cCijenaBP + ",vrsta='" + cVrsta.Replace("'", "") + "',status=" + (chkStatus.Checked == true ? 1 : 0) + " " +
                        " where sifra='" + cSif.Replace("'", "") + "'";

                    try
                    {
                        moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql);
                        updatebarkodove();
                        moduli.variable.cEditVrijednostString = txtSifra.Text;
                        cIzabranaSifra = moduli.variable.cEditVrijednostString;
                        modus = modus_rada.pregled;
                        omogucisifre(true);
                        
                        ispraznikontrole(); displaydata(); grid1.Refresh();
                        cIzabranaSifra = moduli.variable.cEditVrijednostString;  // ovo mi sjebe izabranu šifru kod brisanja pa sam odradio sljedeće
                        
                        
                        moduli.variable.cEditVrijednostString = "";

                    }
                    catch (Exception a) { 
                        MessageBox.Show(a.ToString()); 
                    }
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

                    //pronadjired(grid1);
                    string cSif = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    //MessageBox.Show(cSifFirme);
                    cCreateSql = "delete from " + cDbTabela + " where sifra='" + cSif + "' ";
                    moduli.funkcije.SQLExecute(bIzSetupaLoco, cCreateSql);

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
            displaybarkodove(lblMoveDetalje.Visible == false ? false : true);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            //DialogResult dlgResult = MessageBox.Show("Da li da štampa bude iz text fielda ?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dlgResult == DialogResult.Yes) { frmStampaFirme a = new frmStampaFirme("Txt"); a.Show(this); }
            //else { frmStampaFirme a = new frmStampaFirme("DataSet"); a.Show(this); }



        }

        private void btnOdaberi_Click(object sender, EventArgs e)
        {
            Odaberi();

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
            try{
                if (bLookUpForma == false) return;
                moduli.variable.cLookUpSifra = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                moduli.variable.cLookUpNaziv = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[1].Value.ToString();
                Dispose();
            }
            catch (Exception e) { MessageBox.Show("Morate odabrati vrijednost!"); txtSearch.Text = ""; }

        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
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

        #endregion

        #region "posebno na svim formama "

        private void txtFSifra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtFSifra_Leave(object sender, EventArgs e) //nisam naso lost fcous
        {

            if (modus == modus_rada.izmjena) { return; }
            if (modus == modus_rada.pregled) { return; }
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, cDbTabela, "fiskalnasifra", "fiskalnasifra=" + txtFSifra.Text + "");
            if (a != "nema trazene vrijednosti")
            {
                MessageBox.Show("U bazi postoji vrijednost sa ovom šifrom !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifra.Text = "";
                txtSifra.Focus();
            }
            /*
            if (txtSifra.Text == "")
            {
                MessageBox.Show("Vrijednost ne može biti prazna !", "Upozorenje !", MessageBoxButtons.OK);
                txtSifra.Text = "";
                txtSifra.Focus();
            }
             */
        }
        /*      
        private void txtSifra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        */
        private void displaydata()
        {
            try
            {
                txtSifra.DataBindings.Add("Text",grid1.DataSource,"sifra",false,DataSourceUpdateMode.OnPropertyChanged);
                txtNaziv.DataBindings.Add("Text",grid1.DataSource,"naziv",false,DataSourceUpdateMode.OnPropertyChanged);
                txtJM.DataBindings.Add("Text",grid1.DataSource,"jm",false,DataSourceUpdateMode.OnPropertyChanged);
                txtFSifra.DataBindings.Add("Text",grid1.DataSource,"fiskalnasifra",false,DataSourceUpdateMode.OnPropertyChanged);
                txtTB.DataBindings.Add("Text",grid1.DataSource,"tb",false,DataSourceUpdateMode.OnPropertyChanged);
                txtCijena.DataBindings.Add("Text",grid1.DataSource,"cijena",false,DataSourceUpdateMode.OnPropertyChanged);
                txtCijenaBP.DataBindings.Add("Text", grid1.DataSource, "cijenabp", false, DataSourceUpdateMode.OnPropertyChanged);
                txtVrsta.DataBindings.Add("Text", grid1.DataSource, "vrsta", false, DataSourceUpdateMode.OnPropertyChanged);
                chkStatus.DataBindings.Add("checked", grid1.DataSource, "status", false, DataSourceUpdateMode.OnPropertyChanged);

                /*
                txtSifra.Text = row["sifra"].ToString();
                txtNaziv.Text = row["naziv"].ToString(); 
                txtJM.Text = row["JM"].ToString();
                txtFSifra.Text = row["fiskalnasifra"].ToString();
                txtTB.Text = row["TB"].ToString();//txtTBIme.Text = Convert.ToString(row["pp"].ToString());
                txtCijena.Text = Convert.ToString(row["cijena"].ToString());
                txtVrsta.Text = row["vrsta"].ToString();
                chkStatus.Checked = false;
                if (row["status"].ToString() == "1")
                {
                    chkStatus.Checked = true;
                }
                 */
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ispraznikontrole()
        {

            txtSifra.DataBindings.Clear();
            txtNaziv.DataBindings.Clear();
            txtJM.DataBindings.Clear();
            txtFSifra.DataBindings.Clear();
            txtTB.DataBindings.Clear();
            txtCijena.DataBindings.Clear();
            txtCijenaBP.DataBindings.Clear();
            txtVrsta.DataBindings.Clear();
            chkStatus.DataBindings.Clear();

            txtSifra.Text = "";
            txtNaziv.Text = ""; txtJM.Text = "";
            txtFSifra.Text = "";
            txtTB.Text = ""; txtTBIme.Text = Convert.ToString(0);
            txtCijena.Text = Convert.ToString(0);
            txtCijenaBP.Text = Convert.ToString(0);
            txtVrsta.Text = "";
            chkStatus.Checked = false;
        }

        private void omogucisifre(bool Upali)
        {
            txtSifra.Enabled = false; txtFSifra.Enabled = false;
            if (Upali == true) { txtSifra.Enabled = true; txtFSifra.Enabled = true; }

        }

        private void popunifiskalnusifru()//zovem iz btnDodavanje
        {
            int cBr = moduli.funkcije.RetNextValue("fiskalnasifra", cDbTabela, "", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            if (cBr == 0) return;
            txtFSifra.Text = Convert.ToString(cBr);

        }


        /*private void txtSifra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) ProcessTabKey(true);
            if (e.KeyCode == Keys.Down) ProcessTabKey(true);
            if (e.KeyCode == Keys.Up) ProcessTabKey(false);
        }*/



        #region "REGIJA LookUp TB"


        private void txtTB_TextChanged(object sender, EventArgs e)
        {

            string a;double b;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "tarifnibroj", "pp", "sifra='" + txtTB.Text + "'");

            if (modus != modus_rada.pregled)
            {
                if (a != "nema trazene vrijednosti")
                { b = Convert.ToDouble(a); txtTBIme.Text = string.Format("{0:0,0.00}", Convert.ToDouble(a)); }
                else { txtTBIme.Text = ""; }
            }
            else {if (txtTB.Text =="") return; txtTBIme.Text =string.Format("{0:0,0.00}", Convert.ToDouble(a));}
        }

        private void btnTBLookUp_Click(object sender, EventArgs e)
        {
            //moduli.variable.bIzSetupa = true;
            frmTarifniBroj a = new frmTarifniBroj(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUpTB();
            //'frmOpstine.bLookUpForma = False
            //moduli.variable.bIzSetupa = true;
            txtTB.Focus();
        }


        private void popuniLookUpTB()
        {

            if (moduli.variable.cLookUpSifra != "")
            {
                txtTB.Text = moduli.variable.cLookUpSifra;
                txtTBIme.Text = string.Format("{0:0,0.00}", moduli.variable.mLookupIznosPorez);
                moduli.variable.cLookUpSifra = "";
                moduli.variable.cLookUpNaziv = "";
            }
            else
            {
                if (txtTB.Text == "")
                {
                    txtTB.Text = "*";
                }
            }

        }


        private void txtTB_Validated(object sender, EventArgs e)
        {
            if (txtTB.Text == "" || txtTBIme.Text =="")
            {
                btnTBLookUp_Click(this, e);
            }
        }

        private void txtTB_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion



        #region "REGIJA LookUp Vrsta"


        private void txtVrsta_TextChanged(object sender, EventArgs e)
        {
            string a;
            a = moduli.funkcije.RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "vrstarobe", "naziv", "sifra=" + txtVrsta.Text + "");
            
            if (modus != modus_rada.pregled)
            {
                if (a != "nema trazene vrijednosti")
                { txtVrstaIme.Text = a; }
                else { txtVrstaIme.Text = ""; }
            }
            else { txtVrstaIme.Text = a; }
        }

        private void btnVrstaLookUp_Click(object sender, EventArgs e)
        {
            moduli.variable.bIzSetupa = true;
            frmVrstaRobe a = new frmVrstaRobe(true, bIzSetupaLoco);
            a.ShowDialog();
            popuniLookUpVrsta();
            //'frmOpstine.bLookUpForma = False
            moduli.variable.bIzSetupa = true;
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

        private void txtVrsta_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region "barkodovi"
       
        private void displaybarkodove(bool prikazi)
        {
             if (prikazi)
               {
                        updbrcdapt = null;
                        cmd = null;
                        tbBarkodovi = null;
                        try
                        {
                            cIzabranaSifra = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString() ;
                            updbrcdapt = new OdbcDataAdapter("select  sifra, naziv, barkod, rb from barkod where sifra= '" + cIzabranaSifra + "'", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                            cmd = new OdbcCommandBuilder(updbrcdapt);
                            tbBarkodovi = new DataTable();

                            updbrcdapt.Fill(tbBarkodovi);
                            gridBarkodovi.DataSource = null;
               
                            gridBarkodovi.DataSource = tbBarkodovi;
              

                            try
                            {

                                //string CSQL = "select * from tmptiptel ";
                    
                                //gridBarkodovi.Columns.Remove("rb");
                                gridBarkodovi.Columns["sifra"].Visible = false ;
                                gridBarkodovi.Columns["rb"].Visible = false;
                                gridBarkodovi.ForeColor = Color.Black;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.ToString());
                            } 
                
               
                        }
            
                        catch (Exception e) {  }
             }
        }

        private void updatebarkodove()
        {
            DataTable tbEdit = (DataTable)(tbBarkodovi.GetChanges(DataRowState.Modified));
            DataTable tbAdded = (DataTable)(tbBarkodovi.GetChanges(DataRowState.Added));
            DataTable tbDel = (DataTable)(tbBarkodovi.GetChanges(DataRowState.Deleted));
            try
            {
                if (tbEdit != null) { updbrcdapt.Update(tbEdit); }
                if (tbAdded != null) { updbrcdapt.Update(tbAdded); }
                if (tbDel != null) { updbrcdapt.Update(tbDel); }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

        }

        private void gridBarkodovi_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (modus == modus_rada.pregled) return;
 
                if (txtSifra.Text == "") return;

                gridBarkodovi.Rows[gridBarkodovi.CurrentCell.RowIndex].Cells[0].Value = grid1.Rows[grid1.CurrentCell.RowIndex].Cells[0].Value.ToString();
        }

        private void gridBarkodovi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                int index = this.gridBarkodovi.CurrentCell.RowIndex;
                gridBarkodovi.Rows.Remove(gridBarkodovi.CurrentRow);

            }


        }


        private void txtCijenaBP_TextChanged(object sender, EventArgs e)
        {
            if (txtCijenaBP.Focused)
            {
                double cCijenaBP = double.Parse(txtCijenaBP.Text);
                double cPorezP = double.Parse(txtTBIme.Text);
                double cCijena = Math.Round(cCijenaBP * (1 + (cPorezP / 100)), 2);
                txtCijena.Text = cCijena.ToString("#######0.00");
                
            }
        }

        private void txtCijena_TextChanged(object sender, EventArgs e)
        {
            if (txtCijena.Focused)
            {
                double cCijena = double.Parse(txtCijena.Text);
                double cPorezP = double.Parse(txtTBIme.Text);
                double cCijenaBP = Math.Round(cCijena / (1 + (cPorezP / 100)), 2);
                txtCijenaBP.Text = cCijenaBP.ToString("#######0.00");
                
            }
        }
        #endregion



        #endregion






    }
}

