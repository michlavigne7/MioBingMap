using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BingMapMIO.Model
{
    public class DeliveryAdress
    {
        [Key]
        public string scnum { get; set; }
        public string driverName { get; set; }
        public string driverColor { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string sordnum { get; set; }
        public string custnum { get; set; }
        public DateTime scdate { get; set; }
        public string whcode { get; set; }
        public string driverid { get; set; }   
        public string StartAddress { get; set; }
        public string delivadd { get; set; }
        public int orderby { get; set; }    
        public int run_number { get; set; }
        public float linmetercalculated { get; set; }
        public int qtybdl { get; set; }
    }

    public class PickUpAdress
    {
        [Key]
        public string scnum { get; set; }
        public string driverName { get; set; }
        public string driverColor { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string pordnumber { get; set; }
        public DateTime DatePickUp { get; set; }
        public string driverid { get; set; }
        public string vendnum { get; set; }
        public string whdest { get; set; }
        public string pickUpAdd { get; set; }
        public string itemnum { get; set; }
        public int orderby { get; set; }

    }
    public class AdressContext : DbContext
    {
        public AdressContext() : base("Server=dbserver;Database=UAT_Database;Trusted_Connection=True;TrustServerCertificate=True") { }
        public DbSet<DeliveryAdress> scschdt { get; set; }
    }



}