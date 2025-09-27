using EQMS.Details.EmployeeDetail;
using System.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using EQMS.DatabaseManager;

namespace EQMS.Details.EquipmentDetail
{
    internal class EquipmentDetailVM
    {
        private readonly IEquipmentDetailDA eqpDA;
        public readonly dbm db = new dbm();
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Serial { get; set; }


        public EquipmentDetailVM()
        {
            eqpDA = new EquipmentDetailDA(db.connectionString);
        }

        public bool SaveEdit(string id, string name, string type, string brand, string serial)
        {
            if (NameErrorText(name).Equals("") && TypeErrorText(type).Equals("") && BrandErrorText(brand).Equals("") && SerialErrorText(id, serial).Equals(""))
            {
                return eqpDA.SaveEdit(id, name, type, brand, serial);
            }
            else
            {
                return false;
            }
        }
        public bool DeleteEquipment(string id)
        {
            return eqpDA.DeleteEquipment(id);
        }

        // Load Equipment Info
        public void LoadEquipmentByID(int userID)
        {
            EQMS.Equipment.Equipment equipment = eqpDA.GetID(userID);
            if (equipment != null)
            {
                this.ID = equipment.ID;
                Name = equipment.Name;
                Type = equipment.Type;
                Brand = equipment.Brand;
                Serial = equipment.Serial;
            }
        }


        //Browse Image 
        public void BrowseImage(int id)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {

                string filePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(filePath);

                eqpDA.SaveImageToDatabase(id, imageData);
            }
        }


        // Loading Equipment Image to Image source
        public BitmapImage SetImage(int id)
        {
            BitmapImage image = eqpDA.GetImage(id);
            if (image != null)
            {
                return image;
            }
            else
            {
                return null;
            }
        }

        public string NameErrorText(string name)
        {
            if (name == "")
            {
                return "Please enter equipment's name";
            }
            else
            {
                return "";
            }
        }
        public string TypeErrorText(string type)
        {
            if (type == "")
            {
                return "Please enter equipment's type";
            }
            else
            {
                return "";
            }
        }
        public string BrandErrorText(string brand)
        {
            if (brand == "")
            {
                return "Please enter equipment's brand";
            }
            else
            {
                return "";
            }
        }
        public string SerialErrorText(string id, string serial)
        {
            if (serial != "")
            {
                if (!eqpDA.SerialExist(id, serial))
                {
                    return "";
                }
                else
                {
                    return "Serial number already exist";
                }
            }
            else
            {
                return "Please enter serial number";
            }
        }
    }
}
