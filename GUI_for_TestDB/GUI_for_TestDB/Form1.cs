using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;

namespace GUI_for_TestDB
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource = new BindingSource();

        private Button departmentButton = new Button();
        private Button employeeButton = new Button();
        private Button addButton = new Button();
        private Button editButton = new Button();
        private Button deleteButton = new Button();
        private Button show_departmentButton = new Button();

        public string department_table_name = "dbo.Department";
        public string employee_table_name = "dbo.Empoyee";
        public bool which_table = true;
        public int which_table_switch = 1;

        public Logic view_table = new Logic();

        public Form1()
        {
            InitializeComponent();

            dataGridView.Dock = DockStyle.Fill;
            dataGridView.MultiSelect = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            departmentButton.Text = "Department";
            employeeButton.Text = "Employee";
            addButton.Text = "Add";
            editButton.Text = "Edit";
            deleteButton.Text = "Delete";
            show_departmentButton.Text = "Show";

            departmentButton.Click += new System.EventHandler(departmentButton_Click);
            employeeButton.Click += new System.EventHandler(employeeButton_Click);
            addButton.Click += new System.EventHandler(addButton_Click);
            editButton.Click += new System.EventHandler(editButton_Click);
            deleteButton.Click += new System.EventHandler(deleteButton_Click);
            show_departmentButton.Click += new System.EventHandler(show_departmentButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { departmentButton, employeeButton, addButton, editButton, deleteButton, show_departmentButton });

            this.Controls.AddRange(new Control[] { dataGridView, panel });
            this.Load += new System.EventHandler(Form1_Load);
            this.Text = "TestDB";
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            dataGridView.DataSource = bindingSource;
            view_table.Query(dataGridView, bindingSource, "select * from " + department_table_name);

        }

        private void departmentButton_Click(object sender, System.EventArgs e)
        {
            which_table = true;
            show_departmentButton.Enabled = true;
            dataGridView.DataSource = bindingSource;
            view_table.Query(dataGridView, bindingSource, "select * from " + Which_Table(which_table));
        }

        private void employeeButton_Click(object sender, System.EventArgs e)
        {
            which_table = false;
            show_departmentButton.Enabled = false;
            dataGridView.DataSource = bindingSource;
            view_table.Query(dataGridView, bindingSource, "select * from " + Which_Table(which_table));
            dataGridView.Columns[5].Width = 65;
            dataGridView.Columns[6].Width = 70;
            dataGridView.Columns[9].Width = 50;
        }

        public string Which_Table(bool which)
        {
            if (which)
            {
                which_table_switch = 1;
                return department_table_name;
            }
            else
            {
                which_table_switch = 0;
                return employee_table_name;
            }
        }

        private void addButton_Click(object sender, System.EventArgs e)
        {
            switch (which_table_switch)
            {
                case 1:
                    {
                        AddDepartment ad = new AddDepartment();
                        ad.table_name = Which_Table(which_table);
                        ad.Query(dataGridView, bindingSource);
                        break;
                    }
                case 0:
                    {
                        AddEmployee ae = new AddEmployee();
                        ae.table_name = Which_Table(which_table);
                        ae.Query(dataGridView, bindingSource);
                        dataGridView.Columns[5].Width = 65;
                        dataGridView.Columns[6].Width = 70;
                        dataGridView.Columns[9].Width = 50;
                        break;
                    }
            }

            /*sql = a.Form_Builder(Which_Table(which_table));
            

            GetData(dataAdapter.SelectCommand.CommandText);
            dataGridView.DataSource = bindingSource;
            //GetData("insert into dbo.Department(ID, Name, Code, ParentdepartmentID) values (NEWID(), 'KOKA', 'F32', NEWID())");
            //           GetData("delete from dbo.Department where name = 'KOKA'");

            GetData(sql);
            GetData("select * from " + Which_Table(which_table)); */
        }

        private void editButton_Click(object sender, System.EventArgs e)
        {
            switch (which_table_switch)
            {
                case 1:
                    {
                        EditDepartment ed = new EditDepartment();
                        ed.table_name = Which_Table(which_table);
                        ed.Query(dataGridView, bindingSource);
                        break;
                    }
                case 0:
                    {
                        EditEmployee ee = new EditEmployee();
                        ee.table_name = Which_Table(which_table);
                        ee.Query(dataGridView, bindingSource);
                        dataGridView.Columns[5].Width = 65;
                        dataGridView.Columns[6].Width = 70;
                        dataGridView.Columns[9].Width = 50;
                        break;
                    }
            }
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            Delete d = new Delete();
            d.table_name = Which_Table(which_table);
            d.Query(dataGridView, bindingSource);
        }

        private void show_departmentButton_Click(object sender, System.EventArgs e)
        {
            Form show_dep = new Form();
            DataGridView showDataGridView = new DataGridView();
            Logic show = new Logic();

            show_dep.Text += dataGridView.CurrentRow.Cells[1].FormattedValue;
            show_dep.Controls.Add(showDataGridView);
            show.Execute(showDataGridView, bindingSource, "select FirstName, Surname, Patronymic, Age from dbo.empoyee where DepartmentID = '" + dataGridView.CurrentRow.Cells[0].FormattedValue + "'");
        }
    }
}

 
