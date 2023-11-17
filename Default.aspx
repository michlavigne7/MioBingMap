<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BingMapMIO._Default" %>



    <form id="form1" runat="server">


    <main>
        ﻿<!DOCTYPE html>
<html lang="en">
<head>

    <title>Display Multiple Routes - Bing Maps Samples</title>

    <meta charset="utf-8" />
	<link rel="shortcut icon" href="/favicon.ico"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Calculate and display multiple routes using the getRoute function." />
    <meta name="keywords" content="Microsoft maps, map, gis, API, SDK, Bing, Bing Maps" />
    <meta name="author" content="Microsoft Bing Maps" />
    <meta name="screenshot" content="screenshot.jpg" />
    <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="//code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
  <script src="https://code.jquery.com/jquery-3.6.0.js"></script>
  <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.js"></script>
  <link href="./Stylesheet1.css" rel="stylesheet" type="text/css" />      
    <script>
        $(function () {
            $("#datepicker").datepicker({ dateFormat: "yy-mm-dd" }).val()
        });

        var map, directionsManagers = [];
        
        function GetMap() {
            map = new Microsoft.Maps.Map('#myMap', {}, {
                center: new Microsoft.Maps.Location(45.4, -71.8),
                zoom: -100
            });
            //Create an infobox at the center of the map but don't show it.
            infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
                visible: false
            });

            //Assign the infobox to a map instance.
            infobox.setMap(map);
            map.setOptions({showLocateMeButton: false, showMapTypeSelector: false, showZoomButtons: false })
            //Load the directions module.
            Microsoft.Maps.loadModule('Microsoft.Maps.Directions', function () {
                //Generate routes with multiple stops for each drivers

                var array = <%=new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ListDelivery)%>;
                var arrayPickup = <%=new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.ListPickUp)%>;
                var arrayScnum = <%=new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.Scenario)%>;
                const arrayLat = new Array();
                const arrayLong = new Array();
                const arrayAddress = new Array();
                var startadd;
                var drv;
                var prev;
                var driverColor;
                for (var x = 0; x < arrayScnum.length; x++) {
                    for (var y = 0; y < array.length; y++) {
                        if (arrayScnum[x] == array[y].scnum) {
                            startadd = "";
                            startadd = array[y].StartAddress;
                            if (prev != array[y].delivadd) {
                                arrayLat.push(array[y].latitude);
                                arrayLong.push(array[y].longitude);
                                arrayAddress.push(array[y].delivadd);
                            }

                            prev = "";
                            prev = array[y].delivadd;
                            driverColor = "";
                            driverColor = array[y].driverColor;
                            drv = "";
                            drv = array[y].driverName + ' [' + array[y].run_number + ']';
                        }
                    }
                    getRoute(startadd, arrayAddress, arrayLat, arrayLong, driverColor, arrayScnum[x], drv);
                    while (arrayAddress.length > 0) {
                        arrayAddress.pop();
                        arrayLat.pop();
                        arrayLong.pop();
                    }

                }

                prev = "";
                drv = "";
                for (var z = 0; z < arrayPickup.length; z++) {
                    if (prev != arrayPickup[z].pickUpAdd || drv != arrayPickup[z].driverName) {
                        var titre = arrayPickup[z].driverName;
                        if (arrayPickup[z].latitude != 0) {
                            var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(arrayPickup[z].latitude, arrayPickup[z].longitude), {
                                title: titre,
                                color: arrayPickup[z].driverColor
                            });
                            pin.metadata = {
                                title: arrayPickup[z].driverName,
                                description: 'Destination : ' + arrayPickup[z].whdest + '<br />' + 'Fournisseur : ' + arrayPickup[z].vendnum + '<br />' + '# Scénario : ' + arrayPickup[z].scnum + '<br />'
                            }
                            Microsoft.Maps.Events.addHandler(pin, 'click', pushpinClicked);
                            //Add the pushpin to the map
                            map.entities.push(pin);
                        }
                    }
                    prev = arrayPickup[z].pickUpAdd;
                    drv = arrayPickup[z].driverName;
                }
            });

        }
        function pushpinClicked(e) {
            //Make sure the infobox has metadata to display.
            if (e.target.metadata) {
                //Set the infobox options with the metadata of the pushpin.
                infobox.setOptions({
                    location: e.target.getLocation(),
                    title: e.target.metadata.title,
                    description: e.target.metadata.description,
                    visible: true
                });
            }
        }

        //Used to generate random colors
        function getRandomInt(max) {
            return Math.floor(Math.random() * max);
        }
        //Take a list of stops and generate an itinerary
        function getRoute(start, List, lat, long, color, scnum, drv) {
            var dm = new Microsoft.Maps.Directions.DirectionsManager(map);

            directionsManagers.push(dm);

            dm.setRequestOptions({
                routeMode: Microsoft.Maps.Directions.RouteMode.truck
            });

            dm.setRenderOptions({
                waypointPushpinOptions:{
                    title: scnum,
                    subTitle: drv
                },
                autoUpdateMapView: false,
                drivingPolylineOptions: {
                    strokeColor: color,
                    strokeThickness: 6
                }
            });
            dm.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: start }));
            for (var x = 0; x < List.length; x++)
            {

                if (lat[x] == 0) {
                    dm.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: List[x] }));
                }
               else {
                    dm.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ address: 'Allo', location: new Microsoft.Maps.Location(lat[x], long[x])}));
                }
                
            }       
            dm.calculateDirections();
        }
    </script>
    
</head>
<body>
    <script>
        ( () => {
            let script = document.createElement("script");
            script.setAttribute("src", `https://www.bing.com/api/maps/mapcontrol?callback=GetMap&key=AkVOfK00zRCu8q7z9oc3Ac4Kp8jbD7izECvLO9MEypea_Fghf_05jlGE5LouTNBo`);
            document.body.appendChild(script);
        })();
    </script>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UAT_DatabaseConnectionString2 %>" ProviderName="<%$ ConnectionStrings:UAT_DatabaseConnectionString2.ProviderName %>" SelectCommand="Select distinct ltrim(rtrim(drv.driverid)) as driverid, ltrim(rtrim(descript)) as driverName from ( select sch.scnum, sch.scdate, scd.sordnum, scd.compcode, sch.whcode, scd.driverid, scd.run_number, scd.orderby from scschht sch inner join scschdt scd on sch.scnum = scd.scnum union all select sch.scnum, sch.scdate, scd.sordnum, scd.compcode, sch.whcode, scd.driverid, scd.run_number, scd.orderby from scschha sch inner join scschda scd on sch.scnum = scd.scnum where Convert(Date, sch.scdate) = @date) sc inner join (select soh.ordnum, soh.compcode, soh.whnum, soh.shipcity, soh.shipst, soh.shipctry, soh.shipzip, soh.shipcode, soh.number from soordeht soh  union all  select soh.ordnum, soh.compcode, soh.whnum, soh.shipcity, soh.shipst, soh.shipctry, soh.shipzip, soh.shipcode, soh.number from soordeha soh) soo  on ltrim(sc.sordnum) = ltrim(soo.ordnum) and sc.compcode = soo.compcode    inner join inwaremm w on w.whnum = (case when sc.whcode = '' then soo.whnum else sc.whcode end) left join scdriver drv on sc.driverid = drv.driverid left join arshipdt sp on soo.number = sp.shipnum and sp.compcode = soo.compcode and soo.shipcode = sp.shipcode  where convert(date, sc.scdate) = @date and drv.driverid <> 'CLIENT' and drv.driverid <> 'CUISIN' order by driverName ">
        <SelectParameters>
        <asp:QueryStringParameter Name="date" DbType = "String" QueryStringField="date" DefaultValue="" />
    </SelectParameters></asp:SqlDataSource>
   

    <div style="margin-top: 0; overflow: hidden; width: 100%; background-color: midnightblue; border-bottom-style:solid; border-bottom-color: lightgray ">
        <div style="margin-left: 15px; margin-top: 5px; float: left; width: 93%;">
        <asp:CheckBoxList ID="CheckBoxList1" OnDataBound ="CheckBoxList_DataBound" AutoPostBack ="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged" runat="server" DataSourceID="SqlDataSource1" DataTextField="driverName" DataValueField="driverID" RepeatLayout="Table"
                    TextAlign="Right" BackColor="midnightblue" ForeColor="white" RepeatDirection="Horizontal" CssClass="checkboxlistformat" Width="100%" Font-Names="Arial" Font-Size="Medium">
        </asp:CheckBoxList>
        </div>
        <div style="float: right;">
        <asp:Textbox style="text-align:center;" type="text" runat ="server" OnTextChanged="datepicker_TextChanged" AutoPostBack="true" id="datepicker" Height="30px" Width =" 100px"></asp:Textbox>
        </div>
      </div>
    <div style="width: 100%; margin-top:0; background-color: midnightblue;">
        <div style="float: left; height: 97%; width: 15%; background-color:midnightblue;">
            <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Détail des scénarios" Width="100%" BorderColor="LightGray" style="text-align: center; border-bottom: solid; border-bottom-color: lightgray" Font-Names="Arial"></asp:Label>
            <div style="float: left; overflow-y:auto; border-bottom:solid; border-bottom-color:lightgray; height: 45%; width: 100%; background-color:midnightblue;">
            <asp:TreeView ID="TreeView1" style="margin-top:  20px; margin-left: 10px" ForeColor="White" Font-Names="Arial" runat="server">
            </asp:TreeView>
            </div>
            <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Informations sur les journées des camionneurs" Width="100%" BorderColor="LightGray" style="text-align: center; border-bottom: solid; border-bottom-color: lightgray" Font-Names="Arial"></asp:Label>
            <div style="float: right; margin-top: 10px; width: 44%; margin-right: 12px;">
            <asp:DropDownList ID="DropDownList1" runat="server" Width="100%">
                <asp:ListItem>Hier</asp:ListItem>
                <asp:ListItem>5 derniers jours</asp:ListItem>
                <asp:ListItem>30 derniers jours</asp:ListItem>
            </asp:DropDownList>
            </div>
            </div>
        <div id="myMap" style="float: right;padding: 0; margin: 0; height: 97%; width:85%; background-color: gray;">
            </div>
    </div>

   </form>
</body>
</html>
    </main>


