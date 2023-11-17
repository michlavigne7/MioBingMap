using BingMapMIO.Controller;
using BingMapMIO.Model;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Web.Security;
using System.EnterpriseServices;
using Microsoft.Ajax.Utilities;
using System.Drawing;

namespace BingMapMIO
{
    public partial class _Default : Page
    {
        public List<DeliveryAdress> ListDelivery =  new List<DeliveryAdress>();
        public List<DeliveryAdress> ListDeliveryTV =  new List<DeliveryAdress>();
        public List<DeliveryAdress> ListDeliveryTV2 =  new List<DeliveryAdress>();
        public List<PickUpAdress> ListPickUp =  new List<PickUpAdress>();
        private List<DeliveryAdress> tmplistadd =  new List<DeliveryAdress>();
        private List<PickUpAdress> tmplistpu =  new List<PickUpAdress>();
        public IEnumerable<string> Driver;
        public string date; 
        public IEnumerable<string> Scenario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AdressController adressController = new AdressController();
                ListDelivery = adressController.GetDeliveryAdress(DateTime.Now.ToString("d"));
                ListPickUp = adressController.GetPickUpAdresses(DateTime.Now.ToString("d"));
                Driver = ListDelivery.Select(x => x.driverid).Distinct();
                Scenario = ListDelivery.Select(x => x.scnum).Distinct();
                date = ListDelivery[0].scdate.ToShortDateString();
                SqlDataSource1.SelectParameters["date"].DefaultValue = date;
                datepicker.Text = date;
                PopulateTreeView();
            }
            
        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdressController adressController = new AdressController();
            ListDelivery = adressController.GetDeliveryAdress(datepicker.Text);
            ListPickUp = adressController.GetPickUpAdresses(datepicker.Text);
            Driver = ListDelivery.Select(x => x.driverid).Distinct();
            Scenario = ListDelivery.Select(x => x.scnum).Distinct();
            date = ListDelivery[0].scdate.ToShortDateString();
            datepicker.Text = date;
            tmplistadd = ListDelivery.ToList();
            tmplistpu = ListPickUp.ToList();
            ListDelivery.Clear();
            ListPickUp.Clear();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    foreach (DeliveryAdress add in tmplistadd)
                    {
                        if(item.Value == add.driverid)
                        {
                            ListDelivery.Add(add);
                        }
                    }
                    foreach (PickUpAdress add in tmplistpu)
                    {
                        if (item.Value == add.driverid)
                        {
                            ListPickUp.Add(add);
                        }
                    }
                }
            }
            foreach (ListItem item in CheckBoxList1.Items)
            {
                foreach (DeliveryAdress add in ListDelivery)
                {
                    if (add.driverid == item.Value)
                    {
                        item.Attributes.Add("style", $"accent-color:{add.driverColor}");
                    }
                }
            }
        }

        protected void CheckBoxList_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxList1.Items)
            {
                item.Selected = true;
                foreach (DeliveryAdress add in ListDelivery)
                {
                    if(add.driverid == item.Value)
                    {
                        item.Attributes.Add("style", $"accent-color:{add.driverColor}");
                    }
                }
            }
        }

        protected void datepicker_TextChanged(object sender, EventArgs e)
        {
            ListDelivery.Clear();
            ListPickUp.Clear();
            AdressController adressController = new AdressController();
            ListDelivery = adressController.GetDeliveryAdress(datepicker.Text);
            ListPickUp = adressController.GetPickUpAdresses(datepicker.Text);
            Driver = ListDelivery.Select(x => x.driverid).Distinct();
            Scenario = ListDelivery.Select(x => x.scnum).Distinct();
            if(ListDelivery.Count() != 0) {
                date = ListDelivery[0].scdate.ToShortDateString();
                datepicker.Text = date;
            }
            else { Response.Write("<script>alert('Aucun transport lors de la journée sélectionnée');</script>"); }
            if (ListPickUp.Count() != 0)
            {
                date = ListPickUp[0].DatePickUp.ToShortDateString();
                datepicker.Text = date;
            }
            SqlDataSource1.SelectParameters["date"].DefaultValue = date;
            PopulateTreeView();
        }
        private string OrderLetter(int ordreint)
        {
            var ordrestring = "";
            if (ordreint == 0) ordrestring = "A - ";
            if (ordreint == 1) ordrestring = "B - ";
            if (ordreint == 2) ordrestring = "C - ";
            if (ordreint == 3) ordrestring = "D - ";
            if (ordreint == 4) ordrestring = "E - ";
            if (ordreint == 5) ordrestring = "F - ";
            if (ordreint == 6) ordrestring = "G - ";
            if (ordreint == 7) ordrestring = "H - ";
            if (ordreint == 8) ordrestring = "I - ";
            if (ordreint == 9) ordrestring = "J - ";
            if (ordreint == 10) ordrestring = "K - ";
            if (ordreint == 11) ordrestring = "L - ";
            if (ordreint == 12) ordrestring = "M - ";
            if (ordreint == 13) ordrestring = "N - ";
            if (ordreint == 14) ordrestring = "O - ";
            if (ordreint == 15) ordrestring = "P - ";
            if (ordreint == 16) ordrestring = "Q - ";
            if (ordreint == 17) ordrestring = "R - ";
            if (ordreint == 18) ordrestring = "S - ";
            return ordrestring;
        }


        private void PopulateTreeView()
        {

            TreeView1.Nodes.Clear();
            ListDeliveryTV = ListDelivery.OrderBy(r => r.driverName).ToList();
            var topNode = new TreeNode("Par chauffeur");
            topNode.SelectAction = TreeNodeSelectAction.Expand;
            TreeView1.Nodes.Add(topNode);
            string currentGroup ="";
            var x = -1;
            var y = -1;
            var count = 0;
            var ordreint = 0;
            var ordrestring = "A - ";
            var prev="";
            var prevcust="";
            var childNodes = new List<TreeNode>();
            var childNodes2 = new List<TreeNode>();
            foreach (DeliveryAdress add in ListDeliveryTV)
            {
                ordrestring = OrderLetter(ordreint);

                count++;
                if (currentGroup == add.driverName)
                {
                    if(prev != add.scnum)
                    {
                        y++;
                        var scnode = new TreeNode(add.scnum);
                        scnode.SelectAction = TreeNodeSelectAction.Expand;
                        TreeView1.Nodes[0].ChildNodes[x].ChildNodes.Add(scnode);
                        foreach (TreeNode nd2 in childNodes2)
                        {
                            nd2.SelectAction = TreeNodeSelectAction.Expand; 
                            TreeView1.Nodes[0].ChildNodes[x].ChildNodes[y].ChildNodes.Add(nd2);
                        }
                        childNodes2 = new List<TreeNode>();
                        ordreint = 0;
                        ordrestring = OrderLetter(ordreint);
                    }
                    else
                    {
                        if (add.delivadd != prevcust) ordreint++;
                    }
                    
                    ordrestring = OrderLetter(ordreint);
                    if (ordrestring == "A - ")
                    {
                        var nodedepart = new TreeNode(ordrestring + " Envoi à partir du " +add.whcode);
                        nodedepart.SelectAction = TreeNodeSelectAction.Expand;
                        childNodes2.Add(nodedepart);
                        ordreint++;
                        ordrestring = OrderLetter(ordreint);
                    }
                    
                    childNodes2.Add(new TreeNode(ordrestring + add.custnum + " - " + add.sordnum));
                    
                    
                    

                    if (count == ListDeliveryTV.Count())
                    {
                        y++;
                        foreach (TreeNode nd2 in childNodes2)
                        {
                            nd2.SelectAction = TreeNodeSelectAction.Expand;
                            TreeView1.Nodes[0].ChildNodes[x].ChildNodes[y].ChildNodes.Add(nd2);
                        }
                    }
                }
                    
                else
                {
                    if ((prev != add.scnum && prev != "")|| count == ListDeliveryTV.Count())
                    {
                        y++;
                        foreach (TreeNode nd2 in childNodes2)
                        {
                            nd2.SelectAction=   TreeNodeSelectAction.Expand;
                            TreeView1.Nodes[0].ChildNodes[x].ChildNodes[y].ChildNodes.Add(nd2);
                        }
                        y = -1;
                        childNodes2 = new List<TreeNode>();
                        ordreint = 0;
                        ordrestring = OrderLetter(ordreint);
                    }

                    x++;
                    var node = new TreeNode(add.driverName);
                    node.SelectAction = TreeNodeSelectAction.Expand;
                    topNode.ChildNodes.Add(node);

                    var scnode = new TreeNode(add.scnum);
                    scnode.SelectAction = TreeNodeSelectAction.Expand;
                    TreeView1.Nodes[0].ChildNodes[x].ChildNodes.Add(scnode);
                    if (ordrestring == "A - ")
                    {
                        var nodedepart = new TreeNode(ordrestring + " Envoi à partir du " + add.whcode);
                        nodedepart.SelectAction = TreeNodeSelectAction.Expand;
                        childNodes2.Add(nodedepart);
                        ordreint++;
                        ordrestring = OrderLetter(ordreint);
                    }
                    
                    childNodes2.Add(new TreeNode(ordrestring + add.custnum + " - " + add.sordnum));
                    
                    currentGroup = add.driverName;
                }
                prev = add.scnum;
                prevcust = add.delivadd;
            }

            ListDeliveryTV2 = ListDelivery.OrderBy(r => r.scnum).ToList();
            //Liste de chargement des scénarios
            var secondnode = new TreeNode("Espace de chargement utilisé");
            secondnode.SelectAction = TreeNodeSelectAction.Expand;
            TreeView1.Nodes.Add(secondnode);
            var totespace = 0.0;
            var totbales = 0;
            prev = "";
            x = 0;
            var scenNodes = new List<TreeNode>();
            currentGroup = ListDeliveryTV2[0].scnum;
            for (int i = 0; i < ListDeliveryTV2.Count(); i++)    
            {           
                if(currentGroup == ListDeliveryTV2[i].scnum)
                {
                    totespace += ListDeliveryTV2[i].linmetercalculated;
                    totbales += ListDeliveryTV2[i].qtybdl;
                    scenNodes.Add(new TreeNode(ListDeliveryTV2[i].sordnum + " - " + ListDeliveryTV2[i].linmetercalculated + " linft. " + "- " + ListDeliveryTV2[i].qtybdl + " ballots"));
                    if (i == ListDeliveryTV2.Count() - 1)
                    {
                        var lastofsecondnode = new TreeNode(ListDeliveryTV2[i].scnum + " - " + totespace + " linft. " + "- " + totbales + " ballots");
                        lastofsecondnode.SelectAction = TreeNodeSelectAction.Expand;
                        TreeView1.Nodes[1].ChildNodes.Add(lastofsecondnode);
                        foreach(TreeNode nd in scenNodes)
                        {
                            nd.SelectAction = TreeNodeSelectAction.Expand;
                            TreeView1.Nodes[1].ChildNodes[x].ChildNodes.Add(nd);
                        }
                    }
                }
                else
                {
                    var nodesofsecondnode = new TreeNode(currentGroup + " - " + totespace + " linft. " + "- " + totbales + " ballots");
                    nodesofsecondnode.SelectAction = TreeNodeSelectAction.Expand;
                    TreeView1.Nodes[1].ChildNodes.Add(nodesofsecondnode);
                    totbales = 0;
                    totespace = 0.0;
                    totespace += ListDeliveryTV2[i].linmetercalculated;
                    totbales += ListDeliveryTV2[i].qtybdl;
                    currentGroup = ListDeliveryTV2[i].scnum;
                    if (i == ListDeliveryTV2.Count() - 1)
                    {
                        foreach (TreeNode nd in scenNodes)
                        {
                            nd.SelectAction = TreeNodeSelectAction.Expand;
                            TreeView1.Nodes[1].ChildNodes[x].ChildNodes.Add(nd);
                        }
                        var lastofsecondnode = new TreeNode(currentGroup + " - " + totespace + " linft. " + "- " + totbales + " ballots");
                        lastofsecondnode.SelectAction = TreeNodeSelectAction.Expand;
                        TreeView1.Nodes[1].ChildNodes.Add(lastofsecondnode);
                        scenNodes = new List<TreeNode>();
                        scenNodes.Add(new TreeNode(ListDeliveryTV2[i].sordnum + " - " + ListDeliveryTV2[i].linmetercalculated + " linft. " + "- " + ListDeliveryTV2[i].qtybdl + " ballots"));
                        x++;

                    }
                    foreach (TreeNode nd in scenNodes)
                    {
                        nd.SelectAction = TreeNodeSelectAction.Expand;
                        TreeView1.Nodes[1].ChildNodes[x].ChildNodes.Add(nd);
                    }
                    scenNodes = new List<TreeNode>();
                    scenNodes.Add(new TreeNode(ListDeliveryTV2[i].sordnum + " - " + ListDeliveryTV2[i].linmetercalculated + " linft. " + "- " + ListDeliveryTV2[i].qtybdl + " ballots"));
                    x++;
                }
                
                
                
            }

            TreeView1.CollapseAll();
            
        }
    }
}