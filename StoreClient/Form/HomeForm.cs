﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreClient
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StockForm stockForm = new StockForm();
            stockForm.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuoteRequestForm quoteRequestForm = new QuoteRequestForm();
            quoteRequestForm.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
