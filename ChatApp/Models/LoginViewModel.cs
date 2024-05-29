using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
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
            HttpClient client = httpClient.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new { EmailOrUsername = EmailOrUsername, Password = Password }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7009/api/login/signin", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return RedirectToPage("/ChatPage",responseContent);
            }

            return Page();
        }



    }
}
