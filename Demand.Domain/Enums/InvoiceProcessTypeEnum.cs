namespace Demand.Domain.Enums
{
    public enum InvoiceProcessTypeEnum
    {
        InvoiceTypeChange = 1, //Fatura Tipi Seçme ve Yönlendirme
        SentToDepartment = 2, //Departman Onayına Gönderme
        MatchingToDemand = 3, //Satın Alma Talebi ile Eşleştirme
        Approve = 4, //Onaylama
        Reject = 5, //Reddetme
        SentToNebim = 6 //Nebim'e Gönderme
    }
}
