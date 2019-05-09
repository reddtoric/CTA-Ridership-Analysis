//update error form
//if update option is clicked but no station and stop selection selected, this will display the error

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
    public partial class Form3 : Form
    {
        Form1 mainForm;

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

        }

        private void btnForm3Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
