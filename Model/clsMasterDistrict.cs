using IIITS.JUNCTIONDB.DAL;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;


namespace District_Master_Excise.Models
{
    public class clsMasterDistrict
    {
        public int id { get; set; }
        public int dstId { get; set; }  //Auto_gen defined(if need to work in fetching)
        public int dstCountryId { get; set; } //For Distric country
        public string dstCountryName { get; set; }
        public int dstStateId { get; set; }
        public string dstStateName { get; set; }
        public int dstZoneId { get; set; }
        public string dstZoneName { get; set; }
        public string dstName { get; set; }
        public string dstCreatedBy { get; set; }
        public string dstCreatedON { get; set; }
        public string dstModifiedBy { get; set; }
        public string dstIsActive { get; set; }
        public string dstRemark { get; set; }
        public int response { get; set; }
        public string flag { get; set; }   
        public string ViewEditStatus { get; set; }
        public List<SelectListItem> dstCountryList { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> dstStateList { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> dstZoneList { get; set; } = new List<SelectListItem>();

        public List<clsMasterDistrict> dstMasterDetail { get; set; }


        /// <summary>
        /// The Method is working for the Getting the Data in the datatable from the data and converting it in the kist
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetGeneralDropDown(string type, string id)
        {

            List<SelectListItem> list = null;
            try
            {
                if (type == "COUNTRY")
                {
                    DataTable dt = GetTableDropdownList(type, id);
                    list = GetComboDetails(dt, "Select");
                }
                if (type == "STATE")
                {
                    DataTable dt = GetTableDropdownList(type, id);
                    list = GetComboDetails(dt, "Select");
                }

                if (type == "ZONE")
                {
                    DataTable dt = GetTableDropdownList(type, id);
                    list = GetComboDetails(dt, "Select");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        /// <summary>
        /// This Method Is for Getting drop down  it will take two parameter and it will fetch the datafrom Database 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetTableDropdownList(string type, string id)
        {
            DataTable dt = new DataTable();
            string[] arr = new string[4];
            try
            {
                CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
                objcon.proc_name = "GETGENERALDROPDOWN";
                objcon.dtInputParameter.Rows.Add("IN_DDL_TYPE", type, "INPUT");
                objcon.dtInputParameter.Rows.Add("IN_DDL_ID", id, "INPUT");
                objcon.dtInputParameter.Rows.Add("PROC_OUTPUT", "", "OUTPUT");
                objcon.ArrInputType[0] = OracleDbType.Varchar2;
                objcon.ArrInputType[1] = OracleDbType.Varchar2;
                objcon.ArrInputType[2] = OracleDbType.RefCursor;
                dt = objcon.FetchDataTable(objcon);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strSelect"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetComboDetails(DataTable dt, string strSelect, string strValue = "0")
        {
            List<SelectListItem> lstData = new List<SelectListItem>();
            try
            {

                if (strSelect.Length > 0)
                {
                    lstData.Add(new SelectListItem { Text = strSelect, Value = strValue });
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {
                        lstData.Add(new SelectListItem { Text = Convert.ToString(dRow[1]), Value = Convert.ToString(dRow[0]) });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return lstData;
        }
        /// <summary>
        /// This method in the model will bring the data from the DB and bind it in the properties of the model and showing in the grid as Jquery DataTable
        /// </summary>
        /// <returns></returns>
        public List<clsMasterDistrict> GetMasterDistrict()
        {
            List<clsMasterDistrict> lst= new List<clsMasterDistrict>();
            DataTable clatable = new DataTable();
            CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
            objcon.proc_name = "GetAllDetail_MasterTBLS";
            objcon.dtInputParameter.Rows.Add("PROC_OUTPUT", "", "OUTPUT");
            objcon.ArrInputType[0] = OracleDbType.RefCursor;
            DataTable dt = objcon.FetchDataTable(objcon);
            objcon.Execute(objcon);
            if (dt.Rows.Count > 0)
            {
                lst = (from dr in dt.AsEnumerable()
                                                  select new clsMasterDistrict()
                                                  {
                                                      dstCountryName = Convert.ToString(dr["CTR_NAME"]),
                                                      dstStateName = Convert.ToString(dr["STE_NAME"]),
                                                      dstZoneName = Convert.ToString(dr["ZONE_NAME"]),
                                                      dstName = Convert.ToString(dr["DST_NAME"]),
                                                      dstId = Convert.ToInt32(dr["DST_ID"]),
                                                      dstCountryId = Convert.ToInt32(dr["DST_COUNTRY_ID"]),
                                                      dstStateId = Convert.ToInt32(dr["DST_STATE_ID"]),
                                                      dstZoneId = Convert.ToInt32(dr["DST_ZONE_ID"]),
                                                      flag = Convert.ToString(dr["DST_IS_ACTIVE"])
                                                  }).ToList();
            }
            return lst;
        }

        /// <summary>
        /// This method is for Saving And updating the Data in the Database and using 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int SaveDistrictMaster(clsMasterDistrict obj)
        {
            int smessage;
            try
            {
                string[] arr = new string[20];
                CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
                objcon.proc_name = "INSERT_UPDATE_DISTRICT";
                objcon.dtInputParameter.Rows.Add("IN_DST_ID", obj.dstId, "INPUT");
                objcon.dtInputParameter.Rows.Add("IN_DST_COUNTRY_ID", obj.dstCountryId, "INPUT");
                objcon.dtInputParameter.Rows.Add("IN_DST_STATE_ID", obj.dstStateId, "INPUT");
                 objcon.dtInputParameter.Rows.Add("IN_DST_ZONE_ID", obj.dstZoneId, "INPUT");
                objcon.dtInputParameter.Rows.Add("IN_DST_NAME", obj.dstName, "INPUT");
                objcon.dtInputParameter.Rows.Add("DST_IS_ACTIVE", flag, "INPUT");
                objcon.dtInputParameter.Rows.Add("OUT_ID", response, "OUTPUT");
                objcon.ArrInputType[0] = OracleDbType.Int32;
                objcon.ArrInputType[1] = OracleDbType.Int32;
                objcon.ArrInputType[2] = OracleDbType.Int32;
                objcon.ArrInputType[3] = OracleDbType.Int32;
                objcon.ArrInputType[4] = OracleDbType.Varchar2;
                objcon.ArrInputType[5] = OracleDbType.Varchar2;
                objcon.ArrInputType[6] = OracleDbType.Int32;
                arr = objcon.Execute(objcon);
                response = Convert.ToInt32(arr[0]);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        /// <summary>
        /// GET 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsMasterDistrict GetDistrictMaster(int id)
        {
            DataTable dt = new DataTable();
            clsMasterDistrict obj = new clsMasterDistrict();
            try
            {
                CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
                objcon.proc_name = "GetDetailsById_MasterTBLS";
                objcon.dtInputParameter.Rows.Add("IN_ID", id, "INPUT");
                objcon.dtInputParameter.Rows.Add("PROC_OUTPUT", "", "OUTPUT");
                objcon.ArrInputType[0] = OracleDbType.Int32;
                objcon.ArrInputType[1] = OracleDbType.RefCursor;
                dt = objcon.FetchDataTable(objcon);
                //objcon.Execute(objcon);
                if (dt.Rows.Count > 0)
                {
                    obj.dstCountryId = Convert.ToInt32(dt.Rows[0]["CTR_ID"]);
                    obj.dstStateId = Convert.ToInt32(dt.Rows[0]["STE_ID"]);
                    obj.dstZoneId = Convert.ToInt32(dt.Rows[0]["ZONE_ID"]);
                    obj.dstName = Convert.ToString(dt.Rows[0]["DST_NAME"]);
                    obj.dstId = Convert.ToInt32(dt.Rows[0]["DST_ID"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
        /// <summary>
        /// The method will take the country Id and using Store Procedure it will retrive the data from Database 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>

        public List<SelectListItem> GetCountryByState(int countryId)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            string[] arr = new string[4];
            try
            {
                CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
                objcon.proc_name = "GETSTATEFROM_MASTER";
                objcon.dtInputParameter.Rows.Add("IN_CTR_ID", countryId, "INPUT");
                objcon.dtInputParameter.Rows.Add("PROC_OUTPUT", "", "OUTPUT");
                objcon.ArrInputType = new OracleDbType[3];
                objcon.ArrInputType[0] = OracleDbType.Int32;
                objcon.ArrInputType[1] = OracleDbType.RefCursor;
                DataTable dt = objcon.FetchDataTable(objcon);
                list = GetComboDetails(dt, "Select");
                arr = objcon.Execute(objcon);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        /// <summary>
        /// Get District and Zone using the District ID as i passed the ID through the ajax call and from the model method it will
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsMasterDistrict GetDistrictandZone(int id)
        {
            DataTable dt = new DataTable();
            clsMasterDistrict obj = new clsMasterDistrict();
            try
            {
                CustOledbConnection objcon = new CustOledbConnection("IIITS", "C:\\Users\\binit.kumar\\source\\repos\\District_Master_Excise\\District_Master_Excise\\District_Master_Excise\\constring.json");
                objcon.proc_name = "GETDistrictAndZone";
                objcon.dtInputParameter.Rows.Add("IN_ID", id, "INPUT");

                objcon.dtInputParameter.Rows.Add("PROC_OUTPUT", "", "OUTPUT");
                objcon.ArrInputType[0] = OracleDbType.Int32;
                objcon.ArrInputType[1] = OracleDbType.RefCursor;
                //DataSet ds = objcon.FetchDataSetCursor(objcon);
                //ds = objcon.FetchDataSet(objcon);
                dt = objcon.FetchDataTable(objcon);
                //objcon.Execute(objcon);
                if (dt.Rows.Count > 0)
                {
                    obj.dstStateName = Convert.ToString(dt.Rows[0]["STE_NAME"]);
                    obj.dstZoneName = Convert.ToString(dt.Rows[0]["ZONE_NAME"]);
                    obj.dstZoneId = Convert.ToInt32(dt.Rows[0]["ZONE_ID"]);
                    obj.dstCountryId = Convert.ToInt32(dt.Rows[0]["CTR_ID"]);
                    obj.dstStateId = Convert.ToInt32(dt.Rows[0]["STE_ID"]);
                    obj.dstZoneId = Convert.ToInt32(dt.Rows[0]["ZONE_ID"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
    }
}
