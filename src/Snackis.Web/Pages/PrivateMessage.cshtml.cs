using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Services;
using Snackis.Domain.Entities;

namespace Snackis.Web.Pages
{
    [Authorize(Policy = "ShouldBeUser")]
    public class PrivateMessageModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPrivateMessageService _privateMessageService;

        public PrivateMessageModel(UserManager<AppUser> userManager, IPrivateMessageService privateMessageService)
        {
            _userManager = userManager;
            _privateMessageService = privateMessageService;
        }

        public string CurrentUserId { get; set; } = string.Empty; // Inloggad användares ID


        [BindProperty(SupportsGet = true)]
        public string? SearchEmail { get; set; } 

        
        public AppUser? ReceiverUser { get; set; } // Hittad mottagare

        
        public List<PrivateMessage> Conversation { get; set; } = new(); // Konversation med vald användare

        
        public List<PrivateMessage> Inbox { get; set; } = new(); 

        
        private Dictionary<string, string> _userEmailCache = new(); // Cache för att slå upp e-post från userId i inkorg
        

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return;

            CurrentUserId = currentUser.Id;
            Inbox = await _privateMessageService.GetInboxAsync(CurrentUserId);

            await BuildUserEmailCacheAsync(Inbox);

            if (!string.IsNullOrWhiteSpace(SearchEmail))
            {
                ReceiverUser = await _userManager.FindByEmailAsync(SearchEmail);

                if (ReceiverUser != null && ReceiverUser.Id != CurrentUserId) 
                {
                    Conversation = await _privateMessageService.GetConversationAsync(CurrentUserId, ReceiverUser.Id);
                }
                else if (ReceiverUser?.Id == CurrentUserId)
                {
                    // Tillåt inte att skicka till sig själv
                    ReceiverUser = null;
                }
            }
        }
        public async Task<IActionResult> OnPostSendAsync(string receiverId, string content, string? searchEmail)
        {
            var currentUser = await _userManager.GetUserAsync(User); 
            if (currentUser == null)
            {
                return RedirectToPage();
            }

            if (!string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(receiverId)) 
            {
                await _privateMessageService.SendAsync(currentUser.Id, receiverId, content.Trim());
            }
            return RedirectToPage(new { searchEmail });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int messageId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToPage();
            }

            await _privateMessageService.DeleteAsync(messageId, currentUser.Id);

            return RedirectToPage(new { searchEmail = SearchEmail });
        }

        //  för att hämta e-post från cache (används i vyn)
        public string GetUserEmail(string userId)
        {
            return _userEmailCache.TryGetValue(userId, out var email) ? email : userId;
        }

        private async Task BuildUserEmailCacheAsync(List<PrivateMessage> messages)
        {
            var userIds = messages
                .SelectMany(m => new[] { m.SenderId, m.ReceiverId })
                .Distinct()
                .ToList();

            foreach (var id in userIds)
            {
                if (!_userEmailCache.ContainsKey(id))
                {
                    var user = await _userManager.FindByIdAsync(id);
                    _userEmailCache[id] = user?.Name ?? user?.Email ?? id;
                }
            }
        }
    }
}
