using HiQPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReportMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // set browser width
            htmlToPdfConverter.BrowserWidth = int.Parse("1200");

            // set browser height if specified, otherwise use the default
            if (500 > 0)
                htmlToPdfConverter.BrowserHeight = int.Parse("500");

            // set HTML Load timeout
            htmlToPdfConverter.HtmlLoadedTimeout = int.Parse("120");

            // set PDF page size and orientation
            htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;
            htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;

            // set the PDF standard used by the document
            htmlToPdfConverter.Document.PdfStandard = PdfStandard.Pdf;

            // set PDF page margins
            htmlToPdfConverter.Document.Margins = new PdfMargins(5);

            // set triggering mode; for WaitTime mode set the wait time before convert

            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;


            // set header and footer
            //SetHeader(htmlToPdfConverter.Document);
            //SetFooter(htmlToPdfConverter.Document);

            // convert HTML to PDF
            byte[] pdfBuffer = null;


            // convert HTML code
            string htmlCode = "";
            string baseUrl = "";

            // convert HTML code to a PDF memory buffer
            pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlCode, baseUrl);


            // inform the browser about the binary data format
            HttpContext.Response.Headers.Add("Content-Type", "application/pdf");

            // let the browser know how to open the PDF document, attachment or inline, and the file name
            HttpContext.Response.Headers.Add("Content-Disposition", String.Format("{0}; filename=HtmlToPdf.pdf; size={1}", "attachment", pdfBuffer.Length.ToString()));

            // write the PDF buffer to HTTP response
            HttpContext.Response.Body.WriteAsync(pdfBuffer);

            // call End() method of HTTP response to stop ASP.NET page processing
           






            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
