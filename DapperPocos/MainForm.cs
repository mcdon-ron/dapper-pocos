using PocoExtension;
using System;
using System.Data.SqlClient;
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
    }
}
