using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Data;
namespace Poslovanje.moduli
{
    class fiskal
    {
        #region "fiskalnareklamacija"
        public static void fiskalnareklamacijaTremolXml(long nBrojRacuna, long nFiskalnirac, string mTabela, string mTabela1) { }
        public static void fiskalnareklamacijaDatecsTxt(long nBrojDok, long nFiskalnirac, string mTabela, string mTabela1)
        {


            try
            {
                string cSQL = ""; string cUkupanIznosString = null; string cIznosRacunaDisplay = null;
                if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                {
                    cSQL = "select " + mTabela + ".*,roba.fiskalnasifra from " + mTabela + " left join roba on " + mTabela + ".sifrarobe=roba.sifra " +
                    "where vrstadok = 1 and brojdok=" + nBrojDok + "";
                }
                else
                {
                    //cSQL = "select " + mTabela + ".*,roba.fiskalnasifra from " + mTabela + " left join roba on " + mTabela + ".sifrarobe=roba.sifra " +
                    //"where vrstadok = 1 and brojracuna=" + nBrojRacuna + "";
                    if (moduli.variable.nFiskalnaFakturaGK == true)
                    {
                        cSQL = "select fs.Cijena,fs.Kolicina as Kol, fs.iznosf as iznos, roba.fiskalnasifra from " + mTabela + " fs " +
                            "Left Join roba on fs.roba=roba.sifra where fs.Broj=" + nBrojDok + "";
                    }
                    else
                    {

                        cSQL = "select " + mTabela + ".*,roba.fiskalnasifra from " + mTabela + " left join roba on " + mTabela + ".sifrarobe=roba.sifra where  " +
                        "vrstadok = 1 and brojracuna=" + nBrojDok + "";
                    }
                }

                OdbcDataAdapter adFisRac = new OdbcDataAdapter(cSQL, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet dsFisRac = new DataSet();
                adFisRac.Fill(dsFisRac, mTabela);
                if (dsFisRac.Tables[mTabela].Rows.Count != 0)
                {
                    int cVrstaPlacanja = 0; int cUsrFisSifra; int cUsrFisPass;// double cIznosBaza;
                    bool cFisRacPop; double cIzFisRacPop=0; //double cIspravikol; //double cUkupaniznosBaza = 0;
                    string cIzFisRacPopStr="";
                    string cSQL1 = "";
                    //string cMinusSifra = "";
                    //string cUkupanIznosString = "";

                    //cVrstaPlacanja = int.Parse(dsFisRac.Tables[mTabela].Rows[0]["PLACENO"].ToString()) - 1; //rsFisRac!PLACENO - 1
                    //if (cVrstaPlacanja == 4) cVrstaPlacanja = 3;
                    if (moduli.variable.nFiskalnaFakturaGK == true)
                    {
                        cVrstaPlacanja = int.Parse(moduli.funkcije.RetFieldValue("int", moduli.variable.bIzSetupa == true ?
                                          moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                          mTabela1, "placeno", "Broj=" + nBrojDok + "")) - 1;
                    }
                    else
                    { cVrstaPlacanja = int.Parse(dsFisRac.Tables[mTabela].Rows[0]["PLACENO"].ToString()) - 1; }
                    // fiskal gotovina=0 itd ...
                    if (cVrstaPlacanja == 4) cVrstaPlacanja = 3;

                    cUsrFisSifra = int.Parse(moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "korisnici", "fiskalnasifra", "sifra='" + moduli.variable.cUserProgram + "'"));
                    cUsrFisPass = int.Parse(moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "korisnici", "fiskalnipass", "sifra='" + moduli.variable.cUserProgram + "'"));


                    cFisRacPop = false;
                    if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                    {
                        cSQL1 = "SELECT * From " + mTabela + "  " +
                         "Where " + mTabela + ".VrstaDok=3 AND brojdok=" + nBrojDok + ";";
                    }
                    else
                    {
                        //cSQL1 = "SELECT * From " + mTabela + "  " +
                        // "Where " + mTabela + ".VrstaDok=3 AND brojracuna=" + nBrojRacuna + ";";
                        if (moduli.variable.nFiskalnaFakturaGK == true)
                        {
                            cSQL1 = "select fs.Cijena,fs.Kolicina as Kol, fs.iznosf as iznos, fs.RabatP , roba.fiskalnasifra from " + mTabela + " fs " +
                                "Left Join roba on fs.roba=roba.sifra where fs.Broj=" + nBrojDok + " and RabatP<>0";
                        }
                        else
                        {
                            cSQL1 = "SELECT * From " + mTabela + "  " +
                            "Where " + mTabela + ".VrstaDok=3 AND " + mTabela + ".brojracuna = " + nBrojDok + ";";
                        }
                    }

                    OdbcDataAdapter adFisRacPop = new OdbcDataAdapter(cSQL1, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    DataSet dsFisRacPop = new DataSet();
                    adFisRacPop.Fill(dsFisRacPop, mTabela);
                    if (dsFisRacPop.Tables[mTabela].Rows.Count != 0)
                    {
                        cFisRacPop = true;
                        //cIzFisRacPop = 100 - (Math.Round((Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["iznos"].ToString()) * 100) / (Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["kol"].ToString()) * Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["cijena"].ToString())), 2));
                        double dIznos = Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["iznos"].ToString());
                        double dKol = Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["kol"].ToString());
                        double dCijena = Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["cijena"].ToString());
                        cIzFisRacPop = 100 - (Math.Round((dIznos * 100) / (dKol * dCijena), 2));
                        if (moduli.variable.nFiskalnaFakturaGK == true)
                        { cIzFisRacPop = double.Parse(dsFisRacPop.Tables[mTabela].Rows[0]["RabatP"].ToString()); }
                        
                        cIzFisRacPopStr = "-" + cIzFisRacPop;

                        cSQL1 = "";
                        adFisRacPop = null;
                        dsFisRacPop = null;

                    }


                    StreamWriter sw = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "1.txt");

                    long RowCount = 0;
                    sw.WriteLine("48,1,______,_,__;" + moduli.variable.cIOSA + ";" + cUsrFisSifra + ";" + cUsrFisPass + ";");

                    int cRec = dsFisRac.Tables[mTabela].Rows.Count;
                    double cIznosBaza = 0; double cUkupaniznosBaza = 0;
                    for (int i = 0; i <= cRec - 1; i++)
                    {
                        if (cFisRacPop == false)
                        {


                            double mKol = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["kol"].ToString());
                            double mCijena = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["cijena"].ToString());
                            string cFisSif = dsFisRac.Tables[mTabela].Rows[i]["fiskalnasifra"].ToString();

                            if (mKol >= 0)
                            {
                                //Replace(Format(cIspravikol, "###0.00"), ",", ".", , , vbTextCompare)
                                sw.WriteLine("52,1,______,_,__;" + cFisSif + ";" + mKol.ToString("######0.00") + ";");
                                cIznosBaza = Math.Round(mCijena * mKol, 2);
                            }
                            else
                            {
                                string cMinusSifra = "-" + cFisSif;
                                double cIspravikol = mKol * -1;
                                sw.WriteLine("52,1,______,_,__;" + cMinusSifra + ";" + cIspravikol.ToString("######0.00") + ";");
                            }
                            cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                        }
                        else
                        {
                            double mKol = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["kol"].ToString());
                            double mCijena = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["cijena"].ToString());
                            string cFisSif = dsFisRac.Tables[mTabela].Rows[i]["fiskalnasifra"].ToString();

                            sw.WriteLine("52,1,______,_,__;" + cFisSif + ";" + mKol.ToString("#######0.00") + ";" + cIzFisRacPopStr + ";");
                            cIznosBaza = Math.Round((mCijena * mKol) - ((mCijena * mKol) * cIzFisRacPop / 100), 2);
                            cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                        }
                    }

                    sw.WriteLine("51,1,______,_,__;");
                    cUkupanIznosString = cUkupaniznosBaza.ToString("#######0.00");

                    cIznosRacunaDisplay = "";
                    cIznosRacunaDisplay = cUkupanIznosString;

                    // vrsta plaćanja
                    if (cVrstaPlacanja == 0)
                    {
                        sw.WriteLine("53,1,______,_,__;" + cVrstaPlacanja + ";");
                    }
                    else
                    {
                        sw.WriteLine("53,1,______,_,__;" + cVrstaPlacanja + ";" + cUkupanIznosString + ";");
                    }

                    // kupac
                    //string cKupacSifra = null;
                    //if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                    //{
                    //    cKupacSifra = "";
                    //}
                    //else
                    //{
                    //    if (moduli.variable.nFiskalnaFakturaGK == false)
                    //    {
                    //        cKupacSifra = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        "kupci_promet", "kupac", "racun=" + nBrojDok + "");
                    //    }
                    //    else
                    //    {
                    //        cKupacSifra = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        mTabela1, "partner", "Broj=" + nBrojDok + "");

                    //    }

                    //}


                    //if (cKupacSifra != "nema trazene vrijednosti")
                    //{


                    //    string cKupacID = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        "kupci", "id", "sifra='" + cKupacSifra + "'");
                    //    if (cKupacID == "nema trazene vrijednosti")
                    //    {
                    //        cKupacID = "1234567890123";
                    //    }
                    //    else
                    //    {
                    //        if (cKupacID.Length > 12)
                    //        {
                    //            cKupacID = cKupacID.Substring(0, 13);
                    //        }
                    //    }

                    //    string cKupacIme = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        "kupci", "naziv", "sifra='" + cKupacSifra + "'");
                    //    if (cKupacIme == "nema trazene vrijednosti")
                    //    {
                    //        cKupacIme = "-";
                    //    }
                    //    else
                    //    {
                    //        if (cKupacIme.Length > 35)
                    //        {
                    //            cKupacIme = cKupacIme.Substring(0, 36);
                    //        }
                    //    }

                    //    string cKupacUlica = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        "kupci", "adresa", "sifra='" + cKupacSifra + "'");
                    //    if (cKupacUlica == "nema trazene vrijednosti")
                    //    {
                    //        cKupacUlica = "-";
                    //    }
                    //    else
                    //    {
                    //        if (cKupacUlica.Length > 35)
                    //        {
                    //            cKupacUlica = cKupacUlica.Substring(0, 36);
                    //        }
                    //    }

                    //    string cKupacSud = moduli.funkcije.RetFieldValue("string", moduli.variable.bIzSetupa == true ?
                    //                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                    //                        "kupci", "sud", "sifra='" + cKupacSifra + "'");
                    //    if (cKupacSud == "nema trazene vrijednosti")
                    //    {
                    //        cKupacSud = "-";
                    //    }
                    //    else
                    //    {
                    //        if (cKupacSud.Length > 35)
                    //        {
                    //            cKupacSud = cKupacSud.Substring(0, 36);
                    //        }
                    //    }

                    //    sw.WriteLine("55,1,______,_,__;" + cKupacID + ";" + cKupacIme + ";" + cKupacUlica + ";" + cKupacSud + ";");
                    //}

                    // zatvaranje
                    sw.WriteLine("56,1,______,_,__;");
                    sw.Close();
                    //OdbcCommand adCommFisRac = new OdbcCommand(cSQL, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                    //OdbcDataReader myReaderFisRac;
                    //myReaderFisRac = adCommFisRac.ExecuteReader();

                    //if (myReaderFisRac.HasRows)
                    //{
                        
                        
                    //    sw.WriteLine("48,1,______,_,__;" + moduli.variable.cIOSA + ";" + cUsrFisSifra + ";" + cUsrFisPass + ";"+
                    //                    moduli.variable.cProdavnica+";"+nFiskalnirac+";" );
                    //    while (myReaderFisRac.Read())
                    //    {
                    //        if (cFisRacPop == false)
                    //        {
                    //            if (Convert.ToDouble(myReaderFisRac["kol"]) >= 0)
                    //            {
                    //                sw.WriteLine("52,1,______,_,__;" + myReaderFisRac["fiskalnasifra"] + ";" + Convert.ToString(Math.Round(Convert.ToDouble(myReaderFisRac["kol"]), 2)) + ";");
                    //                cIznosBaza = Math.Round(Convert.ToDouble(myReaderFisRac["kol"]) * Convert.ToDouble(myReaderFisRac["cijena"]), 2);

                    //            }
                    //            else
                    //            {
                    //                cMinusSifra = "-" + myReaderFisRac["fiskalnasifra"];
                    //                cIspravikol = Math.Round(Convert.ToDouble(myReaderFisRac["kol"]), 2) * -1;
                    //                sw.WriteLine("52,1,______,_,__;" + cMinusSifra + ";" + cIspravikol.ToString() + ";");
                    //                cIznosBaza = Math.Round(Convert.ToDouble(myReaderFisRac["cijena"]), 2) * cIspravikol;
                    //            }
                    //            cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                    //        }
                    //        else
                    //        {
                    //            sw.WriteLine("52,1,______,_,__;" + myReaderFisRac["fiskalnasifra"] + ";" + Convert.ToString(Math.Round(Convert.ToDouble(myReaderFisRac["kol"]), 2)) + ";" + cIzFisRacPopStr.ToString() + ";");
                    //            cIznosBaza = Math.Round(Convert.ToDouble(myReaderFisRac["kol"]) * Convert.ToDouble(myReaderFisRac["cijena"]), 2) - ((Math.Round(Convert.ToDouble(myReaderFisRac["kol"]) * Convert.ToDouble(myReaderFisRac["cijena"]), 2)) * cIzFisRacPop / 100);
                    //            cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                    //        }

                    //    }
                    //    myReaderFisRac.Close();

                    //    sw.WriteLine("51,1,______,_,__;");//subtotal

                    //    if (moduli.variable.cHitnaFiskalnaReklamacija == false)
                    //    {
                    //        cUkupanIznosString = Convert.ToString(Math.Round(cUkupaniznosBaza, 2));

                    //        if (cVrstaPlacanja == 0)
                    //        {    //ako je gotovina
                    //            sw.WriteLine("53,1,______,_,__;0;;");
                    //        }
                    //        else
                    //        {
                    //            sw.WriteLine("53,1,______,_,__;" + cVrstaPlacanja + ";" + cUkupanIznosString + ";");
                    //        }
                    //    }
                    //    else //ukoliko je pozvana iz forme fiskalnog servisa
                    //    {
                    //        moduli.variable.cHitnaFiskalnaReklamacija = false;
                    //        sw.WriteLine("53,1,______,_,__;0;;");
                    //    }

                    //}

                    //sw.WriteLine("56,1,______,_,__;");//zatvaranje
                    //sw.Close();


                }

            }
            catch
            {
                MessageBox.Show("Došlo je do greške u čitanju fajlu iz fiskalnog printera!");
                moduli.variable.bFiskalnaGreska = true;
                StreamWriter swGreska = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "1.txt");
                swGreska.WriteLine("301,1,______,_,__;");
                swGreska.Close();
            }
        
        }
        public static void fiskalnareklamacijaMikroelektronikaInp(long nBrojRacuna, long nFiskalnirac, string mTabela, string mTabela1) { }
        public static void fiskalnareklamacija(long nBrojRacuna, long nFiskalnirac, string mTabela, string mTabela1)
            {
                if (moduli.variable.cFiskalnaKasa ==false) return;
                
                if(moduli.variable.nTremolXml==true)
                {
                    fiskalnareklamacijaTremolXml(nBrojRacuna, nFiskalnirac, mTabela, mTabela1);
                    return;
                }

                if(moduli.variable.nMikroelektronikaInp==true)
                {
                    fiskalnareklamacijaMikroelektronikaInp(nBrojRacuna, nFiskalnirac, mTabela, mTabela1);
                    return;
                }

                if (moduli.variable.nDatecsTxt == true)
                {
                    fiskalnareklamacijaDatecsTxt(nBrojRacuna, nFiskalnirac, mTabela, mTabela1);
                    return;               
                
                }
            
 
            }
        #endregion
        
        
        #region "fiskalniracun"

        public static void fiskalniracun(long nBrojDok ,string mTabela, string mTabela1) 
        {
            if (moduli.variable.cFiskalnaKasa != true) return;
            if (moduli.variable.nTremolXml == true) { fiskalniracunTremolXml(nBrojDok, mTabela,mTabela1); return; }
            if (moduli.variable.nMikroelektronikaInp==true) {fiskalniracunMikrolektronikaInp(nBrojDok, mTabela,mTabela1);return;}
            if (moduli.variable.nDatecsTxt == true) {fiskalniracunDatecsTxt(nBrojDok, mTabela, mTabela1);return;}
        }

        public static void fiskalniracunTremolXml(long nBrojDok, string mTabela, string mTabela1)
        { }
        public static void fiskalniracunMikrolektronikaInp(long nBrojDok, string mTabela, string mTabela1)
        { }

        #region " ----------------- Datecs --------------- "

        public static void fiskalniracunDatecsTxt(long nBrojDok, string mTabela, string mTabela1)
        {
            
            //****************************FISKALNO*************************************************************************
            int cVrstaPlacanja=0;string cUsrFisSifra;string cUsrFisPass;
            bool cFisRacPop=false;
            double cIzFisRacPop=0;string cIzFisRacPopStr=null;
            string cSQL="";string cUkupanIznosString=null;string cIznosRacunaDisplay=null ;

            cUsrFisSifra = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "korisnici", "fiskalnasifra", "sifra='" + moduli.variable.cUserProgram + "'");
            cUsrFisPass = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija, "korisnici", "fiskalnipass", "sifra='" + moduli.variable.cUserProgram + "'");
                
            if (moduli.variable.nFiskalnaFakturaProizvoda == true) 
            {
                cSQL = "select " + mTabela + ".*,roba.fiskalnasifra from " + mTabela + " left join roba on " + mTabela + ".sifrarobe=roba.sifra where  " +
                "vrstadok = 1 and brojdok=" + nBrojDok + "";
            }
            else
            {
                if (moduli.variable.nFiskalnaFakturaGK == true)
                {
                    cSQL = "select fs.Cijena,fs.Kolicina as Kol, fs.iznosf as iznos, roba.fiskalnasifra from "+mTabela+" fs " +
                        "Left Join roba on fs.roba=roba.sifra where fs.Broj="+nBrojDok+"";
                }
                else
                {

                    cSQL = "select " + mTabela + ".*,roba.fiskalnasifra from " + mTabela + " left join roba on " + mTabela + ".sifrarobe=roba.sifra where  " +
                    "vrstadok = 1 and brojracuna=" + nBrojDok + "";
                }
            }
            OdbcDataAdapter adFisRac = new OdbcDataAdapter(cSQL, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
            DataSet dsFisRac = new DataSet();
            adFisRac.Fill(dsFisRac, mTabela);
            if (dsFisRac.Tables[mTabela].Rows.Count != 0)
            {



                if (moduli.variable.nFiskalnaFakturaGK == true)
                {
                    cVrstaPlacanja = int.Parse(moduli.funkcije.RetFieldValue("int",moduli.variable.bIzSetupa == true ?
                                      moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                      mTabela1, "placeno", "Broj=" + nBrojDok + ""))-1;
                }
                else
                { cVrstaPlacanja = int.Parse(dsFisRac.Tables[mTabela].Rows[0]["PLACENO"].ToString())-1; }
                // fiskal gotovina=0 itd ...
                if (cVrstaPlacanja == 4) cVrstaPlacanja = 3;
                // popusti
                cFisRacPop = false;

                string cSQL1="";
                if (moduli.variable.nFiskalnaFakturaProizvoda == true) 
                {
                    cSQL1 = "SELECT * From " + mTabela + "  " + 
                        "Where " + mTabela + ".VrstaDok=3 AND " + mTabela + ".BrojDok = " + nBrojDok + ";";
                }
                else
                {
                    if (moduli.variable.nFiskalnaFakturaGK == true)
                    {
                        cSQL1 = "select fs.Cijena,fs.Kolicina as Kol, fs.iznosf as iznos, fs.RabatP , roba.fiskalnasifra from " + mTabela + " fs " +
                            "Left Join roba on fs.roba=roba.sifra where fs.Broj=" + nBrojDok + " and RabatP<>0";
                    }
                    else
                    {
                        cSQL1 = "SELECT * From " + mTabela + "  " +
                        "Where " + mTabela + ".VrstaDok=3 AND " + mTabela + ".brojracuna = " + nBrojDok + ";";
                    }
                }
                OdbcDataAdapter adFisRacPop = new OdbcDataAdapter(cSQL1, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija);
                DataSet dsFisRacPop = new DataSet();
                adFisRacPop.Fill(dsFisRacPop, mTabela);
                if (dsFisRacPop.Tables[mTabela].Rows.Count != 0)
                {
                    cFisRacPop = true; 
                    double dIznos=Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["iznos"].ToString());
                    double dKol=Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["kol"].ToString());
                    double dCijena=Convert.ToDouble(dsFisRacPop.Tables[mTabela].Rows[0]["cijena"].ToString());
                    cIzFisRacPop = 100 - (Math.Round((dIznos * 100) / (dKol * dCijena), 2));
                    if (moduli.variable.nFiskalnaFakturaGK == true)
                    { cIzFisRacPop = double.Parse(dsFisRacPop.Tables[mTabela].Rows[0]["RabatP"].ToString()); }
                    cIzFisRacPopStr = "-" + cIzFisRacPop.ToString();
                }
                adFisRacPop = null;
                dsFisRacPop = null;

            }

            ispraznifiskalniodgovorDatecsTxt();
            
            StreamWriter sw = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "1.txt");
            try
            {
               

                sw.WriteLine("48,1,______,_,__;" + moduli.variable.cIOSA + ";" + cUsrFisSifra + ";" + cUsrFisPass + ";");

                int cRec=dsFisRac.Tables[mTabela].Rows.Count;
                double cIznosBaza = 0; double cUkupaniznosBaza = 0;
                for (int i=0;i<= cRec - 1;i++)
                {
                    if (cFisRacPop == false)
                    { 


                        double mKol=double.Parse (dsFisRac.Tables[mTabela].Rows[i]["kol"].ToString());
                        double mCijena=double.Parse (dsFisRac.Tables[mTabela].Rows[i]["cijena"].ToString());
                        string cFisSif = dsFisRac.Tables[mTabela].Rows[i]["fiskalnasifra"].ToString();

                        if (mKol >=0)
                        {
                                                                    //Replace(Format(cIspravikol, "###0.00"), ",", ".", , , vbTextCompare)
                            sw.WriteLine("52,1,______,_,__;"+cFisSif+";"+mKol.ToString("######0.00")+";");
                            cIznosBaza = Math.Round (mCijena*mKol,2);
                        }
                        else
                        {
                            string cMinusSifra = "-" + cFisSif;
                            double cIspravikol = mKol * -1;
                            sw.WriteLine("52,1,______,_,__;"+cMinusSifra+";"+cIspravikol.ToString("######0.00")+";");
                        }
                        cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                    }
                    else 
                    {
                        double mKol = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["kol"].ToString());
                        double mCijena = double.Parse(dsFisRac.Tables[mTabela].Rows[i]["cijena"].ToString());
                        string cFisSif = dsFisRac.Tables[mTabela].Rows[i]["fiskalnasifra"].ToString();

                        sw.WriteLine("52,1,______,_,__;"+cFisSif+";"+mKol.ToString("#######0.00")+";"+cIzFisRacPopStr+";");
                        cIznosBaza = Math.Round((mCijena * mKol) - ((mCijena * mKol) * cIzFisRacPop / 100),2);
                        cUkupaniznosBaza = cUkupaniznosBaza + cIznosBaza;
                    }
                }

                sw.WriteLine("51,1,______,_,__;");
                cUkupanIznosString = cUkupaniznosBaza.ToString("#######0.00");

                cIznosRacunaDisplay = "";
                cIznosRacunaDisplay = cUkupanIznosString;
                
                // vrsta plaćanja
                if (cVrstaPlacanja==0)
                {
                    sw.WriteLine("53,1,______,_,__;"+ cVrstaPlacanja +";");
                }    
                else
                {
                    sw.WriteLine("53,1,______,_,__;"+ cVrstaPlacanja +";"+ cUkupanIznosString +";");
                }

                // kupac
                string cKupacSifra = null;
                if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                {
                    cKupacSifra = "";
                }
                else
                {
                    if (moduli.variable.nFiskalnaFakturaGK == false)
                    {
                        cKupacSifra = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "kupci_promet", "kupac", "racun=" + nBrojDok + "");
                    }
                    else
                    {
                        cKupacSifra = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        mTabela1, "partner", "Broj=" + nBrojDok + "");

                    }

                }


                if (cKupacSifra != "nema trazene vrijednosti")
                {
                    

                    string cKupacID = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "kupci", "id", "sifra='" + cKupacSifra + "'");
                    if (cKupacID == "nema trazene vrijednosti")
                    {
                        cKupacID = "1234567890123";
                    }
                    else
                    {
                        if (cKupacID.Length > 12)
                        {
                            cKupacID = cKupacID.Substring(0, 13);
                        }
                    }

                    string cKupacIme = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "kupci", "naziv", "sifra='" + cKupacSifra + "'");
                    if (cKupacIme== "nema trazene vrijednosti")
                    {
                        cKupacIme= "-";
                    }
                    else
                    {
                        if (cKupacIme.Length > 35)
                        {
                            cKupacIme = cKupacIme.Substring(0, 36);
                        }
                    }

                    string cKupacUlica = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "kupci", "adresa", "sifra='" + cKupacSifra + "'");
                    if (cKupacUlica == "nema trazene vrijednosti")
                    {
                        cKupacUlica = "-";
                    }
                    else
                    {
                        if (cKupacUlica.Length > 35)
                        {
                            cKupacUlica = cKupacUlica.Substring(0, 36);
                        }
                    }

                    string cKupacSud = moduli.funkcije.RetFieldValue("string",moduli.variable.bIzSetupa == true ?
                                        moduli.variable.mKonekcijaOsnovna : moduli.variable.mKonekcija,
                                        "kupci", "sud", "sifra='" + cKupacSifra + "'");
                    if (cKupacSud == "nema trazene vrijednosti")
                    {
                        cKupacSud = "-";
                    }
                    else
                    {
                        if (cKupacSud.Length > 35)
                        {
                            cKupacSud = cKupacSud.Substring(0, 36);
                        }
                    }

                    sw.WriteLine("55,1,______,_,__;" + cKupacID + ";" + cKupacIme + ";" + cKupacUlica + ";" + cKupacSud + ";");
                }

                // zatvaranje
                sw.WriteLine("56,1,______,_,__;");
                sw.Close();

                fiskalniodgovorDatecsTxt(nBrojDok, mTabela,mTabela1);
            }
            catch (Exception e)
            {
                // greška u proizvodnji fiskalnog racuna
                moduli.variable.bFiskalnaGreska = true;
                //StreamWriter sw = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "1.txt");
                try
                {
                    sw.WriteLine("301,1,______,_,__;"); sw.WriteLine("56,1,______,_,__;");
                    sw.Close();
                }catch (Exception ss){}
                
                MessageBox.Show("Došlo je do greške u pravljenju fiskalnog računa. Račun neće biti zaključen u kasi i u fiskalnom printeru !");
                
                string csql = "";
                if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                { 
                    csql="update "+mTabela+" set zakljucen =0 where FakturaProizvodnje=" + nBrojDok + "";//mp_dnevnik ili mp_promet
                    moduli.funkcije.SQLExecute(false, csql);
                    csql = "Update "+mTabela1+" set zakljuceno=0 where brojdok=" + nBrojDok + "";//mp_fakturezag
                    moduli.funkcije.SQLExecute(false, csql);
                }
                else
                {
                    if (moduli.variable.nFiskalnaFakturaGK == false)
                    {
                        csql = "Update " + mTabela + " set zakljucen=0 where brojracuna=" + nBrojDok + " and VrstaDok <> 0 And VrstaDok <> 2";
                    }
                    else
                    {
                        csql = "update "+mTabela1+" set knjizeno=0, fiskalniracun=0 where broj="+nBrojDok+"";
                    }
                    moduli.funkcije.SQLExecute(false, csql);
                }
            }
                


            adFisRac = null;
            dsFisRac = null;
                
        }

        public static void ispraznifiskalniodgovorDatecsTxt()
        {


            if (moduli.variable.cFiskalnaKasa == false) return;
            
             
             StreamWriter sw = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "2.txt");
            sw.Close(); 

         
        
        }

        public static void fiskalsinhronizirajrobuizracunaDatecsTxt(long cBrojRacuna, string mTabela) 
        {
            int cTar ; int cSifra ; string cIme ; string cCijena;double cdCijena;
            string csql = "";
            if (moduli.variable.nFiskalnaFakturaProizvoda == true)
            {
                csql = "select * from " + mTabela + " where brojdok=" + cBrojRacuna + " and vrstadok=1";
            }
            else
            {
                if (moduli.variable.nFiskalnaFakturaGK == false)
                {
                    csql = "select * from " + mTabela + " where brojracuna=" + cBrojRacuna + " and vrstadok=1";
                }
                else
                {
                    csql = "select p.roba as sifrarobe, p.tarifa as tarbr, p.cijena, r.naziv as nazivrobe from " + mTabela + " p left join roba r on " +
                        "p.roba=r.sifra where p.Broj=" + cBrojRacuna + "";
                }
            }
            OdbcCommand rsComm = new OdbcCommand(csql, moduli.variable.bIzSetupa == true ? moduli.variable.mKonekcijaOsnovnaKomandna : moduli.variable.mKonekcijaKomandna);

            if (moduli.variable.bIzSetupa == true)
            { moduli.kon.osnovnakomandnakonektuj(); rsComm.Connection = moduli.variable.mKonekcijaOsnovnaKomandna; }
            else { moduli.kon.komandnakonektuj(); rsComm.Connection = moduli.variable.mKonekcijaKomandna; }

            OdbcDataReader rsReader;
            rsReader =rsComm.ExecuteReader();
            if (rsReader.HasRows)
            {
                StreamWriter sw = new StreamWriter(moduli.variable.cPutanjaDoFiskalFajlova + "1.txt");
                while (rsReader.Read())
                {
                    cTar = int.Parse(moduli.funkcije.RetFieldValue("int", moduli.variable.mKonekcija, "tarifnibroj", "fiskalnasifra", "sifra='" + rsReader["tarbr"] + "'"));
                    cSifra = int.Parse(moduli.funkcije.RetFieldValue("int", moduli.variable.mKonekcija, "roba", "fiskalnasifra", "sifra='" + rsReader["sifrarobe"] + "'"));
                    cIme =  rsReader["nazivrobe"].ToString() ;

                    sw.WriteLine("107,1,______,_,__;1;" + cTar + ";" + cSifra + ";" + moduli.funkcije.FiskalDoubleBr(rsReader["cijena"].ToString()) + ";" + cIme + ";");
                    sw.WriteLine("107,1,______,_,__;4;" + cSifra + ";" + moduli.funkcije.FiskalDoubleBr(rsReader["cijena"].ToString()) + ";" + cIme + ";");
                    //w.WriteLine("**********. " + cCijena + "" );
                }
                rsReader.Close();
                sw.Close();

                if (moduli.variable.bIzSetupa == true)
                { moduli.variable.mKonekcijaOsnovnaKomandna.Close(); }
                else { moduli.variable.mKonekcijaKomandna.Close(); }
            
            }

        }

        public static void fiskalniodgovorDatecsTxt(long nBrojDok, string mTabela,string mTabela1)
        {
            string c = null; string s = null; bool bNasao = false;
            string cVrati = null;
            int krug = 0; StreamReader sr;
            string cFile = moduli.variable.cPutanjaDoFiskalFajlova + "2.txt";
            try
             {
                    while (krug <= moduli.variable.cFiskalDelay)
                    {
                        bool bPostojiFajl = File.Exists(cFile) ? true : false;
                        if (bPostojiFajl)
                        {
                            using (sr = File.OpenText(cFile))
                            {

                                while ((c = sr.ReadLine()) != null)
                                {
                                    s=c.Substring(0, 2);
                                    if (s == "56") bNasao = true;
                        
                                    if (bNasao == true) 
                                    {
                                        string [] cOstalo = c.Split(';');
                                        string [] cOst=cOstalo[1].Split(',');
                                        cVrati = cOst[1].ToString();
  
                                    }
                                    Console.WriteLine(c); 
                    
                                }
                                sr.Close();
                            }
                    
                            string csql = "";
                            if (cVrati != "")
                            {
                                if (moduli.variable.nFiskalnaFakturaProizvoda == true)
                                {
                                    csql = "update " + mTabela + " set fiskalniracun=" + int.Parse(cVrati) + " where brojdok=" + nBrojDok + " and VrstaDok <> 0 And VrstaDok <> 2 ";
                                }
                                else
                                {
                                    if (moduli.variable.nFiskalnaFakturaGK == false)
                                    {
                                        csql = "update " + mTabela + " set fiskalniracun=" + int.Parse(cVrati) + " where brojracuna=" + nBrojDok + " and VrstaDok <> 0 And VrstaDok <> 2 ";
                                    }
                                    else
                                    {
                                        csql = "update " + mTabela1 + " set FiskalniRacun=" + int.Parse(cVrati) + " where Broj=" + nBrojDok + "";
                                    }
                                }
                                funkcije.SQLExecute(false, csql);
                            }
                            else
                            {
                                moduli.variable.bFiskalnaGreska = true;
                                moduli.variable.bFiskalnaGreskaNijeVratioBroj = true;
                            }
                            break;

                        }
                        krug++;
                    }
                }
                catch (Exception e)
                {
                    moduli.variable.bFiskalnaGreska = true;
                    moduli.variable.bFiskalnaGreskaNijeVratioBroj = true;
                    
                }
        
        }
        
        #endregion
       

        #endregion
    }
}
