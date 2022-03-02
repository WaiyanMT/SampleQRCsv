using SampleQRCsv.BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleQRCsv
{
    public partial class Form1 : Form
    {
        BusinessLayer _BusinessLayer = new BusinessLayer();
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtSampleID.Text.Trim() == string.Empty)
            {
                MessageBox.Show("SampleQR is empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtMachineId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("MachineID is empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string bearerToken = _BusinessLayer.GetToken();

                if (bearerToken == "")
                {
                    lblError.Text = "Incorrect UserName or Password.";
                    lblError.Visible = true;
                    lblMsg.Visible = true;
                }
                else if (bearerToken == "Connection issue")
                {
                    lblError.Text = bearerToken;
                    lblError.Visible = true;
                    lblMsg.Visible = true;
                }
                else
                {
                    dynamic resource = _BusinessLayer.RetrieveResource(txtMachineId.Text.Trim(), bearerToken);

                    if (resource.Id > 0)
                    {

                        //Generate Csv file
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "CSV (*.csv)|*.csv";
                        sfd.FileName = "Output.csv";
                        bool fileError = false;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (File.Exists(sfd.FileName))
                            {
                                try
                                {
                                    File.Delete(sfd.FileName);
                                }
                                catch (IOException ex)
                                {
                                    fileError = true;
                                    MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                                }
                            }
                            if (!fileError)
                            {
                                try
                                {
                                    List<string> list = new List<string>();
                                    list.Add(txtSampleID.Text);
                                    list.Add(Convert.ToString(resource.Id));

                                    string[] array = list.ToArray();

                                    int columnCount = array.Length;
                                    string columnNames = "";
                                    string[] outputCsv = new string[array.Length];
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        if (i == 0)
                                            columnNames += "SampleQRCode" + ",";
                                        else
                                            columnNames += "ResourceId";
                                    }
                                    outputCsv[0] += columnNames;

                                    for (int i = 1; (i - 1) < 1; i++)
                                    {
                                        for (int j = 0; j < columnCount; j++)
                                        {
                                            outputCsv[i] += array[j].ToString() + ",";

                                        }
                                    }

                                    File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                                    MessageBox.Show("Data Exported Successfully !!!", "Info");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error :" + ex.Message);
                                }
                            }
                        }
                        lblError.Visible = false;
                        lblMsg.Visible = false;
                    }
                    else
                    {
                        lblError.Text = "Can't retrieve resoureid for this machine tagid";
                        lblError.Visible = true;
                        lblMsg.Visible = true;
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtSampleID.Text = "";
            txtMachineId.Text = "";
            lblError.Visible = false;
            lblMsg.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
