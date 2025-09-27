using EQMS.Borrower;
using EQMS.Details;
using EQMS.Details.BorrowerDetail;
using EQMS.Employee;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace EQMS.Borrower
{
    /// <summary>
    /// Interaction logic for Borrower.xaml
    /// </summary>
    public partial class Borrower : Page
    {
        BorrowerViewModel viewModel = new BorrowerViewModel();
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Borrower()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }
        private void LoadData()
        {
            if (DataContext is BorrowerViewModel viewModel)
            {
                BorrowerDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        private void BorrowerDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    Borrower selectedBorrower = ConvertDataRow(row1);

                    if (selectedBorrower != null)
                    {
                        int id = (selectedBorrower.ID);
                        DetailWindow detail = new DetailWindow(selectedBorrower, id);
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

        private Borrower ConvertDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    Borrower borrower = new Borrower();
                    borrower.ID = Convert.ToInt32(row["ID"]);
                    return borrower;
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

                if (DataContext is BorrowerViewModel viewModel)
                {
                    BorrowerDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is BorrowerViewModel viewModel)
                {
                    BorrowerDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }
    }
}
