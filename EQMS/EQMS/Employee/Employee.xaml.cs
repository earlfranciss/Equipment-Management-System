using EQMS.Home;
using EQMS.Details;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Policy;
using System.Data;
using System.Xml.Linq;
using System.ComponentModel;
using EQMS.Equipment;

namespace EQMS.Employee
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Page
    {
        EmployeeViewModel viewModel = new EmployeeViewModel();
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Employee()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }

        public void LoadData()
        {
            if (DataContext is EmployeeViewModel viewModel)
            {
                EmployeeDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        private void EmployeeDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    Employee selectedEmployee = ConvertDataRow(row1);

                    if (selectedEmployee != null)
                    {
                        int id = (selectedEmployee.ID);
                        DetailWindow detail = new DetailWindow(selectedEmployee, id);
                        detail.DataSaved += DetailWindow_DataSaved;
                        detail.Show();
                    }
                }
            }
        }

        // Refresh the grid table
        private void DetailWindow_DataSaved(object sender, EventArgs e)
        {
            LoadData();
        }

        private Employee ConvertDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    Employee employee = new Employee(); 
                    employee.ID = Convert.ToInt32(row["ID"]);
                    return employee;        
                }
                else
                {
                    MessageBox.Show("DataRow is null.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        private void CloseOtherWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is DetailWindow)
                {
                    window.Close();
                    break;
                }
            }

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
            {
                textSearch.Visibility = Visibility.Collapsed;

                if (DataContext is EmployeeViewModel viewModel)
                {
                    EmployeeDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is EmployeeViewModel viewModel)
                {
                    EmployeeDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }
    }
}
