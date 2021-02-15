using EmployeeApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Create(EmployeeModel _data)
        {
            CreateCommand("exec AddEmployee " + _data.id + ",'" + _data.Name + "','" + _data.Email + "','" + _data.AddressLine + "','" + _data.Designation + "'");
            if (_data.id > 0)
            {
                return Json("Edited Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Added Successfully", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListEmployees()
        {
            var result = loaddata("ListEmployees");

            List<EmployeeModel> _list = new List<EmployeeModel>();
            foreach(DataRow item in result.Rows)
            {
                EmployeeModel model = new EmployeeModel();
                model.Name = item["Name"].ToString();
               // model.MobileNo = item["MobileNo"].ToString();
                model.Designation = item["Designation"].ToString();
                model.AddressLine = item["AddressLine"].ToString();
                model.Email = item["Email"].ToString();
                _list.Add(model);
            }

            return Json(_list, JsonRequestBehavior.AllowGet);
        }

        private static void CreateCommand(string queryString)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static DataTable loaddata(string queryString)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter(queryString, connection);
                DataTable d = new DataTable();
                sqlDA.Fill(d);
                return d;
            }

        }
    }
}