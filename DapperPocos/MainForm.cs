using PocoExtension;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DapperPocos
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            RestoreWindow();
        }

        private void btnSavePocos_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbInputPoco.Lines.Length > 0)
                {
                    saveFileDialog.Title = "Input Poco Save As";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        File.WriteAllLines(saveFileDialog.FileName, txbInputPoco.Lines, Encoding.UTF8);
                }

                if (txbOutputPoco.Lines.Length > 0)
                {
                    saveFileDialog.Title = "Output Poco Save As";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        File.WriteAllLines(saveFileDialog.FileName, txbOutputPoco.Lines, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to Save Pocos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                var sprocOrSelect = txbSprocOrSelect.Text;
                if (string.IsNullOrEmpty(sprocOrSelect))
                    return;

                var isSelectStatement = sprocOrSelect.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase);
                using (var con = new SqlConnection(txbConnectionString.Text))
                {
                    txbOutputPoco.Text = con.GetOutputPoco(sprocOrSelect);
                    // can't get an input poco for a select statement, would throw a SqlException
                    // looks like "sp_HELP" is only valid for stored procedures
                    //txbInputPoco.Text = con.GetInputPoco(sprocOrSelect);
                    if (!isSelectStatement)
                        txbInputPoco.Text = con.GetInputPoco(sprocOrSelect);
                    else
                        txbInputPoco.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to Generate Pocos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                    return true;
            }

            return false;
        }

        // Window size/position save/restore logic based on
        // https://stackoverflow.com/questions/937298/restoring-window-size-position-with-multiple-monitors
        private void RestoreWindow()
        {
            var settings = Properties.Settings.Default;

            // set defaults for WindowState and StartPosition
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.WindowsDefaultLocation;

            var restorePosition = settings.WindowPosition;
            var restoreState = settings.WindowState;

            if (restorePosition != Rectangle.Empty)
            {
                // check if the saved bounds are nonzero and visible on any screen
                if (IsVisibleOnAnyScreen(restorePosition))
                {
                    // first set the bounds
                    StartPosition = FormStartPosition.Manual;
                    DesktopBounds = restorePosition;
                }
                else // the restorePosition was invalid, but can still apply the saved size
                {
                    Size = restorePosition.Size;
                }
            }

            // afterwards, if the window wasn't minimized, will restore WindowState
            // otherwise had previously defaulted WindowState to Normal
            if (restoreState != FormWindowState.Minimized)
                WindowState = restoreState;

            RestoreSplitter(splitterConnectionString, settings.SplitterConnectionString);
            RestoreSplitter(splitterSelectOrSproc, settings.SplitterSelectOrSproc);
            RestoreSplitter(splitterOutput, settings.SplitterOutput);
        }

        private void RestoreSplitter(Splitter splitter, int splitPosition)
        {
            if (splitPosition > splitter.MinSize)
                splitter.SplitPosition = splitPosition;
        }

        private void SaveWindow()
        {
            var settings = Properties.Settings.Default;

            // only save the WindowState if Normal or Maximized
            switch (WindowState)
            {
                // if Normal, save WindowState and DesktopBounds
                case FormWindowState.Normal:
                    settings.WindowPosition = DesktopBounds;
                    settings.WindowState = WindowState;
                    break;
                // if Maximized, save WindowState and RestoreBounds
                case FormWindowState.Maximized:
                    settings.WindowPosition = RestoreBounds;
                    settings.WindowState = WindowState;
                    break;
                // if Minimized, save WindowState as Normal and RestoreBounds
                case FormWindowState.Minimized:
                    settings.WindowPosition = RestoreBounds;
                    settings.WindowState = FormWindowState.Normal;
                    break;
            }

            settings.SplitterConnectionString = splitterConnectionString.SplitPosition;
            settings.SplitterSelectOrSproc = splitterSelectOrSproc.SplitPosition;
            settings.SplitterOutput = splitterOutput.SplitPosition;

            settings.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindow();
        }
    }
}
