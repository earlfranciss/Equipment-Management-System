using EQMS.DatabaseManager;
using EQMS.Details.EquipmentDetail;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EQMS.Report
{
    public class ReportViewModel
    {
        public readonly IReportDAO reportDAO;
        public readonly dbm db = new dbm();
        public ReportViewModel()
        {
            reportDAO = new ReportDAO(db.connectionString);
        }

        public List<(string EquipmentName, int TotalCount)> GetData()
        {
            return reportDAO.GetData(MostUsedEquipment());
        }

        private string MostUsedEquipment()
        {
            return "WITH CheckoutCounts AS (SELECT e.Name AS EquipmentName, " +
                "COUNT(c.ReservationID) AS CheckoutCount " +
                "FROM Checkout c " +
                "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                "GROUP BY e.Name),  " +
                "ReservationCounts AS(SELECT e.Name AS EquipmentName, " +
                "COUNT(DISTINCT r.ReservationID) AS ReservationCount " +
                " FROM Reservation r " +
                " LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                " LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID  " +
                "WHERE c.ReservationID IS NULL " +
                "GROUP BY e.Name)  " +
                "SELECT e.Name AS EquipmentName,  " +
                "COALESCE(cc.CheckoutCount, 0) + COALESCE(rc.ReservationCount, 0) AS TotalCount " +
                "FROM Equipment e " +
                "LEFT JOIN ReservationCounts rc ON e.Name = rc.EquipmentName " +
                " LEFT JOIN CheckoutCounts cc ON e.Name = cc.EquipmentName; ";
        }
    }
}
