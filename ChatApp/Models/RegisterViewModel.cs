using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace ChatApp.Models
{
    public class RegisterViewModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegisterViewModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            HttpClient client = httpClientFactory.CreateClient();

            StringContent content = new StringContent(JsonConvert.SerializeObject(new { Name = Name, Username = Username, Email = Email, Password = Password }),Encoding.UTF8,"application/json");

            var response = await client.PostAsync("https://localhost:7009/api/login/signup", content);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return RedirectToPage("Login", responseContent);
            }
            Message = await response.Content.ReadAsStringAsync() ;
            return Page();
        }

    }
}
