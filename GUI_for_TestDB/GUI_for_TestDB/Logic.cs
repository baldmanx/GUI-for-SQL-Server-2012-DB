using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_for_TestDB
{
    public class Logic
    {
        public SqlDataAdapter dataAdapter = new SqlDataAdapter();

        public Form action_form = new Form();

        public Logic()
        {

        }

        public virtual void Execute(DataGridView dataGridView, BindingSource bindingSource, string command)
        {
            try
            {
                String connectionString = @"Data source = DESKTOP-RP185DV\SQLEXPRESS; Initial catalog = TestDB; Integrated security = true; User ID = DESKTOP-RP185DV\dehnk; Password = 1803";

                dataAdapter = new SqlDataAdapter(command, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Query(DataGridView dataGridView, BindingSource bindingSource, string command)
        {
            Execute(dataGridView, bindingSource, command);
        }

        public virtual void Query(DataGridView dataGridView, BindingSource bindingSource)
        {

        }
    }

    public class AddDepartment : Logic
    {
        public string table_name;

        public AddDepartment()
        {

        }

        public override void Query(DataGridView dataGridView, BindingSource bindingSource)
        {

            action_form = new Form();
            Label nameLabel = new Label();
            Label codeLabel = new Label();
            TextBox nameTextBox = new TextBox();
            TextBox codeTextBox = new TextBox();
            Button addButton = new Button();

            string sql = "";

            action_form.AutoSize = true;
            action_form.StartPosition = FormStartPosition.CenterScreen;
            action_form.Text = "Adding data into the " + table_name;

            nameLabel.Text = "Name:";
            codeLabel.Text = "Code:";
            addButton.Text = "Add";
                        
            nameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            addButton.Click += new System.EventHandler(addButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { nameLabel, nameTextBox, codeLabel, codeTextBox, addButton });
            action_form.Controls.Add(panel);

            action_form.ShowDialog();

            void IsLetter(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsLetter(symbol))
                {
                    e.Handled = true;
                }
            }

            void addButton_Click(object sender, System.EventArgs e)
            {
                sql = "insert into " + table_name + " (ID, Name, Code, ParentdepartmentID) values (NEWID(), '" + nameTextBox.Text + "', '" + codeTextBox.Text + "', NEWID())";

                Execute(dataGridView, bindingSource, sql);
                Execute(dataGridView, bindingSource, "select * from " + table_name);

                action_form.Dispose();
            }
        }
    }

    public class AddEmployee : Logic
    {
        public string table_name;

        public AddEmployee()
        {

        }

        public override void Query(DataGridView dataGridView, BindingSource bindingSource)
        {

            action_form = new Form();

            Label firstNameLabel = new Label();
            Label surnameLabel = new Label();
            Label patronLabel = new Label();
            Label dateLabel = new Label();
            Label docSeriesLabel = new Label();
            Label docNumberLabel = new Label();
            Label positionLabel = new Label();

            TextBox firstNameTextBox = new TextBox();
            TextBox surnameTextBox = new TextBox();
            TextBox patronTextBox = new TextBox();
            DateTimePicker dateTime = new DateTimePicker();
            TextBox docSeriesTextBox = new TextBox();
            TextBox docNumberTextBox = new TextBox();
            TextBox positionTextBox = new TextBox();

            Button addButton = new Button();
      
            dateTime.CustomFormat = ("yyyy.MM.dd");
            dateTime.Format = DateTimePickerFormat.Custom;
            dateTime.Width = firstNameTextBox.Width;

            string sql = "";

            action_form.AutoSize = true;
            action_form.StartPosition = FormStartPosition.CenterScreen;
            action_form.Text = "Adding data into the " + table_name;
            
            firstNameLabel.Text = "First name:";
            surnameLabel.Text = "Surname:";
            patronLabel.Text = "Patronymic:";
            dateLabel.Text = "Date of birth:";
            docSeriesLabel.Text = "Document series:";
            docNumberLabel.Text = "Document number:";
            positionLabel.Text = "Position:";
            addButton.Text = "Add";

            firstNameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            surnameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            patronTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            docSeriesTextBox.KeyPress += new KeyPressEventHandler(IsDigit);
            docNumberTextBox.KeyPress += new KeyPressEventHandler(IsDigit);
            positionTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            addButton.Click += new System.EventHandler(addButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { firstNameLabel, firstNameTextBox, surnameLabel, surnameTextBox, patronLabel, patronTextBox, dateLabel, dateTime, docSeriesLabel, docSeriesTextBox, docNumberLabel, docNumberTextBox, positionLabel, positionTextBox, addButton });
            action_form.Controls.Add(panel);

            action_form.ShowDialog();

            void IsLetter(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsLetter(symbol))
                {
                    e.Handled = true;
                }
            }

            void IsDigit(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsDigit(symbol))
                {
                    e.Handled = true;
                }
            }

            void addButton_Click(object sender, System.EventArgs e)
            {
                sql = "insert into " + table_name + "(FirstName, Surname, Patronymic, DateOfBirth, DocSeries, DocNumber, Position, DepartmentID, Age) values('" + firstNameTextBox.Text + "', '" + surnameTextBox.Text + "', '" + patronTextBox.Text + "', (select convert(datetime, '" + DateFormat(dateTime.Text) + "')), " + docSeriesTextBox.Text + ", " + docNumberTextBox.Text + ", '" + positionTextBox.Text + "', NEWID(), (year(GetDate()) -  year('" + dateTime.Text + "')))";

                // GetData("ALTER TABLE dbo.Department NOCHECK CONSTRAINT FK_Empoyee_DepartmentID");
                Execute(dataGridView, bindingSource, sql);
                //Execute(dataGridView, bindingSource, "update dbo.empoyee set DateOfBirth = (select convert(datetime, '1980.10.25')) where ID = 1");
                Execute(dataGridView, bindingSource, "select * from " + table_name);

                action_form.Dispose();
            }


            string DateFormat(string date_string)
            {
                string[] date_string_arr = date_string.Split('.');

                date_string = date_string_arr[0] + date_string_arr[1] + date_string_arr[2];

                return date_string;
            }
        }
    }

    public class EditDepartment : Logic
    {
        public string table_name;

        public EditDepartment()
        {

        }

        public override void Query(DataGridView dataGridView, BindingSource bindingSource)
        {
            action_form = new Form();

            Label nameLabel = new Label();
            Label codeLabel = new Label();

            TextBox nameTextBox = new TextBox();
            TextBox codeTextBox = new TextBox();

            Button editButton = new Button();

            string sql = "";

            action_form.AutoSize = true;
            action_form.StartPosition = FormStartPosition.CenterScreen;
            action_form.Text = "Editting " + table_name + " data";

            nameLabel.Text = "Name:";
            codeLabel.Text = "Code:";

            nameTextBox.Text += dataGridView.CurrentRow.Cells[1].FormattedValue;
            codeTextBox.Text += dataGridView.CurrentRow.Cells[2].FormattedValue;            
            editButton.Text = "Edit";

            nameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            editButton.Click += new System.EventHandler(addButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { nameLabel, nameTextBox, codeLabel, codeTextBox, editButton });
            action_form.Controls.Add(panel);

            action_form.ShowDialog();

            void IsLetter(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsLetter(symbol))
                {
                    e.Handled = true;
                }
            }

            void addButton_Click(object sender, System.EventArgs e)
            {
                sql = "update " + table_name + " set Name = '" + nameTextBox.Text + "', Code = '" + codeTextBox.Text + "' where ID = '" + dataGridView.CurrentRow.Cells[0].FormattedValue + "'";

                Execute(dataGridView, bindingSource, sql);
                Execute(dataGridView, bindingSource, "select * from " + table_name);

                action_form.Dispose();
            }
        }
    }

    public class EditEmployee : Logic
    {
        public string table_name;

        public EditEmployee()
        {

        }

        public override void Query(DataGridView dataGridView, BindingSource bindingSource)
        {
            action_form = new Form();

            Label firstNameLabel = new Label();
            Label surnameLabel = new Label();
            Label patronLabel = new Label();
            Label dateLabel = new Label();
            Label docSeriesLabel = new Label();
            Label docNumberLabel = new Label();
            Label positionLabel = new Label();

            TextBox firstNameTextBox = new TextBox();
            TextBox surnameTextBox = new TextBox();
            TextBox patronTextBox = new TextBox();
            DateTimePicker dateTime = new DateTimePicker();
            TextBox docSeriesTextBox = new TextBox();
            TextBox docNumberTextBox = new TextBox();
            TextBox positionTextBox = new TextBox();

            Button editButton = new Button();

            dateTime.CustomFormat = ("yyyy.MM.dd");
            dateTime.Format = DateTimePickerFormat.Custom;
            dateTime.Width = firstNameTextBox.Width;

            string sql = "";
            action_form.StartPosition = FormStartPosition.CenterScreen;
            action_form.Text = "Editting " + table_name + " data";

            firstNameLabel.Text = "First name:";
            surnameLabel.Text = "Surname:";
            patronLabel.Text = "Patronymic:";
            dateLabel.Text = "Date of birth:";
            docSeriesLabel.Text = "Document series:";
            docNumberLabel.Text = "Document number:";
            positionLabel.Text = "Position:";
                        
            firstNameTextBox.Text += dataGridView.CurrentRow.Cells[1].FormattedValue;
            surnameTextBox.Text += dataGridView.CurrentRow.Cells[2].FormattedValue;
            patronTextBox.Text += dataGridView.CurrentRow.Cells[3].FormattedValue;
            dateTime.Text += dataGridView.CurrentRow.Cells[4].FormattedValue;
            docSeriesTextBox.Text += dataGridView.CurrentRow.Cells[5].FormattedValue;
            docNumberTextBox.Text += dataGridView.CurrentRow.Cells[6].FormattedValue;
            positionTextBox.Text += dataGridView.CurrentRow.Cells[7].FormattedValue;
           
            editButton.Text = "Edit";

            firstNameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            surnameTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            patronTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            docSeriesTextBox.KeyPress += new KeyPressEventHandler(IsDigit);
            docNumberTextBox.KeyPress += new KeyPressEventHandler(IsDigit);
            positionTextBox.KeyPress += new KeyPressEventHandler(IsLetter);
            editButton.Click += new System.EventHandler(addButton_Click);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.Controls.AddRange(new Control[] { firstNameLabel, firstNameTextBox, surnameLabel, surnameTextBox, patronLabel, patronTextBox, dateLabel, dateTime, docSeriesLabel, docSeriesTextBox, docNumberLabel, docNumberTextBox, positionLabel, positionTextBox, editButton });
            action_form.Controls.Add(panel);

            action_form.ShowDialog();

            void IsLetter(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsLetter(symbol))
                {
                    e.Handled = true;
                }
            }

            void IsDigit(object sender, System.Windows.Forms.KeyPressEventArgs e)
            {
                char symbol = e.KeyChar;

                if (!Char.IsDigit(symbol))
                {
                    e.Handled = true;
                }
            }

            void addButton_Click(object sender, System.EventArgs e)
            {
                sql = "update " + table_name + " set FirstName = '" + firstNameTextBox.Text + "', SurName = '" + surnameTextBox.Text + "', Patronymic = '" + patronTextBox.Text + "', DateOfBirth = (select convert(datetime, '" + DateFormat(dateTime.Text) + "')), DocSeries = " + docSeriesTextBox.Text + ", DocNumber = " + docNumberTextBox.Text + ", Position = '" + positionTextBox.Text + "', Age = (year(GetDate()) - (select year('" + dateTime.Text + "') from dbo.empoyee where ID =  " + dataGridView.CurrentRow.Cells[0].FormattedValue + ")) where ID = " + dataGridView.CurrentRow.Cells[0].FormattedValue;

                Execute(dataGridView, bindingSource, sql);
                Execute(dataGridView, bindingSource, "select * from " + table_name);

                action_form.Dispose();
            }

            string DateFormat(string date_string)
            {
                string[] date_string_arr = date_string.Split('.');

                date_string = date_string_arr[0] + date_string_arr[1] + date_string_arr[2];

                return date_string;
            }
        }
    }

        public class Delete : Logic
    {
        public string table_name;

        public Delete()
        {

        }

        public override void Query(DataGridView dataGridView, BindingSource bindingSource)
        {
            string sql = "";

            var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                sql = "delete from " + table_name + " where ID = '" + dataGridView.CurrentRow.Cells[0].FormattedValue + "'";

                Execute(dataGridView, bindingSource, sql);
                Execute(dataGridView, bindingSource, "select * from " + table_name);
            }
        }
    }

}

