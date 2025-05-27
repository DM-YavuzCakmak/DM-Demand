using Demand.Presentation.Utilities.Email;
using Demand.Domain.Entities.FormDataEntity;

namespace Demand.Presentation.Services
{
    public class EMailService
    {
        private readonly IConfiguration _config;

        public EMailService(IConfiguration config)
        {
            _config = config;
        }


        public async Task<bool> SendEmailAsync(FormData formData)
        {
            try
            {
                var selectedServices = string.Join(", ",
                    formData.Services.GetType().GetProperties()
                        .Where(p => (bool)p.GetValue(formData.Services))
                        .Select(p => p.Name)
                );
                string body = $@"
                    <b>Name:</b> {formData.Name}<br/>
                    <b>Email:</b> {formData.Email}<br/>
                    <b>Subject:</b> {formData.Subject}<br/>
                    <b>Services:</b> {selectedServices}<br/>
                    <b>Message:</b><br/>{formData.Message}
                    ";
                await Task.Run(() =>
                {
                    EmailHelper.SendEmail(
                        new List<string> { formData.Email /*"samet.bas@demmuseums.com" */},
                        $"Contact Form: {formData.Subject}",
                        body
                    );
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail gönderim hatası: " + ex.Message);
                return false;
            }
        }
    }
}
