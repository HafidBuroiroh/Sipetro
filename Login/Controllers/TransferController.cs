using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Login.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;
using SqlBulkCopy = Microsoft.Data.SqlClient.SqlBulkCopy;
using Login.Data;
using System.Data.Entity;
using DocumentFormat.OpenXml.InkML;


namespace Login.Controllers
{
    
    public class TransferController : Controller
    {
        private DateTime? DateFrom;
        private DateTime? DateUntil;
        private readonly DataContext _context;
        public IConfiguration _Configuration { get; set; }

        public TransferController(IConfiguration configuration, DataContext context)
        {
            _Configuration = configuration;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Master_Data.ToList());
        }

        public IActionResult Search(DateTime from, DateTime until, string btnsubmit)
        {
            var data = from d in _context.Master_Data select d;
            ViewData["CurrentFilter"] = from;
            ViewData["CurrentFilters"] = until;


            if (btnsubmit == "Search")
            {
                if (from != null && until != null)
                {
                    data = data.Where(d => d.DateCreated >= from && d.DateCreated <= until);
                }
            }
            if (btnsubmit == "Transfer")
            {
                    var connectionstring = _Configuration["ConnectionStrings:DefaultConnection"];
                    SqlConnection con = new SqlConnection(connectionstring);
                    var sql = $"SELECT * FROM master_data WHERE DateCreated Between '{from.ToString("yyyy-MM-dd")}' AND '{until.ToString("yyyy-MM-dd")}'";
                    SqlCommand com = new SqlCommand(sql, con);

                    con.Open();

                    SqlDataReader dataReader = com.ExecuteReader();

                    SqlBulkCopy sqlBulk = new SqlBulkCopy(con);
                    sqlBulk.DestinationTableName = "dbo.saved_data";

                    sqlBulk.ColumnMappings.Add("PERSONID", "PERSONID");
                    sqlBulk.ColumnMappings.Add("POPULATIONREGISTERNO", "POPULATIONREGISTERNO");
                    sqlBulk.ColumnMappings.Add("TAXREGISTERNO", "TAXREGISTERNO");
                    sqlBulk.ColumnMappings.Add("PASSPORTNO", "PASSPORTNO");
                    sqlBulk.ColumnMappings.Add("KITAS", "KITAS");
                    sqlBulk.ColumnMappings.Add("FULL_NAME", "FULL_NAME");
                    sqlBulk.ColumnMappings.Add("PHONE", "PHONE");
                    sqlBulk.ColumnMappings.Add("GENDER", "GENDER");
                    sqlBulk.ColumnMappings.Add("EMAIL", "EMAIL");
                    sqlBulk.ColumnMappings.Add("BIRTHDATE", "BIRTHDATE");
                    sqlBulk.ColumnMappings.Add("HIGHSCHOOL", "HIGHSCHOOL");
                    sqlBulk.ColumnMappings.Add("UNIVERSITY", "UNIVERSITY");
                    sqlBulk.ColumnMappings.Add("UNIVERSITYMAJOR", "UNIVERSITYMAJOR");
                    sqlBulk.ColumnMappings.Add("MSSCHOOL", "MSSCHOOL");
                    sqlBulk.ColumnMappings.Add("MSMAJOR", "MSMAJOR");
                    sqlBulk.ColumnMappings.Add("PHDSCHOOL", "PHDSCHOOL");
                    sqlBulk.ColumnMappings.Add("PHDMAJOR", "PHDMAJOR");
                    sqlBulk.ColumnMappings.Add("POSITION", "POSITION");
                    sqlBulk.ColumnMappings.Add("COUNTRYCODE", "COUNTRYCODE");
                    sqlBulk.ColumnMappings.Add("PROVINCECODE", "PROVINCECODE");
                    sqlBulk.ColumnMappings.Add("ADDRESS", "ADDRESS");
                    sqlBulk.ColumnMappings.Add("CITY", "CITY");
                    sqlBulk.ColumnMappings.Add("POSTALCODE", "POSTALCODE");
                    sqlBulk.ColumnMappings.Add("BUSINESS_REG_NO", "BUSINESS_REG_NO");
                    sqlBulk.ColumnMappings.Add("OCCUPATION", "OCCUPATION");
                    sqlBulk.ColumnMappings.Add("LOC_ASING", "LOC_ASING");
                    sqlBulk.ColumnMappings.Add("NASIONALITY", "NASIONALITY");
                    sqlBulk.ColumnMappings.Add("DateCreated", "DateCreated");

                    sqlBulk.WriteToServer(dataReader);
            }
            return View("Index", data);

        }


        public IActionResult TransferData(DateTime? from, DateTime? until)
        {
            var data = from d in _context.Master_Data select d;
            ViewData["CurrentFilter"] = from;
            ViewData["CurrentFilters"] = until;

            var connectionstring = _Configuration["ConnectionStrings:DefaultConnection"];
            SqlConnection con = new SqlConnection(connectionstring);
            var sql = $"SELECT * FROM master_data";
            SqlCommand com = new SqlCommand(sql, con);

            con.Open();

            SqlDataReader dataReader = com.ExecuteReader();

            SqlBulkCopy sqlBulk = new SqlBulkCopy(con);
            sqlBulk.DestinationTableName = "saved_data";

            sqlBulk.ColumnMappings.Add("PERSONID", "PERSONID");
            sqlBulk.ColumnMappings.Add("POPULATIONREGISTERNO", "POPULATIONREGISTERNO");
            sqlBulk.ColumnMappings.Add("TAXREGISTERNO", "TAXREGISTERNO");
            sqlBulk.ColumnMappings.Add("PASSPORTNO", "PASSPORTNO");
            sqlBulk.ColumnMappings.Add("KITAS", "KITAS");
            sqlBulk.ColumnMappings.Add("FULL_NAME", "FULL_NAME");
            sqlBulk.ColumnMappings.Add("PHONE", "PHONE");
            sqlBulk.ColumnMappings.Add("GENDER", "GENDER");
            sqlBulk.ColumnMappings.Add("EMAIL", "EMAIL");
            sqlBulk.ColumnMappings.Add("BIRTHDATE", "BIRTHDATE");
            sqlBulk.ColumnMappings.Add("HIGHSCHOOL", "HIGHSCHOOL");
            sqlBulk.ColumnMappings.Add("UNIVERSITY", "UNIVERSITY");
            sqlBulk.ColumnMappings.Add("UNIVERSITYMAJOR", "UNIVERSITYMAJOR");
            sqlBulk.ColumnMappings.Add("MSSCHOOL", "MSSCHOOL");
            sqlBulk.ColumnMappings.Add("MSMAJOR", "MSMAJOR");
            sqlBulk.ColumnMappings.Add("PHDSCHOOL", "PHDSCHOOL");
            sqlBulk.ColumnMappings.Add("PHDMAJOR", "PHDMAJOR");
            sqlBulk.ColumnMappings.Add("POSITION", "POSITION");
            sqlBulk.ColumnMappings.Add("COUNTRYCODE", "COUNTRYCODE");
            sqlBulk.ColumnMappings.Add("PROVINCECODE", "PROVINCECODE");
            sqlBulk.ColumnMappings.Add("ADDRESS", "ADDRESS");
            sqlBulk.ColumnMappings.Add("CITY", "CITY");
            sqlBulk.ColumnMappings.Add("POSTALCODE", "POSTALCODE");
            sqlBulk.ColumnMappings.Add("BUSINESS_REG_NO", "BUSINESS_REG_NO");
            sqlBulk.ColumnMappings.Add("OCCUPATION", "OCCUPATION");
            sqlBulk.ColumnMappings.Add("LOC_ASING", "LOC_ASING");
            sqlBulk.ColumnMappings.Add("NASIONALITY", "NASIONALITY");
            sqlBulk.ColumnMappings.Add("DateCreated", "DateCreated");

            sqlBulk.WriteToServer(dataReader);

            return View("Index", data);
        }
    }
}
