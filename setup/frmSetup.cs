using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.DirectoryServices;
namespace Poslovanje.forme
{
    public partial class frmSetup : Form
    {
        public static string cTipBaze;
        public static string cImeMreze;
        public static string cPutanjaDoServera;
        public static string cOsnovnaBaza;
        public static string cKorisnickoIme;
        public static string cLozinka;
        public static string cLokacijaTempDir;
        public static bool bPrvoPokretanje;

        public frmSetup()
        {
            InitializeComponent();
            procitajparametre(this);
            //popunikontrole();
        }

        public static void procitajparametre(frmSetup instanca)
        {
            try
            {
                StreamReader sr = new StreamReader("InfoPos.ini");
                String line;
                line = sr.ReadLine();
                cTipBaze = line;
                line = sr.ReadLine();
                cImeMreze = line;
                line = sr.ReadLine();
                cPutanjaDoServera = line;
                line = sr.ReadLine();
                cOsnovnaBaza = line;
                line = sr.ReadLine();
                cKorisnickoIme = line;
                line = sr.ReadLine();
                cLozinka = line;
                line = sr.ReadLine();
                cLokacijaTempDir = line;
                moduli.variable.cNazivOsnovneBaze = cOsnovnaBaza;
                moduli.variable.cNazivServera = cPutanjaDoServera;
                moduli.variable.cUser = cKorisnickoIme;
                moduli.variable.cLozinka = cLozinka;
                moduli.variable.DbTipBaze = moduli.variable.DbTip.Access;
                if (cTipBaze == "MYSQL") moduli.variable.DbTipBaze = moduli.variable.DbTip.Mysql;
                sr.Close();
                bPrvoPokretanje = true;
                instanca.popunikontrole();
                bPrvoPokretanje = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Fajl nije moguće pročitati :");
                Console.WriteLine(e.Message);
            }
            
           
        }

        protected void popunikontrole()
        {
            txtImeMreze.Text = cImeMreze;
            txtPutanjaDoServera.Text = cPutanjaDoServera;
            txtOsnovnaBaza.Text = cOsnovnaBaza;
            txtKorisnickoIme.Text = cKorisnickoIme;
            txtLozinka.Text = cLozinka;
            txtLokacijaTmpDirektorijuma.Text = cLokacijaTempDir;
            optAccess.Checked = true;
            Kontrole(false);
            if (cTipBaze == "MYSQL") { optMysql.Checked = true; Kontrole(true); }

        }

        private void btnUpisi_Click(object sender, EventArgs e)
        {
            upisiparametre();
            MessageBox.Show("Pokrenite aplikaciju ponovo kako bi osnovni parametri bili učitani !", "Obavještenje", MessageBoxButtons.OK);
            Dispose();
        }
        private void upisiparametre() { 
            StreamWriter sw = new StreamWriter("InfoPos.ini");
            sw.WriteLine(optMysql.Checked==true ? "MYSQL":"MSACCESS");
            sw.WriteLine(txtImeMreze.Text);
            sw.WriteLine(txtPutanjaDoServera.Text);
            sw.WriteLine(txtOsnovnaBaza.Text);
            sw.WriteLine(txtKorisnickoIme.Text);
            sw.WriteLine(txtLozinka.Text);
            sw.WriteLine(txtLokacijaTmpDirektorijuma.Text);
            sw.Close();
        
        }
  
        public static string GetAllIP(string args) {
            //ovom funkcijom na osnovu imena kompjutera dobijam ip adresu

            string cIP ="";
            string strHostName ="";
            if (args.Length == 0) {
                 //Getting Ip address of local machine...
                 //First get the host name of local machine.
                strHostName = Dns.GetHostName();
                Console.WriteLine("Local Machine's Host Name: " + strHostName);
            }
            else
            {
                strHostName = args;
            }
        

            //Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            int i = 0;
            //While i < addr.Length
            cIP = addr[i].ToString();
            //Console.WriteLine("IP Address {0}: {1} ", i, addr(i).ToString())
            //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            //End While
            return cIP;
        }

        private void btnPretraga_Click(object sender, EventArgs e)
        {
           if (txtImeMreze.Text == "" ){
            MessageBox.Show("Unesite ime mreže !", "Upozorenje!");
            txtImeMreze.Focus();
            return ;
           }
        // ovde prikupljam mrežna imena kompjutera u domeni txtImeMreze.Text
        DirectoryEntry de= new DirectoryEntry();
         
        de.Path = "WinNT://" + txtImeMreze.Text;
        int i ;
        i = 0;
        foreach (DirectoryEntry d in de.Children){
            switch  (d.SchemaClassName){
                case "Computer":
                    string cIme;
                    cIme = d.Name + ":" + GetAllIP(d.Name);
                    lstCMP.Items.Add(cIme);
                    lstCMP.Items[i].ImageIndex = 0;
                    i = i + 1;
                    break;
            }

        }

        }

        private void lstCMP_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            int cDVA;
            string cPun;
            string cIPAD;
            cPun = lstCMP.FocusedItem.Text;
            cDVA = cPun.IndexOf(":");
            cIPAD = cPun.Substring(cDVA + 1);
            txtPutanjaDoServera.Text = cIPAD;
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void frmSetup_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (optMysql.Checked == true) return;


            if (this.optAccess.Checked == true)
            {
                string c = Application.ExecutablePath + "/";
                dlg.InitialDirectory = c;
                dlg.Filter = "mdb (*.mdb)|*.mdb";
                dlg.FilterIndex = 1;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtPutanjaDoServera.Text = System.IO.Path.GetDirectoryName(dlg.FileName.Replace((char)92, (char)47))+@"\";//samo putanja
                    this.txtOsnovnaBaza.Text = System.IO.Path.GetFileName(dlg.FileName);//samo ime fajla
                    string cPunaputanjasanazivom = dlg.FileName.Replace((char)92, (char)47);//komplet put + naziv
                    //MessageBox.Show(dlg.FileName)

                }
            }
        }

        private void optAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (bPrvoPokretanje == true) return;
            if (optAccess.Checked == true)
            {
                moduli.variable.DbTipBaze = moduli.variable.DbTip.Access;
                this.txtPutanjaDoServera.Text = ""; 
                this.txtOsnovnaBaza.Text = "-"; 
                this.txtKorisnickoIme.Text = "-"; 
                this.txtLozinka.Text = "-";
                Kontrole(false);
            }
            else
            {
                moduli.variable.DbTipBaze = moduli.variable.DbTip.Mysql;
                this.txtPutanjaDoServera.Text = ""; 
                this.txtOsnovnaBaza.Text = "";
                this.txtKorisnickoIme.Text = ""; 
                this.txtLozinka.Text = "";
                Kontrole(true);
            }
        }

        private void Kontrole(bool omoguci) 
        {
            txtPutanjaDoServera.Enabled = false;
            txtOsnovnaBaza.Enabled = false;
            txtKorisnickoIme.Enabled = false;
            txtLozinka.Enabled = false;
            if (omoguci) {
                txtPutanjaDoServera.Enabled = true;
                txtOsnovnaBaza.Enabled = true;
                txtKorisnickoIme.Enabled = true;
                txtLozinka.Enabled = true;
            }
        }

    }
}
