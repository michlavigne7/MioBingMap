using BingMapMIO.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;

namespace BingMapMIO.Controller
{
    public class AdressController
    {
            

            public AdressController()
            {
               
            }
            public List<PickUpAdress> GetPickUpAdresses(string date)
            {
            try
            {
                SqlConnection con = new SqlConnection();
                con = new SqlConnection("data source=dbserver; initial catalog = uat_database; persist security info = True; Integrated Security = SSPI; ");
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("[Operations].[bel_sp_mio_PickUpList]", con);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@date", date));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlCommand;
                List<PickUpAdress> PickUpAdresses = new List<PickUpAdress>();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    PickUpAdress ad = new PickUpAdress();
                    ad.scnum = dr["scnum"].ToString();
                    ad.driverColor = dr["driverColor"].ToString();
                    ad.driverName = dr["driverName"].ToString();
                    ad.pordnumber = dr["pordnumber"].ToString();
                    ad.driverid = dr["drvID"].ToString();
                    ad.pickUpAdd = dr["pickUpAdd"].ToString();
                    ad.whdest = dr["whdest"].ToString();
                    ad.itemnum = dr["itemnum"].ToString();
                    ad.vendnum = dr["vendnum"].ToString();
                    ad.latitude = Convert.ToDouble(dr["latitude"]);
                    ad.longitude = Convert.ToDouble(dr["longitude"]);
                    ad.DatePickUp = Convert.ToDateTime(dr["DatePickUp"]);
                    ad.orderby = Convert.ToInt32(dr["orderby"]);
                    PickUpAdresses.Add(ad);
                }
                con.Close();
                return PickUpAdresses;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
            public List<DeliveryAdress> GetDeliveryAdress(string date)
            {
            try
            {
                SqlConnection con = new SqlConnection();
                con = new SqlConnection("data source=dbserver; initial catalog = uat_database; persist security info = True; Integrated Security = SSPI; ");
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("[Operations].[bel_sp_mio_deliveryList]", con);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@date", date));
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlCommand;
                List<DeliveryAdress> deliveryAdresses = new List<DeliveryAdress>();
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    DeliveryAdress ad = new DeliveryAdress();
                    ad.scnum = dr["scnum"].ToString();
                    ad.driverColor = dr["driverColor"].ToString();
                    ad.driverName = dr["driverName"].ToString();
                    ad.sordnum = dr["sordnum"].ToString();
                    ad.StartAddress = dr["StartAddress"].ToString();
                    ad.driverid = dr["drvid"].ToString();
                    ad.delivadd = dr["delivadd"].ToString();
                    ad.custnum = dr["custnumber"].ToString();
                    ad.whcode = dr["whcode"].ToString();
                    ad.scdate = Convert.ToDateTime(dr["DateScenario"]);
                    ad.orderby = Convert.ToInt32(dr["orderby"]);
                    ad.linmetercalculated = Convert.ToSingle(dr["linmetercalculated"]);
                    ad.qtybdl = Convert.ToInt32(dr["qtybdl"]);
                    ad.latitude = Convert.ToDouble(dr["latitude"]);
                    ad.longitude = Convert.ToDouble(dr["longitude"]);
                    ad.run_number = Convert.ToInt32(dr["run_number"]);
                    deliveryAdresses.Add(ad);
                }
                con.Close();
                return deliveryAdresses;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

       
    }
}