using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poslovanje.moduli
{
    public class variable
    {
          public enum DbTip {
              Access = 0,
              Mysql = 1
              };
          public enum constRW
          {
              dsRead = 0,
              dsWrite = 1
          }
        


    //public static MySql.Data.MySqlClient.OdbcConnection mKonekcijaOsnovnaKomandna;
    //public static  MySql.Data.MySqlClient.OdbcConnection mKonekcijaOsnovna;
    //public static  MySql.Data.MySqlClient.OdbcConnection mKonekcija;
    //public static MySql.Data.MySqlClient.OdbcConnection mKonekcijaKomandna;
    public static System.Data.Odbc.OdbcConnection  mKonekcijaOsnovnaKomandna;
    public static System.Data.Odbc.OdbcConnection mKonekcijaOsnovna;
    public static System.Data.Odbc.OdbcConnection mKonekcija;
    public static System.Data.Odbc.OdbcConnection mKonekcijaKomandna;
    public static bool cGreskaKonekcije;

        
    //------------------------------------------------------------------------------------------------
    //definicija osnovih varijabli
    public static string cUser;//za konekciju na server
    public static string cLozinka;//za konekciju na server
    public static string cUserProgram;//za program
    public static string cLozinkaProgram;//za program
    public static string cNazivOsnovneBaze ;
    public static string cServerskaPutanja;
    public static string cNazivServera;
    public static string cSifraFirme;
    public static string cPutanjaDoBaze;
    public static string cNazivBaze;
    public static Boolean  bIzSetupa;
    public static Boolean bIzProizvodnje;
    public static Boolean nPdv;
    public static string cProdavnica;
    public static string cAdresa;
    public static string cPoreskiBroj;
    public static string cSudskiBroj;
    public static string cPdvBroj;
    public static string cIdBroj;
    public static string cOpstina; 
    public static string cDobavljac;
    public static string cPrinterPort;
    public static string cPrinterName;
    public static string cGodina;
    public static string cIOSA;
    public static Boolean cFiskalnaKasa;
    public static Boolean nFiskalnaFakturaProizvoda;//proizvodnja
    public static Boolean nFiskalnaFakturaGK;//glavna knjiga faktura
    public static Boolean nArhivirajDnevnik;//arhiviranje dnevnika iz faktura proizvodnje
    public static Boolean bFiskalnaGreska;//greška fis.printera
    public static Boolean bFiskalnaGreskaNijeVratioBroj;//greška fis.printera
    public static long cFiskalDelay;//vrijeme odlaganja dobivanja odgovora od fis.printera
    public static string cPutanjaDoFiskalFajlova;
    public static Boolean nDatecsTxt;
    public static Boolean nTremolXml;
    public static Boolean nEsencijalniFisRacun;//samo esencijalni fiskalni račun
    public static Boolean nMikroelektronikaInp;
    public static Boolean bIskljucivoProgramKasa;// ako je True samo se može raditi u kasi
    public static Boolean bObavjestOminusuRobe;//obavijest o dopuštenom minusu
//------------------------------------------------------------------------------------------------

    public static Boolean cHitnaFiskalnaReklamacija;//hitna fiskalna reklamacija 

    public static int cPublicSifraOperatera = 1;
    public static DateTime cPublicDatumOperater;

    //koristim kod edit/add formi za prenos modusa i vrijednosti sifre kod updatea 
    
   public enum modus_rada {
            dodavanje = 0,
            izmjena = 1,
            brisanje = 2,
            pregled = 3
            }
    /*public static  modus_rada modus;*/

    public static DbTip DbTipBaze;
    public static int cEditVrijednostInt; //ako je sifra sa forme broj
    public static string cEditVrijednostString; //ako je sifra sa forme tekst

    public static string cLookUpSifra;
    public static string cLookUpNaziv;
    public static double mLookupIznosPorez;


    public static DateTime cPubDatum;
    public static long cPubBroj;

    public static bool bArhiviraniRacuni = false;//ako se radi o računima iz mp_dnevnika ili iz mp_prometa
    
    /* za reporte varijable */
    public static string mReportBrojFiskalnogRacuna;
    public static string mReportBrojRacuna;
    public static string mReportDatum;

    /* za reporte varijable */
    }
}
