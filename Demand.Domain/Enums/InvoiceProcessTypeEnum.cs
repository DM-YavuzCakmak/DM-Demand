namespace Demand.Domain.Enums
{
    public enum InvoiceProcessTypeEnum
    {
        InvoiceTypeChange = 1, //Fatura Tipi Seçme ve Yönlendirme
        SentToDepartment = 2, //Departman Onayına Gönderme
        MatchingToDemand = 3, //Satın Alma Talebi ile Eşleştirme
        FirstLevelApprove = 4, //Onaylama
        SecondLevelApprove = 5, //Onaylama
        ThirdLevelApprove = 6, //Onaylama
        Reject = 7, //Reddetme
        SentToNebim = 8, //Nebim'e Gönderme
        Return = 9 //Hatalı İletilen Faturayı Geri Gönderme
    }
}
