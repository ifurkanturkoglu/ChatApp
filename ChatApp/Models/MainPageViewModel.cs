using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChatApp.Models
{
    public class MainPageViewModel : PageModel
    {
        private readonly IHttpClientFactory client;

        public MainPageViewModel(IHttpClientFactory _client)
        {
            client = _client;
        }
        public string Token { get; set; }
        public List<string> FriendList { get; set; }
        public List<string> GroupsList { get; set; }


        public async Task<IActionResult> OnGet()
        {
            string token = TempData["Token"].ToString();
          
            HttpClient newClient = client.CreateClient();
            
            newClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            var response = await newClient.GetAsync("https://localhost:7009/api/friends");


            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                FriendList = JsonSerializer.Deserialize<List<string>>(responseString);
            }

            //var ss = Request.Query.TryGetValue("handler", out var deneme);


            return Page();
        }
    }
}
