using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

using Microsoft.Office.Interop.Word;

namespace Poslovanje.moduli
{
    class funkcije
    {




        public static void SQLExecute(bool bIzSetupaLoco, string cCreateSql)
        {
            OdbcCommand cCreateSqlComm = new OdbcCommand(cCreateSql, bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovnaKomandna : moduli.variable.mKonekcijaKomandna);
            try
              {        
                if (bIzSetupaLoco == true)
                { moduli.variable.mKonekcijaOsnovna.Close(); moduli.kon.osnovnakomandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
                else { moduli.variable.mKonekcija.Close(); moduli.kon.komandnakonektuj(); cCreateSqlComm.Connection = moduli.variable.mKonekcijaKomandna; }

                cCreateSqlComm.CommandText = cCreateSql;
                cCreateSqlComm.ExecuteNonQuery();

                if (bIzSetupaLoco == true)
                { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); moduli.kon.osnovnakonektuj(); }
                else { moduli.variable.mKonekcijaKomandna.Close(); moduli.kon.konektuj(); }
              }
                catch (Exception a) {
                    if (bIzSetupaLoco == true)
                    { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); moduli.kon.osnovnakonektuj(); } else { moduli.variable.mKonekcijaKomandna.Close(); moduli.kon.konektuj(); }
                    MessageBox.Show("Došlo je do greške pri upisu u bazu. Originalna poruka : " + a.ToString());
                
                }
        
        }

        static OdbcDataReader cReader;
        public static string RetFieldValue(string cTip, OdbcConnection  mKon, string cTblName, string cFieldName, string cFilter)
        {
            string a = "";
            OdbcCommand  csql;

            
                string strComm = "select " + cFieldName + " from " + cTblName + " where " + cFilter + "";
                csql = new OdbcCommand(strComm, mKon);
                //mKon.Open();
                try
                {
                    cReader = csql.ExecuteReader();
                    cReader.Read();

                    a = cReader.GetValue(0).ToString();
                    cReader.Close();
                    return a;
                }
                catch (Exception e) 
                {
                    cReader.Close();
                    switch (cTip)
                    {
                        case "int":
                            a = "0";
                            break;
                        case "string":
                            a = "nema trazene vrijednosti";
                            break;
                        case "double":
                            a = "0";
                            break; 
                    }
                    return a; 
                } 

                //cReader.Close();
                //mKon.Close();
    
        }

        public static int RetFieldValueInt(OdbcConnection mKon, string cTblName, string cFieldName, string cFilter)
        {
            int a = 0;
            OdbcCommand csql;


            string strComm = "select " + cFieldName + " from " + cTblName + " where " + cFilter + "";
            csql = new OdbcCommand(strComm, mKon);
            //mKon.Open();
            try
            {
                cReader = csql.ExecuteReader();
                cReader.Read();

                a = int.Parse(cReader.GetValue(0).ToString());
                cReader.Close();
                return a;
            }
            catch (Exception e)
            {
                cReader.Close();
                return a = 0;
            }
            //cReader.Close();
            //mKon.Close();

        }

        public static int RetNextValue(string cPolje,string cTabela, string cUslov, OdbcConnection mKon) {
         int c;  
         OdbcCommand csql ;
         OdbcDataReader cReader ;
             
         csql = new OdbcCommand("select MAX(" + cPolje + ")AS maks from " + cTabela + cUslov + " ", mKon);                  
         cReader = csql.ExecuteReader();
                
         try{
                 cReader.Read();
                 if (cReader.GetValue(0)==null) {
                        c = 0;
                 } else {
                    c = int.Parse(cReader.GetValue(0).ToString());}
                 cReader.Close();
                 c=c+1;
                 return c;

             }
             catch (Exception e) {
                 Console.WriteLine(e);
                 cReader.Close();
                 return 1; }
            

         }

        public static long RetNextValuelng(OdbcConnection mKon, string TableName, string FieldName, string sFilter)
        {
            long RetNextValuelng;

            OdbcCommand csql;
            OdbcDataReader cReader;

            csql = new OdbcCommand("select MAX(" + FieldName + ") AS maks from " + TableName + " where " + sFilter + " ", mKon);
            cReader = csql.ExecuteReader();

            try
            {
                cReader.Read();
                if (cReader.GetValue(0) == null)
                {
                    RetNextValuelng = 1;
                }
                else
                {
                    RetNextValuelng = long.Parse(cReader.GetValue(0).ToString());
                }
                cReader.Close();
                RetNextValuelng = RetNextValuelng + 1;
                return RetNextValuelng;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cReader.Close();
                return 1;
            }


        }

        //FORMA = IZGLED cred LOKACIJA NA EKRANU
        public static  void FormLayout(Form Forma,  moduli.variable.constRW RW , String Prefix){
            string fd,f,s;
            string[]  a;
            Double  T , l , h ;
            //Form frm ;
            Form frm = Forma;

            string cDirektorijum = forme.frmSetup.cLokacijaTempDir.ToString() ;
            if (cDirektorijum=="") cDirektorijum="temp";
            fd = @"c:\\" + cDirektorijum + "\\";
            f = @"c:\\" + cDirektorijum + "\\" + Prefix + frm.Name + ".ini";
            string cDir = fd;
            string cFile = f;
            cDir = cDir.Trim();

           
            bool bPostojiDir = Directory.Exists(cDir) ? true : false;
            if (bPostojiDir==false){
                Directory.CreateDirectory(cDir);
            }

            bool bPostojiFajl = File.Exists(cFile) ? true : false;
            if (bPostojiFajl == false) {
                using (FileStream fs = File.Create(cFile, 1024))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes("183;200;549");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            } 


            if(RW ==  moduli.variable.constRW.dsRead)
            {

                StreamReader sr = new StreamReader(cFile);
                s = sr.ReadLine();
                sr.Close();
                char[] delimiterChar = { ';' };
                a = s.Split(delimiterChar);
                T = double.Parse(a[0]);
                l = double.Parse(a[1]);
                h = double.Parse(a[2]);
                frm.Top = int.Parse(T.ToString());
                frm.Left = int.Parse(l.ToString());
                frm.Height = int.Parse(h.ToString());
                   
            }
            else
            {
                T = frm.Top;
                l = frm.Left;
                h = frm.Height;
                s = T + ";" + l + ";" + h;
                StreamWriter sw = new StreamWriter(cFile);
                sw.WriteLine(s);
                sw.Close();
                }

            
           

        
     }

        public static string MySqlBr (string cold) {

            string[] a;
            string cformatold; string cnew; double cDblOld; string cStrOld;
            cDblOld = Double.Parse(cold);
            cStrOld = cDblOld.ToString("##########0.00");
            cformatold = cStrOld.Replace(",", ".");
            cnew = cformatold;
            return cnew;
     
        }
        public static string MySqlBr4D(string cold)
        {

            string[] a;
            string cformatold; string cnew; double cDblOld; string cStrOld;
            cDblOld = Double.Parse(cold);
            cStrOld = cDblOld.ToString("##########0.0000");
            cformatold = cStrOld.Replace(",", ".");
            cnew = cformatold;
            return cnew;

        }            

        public static string FiskalDoubleBr(string cold)
        {

            string[] a;
            string cformatold; string cnew; double cDblOld; string cStrOld; 
            cDblOld = Double.Parse(cold);
            cStrOld = cDblOld.ToString("##########0.00");
            cformatold = cStrOld.Replace(",", ".");
            cnew = cformatold;
            return cnew;
        }

        public static string SQLuslov ()
        {
            string cUslov="if";
            if (variable.DbTipBaze != variable.DbTip.Mysql ) {
                cUslov = "iif";
            }

            return cUslov;
        
        }

        public static  string DirektorijumPut(string cImeDir)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, cImeDir);
                       
            
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
            
        public static string NazivMjeseca(int Mjesec) {
            string cNazivMjeseca=null;
            switch(Mjesec){
                    case 1:
                         cNazivMjeseca = "Januar";
                    break;
                    case 2:
                        cNazivMjeseca = "Februar";
                    break ;
                    case 3:
                        cNazivMjeseca = "Mart";
                    break ;
                    case 4:
                        cNazivMjeseca = "April";
                    break ;
                    case 5:
                        cNazivMjeseca = "Maj";
                    break ;
                    case 6:
                        cNazivMjeseca = "Juni";
                    break ;
                    case 7:
                        cNazivMjeseca = "Juli";
                    break;
                    case 8:
                        cNazivMjeseca = "Avgust";
                    break;
                    case 9:
                        cNazivMjeseca = "Septembar";
                    break;
                    case  10:
                        cNazivMjeseca = "Oktobar";
                    break;
                    case 11:
                        cNazivMjeseca = "Novembar";
                    break;
                    case 12:
                        cNazivMjeseca = "Decembar";
                    break;
                }
            return cNazivMjeseca;
        }

        public static int ZadnjiDan(int Mjesec) {
            string a; DateTime d; int b;
            a = "01." + String.Format("{0:00}",Mjesec + 1) + "." + DateTime.Now.Year;
            d=DateTime.Parse(a);
            d=d.AddDays(-1);//a = DateAdd("d", -1, a);
            b=(d.Day);
            return b;
        }

        public static string SQLDate(DateTime datum)
        {

            DateTime dat;
            dat=Convert.ToDateTime(datum);
            string cRet="";

            switch (variable.DbTipBaze)
            {
                case variable.DbTip.Access:
                    cRet = "#" + dat.Month + "/" + dat.Day + "/" + dat.Year + "#";
                    break;
                case variable.DbTip.Mysql:
                    cRet = "'" + dat.Year + "-" + dat.Month + "-" + dat.Day + "'";
                    break;
            }   
            
            return cRet ;
        
        
        }

        public static DateTime PrviDanMjeseca(DateTime datum)
        {
            return new DateTime(datum.Year, datum.Month, 1);
        }

        public static DateTime ZadnjiDanMjeseca(DateTime datum)
        {
            DateTime prviDanMjeseca = new DateTime(datum.Year, datum.Month, 1);
            return prviDanMjeseca.AddMonths(1).AddDays(-1);
        }

        public static string SQLUslov() {
            string cRet = "";
            switch (variable.DbTipBaze)
            {
                case variable.DbTip.Access:
                    cRet = "iif";
                    break;
                case variable.DbTip.Mysql:
                    cRet = "IF";
                    break;
            }

            return cRet;
        }

       
        public static void PrintToMsWord (string cSQL,string cMainTabela,string CSQLstampaTB, string cNaslovDokumenta,string cKupac, 
            long cBrojRacuna,long cBrojFisRacuna,DateTime cReportDatum, bool bIzSetupaLoco){
            //ukljuci na referencama na COM tabu Microsoft Word 12 library
            Object oMissing = System.Reflection.Missing.Value;

            Object oTemplatePath = "C:\\Faktura.dot";
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document wDoc = new Document();
            wDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
            int i = 0;
            int cBroj = wDoc.Shapes.Count;
            for (i = 1; i<=wDoc.Shapes.Count; i++)
            {
                /*nadji ove informacije cPubDatum,cPubBroj,bIzSetupa,cOpstina,cImeGradaIzSetupa
                 * strtanice
                 * http://www.mindstick.com/Articles/a25ba73f-324d-4926-93b5-89460f77621d/
                 * http://www.codeshode.com/2010/07/create-edit-word-document-using-c-vbnet.html
                 * http://vivekcek.wordpress.com/2012/08/25/create-a-word-document-from-a-template-using-c-mail-merge/
                */
                switch (wDoc.Shapes[i].AlternativeText) { 
                    case "faktura":
                        wDoc.Shapes[i].TextFrame.TextRange.Text = cNaslovDokumenta;
                        break;
                    case "mjdatum":
                        wDoc.Shapes[i].TextFrame.TextRange.Text = cReportDatum.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "valuta":
                        wDoc.Shapes[i].TextFrame.TextRange.Text = cReportDatum.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case  "isporuka":
                        wDoc.Shapes[i].TextFrame.TextRange.Text = cReportDatum.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "broj":
                        if( moduli.variable.cFiskalnaKasa == true) {
                            wDoc.Shapes[i].TextFrame.TextRange.Text = "BROJ: " + cBrojRacuna + "/" + cReportDatum.Year + " BF: " + cBrojFisRacuna;
                        }
                        else {
                            wDoc.Shapes[i].TextFrame.TextRange.Text = "BROJ: " + cBrojRacuna + "/" + cReportDatum.Year + "";
                        }
                        break;
                        
                    case "dodatno":
                        wDoc.Shapes[i].TextFrame.TextRange.Text = "neki naši uslovi";
                        break;
                    case "tuzlatuzla":
                        string  cImeGradaIzSetupa;
                        cImeGradaIzSetupa = RetFieldValue("string",moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "opstine", "naziv", "sifra='" + moduli.variable.cOpstina + "'");
                        string cCijeliText ;
                        cCijeliText = "Mjesto i datum : " + cImeGradaIzSetupa;
                        wDoc.Shapes[i].TextFrame.TextRange.Text = cCijeliText;
                        break;
                    case "narudzba":
                        break;
                
                }

            }

                string cKupacIme="";string cIDbroj="";string cPDVbroj="";string cMjesto="";
                OdbcDataAdapter  AdKupac = new OdbcDataAdapter("SELECT * FROM kupci WHERE Sifra='" + cKupac + "'",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet DsKupac=new DataSet();
                AdKupac.Fill (DsKupac,"kupci");

                if(DsKupac.Tables["kupci"].Rows.Count != 0 ){
                    cKupacIme=DsKupac.Tables["kupci"].Rows[0]["naziv"].ToString();
                    cIDbroj=DsKupac.Tables["kupci"].Rows[0]["ID"].ToString();
                    cPDVbroj=DsKupac.Tables["kupci"].Rows[0]["pdvbroj"].ToString();
                    cMjesto=DsKupac.Tables["kupci"].Rows[0]["adresa"].ToString() + " , " + RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,"opstine","grad","sifra='" + DsKupac.Tables["kupci"].Rows[0]["opstina"].ToString() + "'") ;
                
                }
                DsKupac.Dispose();
                AdKupac.Dispose();

                wDoc.Tables[1].Rows[1].Cells[1].Range.Text = cKupacIme;
                wDoc.Tables[1].Rows[2].Cells[1].Range.Text= "ID/PDV : " + cIDbroj + " / " + cPDVbroj;
                wDoc.Tables[1].Rows[3].Cells[1].Range.Text = "Adresa : " + cMjesto;
                //wDoc.Tables[1].Rows[4].Cells[1].Range.Text = cTel
                //wDoc.Tables[1].Rows[5].Cells[1].Range.Text = cAttn


                OdbcDataAdapter AdMain = new OdbcDataAdapter(cSQL,bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet DsMain = new DataSet();
                AdMain.Fill(DsMain,cMainTabela);
                if (DsMain.Tables[cMainTabela].Rows.Count != 0 ) {
                    
                    int cI = 0;double cSumaUkupno=0;int cred=0;string cSerNum="";
                    long cCount= DsMain.Tables[cMainTabela].Rows.Count;
                    cred = 3;
                    for (cI = 0; cI <= cCount - 1; cI++)
                    {
                        if (cred > 3)
                        {
                            wDoc.Tables[2].Rows.Add();
                        }

                        switch (cNaslovDokumenta)
                        {

                            case "PREDRAČUN":
                                double cKol = 0; double cuvk = 0; double ciznos = 0;
                                wDoc.Tables[2].Rows[cred].Cells[1].Range.Text = "" + (cred - 2) + ".";
                                wDoc.Tables[2].Rows[cred].Cells[2].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["naziv"].ToString() + cSerNum;
                                wDoc.Tables[2].Rows[cred].Cells[3].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["jm"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[4].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["kolicina"].ToString());
                                cKol = Double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["kolicina"].ToString());
                                cuvk = Double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["uvk"].ToString());
                                ciznos = 0; ciznos = cuvk / cKol;
                                wDoc.Tables[2].Rows[cred].Cells[5].Range.Text = string.Format("{0:0,0.00}", ciznos.ToString());
                                ciznos = 0; ciznos = (cuvk / cKol) * cKol;
                                wDoc.Tables[2].Rows[cred].Cells[6].Range.Text = string.Format("{0:0,0.00}", ciznos.ToString());
                                wDoc.Tables[2].Rows[cred].Cells[7].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["rabat"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[8].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["iznosr"].ToString());
                                ciznos = 0; ciznos = Double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["uvr"].ToString()) - Double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznospr"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[9].Range.Text = string.Format("{0:0,0.00}", ciznos.ToString());
                                wDoc.Tables[2].Rows[cred].Cells[10].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["pdv"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[11].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["iznospr"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[12].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["uvr"].ToString());
                                cSumaUkupno = cSumaUkupno + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["uvr"].ToString());
                                break;
                            case "FAKTURA":
                                wDoc.Tables[2].Rows[cred].Cells[1].Range.Text = "" + (cred - 2) + ".";
                                wDoc.Tables[2].Rows[cred].Cells[2].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["nazivrobe"].ToString() + cSerNum;
                                wDoc.Tables[2].Rows[cred].Cells[3].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["jm"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[4].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["kol"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[5].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["cijena"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[6].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString());
                                if (double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()) == 0)
                                {
                                    wDoc.Tables[2].Rows[cred].Cells[7].Range.Text = string.Format("{0:0,0.00}", 0);
                                    wDoc.Tables[2].Rows[cred].Cells[8].Range.Text = string.Format("{0:0,0.00}", 0);
                                }
                                else
                                {
                                    double a = 100 - (((double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString())) * 100) / double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()));
                                    wDoc.Tables[2].Rows[cred].Cells[7].Range.Text = string.Format("{0:0,0.00}", a);
                                    double b = double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()) - (double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString()));
                                    wDoc.Tables[2].Rows[cred].Cells[8].Range.Text = string.Format("{0:0,0.00}", b);
                                }

                                wDoc.Tables[2].Rows[cred].Cells[9].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[10].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["porezp"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[11].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString());
                                double c = double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[12].Range.Text = string.Format("{0:0,0.00}", c);
                                cSumaUkupno = cSumaUkupno + c;
                                break;
                            case "AVANSNA FAKTURA":
                                wDoc.Tables[2].Rows[cred].Cells[1].Range.Text = "" + (cred - 2) + ".";
                                wDoc.Tables[2].Rows[cred].Cells[2].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["nazivrobe"].ToString() + cSerNum;
                                wDoc.Tables[2].Rows[cred].Cells[3].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["jm"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[4].Range.Text = DsMain.Tables[cMainTabela].Rows[cI]["kol"].ToString();
                                wDoc.Tables[2].Rows[cred].Cells[5].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["cijena"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[6].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString());
                                if (double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()) == 0)
                                {
                                    wDoc.Tables[2].Rows[cred].Cells[7].Range.Text = string.Format("{0:0,0.00}", 0);
                                    wDoc.Tables[2].Rows[cred].Cells[8].Range.Text = string.Format("{0:0,0.00}", 0);
                                }
                                else
                                {
                                    double a = 100 - (((double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString())) * 100) / double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()));
                                    wDoc.Tables[2].Rows[cred].Cells[7].Range.Text = string.Format("{0:0,0.00}", a);
                                    double b = double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["iznos"].ToString()) - (double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString()));
                                    wDoc.Tables[2].Rows[cred].Cells[8].Range.Text = string.Format("{0:0,0.00}", b);
                                }

                                wDoc.Tables[2].Rows[cred].Cells[9].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[10].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["porezp"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[11].Range.Text = string.Format("{0:0,0.00}", DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString());
                                double c1 = double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["osnovica"].ToString()) + double.Parse(DsMain.Tables[cMainTabela].Rows[cI]["porezi"].ToString());
                                wDoc.Tables[2].Rows[cred].Cells[12].Range.Text = string.Format("{0:0,0.00}", c1);
                                cSumaUkupno = cSumaUkupno + c1;
                                break;
                                
                        }

                        cred++;


                    }

                    AdMain.Dispose();
                    DsMain.Dispose();

                    //Dodati redove za ukupno i vrijednost
                    wDoc.Tables[2].Rows.Add();
                    wDoc.Tables[2].Rows[cred].Cells[4].Merge(wDoc.Tables[2].Rows[cred].Cells[1]);
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Text = "UKUPNO SA PDV:";
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Name = "Arial";
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Size = 8;
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Bold = 1;
                    wDoc.Tables[2].Rows[cred].Cells[9].Range.Text = string.Format("{0:0,0.00}", cSumaUkupno);
                    wDoc.Tables[2].Rows[cred].Cells[9].Range.Font.Name = "Arial";
                    wDoc.Tables[2].Rows[cred].Cells[9].Range.Font.Size = 8;
                    wDoc.Tables[2].Rows[cred].Cells[9].Range.Font.Bold = 1;

                    wDoc.Tables[2].Rows[cred].Cells[1].Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    cred++;

                    wDoc.Tables[2].Rows.Add();
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Name = "Arial";
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Size = 8;
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Font.Bold = 1;
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.Text = "VRIJEDNOST:";
                    wDoc.Tables[2].Rows[cred].Cells[1].Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    wDoc.Tables[2].Rows[cred].Cells[3].Merge(wDoc.Tables[2].Rows[cred].Cells[2]);
                    cred++;

                    wDoc.Tables[2].Rows[1].Borders[WdBorderType.wdBorderBottom].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;
                
                }

                if (cNaslovDokumenta == "PREDRAČUN") {
                    wDoc.Tables[3].Delete();}
                else
                {
                    OdbcDataAdapter AdTarife =new OdbcDataAdapter(CSQLstampaTB,bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet DsTarife = new DataSet();
                    AdTarife.Fill(DsTarife,cMainTabela);
                    int cRed1 =2;
                    if (DsTarife.Tables[0].Rows.Count !=0){
                        for (i=0;i<=DsTarife.Tables[0].Rows.Count-1;i++){
                            if (cRed1 > 2)
                            {
                                wDoc.Tables[3].Rows.Add();
                            }

                            wDoc.Tables[3].Rows[cRed1].Cells[1].Range.Text = DsTarife.Tables[cMainTabela].Rows[i]["opis"].ToString();
                            wDoc.Tables[3].Rows[cRed1].Cells[2].Range.Text = string.Format("{0:0,0.00}", DsTarife.Tables[cMainTabela].Rows[i]["osnovica"].ToString());
                            wDoc.Tables[3].Rows[cRed1].Cells[3].Range.Text = string.Format("{0:0,0.00}", DsTarife.Tables[cMainTabela].Rows[i]["porezp"].ToString()) + " %";
                            wDoc.Tables[3].Rows[cRed1].Cells[4].Range.Text = string.Format("{0:0,0.00}", DsTarife.Tables[cMainTabela].Rows[i]["porezi"].ToString());
                            cRed1++;
                        }
                    
                    
                    }
                    AdTarife.Dispose();
                    DsTarife.Dispose();

                }



                if (cNaslovDokumenta == "PREDRAČUN") {
                    wDoc.Tables[3].Delete();}
                else
                {
                    string mA; string mstrNapomenaNaFakturi; string mstrNapomenaoKupcu = "";
                        
                    mstrNapomenaNaFakturi = RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "podesenjacjenovnika", "dodatnitekstmpfakture", "dodatnitekstmpfakture<>''");
                    mA = RetFieldValue("string",bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "kupci", "napomenanafakturi", "sifra='" + cKupac + "'");
                    if (mA != "nema trazene vrijednosti") {
                        mstrNapomenaoKupcu = "Napomena o kupcu : " + RetFieldValue("string", bIzSetupaLoco == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "kupci", "napomena", "sifra='" + cKupac + "'");
                    }else{
                        //wDoc.Tables(4).Delete
                    }
                    wDoc.Tables[4].Rows[1].Cells[1].Range.Text =  mstrNapomenaoKupcu + "\r\n" + mstrNapomenaNaFakturi;
                }

            
                long nBrojStrana = wDoc.ComputeStatistics( Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages);
                
                while (true){
                    wDoc.Paragraphs[wDoc.Paragraphs.Count - 4].Range.Text = "\r\n" +"\r\n";   
                    if (wDoc.ComputeStatistics( Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages) != nBrojStrana) {
                        wDoc.Undo();
                        break;
                    }
                
                }

                wordApp.Visible = true;
                wordApp.Activate();
                wordApp.Activate();
            
            }


        #region "Ne koristim"

        public static string _MySQLDate(DateTime  datum){
            DateTime dat;
            dat=Convert.ToDateTime(datum);
            string cRet;
            cRet = "'" + dat.Year  + "-" + dat.Month  + "-" + dat.Day + "'";
            return cRet ;
        }

        public static string _AccSQLDate(DateTime datum)
        {
            DateTime dat;
            dat = Convert.ToDateTime(datum);
            string cRet;
            cRet = "#" + dat.Month + "/" + dat.Day + "/" + dat.Year + "#";
            return cRet;
        }

        #endregion



    }



  

}
