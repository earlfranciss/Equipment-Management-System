using EQMS.Details;
using EQMS.Employee;
using EQMS.Equipment;
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

namespace EQMS.Equipment
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : Page
    {
        EquipmentViewModel viewModel = new EquipmentViewModel();
        public int ID { get; set; }
        public new string Name { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Serial { get; set; }
        public string State { get; set; }
        public string Status { get; set; }

        public Equipment()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }
        private void LoadData()
        {
            if (DataContext is EquipmentViewModel viewModel)
            {
                EquipmentDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        private void EquipmentDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    Equipment selectedEquipment = ConvertDataRow(row1);

                    if (selectedEquipment != null)
                    {
                        int id = (selectedEquipment.ID);
                        DetailWindow detail = new DetailWindow(selectedEquipment, id);
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

        private Equipment ConvertDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    Equipment equipment = new Equipment();
                    equipment.ID = Convert.ToInt32(row["ID"]);
                    return equipment;
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

                if (DataContext is EquipmentViewModel viewModel)
                {
                    EquipmentDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is EquipmentViewModel viewModel)
                {
                    EquipmentDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }
    }
}
