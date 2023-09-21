using District_Master_Excise.Models;

using Microsoft.AspNetCore.Mvc;

using System.Data;

namespace MasterDistrict_Excise.Controllers
{
    public class MasterDistrictController : Controller
    {
        // GET: District
        public ActionResult District()
        {
            try
            {
                clsMasterDistrict objStdReg = new clsMasterDistrict();
                if (TempData["id"] != null)
                {
                    objStdReg.dstId = Convert.ToInt32(TempData.Peek("id"));
                    objStdReg = objStdReg.GetDistrictMaster(objStdReg.dstId);
                    objStdReg.dstCountryList = clsMasterDistrict.GetGeneralDropDown("COUNTRY", objStdReg.dstCountryId.ToString());
                    objStdReg.dstStateList = clsMasterDistrict.GetGeneralDropDown("STATE", objStdReg.dstCountryId.ToString());
                    objStdReg.dstZoneList = clsMasterDistrict.GetGeneralDropDown("ZONE", objStdReg.dstStateId.ToString());
                    objStdReg.ViewEditStatus = Convert.ToString(TempData.Peek("ViewEditStatus"));
                }

                else
                {
                    //DataTable dtCountry = new DataTable();
                    objStdReg.dstCountryList = clsMasterDistrict.GetGeneralDropDown("COUNTRY", "0");
                    objStdReg.dstStateList = clsMasterDistrict.GetGeneralDropDown("STATE", "0");
                    objStdReg.dstZoneList = clsMasterDistrict.GetGeneralDropDown("ZONE", "0");
                }
                return View(objStdReg);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //post method to send and Update the data in the DB
        /// <summary>
        /// POST Method For Update And Insert the data from the Front-End to DB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult District(clsMasterDistrict obj)
        {
            clsMasterDistrict objMaster = new clsMasterDistrict();
            try
            {
                objMaster.response = objMaster.SaveDistrictMaster(obj);
                if (TempData.Peek("ViewEditStatus") != null)
                {
                    TempData.Remove("ViewEditStatus");
                    TempData.Remove("id");
                    objMaster.ViewEditStatus = "";
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return View(objMaster);
        }

        //Method For Onchange Flow for GetCountryByState
        /// <summary>
        /// This Method has Param of CTR_ID which will received from front-end on based on the parameter we will receive the state data and bind that in dro
        /// </summary>
        /// <param name="CTR_ID"></param>
        /// <returns></returns>
        public ActionResult GetStateByCountry(string CountryID)
        {
            try
            {
                clsMasterDistrict objStdReg = new clsMasterDistrict();
                objStdReg.dstStateList = clsMasterDistrict.GetGeneralDropDown("STATE", CountryID.ToString());

                //objStdReg.DstStateList = new SelectList(dtstate.AsDataView(), "STE_ID","STE_COUNTRY_ID" ,"STE_NAME");
                return Json(objStdReg.dstStateList);
            }


            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// getting zone by using state id 
        /// </summary>
        /// <param name="DstStateId"></param>
        /// <returns> We are returning the list in the view </returns>
        public ActionResult GetZoneByState(string DstStateId)
        {
            try
            {
                clsMasterDistrict objStdReg = new clsMasterDistrict();
                objStdReg.dstZoneList = clsMasterDistrict.GetGeneralDropDown("ZONE", DstStateId.ToString());
                return Json(objStdReg.dstZoneList);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Method for Grid View Binding 

        /// <summary>
        /// Method: Grid View For binding the data from the DB to the properties of the model and displayed in the view
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMasterDetail()
        {
            clsMasterDistrict obj = new clsMasterDistrict();
            return Json(new { data = obj.GetMasterDistrict() }); ;

        }
        /// <summary>
        /// Method for Getting the State and Zone Data from the DataBase Based on the District ID and bind it in the prpperties in the model and displayed in the view
        /// </summary>
        /// <param name="DistId"></param>
        /// <returns> We are returning the object here</returns>
        public ActionResult getStateandZone(string DistId, string Status)
        {
            clsMasterDistrict obj = new clsMasterDistrict();
            try
            {
                TempData["id"] = DistId;
                TempData["ViewEditStatus"] = Status;

            }
            catch (Exception)
            {

                throw;
            }
            return Json(obj);

        }

        public ActionResult ISActive(string  id, string flag)
        {
            if (flag == null)
            {

            }
            clsMasterDistrict obj = new clsMasterDistrict();
            obj.id= Convert.ToInt32(id);
            obj.flag =Convert.ToString(flag);
            obj.dstId= Convert.ToInt32(id);
            //obj.dstCountryId= Convert.ToInt32(dstCountryId);
            //obj.dstStateId= Convert.ToInt32(stateId);
            //obj.dstZoneId= Convert.ToInt32(dstZoneId);
            //obj.dstName=dstName;
            obj.SaveDistrictMaster(obj);



            return Json(obj.response);
        }
    }
}


