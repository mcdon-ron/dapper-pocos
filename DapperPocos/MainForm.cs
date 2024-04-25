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
            // this is the default
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.WindowsDefaultLocation;

            // check if the saved bounds are nonzero and visible on any screen
            if ((Properties.Settings.Default.WindowPosition != Rectangle.Empty) &&
                (IsVisibleOnAnyScreen(Properties.Settings.Default.WindowPosition)))
            {
                // first set the bounds
                StartPosition = FormStartPosition.Manual;
                DesktopBounds = Properties.Settings.Default.WindowPosition;

                // afterwards set the window state to the saved value (which could be Maximized)
                WindowState = Properties.Settings.Default.WindowState;
            }
            else
            {
                // this resets the upper left corner of the window to windows standards
                StartPosition = FormStartPosition.WindowsDefaultLocation;

                // we can still apply the saved size
                // added gatekeeper, otherwise first time appears as just a title bar
                if (Properties.Settings.Default.WindowPosition != Rectangle.Empty)
                {
                    Size = Properties.Settings.Default.WindowPosition.Size;
                }

                // afterwards set the window state to the saved value (which could be Maximized)
                WindowState = Properties.Settings.Default.WindowState;
            }

            RestoreSplitter(splitterConnectionString, Properties.Settings.Default.SplitterConnectionString);
            RestoreSplitter(splitterSelectOrSproc, Properties.Settings.Default.SplitterSelectOrSproc);
            RestoreSplitter(splitterOutput, Properties.Settings.Default.SplitterOutput);
        }

        private void RestoreSplitter(Splitter splitter, int splitPosition)
        {
            if (splitPosition > splitter.MinSize)
                splitter.SplitPosition = splitPosition;
        }

        private void SaveWindow()
        {
            // only save the WindowState if Normal or Maximized
            switch (WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    Properties.Settings.Default.WindowState = WindowState;
                    break;
                // if Minimized, save WindowState as Normal
                default:
                    Properties.Settings.Default.WindowState = FormWindowState.Normal;
                    break;
            }

            // if WindowsState is Normal, save the DesktopBounds
            if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowPosition = DesktopBounds;
            }
            // for Minimized or Maximized save RestoreBounds
            else
            {
                Properties.Settings.Default.WindowPosition = RestoreBounds;
            }

            Properties.Settings.Default.SplitterConnectionString = splitterConnectionString.SplitPosition;
            Properties.Settings.Default.SplitterSelectOrSproc = splitterSelectOrSproc.SplitPosition;
            Properties.Settings.Default.SplitterOutput = splitterOutput.SplitPosition;

            Properties.Settings.Default.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindow();
        }
    }
}
