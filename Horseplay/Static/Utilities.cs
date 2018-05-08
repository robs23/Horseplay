using Horseplay.DAL;
using Horseplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Horseplay.Static
{
    public static class Utilities
    {
        public static SelectList EnumToSelectList(Enum TEnum, string selectedValue = null)
        {
            Array values = Enum.GetValues(TEnum.GetType());
            List<ListItem> items = new List<ListItem>(values.Length);
            foreach (var i in values)
            {
                items.Add(new ListItem
                {
                    Text = EnumString(TEnum, (int)i),
                    Value = ((int)i).ToString(),
                });
            }
            if (selectedValue == null)
            {
                return new SelectList(items, "Value", "Text");
            }else
            {
                return new SelectList(items, "Value", "Text",selectedValue);
            }
        }

        public static string EnumString(Enum Tenum, int i)
        {
            string str = "";
            if(Tenum.GetType() == typeof(StopType))
            {
                //stop types
                switch (i){
                    case 0: str = "Załadunek";
                        break;
                    case 1: str = "Rozładunek";
                        break;
                    case 2: str = "Parking";
                        break;
                    case 3: str = "Inne";
                        break;
                }

            }else if (Tenum.GetType() == typeof(TransportType))
            {
                //transport types
                switch (i)
                {
                    case 0: str = "Własny";
                        break;
                    case 1: str = "Podnajęty";
                        break;
                }
            }
            else if (Tenum.GetType() == typeof(CompanyType))
            {
                //company types
                switch (i)
                {
                    case 0:
                        str = "Klient";
                        break;
                    case 1:
                        str = "Przewoźnik";
                        break;
                    case 2:
                        str = "Dostawca";
                        break;
                    case 3:
                        str = "Pośrednik";
                        break;
                    case 4:
                        str = "Outsourcing";
                        break;
                    case 5:
                        str = "Inny";
                        break;
                }
            }
            else if (Tenum.GetType() == typeof(Country))
            {
                //countries 
                switch (i)
                {
                    case 0:
                        str = "";
                        break;
                    case 1:
                        str = "Polska";
                        break;
                    case 2:
                        str = "Ukraina";
                        break;
                    case 3:
                        str = "Litwa";
                        break;
                    case 4:
                        str = "Łotwa";
                        break;
                    case 5:
                        str = "Estonia";
                        break;
                    case 6:
                        str = "Niemcy";
                        break;
                    case 7:
                        str = "Białoruś";
                        break;
                    case 8:
                        str = "Rosja";
                        break;
                    case 9:
                        str = "Słowacja";
                        break;
                    case 10:
                        str = "Słowenia";
                        break;
                    case 11:
                        str = "Czechy";
                        break;
                    case 12:
                        str = "Rumunia";
                        break;
                    case 13:
                        str = "Węgry";
                        break;
                    case 14:
                        str = "Dania";
                        break;
                    case 15:
                        str = "Holandia";
                        break;
                    case 16:
                        str = "Francja";
                        break;
                    case 17:
                        str = "Austria";
                        break;
                    case 18:
                        str = "Belgia";
                        break;
                    case 19:
                        str = "Włochy";
                        break;
                    case 20:
                        str = "Hiszpania";
                        break;
                    case 21:
                        str = "Portugalia";
                        break;
                    case 22:
                        str = "Albania";
                        break;
                    case 23:
                        str = "Armenia";
                        break;
                    case 24:
                        str = "Bośnia i Hercegowina";
                        break;
                    case 25:
                        str = "Bułgaria";
                        break;
                    case 26:
                        str = "Szwajcaria";
                        break;
                    case 27:
                        str = "Grecja";
                        break;
                    case 28:
                        str = "Finlandia";
                        break;
                    case 29:
                        str = "Gruzja";
                        break;
                    case 30:
                        str = "Chorwacja";
                        break;
                    case 31:
                        str = "Irlandia";
                        break;
                    case 32:
                        str = "Liechtenstein";
                        break;
                    case 33:
                        str = "Luxemburg";
                        break;
                    case 34:
                        str = "Monako";
                        break;
                    case 35:
                        str = "Mołdawia";
                        break;
                    case 36:
                        str = "Czarnogóra";
                        break;
                    case 37:
                        str = "Malta";
                        break;
                    case 38:
                        str = "Norwegia";
                        break;
                    case 39:
                        str = "Serbia";
                        break;
                    case 40:
                        str = "Szwecja";
                        break;
                    case 41:
                        str = "San Marino";
                        break;
                    case 42:
                        str = "Wielka Brytania";
                        break;
                    case 43:
                        str = "Macedonia";
                        break;
                    case 44:
                        str = "Turcja";
                        break;
                }
            }
            else if (Tenum.GetType() == typeof(ContractType))
            {
                //contract types
                switch (i)
                {
                    case 0:
                        str = "Bezterminowa";
                        break;
                    case 1:
                        str = "Tymczasowa";
                        break;
                    case 2:
                        str = "Zlecenia";
                        break;
                    case 3:
                        str = "Własna firma";
                        break;
                    case 4:
                        str = "Inna";
                        break;
                }
            }
            else if (Tenum.GetType() == typeof(VehicleType))
            {
                //vehicle types
                switch (i)
                {
                    case 0:
                        str = "Ciągnik";
                        break;
                    case 1:
                        str = "Naczepa";
                        break;
                    case 2:
                        str = "Samochód osobowy";
                        break;
                    case 3:
                        str = "Bus";
                        break;
                    case 4:
                        str = "Inny";
                        break;
                }
            }
            return str;
        }

        public static string UserName(int userId)
        {
            HorseplayContext db = new HorseplayContext();
            string name = "";
            var data = db.Users.Where(u => u.UserId == userId);
            if (data.Any())
            {
                name = data.FirstOrDefault().Name + " " + data.FirstOrDefault().Surname;
            }
            return name;
        }

        public static string uniqueToken(HorseplayContext db)
        {
            bool unique = false;
            string token;
            do
            {
                Guid g = Guid.NewGuid();
                token = Convert.ToBase64String(g.ToByteArray());
                token = token.Replace("=", "");
                token = token.Replace("+", "");
                token = token.Replace("/", "");
                token = token.Replace("\\", "");
                var tokens = db.Users.Where(u => u.ConfirmationToken == token.Trim());
                if (!tokens.Any())
                {
                    var invTokens = db.Invitations.Where(i => i.InvitationToken == token.Trim());
                    if (!invTokens.Any())
                    {
                        unique = true;
                    }
                }
            } while (unique == false);

            return token;
        }
    }
}