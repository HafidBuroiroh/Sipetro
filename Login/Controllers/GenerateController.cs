using Microsoft.AspNetCore.Mvc;
using Login.Models;
using System.Diagnostics;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Login.Data;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Math;
using Microsoft.CodeAnalysis.Text;
using DocumentFormat.OpenXml.Office2016.Excel;
using ICSharpCode.SharpZipLib.Zip;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using ZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;
using ICSharpCode.SharpZipLib.Core;
using System.IO;

namespace Login.Controllers
{
    public class GenerateController : Controller
    {
        private readonly DataContext _context;
        private IHostingEnvironment _IHosting;
        private readonly IWebHostEnvironment hostEnvironment;

        public GenerateController(DataContext context, IHostingEnvironment IHosting, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _IHosting = IHosting;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_context.Saved_Data.ToList());
        }

        public IActionResult Generate(DateTime from, DateTime until, string btnsubmit)
        {
            var data = from d in _context.Saved_Data select d;
            ViewData["CurrentFilter"] = from;
            ViewData["CurrentFilters"] = until;

            if (btnsubmit == "Search")
            {
                if (from != null && until != null)
                {
                    data = data.Where(d => d.DateCreated >= from && d.DateCreated <= until);
                }
            }
            if (btnsubmit == "Generate")
            {
                if (from != null && until != null)
                {
                    string[] columnNames = new string[] { "PERSONID", "POPULATIONREGISTERNO", "TAXREGISTERNO", "PASSPORTNO", "KITAS", "FULL_NAME", "PHONE", "GENDER", "EMAIL", "BIRTHDATE", "HIGHSCHOOL", "UNIVERSITY", "UNIVERSITYMAJOR", "MSSCHOOL", "MSMAJOR", "PHDSCHOOL", "PHDMAJOR", "POSITION", "COUNTRYCODE", "PROVINCECODE", "ADDRESS", "CITY", "POSTALCODE", "BUSINESS_REG_NO", "OCCUPATION", "LOC_ASING", "NASIONALITY" };

                    //Build the CSV file data as a Comma separated string.
                    string csv = string.Empty;
                    string path = _IHosting.WebRootPath;
                    var tempOutput = path + "/Text/";

                    foreach (string columnName in columnNames)
                    {
                        //Add the Header row for CSV file.
                        csv += columnName + '|';
                    }

                    //Add new line.
                    csv += "\r\n";

                    foreach (var customer in data)
                    {
                        //Add the Data rows.
                        csv += customer.PERSONID.ToString().Replace(",", ";") + '|';
                        csv += customer.POPULATIONREGISTERNO.ToString().Replace(",", ";") + '|';
                        csv += customer.TAXREGISTERNO.ToString().Replace(",", ";") + '|';
                        csv += customer.PASSPORTNO.ToString().Replace(",", ";") + '|';
                        csv += customer.KITAS.ToString().Replace(",", ";") + '|';
                        csv += customer.FULL_NAME.Replace(",", ";") + '|';
                        csv += customer.PHONE.ToString().Replace(",", ";") + '|';
                        csv += customer.GENDER.ToString().Replace(",", ";") + '|';
                        csv += customer.EMAIL.Replace(",", ";") + '|';
                        csv += customer.BIRTHDATE.ToString().Replace(",", ";") + '|';
                        csv += customer.HIGHSCHOOL?.Replace(",", ";") + '|';
                        csv += customer.UNIVERSITY?.Replace(",", ";") + '|';
                        csv += customer.UNIVERSITYMAJOR?.Replace(",", ";") + '|';
                        csv += customer.MSSCHOOL?.Replace(",", ";") + '|';
                        csv += customer.MSMAJOR?.Replace(",", ";") + '|';
                        csv += customer.PHDSCHOOL?.Replace(",", ";") + '|';
                        csv += customer.PHDMAJOR?.Replace(",", ";") + '|';
                        csv += customer.POSITION?.Replace(",", ";") + '|';
                        csv += customer.COUNTRYCODE.Replace(",", ";") + '|';
                        csv += customer.PROVINCECODE.ToString().Replace(",", ";") + '|';
                        csv += customer.ADDRESS.Replace(",", ";") + '|';
                        csv += customer.CITY.ToString().Replace(",", ";") + '|';
                        csv += customer.POSTALCODE.ToString().Replace(",", ";") + '|';
                        csv += customer.BUSINESS_REG_NO.ToString().Replace(",", ";") + '|';
                        csv += customer.OCCUPATION.ToString().Replace(",", ";") + '|';
                        csv += customer.LOC_ASING.ToString().Replace(",", ";") + '|';
                        csv += customer.NASIONALITY.ToString().Replace(",", ";") + '|';


                        //Add new line.
                        csv += "\r\n";
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(tempOutput, $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.txt")))
                    {
                        outputFile.WriteLine(csv);
                    }
                    var location = _IHosting.WebRootPath;
                    var fileName = $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.zip";
                    var Output = location + "/Text/" + fileName;

                   

                using (ZipOutputStream IzipOutputStream = new ZipOutputStream(System.IO.File.Create(Output)))
                    {
                        IzipOutputStream.SetLevel(9);
                        IzipOutputStream.Password = "ojk-sipetro";
                        byte[] buffer = new byte[4096];
                        var textfile = new List<string>();
                        

                        textfile.Add(location + "/Text/" + $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.txt");
                        for (int i = 0; i < textfile.Count; i++)
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(textfile[i]));
                            entry.DateTime = DateTime.Now;
                            entry.IsUnicodeText = true;
                            IzipOutputStream.PutNextEntry(entry);

                            using (FileStream oFileStream = System.IO.File.OpenRead(textfile[i]))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                                    IzipOutputStream.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                        IzipOutputStream.Finish();
                        IzipOutputStream.Flush();
                        IzipOutputStream.Close();
                    }

                    byte[] finalResult = System.IO.File.ReadAllBytes(Output);
                    if (System.IO.File.Exists(Output))
                    {
                        System.IO.File.Delete(Output);
                    }
                    if (finalResult == null || !finalResult.Any())
                    {
                        throw new Exception(String.Format("Nothing found"));

                    }
                    return File(finalResult, "application/zip", fileName);
                }
            }

            return View("Index", data);
        }


        //public IActionResult Export()
        //{
        //    string[] columnNames = new string[] { "PERSONID", "POPULATIONREGISTERNO", "TAXREGISTERNO", "PASSPORTNO", "KITAS", "FULL_NAME", "PHONE", "GENDER", "EMAIL", "BIRTHDATE", "HIGHSCHOOL", "UNIVERSITY", "UNIVERSITYMAJOR", "MSSCHOOL", "MSMAJOR", "PHDSCHOOL", "PHDMAJOR", "POSITION", "COUNTRYCODE", "PROVINCECODE", "ADDRESS", "CITY", "POSTALCODE", "BUSINESS_REG_NO", "OCCUPATION", "LOC_ASING", "NASIONALITY" };
        //    var data = this._context.Saved_Data.ToList();

        //    //Build the CSV file data as a Comma separated string.
        //    string csv = string.Empty;
        //    string path = _IHosting.WebRootPath;
        //    var output = path + "/Text/";

        //    foreach (string columnName in columnNames)
        //    {
        //        //Add the Header row for CSV file.
        //        csv += columnName + '|';
        //    }

        //    //Add new line.
        //    csv += "\r\n";

        //    foreach (var customer in data)
        //    {
        //        //Add the Data rows.
        //        csv += customer.PERSONID.ToString().Replace(",", ";") + '|';
        //        csv += customer.POPULATIONREGISTERNO.ToString().Replace(",", ";") + '|';
        //        csv += customer.TAXREGISTERNO.ToString().Replace(",", ";") + '|';
        //        csv += customer.PASSPORTNO.ToString().Replace(",", ";") + '|';
        //        csv += customer.KITAS.ToString().Replace(",", ";") + '|';
        //        csv += customer.FULL_NAME.Replace(",", ";") + '|';
        //        csv += customer.PHONE.ToString().Replace(",", ";") + '|';
        //        csv += customer.GENDER.ToString().Replace(",", ";") + '|';
        //        csv += customer.EMAIL.Replace(",", ";") + '|';
        //        csv += customer.BIRTHDATE.ToString().Replace(",", ";") + '|';
        //        csv += customer.HIGHSCHOOL?.Replace(",", ";") + '|';
        //        csv += customer.UNIVERSITY?.Replace(",", ";") + '|';
        //        csv += customer.UNIVERSITYMAJOR?.Replace(",", ";") + '|';
        //        csv += customer.MSSCHOOL?.Replace(",", ";") + '|';
        //        csv += customer.MSMAJOR?.Replace(",", ";") + '|';
        //        csv += customer.PHDSCHOOL?.Replace(",", ";") + '|';
        //        csv += customer.PHDMAJOR?.Replace(",", ";") + '|';
        //        csv += customer.POSITION?.Replace(",", ";") + '|';
        //        csv += customer.COUNTRYCODE.Replace(",", ";") + '|';
        //        csv += customer.PROVINCECODE.ToString().Replace(",", ";") + '|';
        //        csv += customer.ADDRESS.Replace(",", ";") + '|';
        //        csv += customer.CITY.ToString().Replace(",", ";") + '|';
        //        csv += customer.POSTALCODE.ToString().Replace(",", ";") + '|';
        //        csv += customer.BUSINESS_REG_NO.ToString().Replace(",", ";") + '|';
        //        csv += customer.OCCUPATION.ToString().Replace(",", ";") + '|';
        //        csv += customer.LOC_ASING.ToString().Replace(",", ";") + '|';
        //        csv += customer.NASIONALITY.ToString().Replace(",", ";") + '|';
                

        //        //Add new line.
        //        csv += "\r\n";
        //    }
        //    using (StreamWriter outputFile = new StreamWriter(Path.Combine(tempOutput, $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.txt")))
        //    {
        //        outputFile.WriteLine(csv);
        //    }
        //    return View();
        //}
        public async Task<IActionResult> DownLoadZip()
        {
            var location = _IHosting.WebRootPath;
            var fileName = $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.zip";
            var tempOutput = location + "/Text/" + fileName;

            using (ZipOutputStream IzipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                IzipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var textfile = new List<string>();

                textfile.Add(location + "/Text/" + $"sipetro{DateTime.Now.ToString("yyyyMMdd")}.txt");
                for (int i = 0; i < textfile.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(textfile[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    IzipOutputStream.PutNextEntry(entry);

                    using (FileStream oFileStream = System.IO.File.OpenRead(textfile[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                            IzipOutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                IzipOutputStream.Finish();
                IzipOutputStream.Flush();
                IzipOutputStream.Close();
            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
            if (System.IO.File.Exists(tempOutput))
            {
                System.IO.File.Delete(tempOutput);
            }
            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(String.Format("Nothing found"));

            }
            //var botsFolderPath = Path.Combine(hostEnvironment.ContentRootPath, location + "/Text/");
            //var botFilePaths = Directory.GetFiles(botsFolderPath);
            //using (ZipArchive archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create))
            //{
            //    foreach (var botFilePath in botFilePaths)
            //    {
            //        var botFileName = Path.GetFileName(botFilePath);
            //        var entry = archive.CreateEntry(botFileName);
            //        using (var entryStream = entry.Open())
            //        using (var fileStream = System.IO.File.OpenRead(botFilePath))
            //        {
            //          await fileStream.CopyToAsync(entryStream);
            //        }
            //    }
            //}
           
                return File(finalResult, "application/zip", fileName);
        }

    }
}