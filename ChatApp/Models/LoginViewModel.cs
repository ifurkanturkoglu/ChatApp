using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace ChatApp.Models
{
    public class LoginViewModel : PageModel
    {

        private readonly IHttpClientFactory httpClient;

        public LoginViewModel(IHttpClientFactory _httpClient)
        {
            httpClient = _httpClient;
        }

        [BindProperty]
        public string EmailOrUsername { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClientHandler = new HttpClientHandler();
  
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            HttpClient client = new HttpClient(httpClientHandler);
            client = httpClient.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new { EmailOrUsername = EmailOrUsername, Password = Password }),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("http://localhost:7009/api/auth/signin", content);

            Console.Write("girdi");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return RedirectToPage("/MainPage");
            }

            return Page();
        }
    }
}
