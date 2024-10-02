﻿using StoreClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StoreClient
{
    public partial class QuoteRequestForm : Form
    {
        public QuoteRequestForm()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            string url = "https://localhost:7135/api/QuoteRequest";
            HttpClient client = new HttpClient();
            var resTask = client.GetAsync(url);
            resTask.Wait();
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();
                var items = readTask.Result;
                dgvItems.DataSource = null;
                dgvItems.DataSource = (new JavaScriptSerializer()).
                                        Deserialize<List<QuoteRequest>>(items);

                AddActionColumn();
            }
        }

        private void QuoteRequestForm_Load(object sender, EventArgs e)
        {
            LoadData();
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Gabriola", 13, FontStyle.Bold);
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvItems.DefaultCellStyle.Font = new Font("Gabriola", 12, FontStyle.Regular);
            dgvItems.DefaultCellStyle.ForeColor = Color.Black;
            dgvItems.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvItems.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvItems.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvItems.RowTemplate.Height = 30;
            dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            AddActionColumn();
        }

        private void AddActionColumn()
        {
            // Remove existing action column if it exists
            if (dgvItems.Columns["EditColumn"] != null)
                dgvItems.Columns.Remove("EditColumn");

            //remove existing action column if it exists
            if (dgvItems.Columns["AddItemsColumn"] != null)
                dgvItems.Columns.Remove("AddItemsColumn");

            // Add Edit button column
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            editButton.Name = "EditColumn";
            editButton.HeaderText = "Action";
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(editButton);

            // Add Order Items button column
            DataGridViewButtonColumn addItemsButton = new DataGridViewButtonColumn();
            addItemsButton.Name = "AddItemsColumn";
            addItemsButton.HeaderText = "Action";
            addItemsButton.Text = "Add Items";
            addItemsButton.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(addItemsButton);
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/QuoteRequest";

            using (HttpClient client = new HttpClient())
            {
                QuoteRequest item = new QuoteRequest
                {
                    Name = txtName.Text,
                    Date = dateTimePicker1.Value,
                    SupplierId = comboBox1.SelectedIndex + 1
                };

                string info = JsonConvert.SerializeObject(item);
                var content = new StringContent(info, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Quote Request added successfully");
                        LoadData();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to add Quote Request: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            if (c == dgvItems.Columns.Count - 2)
            {
                txtID.Text = dgvItems.Rows[r].Cells["Id"].Value.ToString();
                txtName.Text = dgvItems.Rows[r].Cells["Name"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dgvItems.Rows[r].Cells["Date"].Value);
                comboBox1.SelectedIndex = Convert.ToInt32(dgvItems.Rows[r].Cells["SupplierId"].Value) - 1;
            } else if (c == dgvItems.Columns.Count - 1)
            {
                int id = Convert.ToInt32(dgvItems.Rows[r].Cells["Id"].Value);
                this.Hide();
                QuoteRequestItemForm form = new QuoteRequestItemForm(id);
                form.Show();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/QuoteRequest/" + txtID.Text;

            using (HttpClient client = new HttpClient())
            {
                QuoteRequest item = new QuoteRequest
                {
                    Name = txtName.Text,
                    Date = dateTimePicker1.Value
                };

                string info = JsonConvert.SerializeObject(item);
                var content = new StringContent(info, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PutAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Quote Request updated successfully");
                        LoadData();
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        MessageBox.Show($"Failed to update Quote Request: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/QuoteRequest/" + txtID.Text;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.DeleteAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Quote Request deleted successfully");
                        LoadData();
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        MessageBox.Show($"Failed to delete Quote Request: {response.StatusCode} - {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomeForm homeForm = new HomeForm();
            homeForm.ShowDialog();
            this.Close();
        }
    }
}
