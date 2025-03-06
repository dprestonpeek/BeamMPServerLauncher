using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeamMPServerLauncher
{
    public enum OkFunction { None, CloseApp, CloseErrorWindow, YesDeleteMap };
    public enum AltFunction { None, OpenDirectory, OpenLog, CloseErrorWindow };
    public partial class ErrorWindow : Form
    {
        OkFunction okFunction;
        AltFunction altFunction;

        public ErrorWindow(ErrorCode errorCode, string extraMsg)
        {
            InitializeComponent();

            Error error = ErrorCodes.GetErrorInfo(errorCode, extraMsg);

            Text = error.title;
            ErrorMessage.Text = error.message + " " + extraMsg;
            ErrorOKButton.Text = error.ok;
            ErrorAltButton.Text = error.alt;
            okFunction = error.okFunc;
            altFunction = error.altFunc;

            ErrorAltButton.Visible = altFunction != AltFunction.None;

            ShowDialog();
        }

        private void ErrorOKButton_Click(object sender, EventArgs e)
        {
            switch (okFunction)
            {
                case OkFunction.CloseApp:
                    Application.Exit();
                    break;
                case OkFunction.CloseErrorWindow:
                    Close();
                    break;
                case OkFunction.YesDeleteMap:
                    Application.OpenForms.OfType<Main>().FirstOrDefault().RemoveSelectedMap();
                    Close();
                    break;
            }
        }

        private void ErrorAltButton_Click(object sender, EventArgs e)
        {
            switch (altFunction)
            {
                case AltFunction.OpenDirectory:
                    Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    break;
                case AltFunction.CloseErrorWindow:
                    Close();
                    break;
            }
        }
    }
}
