using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Poslovanje.moduli
{
    public class kon
    {
        public static  string cKonString = "";

        public static void osnovnakonektuj() {
            try
            {
                

                if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql)
                {
                    //cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=localhost;Port=3306;Option=131072;Database=FIRMA;Uid=root;";
                    cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Port=3306;Option=131072;Database=" + variable.cNazivOsnovneBaze + "; Uid=" + variable.cUser + ";";
                }
                else
                {
                    string cB=variable.cNazivServera + variable.cNazivOsnovneBaze;
                    cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + cB.Replace((char)47, (char)92) + ";";

                }
                variable.mKonekcijaOsnovna = new System.Data.Odbc.OdbcConnection(cKonString);
                variable.mKonekcijaOsnovna.Open();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            
            }

        public static void osnovnakomandnakonektuj()
        {
            try
            {
                if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql)
                {
                    //cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Database=" + variable.cNazivOsnovneBaze + "; User=" + moduli.variable.cUser + ";password=" + moduli.variable.cLozinka + ";Option=3;CharSet=cp1250;";
                    cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Port=3306;Option=131072;Database=" + variable.cNazivOsnovneBaze + "; Uid=" + variable.cUser + ";";
                }
                else
                {
                    string cB = variable.cNazivServera + variable.cNazivOsnovneBaze;
                    cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + cB.Replace((char)47, (char)92) + ";";
                    //cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + variable.cNazivServera + variable.cNazivOsnovneBaze + ";";
                }
                variable.mKonekcijaOsnovnaKomandna = new System.Data.Odbc.OdbcConnection(cKonString);
                variable.mKonekcijaOsnovnaKomandna.Open();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }

        }

        public static void konektuj()
        {
            try
            {
                moduli.variable.cGreskaKonekcije = false;
                if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql)
                {
                    //cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Database=" + variable.cNazivBaze + "; User=" + moduli.variable.cUser + ";password=" + moduli.variable.cLozinka + ";Option=3;CharSet=cp1250;";
                    cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Port=3306;Option=131072;Database=" + variable.cNazivBaze + "; Uid=" + variable.cUser + ";";
                }
                else
                {
                    string cB = variable.cPutanjaDoBaze  + variable.cNazivBaze;
                    cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + cB.Replace((char)47, (char)92) + ";";
                    //cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + variable.cNazivBaze + ";";
                }
                variable.mKonekcija = new System.Data.Odbc.OdbcConnection(cKonString);
                variable.mKonekcija.Open();
            }
            catch (Exception e) { moduli.variable.cGreskaKonekcije = true; //MessageBox.Show(e.ToString()); 
            }

        }

        public static void komandnakonektuj()
        {
            try
            {
                moduli.variable.cGreskaKonekcije = false;
                if (moduli.variable.DbTipBaze == moduli.variable.DbTip.Mysql)
                {
                    //cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Database=" + variable.cNazivBaze + "; User=" + moduli.variable.cUser + ";password=" + moduli.variable.cLozinka + ";Option=3;CharSet=cp1250;";
                    cKonString = "Driver={MySQL ODBC 5.1 Driver};Server=" + variable.cNazivServera + ";Port=3306;Option=131072;Database=" + variable.cNazivBaze + "; Uid=" + variable.cUser + ";";
                }
                else
                {
                    string cB = variable.cPutanjaDoBaze + variable.cNazivBaze;
                    cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + cB.Replace((char)47, (char)92) + ";";
                    //cKonString = "Driver={Microsoft Access Driver (*.mdb)};DBQ=" + variable.cNazivBaze + ";";
                }
                variable.mKonekcijaKomandna = new System.Data.Odbc.OdbcConnection(cKonString);
                variable.mKonekcijaKomandna.Open();
            }
            catch (Exception e) { moduli.variable.cGreskaKonekcije = true; //MessageBox.Show(e.ToString());
            }

        }
  
          
    }  
}
