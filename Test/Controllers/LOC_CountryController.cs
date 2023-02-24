﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Test.DAL;
using Test.Models;
namespace Test.Controllers
{
    public class LOC_CountryController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region Index
        #region SelectAll
        public IActionResult Index()
        {
            String str = this.Configuration.GetConnectionString("myConnectionStings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View("LOC_CountryList", dt);
           
        }
        #endregion
        #endregion

        #region Add
        public IActionResult Add(int CountryID)
        {
            #region Select By PK
            if (CountryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStings");
                LOC_DAL dalLOC = new LOC_DAL();

                DataTable dt = dalLOC.dbo_PR_LOC_Country_SelectByPK(connectionstr, CountryID);
                if (dt.Rows.Count > 0)
                {
                    LOC_CountryModel model = new LOC_CountryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.CountryName = dr["CountryName"].ToString();
                        
                    }
                    return View("LOC_CountryAddEdit", model);
                }



                /*  string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                  SqlConnection conn = new SqlConnection(connectionstr);

                  conn.Open();

                  SqlCommand objCmd = conn.CreateCommand();
                  objCmd.CommandType = CommandType.StoredProcedure;
                  objCmd.CommandText = "PR_LOC_Country_SelectByPK";
                  objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                  DataTable dt= new DataTable();
                  SqlDataReader objSDR = objCmd.ExecuteReader();
                  dt.Load(objSDR);

                  LOC_CountryModel modelLOC_CountryModel = new LOC_CountryModel();

                  foreach (DataRow dr in dt.Rows)
                  {
                      modelLOC_CountryModel.CountryID = Convert.ToInt32(dr["CountryID"]); 
                      modelLOC_CountryModel.CountryName = dr["CountryName"].ToString(); 
                      modelLOC_CountryModel.CountryCode = dr["CountryCode"].ToString(); 
                      modelLOC_CountryModel.CreationDate = Convert.ToDateTime(dr["CreationDate"]); 
                      modelLOC_CountryModel.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                      return View("LOC_CountryAddEdit", modelLOC_CountryModel);
                  }
                  conn.Close();*/
            }
            #endregion

            return View("LOC_CountryAddEdit");
        }
        #endregion

        #region Save 
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            if (ModelState.IsValid)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStings");

                LOC_DAL dalLOC = new LOC_DAL();

                if (modelLOC_Country.CountryID == null)
                {

                    if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_Insert(connectionstr, modelLOC_Country)))
                    {
                        TempData["CountryInsertMessage"] = "Record inserted successfully";

                    }
                }
                else
                {
                    if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_UpdateByPK(connectionstr, modelLOC_Country)))
                    {

                        TempData["CountryUpdateMessage"] = "Record Update Successfully";

                    }
                    return RedirectToAction("Index");
                }

            }

            return RedirectToAction("Add");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStings");

            LOC_DAL dalLOC = new LOC_DAL();

            if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_DeleteByPK(connectionstr, CountryID)))
            {
                return RedirectToAction("Index");
            }
            return View("Index");


            /*DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_DeleteByPK";
           
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);

            objCmd.ExecuteNonQuery();

            
            conn.Close();*/


        }
        #endregion
    }
}
