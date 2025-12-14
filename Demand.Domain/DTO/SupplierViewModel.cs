namespace Demand.Domain.DTO
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        // --- Tedarikçi Bilgileri ---
        public string SupplierName { get; set; }          // Ünvan
        public string TaxNumber { get; set; }             // VKN/TCKN
        public string TaxOffice { get; set; }             // Vergi Dairesi

        // --- İletişim Bilgileri ---
        public string Phone { get; set; }
        public string Address { get; set; }

        // --- Diğer ---
        public string Note { get; set; }

        // --- Sistemsel ---
        public SupplierStatus Status { get; set; }        // Pending / Approved / Rejected
        public string StatusText =>
            Status == SupplierStatus.Pending ? "Onay Bekliyor" :
            Status == SupplierStatus.Approved ? "Onaylandı" :
            Status == SupplierStatus.Rejected ? "Reddedildi" :
            "-";

        public DateTime CreatedDate { get; set; }         // Kayıt tarihi
        public DateTime? ApprovedDate { get; set; }       // Onay tarihi
        public DateTime? RejectedDate { get; set; }       // Red tarihi

        // Onay / Red yapan kişi
        public int? ApprovedByUserId { get; set; }
        public int? RejectedByUserId { get; set; }
        public string ApprovedByUserFullName { get; set; }
        public string RejectedByUserFullName { get; set; }
    }

    public enum SupplierStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

}
