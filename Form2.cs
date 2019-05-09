//update ADA form

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTA
{
    public partial class Form2 : Form
    {
        TextBox txtAccessible;
        String filename;
        String StationName;
        String StopName;
        bool RevisedADA;
        bool userSelected;
        

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(TextBox txtAccessible, String filename, string stationName, string stopName)
        {
            this.txtAccessible = txtAccessible;
            this.filename = filename;
            this.StationName = stationName;
            this.StopName = stopName;
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.txtBoxStationName.Text = StationName;
            this.txtBoxStopName.Text = StopName;
        }

        //if Yes is checked, ADA field will be updated to Yes
        private void radioADAUpdateYes_CheckedChanged(object sender, EventArgs e)
        {
            RevisedADA = true;
            userSelected = true;
        }

        //if No is checked, ADA field will be updated to No
        private void radioADAUpdateNo_CheckedChanged(object sender, EventArgs e)
        {
            RevisedADA = false;
            userSelected = true;
        }

        //updates ADA field of a particular station and stop if user has selected yes or no
        private void buttonADAUpdateConfirm_Click(object sender, EventArgs e)
        {
            if (userSelected)
            {
                try
                {
                    BusinessTier.Business bizTier;
                    bizTier = new BusinessTier.Business(filename);
                    bizTier.TestConnection();

                    bizTier.UpdateADA(RevisedADA, StationName, StopName);

                    this.txtAccessible.Clear();
                    this.txtAccessible.Refresh();

                    bool accessible = bizTier.GetADA(StopName, StationName);

                    if (accessible)
                        txtAccessible.Text = "Yes";
                    else
                        txtAccessible.Text = "No";
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Error: '{0}'.", ex.Message);
                    MessageBox.Show(msg);
                }
                finally
                {
                    this.Close();
                }
            }
        }
    }//class
}//namespace
