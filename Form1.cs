//
// N-tier C# and SQL program to analyze CTA Ridership data.
//
// Edward Hughes - ehughe5
// U. of Illinois, Chicago
// CS341, Fall 2016
// Homework 7
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////using System.Data.SqlClient;

namespace CTA
{

    public partial class Form1 : Form
    {
        private string BuildConnectionString()
        {
            string version = "MSSQLLocalDB";
            string filename = this.txtDatabaseFilename.Text;

            string connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename={1};Integrated Security=True;", version, filename);

            return connectionInfo;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
            // setup GUI:
            //
            this.lstStations.Items.Add("");
            this.lstStations.Items.Add("[ Use File>>Load to display L stations... ]");
            this.lstStations.Items.Add("");

            this.lstStations.ClearSelected();

            toolStripStatusLabel1.Text = string.Format("Number of stations:  0");

            // 
            // open-close connect to get SQL Server started:
            //

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);

                bizTier.TestConnection();
            }
            catch
            {
                //
                // ignore any exception that occurs, goal is just to startup
                //
            }
        }


        //
        // File>>Exit:
        //
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //
        // File>>Load Stations:
        //
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //
            // clear the UI of any current results:
            //
            ClearStationUI(true /*clear stations*/);

            try
            {
                BusinessTier.Business bizTier;

                bizTier = new BusinessTier.Business(this.txtDatabaseFilename.Text);

                var stations = bizTier.GetStations();

                foreach (var station in stations)
                {
                    this.lstStations.Items.Add(station.Name);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }


        //
        // User has clicked on a station for more info:
        //
        private void lstStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            // sometimes this event fires, but nothing is selected...
            if (this.lstStations.SelectedIndex < 0)   // so return now in this case:
                return;

            //
            // clear GUI in case this fails:
            //
            ClearStationUI();

            //
            // now display info about selected station:
            //
            string stationName = this.lstStations.Text;

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get stationID from station name
                int stationID = bizTier.GetStationID(stationName);

                //set the txt boxes to station's information
                this.txtTotalRidership.Text = bizTier.GetTotalRidership(stationID).ToString("#,##0");
                this.txtAvgDailyRidership.Text = string.Format("{0:#,##0}/day", bizTier.GetAvgRidership(stationID));
                this.txtPercentRidership.Text = string.Format("{0:0.00}%", bizTier.GetPercentageRidership(stationID));

                this.txtStationID.Text = stationID.ToString();
                
                this.txtWeekdayRidership.Text = bizTier.GetRidershipsWeekday(stationID).ToString("#,##0");
                this.txtSaturdayRidership.Text = bizTier.GetRidershipsSaturday(stationID).ToString("#,##0");
                this.txtSundayHolidayRidership.Text = bizTier.GetRidershipsSundayHoliday(stationID).ToString("#,##0");


                var stops = bizTier.GetStops(stationID);

                // display stops:
                foreach (var stop in stops)
                {
                    this.lstStops.Items.Add(stop.Name);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        private void ClearStationUI(bool clearStatations = false)
        {
            ClearStopUI();

            this.txtTotalRidership.Clear();
            this.txtTotalRidership.Refresh();

            this.txtAvgDailyRidership.Clear();
            this.txtAvgDailyRidership.Refresh();

            this.txtPercentRidership.Clear();
            this.txtPercentRidership.Refresh();

            this.txtStationID.Clear();
            this.txtStationID.Refresh();

            this.txtWeekdayRidership.Clear();
            this.txtWeekdayRidership.Refresh();
            this.txtSaturdayRidership.Clear();
            this.txtSaturdayRidership.Refresh();
            this.txtSundayHolidayRidership.Clear();
            this.txtSundayHolidayRidership.Refresh();

            this.lstStops.Items.Clear();
            this.lstStops.Refresh();

            if (clearStatations)
            {
                this.lstStations.Items.Clear();
                this.lstStations.Refresh();
            }
        }


        //
        // user has clicked on a stop for more info:
        //
        private void lstStops_SelectedIndexChanged(object sender, EventArgs e)
        {
            // sometimes this event fires, but nothing is selected...
            if (this.lstStops.SelectedIndex < 0)   // so return now in this case:
                return;

            //
            // clear GUI in case this fails:
            //
            ClearStopUI();

            //
            // now display info about this stop:
            //
            string stopName = this.lstStops.Text;

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                int stopID = bizTier.GetStopID(stopName);
                
                // handicap accessible?
                bool accessible = bizTier.GetADA(stopID, Convert.ToInt32(this.txtStationID.Text));

                if (accessible)
                    this.txtAccessible.Text = "Yes";
                else
                    this.txtAccessible.Text = "No";

                // direction of travel:
                this.txtDirection.Text = bizTier.GetDirection(stopID, Convert.ToInt32(this.txtStationID.Text));

                // lat/long position:
                this.txtLocation.Text = string.Format("({0:00.0000}, {1:00.0000})",
                  bizTier.GetLatitude(stopID, Convert.ToInt32(this.txtStationID.Text)),
                  bizTier.GetLongitude(stopID, Convert.ToInt32(this.txtStationID.Text)));

                //
                // now we need to know what lines are associated 
                // with this stop:
                //


                // display colors:

                var lines = bizTier.GetLinesAtStop(stopID);

                foreach (var line in lines)
                {
                    this.lstLines.Items.Add(line);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        private void ClearStopUI()
        {
            this.txtAccessible.Clear();
            this.txtAccessible.Refresh();

            this.txtDirection.Clear();
            this.txtDirection.Refresh();

            this.txtLocation.Clear();
            this.txtLocation.Refresh();

            this.lstLines.Items.Clear();
            this.lstLines.Refresh();
        }


        //
        // Top-10 stations in terms of ridership:
        //
        private void top10StationsByRidershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
            // clear the UI of any current results:
            //
            ClearStationUI(true /*clear stations*/);

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get the top 10 stations by ridership
                var top10 = bizTier.GetTopStations(10);

                //display top 10 stations by ridership
                foreach(var station in top10)
                {
                    this.lstStations.Items.Add(station.Name);
                }

                toolStripStatusLabel1.Text = string.Format("Number of stations:  {0:#,##0}", top10.Count);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        //find feature that finds stations with partial name entered by user (case sensitive)
        private void btnFindStation_Click(object sender, EventArgs e)
        {
            //if user entered something
            if( !(string.Equals(this.txtFindStation.Text.ToString(), "")) )
            {
                //clear the list box lstStation
                ClearStationUI(true /*clear stations*/);

                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get the stations with partial name entered by user
                var stationsFound = bizTier.FindStations(this.txtFindStation.Text);

                //display stations
                foreach (var station in stationsFound)
                {
                    this.lstStations.Items.Add(station.Name);
                }

                toolStripStatusLabel1.Text = string.Format("Number of stations:  {0:#,##0}", stationsFound.Count);
            }
        }

        //top 10 stations by weekday riderships
        private void top10StationsByWeekdayRidershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStationUI(true /*clear stations*/);

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get top 10 stations by weekday riderships
                var top10 = bizTier.GetTopStationsWeekDay(10);

                //display stations
                foreach (var station in top10)
                {
                    this.lstStations.Items.Add(station.Name);
                }

                toolStripStatusLabel1.Text = string.Format("Number of stations:  {0:#,##0}", top10.Count);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        //top 10 stations by saturday riderships
        private void top10StationsBySaturdayRidershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStationUI(true /*clear stations*/);

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get top 10 stations by saturday riderships
                var top10 = bizTier.GetTopStationsSaturday(10);

                //display stations
                foreach (var station in top10)
                {
                    this.lstStations.Items.Add(station.Name);
                }

                toolStripStatusLabel1.Text = string.Format("Number of stations:  {0:#,##0}", top10.Count);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        //top 10 stations by sunday/holiday riderships
        private void top10StationsBySundayHolidayRidershipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStationUI(true /*clear stations*/);

            try
            {
                string filename = this.txtDatabaseFilename.Text;

                BusinessTier.Business bizTier;
                bizTier = new BusinessTier.Business(filename);
                bizTier.TestConnection();

                //get top 10 stations by sunday/holiday riderships
                var top10 = bizTier.GetTopStationsSundayHoliday(10);

                //display stations
                foreach (var station in top10)
                {
                    this.lstStations.Items.Add(station.Name);
                }

                toolStripStatusLabel1.Text = string.Format("Number of stations:  {0:#,##0}", top10.Count);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error: '{0}'.", ex.Message);
                MessageBox.Show(msg);
            }
        }

        //allows user to change the ADA field of a stop
        private void handicapAccessiblityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if station and stop not selected, display error 
            if (this.lstStations.SelectedIndex < 0 || this.lstStops.SelectedIndex < 0)
            {
                Form3 errorForm = new CTA.Form3(this);
                errorForm.Show();
                return;
            }
            //else show update form to allow user to update ADA field
            else
            {
                Form2 updateForm = new CTA.Form2(this.txtAccessible, this.txtDatabaseFilename.Text, this.lstStations.Text.ToString(), this.lstStops.Text.ToString());
                updateForm.Show();
            }
        }

        private void txtDatabaseFilename_TextChanged(object sender, EventArgs e)
        {

        }

        private void useCustomConnectionStringToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }//class
}//namespace
