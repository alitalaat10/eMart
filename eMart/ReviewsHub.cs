using eMart.Models;
using Microsoft.AspNetCore.SignalR;

namespace eMart
{
    public class ReviewsHub: Hub
    {
        public async Task SendReview(string username,string comment,int rating)
        {
            
            try
            {
                // Your logic here
                await Clients.All.SendAsync("ReceiveReview", username, comment, rating);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a user-friendly message
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
