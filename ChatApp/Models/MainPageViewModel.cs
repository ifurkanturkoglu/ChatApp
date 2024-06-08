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
           
            var responseFriends = await newClient.GetAsync("https://localhost:7009/api/chat/friends");
            var responseGroups = await newClient.GetAsync("https://localhost:7009/api/chat/groups");

            if (responseFriends.IsSuccessStatusCode)
            {
                var responseFriendsString = await responseFriends.Content.ReadAsStringAsync();
                var responseGroupsString = await responseGroups.Content.ReadAsStringAsync();
                FriendList = JsonSerializer.Deserialize<List<string>>(responseFriendsString);
                GroupsList = JsonSerializer.Deserialize<List<string>>(responseGroupsString);
            }
            //var ss = Request.Query.TryGetValue("handler", out var deneme);

            return Page();
        }
    }
}
