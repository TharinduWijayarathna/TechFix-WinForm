﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Web.Script.Serialization;
using StoreClient.Model;

namespace StoreClient
{
    public partial class StockForm : Form
    {
        public StockForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/Stock";
            HttpClient client = new HttpClient();
            Stock item = new Stock();
            item.Name = txtName.Text;
            item.Description = txtDes.Text;
            item.Price = Convert.ToDecimal(txtPrice.Text);
            item.Quantity = Convert.ToInt32(txtStock.Text);
            string info=(new JavaScriptSerializer()).Serialize(item);
            var content=new StringContent(info,
                Encoding.UTF8,"application/json");
            var response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Prodcut added");
                LoadData();
            }
            else
                MessageBox.Show("Fail to add Stock");
        }

        private void LoadData()
        {
            string url = "https://localhost:7135/api/Stock";
            HttpClient client = new HttpClient();
            var resTask = client.GetAsync(url);
            resTask.Wait();
            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();
                var items= readTask.Result;
                dgvItems.DataSource = null;
                dgvItems.DataSource = (new JavaScriptSerializer()).
                                        Deserialize<List<Stock>>(items);

                AddActionColumn();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
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

            // Add Edit button column
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            editButton.Name = "EditColumn";
            editButton.HeaderText = "Action";
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            dgvItems.Columns.Add(editButton);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/Stock/"+txtID.Text;
            HttpClient client = new HttpClient();
            Stock item = new Stock();
            item.Name = txtName.Text;
            item.Description = txtDes.Text;
            item.Price = Convert.ToDecimal(txtPrice.Text);
            item.Quantity = Convert.ToInt32(txtStock.Text);
            string info = (new JavaScriptSerializer()).Serialize(item);
            var content = new StringContent(info,
                Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Prodcut Updated");
                LoadData();
            }
            else
                MessageBox.Show("Fail to update Stock");
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            if (c == dgvItems.Columns.Count - 1)
            {
                txtID.Text = dgvItems.Rows[r].Cells["Id"].Value.ToString();
                txtName.Text = dgvItems.Rows[r].Cells["Name"].Value.ToString();
                txtPrice.Text = dgvItems.Rows[r].Cells["Price"].Value.ToString();
                txtStock.Text = dgvItems.Rows[r].Cells["Quantity"].Value.ToString();
                txtDes.Text = dgvItems.Rows[r].Cells["Description"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7135/api/Stock/" + txtID.Text;
            HttpClient client = new HttpClient();
            var res=client.DeleteAsync(url).Result;
            if (res.IsSuccessStatusCode)
                LoadData();
            else
                MessageBox.Show("Fail to Delete");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomeForm homeForm = new HomeForm();
            homeForm.ShowDialog();
            this.Close();
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtDes_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
