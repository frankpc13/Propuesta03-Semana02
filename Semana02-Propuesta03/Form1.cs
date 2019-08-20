using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Semana02_Propuesta03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["kotoha"].ConnectionString);

        public void fillComboYear()
        {
            using (SqlCommand command = new SqlCommand("usp_years_orders", connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    comboBoxYear.DataSource = dataTable;
                    comboBoxYear.DisplayMember = "Years";
                    comboBoxYear.ValueMember = "order_years";
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            fillComboYear();
        }

        private void comboBoxYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int codigo;
            codigo = Convert.ToInt32(comboBoxYear.SelectedValue);
            using (SqlCommand command = new SqlCommand("usp_month_orders_2", connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@orderYear", codigo);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    comboBoxMonth.DataSource = dataTable;
                    comboBoxMonth.DisplayMember = "month_name";
                    comboBoxMonth.ValueMember = "month_number";
                }
            }
        }

        private void comboBoxMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int year, month;
            year = Convert.ToInt32(comboBoxYear.SelectedValue);
            month = Convert.ToInt32(comboBoxMonth.SelectedValue);
            using (SqlCommand command = new SqlCommand("usp_filter_by_month_and_date", connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@orderYear", year);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@orderMonth", month);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridOrders.DataSource = dataTable;
                }
            }
        }
    }
}
