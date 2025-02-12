using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeamMPServerLauncher
{
    public partial class ProgressWindow : Form
    {
        int stepValue = 0;
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void SetStepValue(float value)
        {
            stepValue = Convert.ToInt32(value);
            LoadingProgressBar.Value = 1;
        }

        public void WriteToProgressWindow(string line, bool progress)
        {
            string details = LoadingDetails.Text + "\n" + line;
            LoadingDetails.Text = details;
            if (progress)
            {
                LoadingProgressBar.Value += stepValue;
            }
        }
    }
}
