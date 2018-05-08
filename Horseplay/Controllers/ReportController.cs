using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Horseplay.Classes;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.Static;

namespace Horseplay.Controllers
{
    public class ReportController : Controller
    {

        [HttpGet]
        public ActionResult GenerateRouteReport(int id)
        {
            if (Session["TenantId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                var tos = db.TransportOrders.Where(t => t.TenantId == tenantId && t.TransportOrderId == id);
                if (tos.Any())
                {
                    TransportOrder to = tos.FirstOrDefault();
                    string path = Server.MapPath(Url.Content("~/PdfTemplates/routeReport.pdf"));
                    PdfDocument PDFDoc = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                    PdfDocument PDFNewDoc = new PdfDocument();

                    for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
                    {
                        PdfPage pp = PDFNewDoc.AddPage(PDFDoc.Pages[Pg]);

                        XGraphics gfx = XGraphics.FromPdfPage(pp);
                        XFont font = new XFont("Arial", 10, XFontStyle.Regular);
                        if(to.Type == TransportType.Own)
                        {
                            string VehicleString = "";
                            if(to.Truck != null)
                            {
                                VehicleString = to.Truck.Plate;
                                if(to.Trailer != null)
                                {
                                    VehicleString += " / " + to.Trailer.Plate;
                                }
                            }
                            string DriverString = "";
                            if (to.PrimaryDriver != null)
                            {
                                DriverString = to.PrimaryDriver.FullName;
                                if (to.SecondaryDriver != null)
                                {
                                    DriverString += " / ";
                                }
                            }
                            if (to.SecondaryDriver != null)
                            {
                                DriverString += to.SecondaryDriver.FullName;
                            }

                            //Driver's name
                            gfx.DrawString(DriverString, font, XBrushes.Black, new XRect(191, 75, 369, 31), XStringFormats.Center);
                            //Truck numbers
                            gfx.DrawString(VehicleString, font, XBrushes.Black, new XRect(191, 105, 369, 31), XStringFormats.Center);
                            
                        }
                        //Customer
                        gfx.DrawString(to.Customer != null ? to.Customer.FullName : "", font, XBrushes.Black, new XRect(191, 135, 369, 31), XStringFormats.Center);
                        //Carrier
                        gfx.DrawString(to.Carrier != null ? to.Carrier.FullName : "", font, XBrushes.Black, new XRect(191, 165, 369, 31), XStringFormats.Center);
                        //Route lenght
                        gfx.DrawString(to.Route != null ? to.Route.Length.ToString() + " km" : "", font, XBrushes.Black, new XRect(191, 195, 369, 31), XStringFormats.Center);
                        //Comments
                        gfx.DrawString(to.Remarks != null ? to.Remarks : "", font, XBrushes.Black, new XRect(191, 225, 369, 31), XStringFormats.Center);
                        //Print date
                        gfx.DrawString("Data wydruku: " + DateTime.Now.ToString(), new XFont("Arial", 8, XFontStyle.Regular), XBrushes.Black, new XRect(451, 20, 100, 31), XStringFormats.Center);
                        //let's add route's stops
                        if (to.Route != null)
                        {
                            int i = 0;
                            double sPoint = 290;//start point
                            int fontSize = GetFont(to);
                            font = new XFont("Arial", fontSize, XFontStyle.Regular);
                            foreach (var stop in to.Route.Stops.OrderBy(o=>o.Order))
                            {
                                //Lp
                                gfx.DrawString((i+1).ToString(), font, XBrushes.Black, new XRect(31, sPoint+i*25, 23, 25), XStringFormats.Center);
                                //Address
                                gfx.DrawString(stop.Address.FullName, font, XBrushes.Black, new XRect(54, sPoint + i * 25, 282, 25), XStringFormats.Center);
                                //Arrival date
                                gfx.DrawString(stop.ArrivalDate!=null?stop.ArrivalDate.Value.ToShortDateString(): "", font, XBrushes.Black, new XRect(336, sPoint + i * 25, 83, 25), XStringFormats.Center);
                                //Departure date
                                gfx.DrawString(stop.DepartureDate != null ? stop.DepartureDate.Value.ToShortDateString() : "", font, XBrushes.Black, new XRect(415, sPoint + i * 25, 83, 25), XStringFormats.Center);
                                //Stop type
                                gfx.DrawString(Utilities.EnumString(new StopType(),(int)stop.StopType), font, XBrushes.Black, new XRect(498, sPoint + i * 25, 83, 25), XStringFormats.Center);
                                i++;
                            }
                        }
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        PDFNewDoc.Save(stream, false);
                        //document.Save(stream, false);
                        return File(stream.ToArray(), "application/pdf");
                    }
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
                
            }
        } 

        public int GetFont(TransportOrder to)
        {
            int maxCharacters = 0;
            int fontSize = 0;

            if (to.Route != null)
            {
                foreach(var stop in to.Route.Stops)
                {
                    if(stop.Address.FullName.Trim().Length > maxCharacters)
                    {
                        maxCharacters = stop.Address.FullName.Trim().Length;
                    }
                }
            }
            if (maxCharacters > 76)
            {
                fontSize = 7;
            }else if(maxCharacters <= 76 && maxCharacters > 55){
                fontSize = 8;
            }else if(maxCharacters <= 55 && maxCharacters > 40)
            {
                fontSize = 9;
            }
            else
            {
                fontSize = 10;
            }
            return fontSize;
        }
    }
}