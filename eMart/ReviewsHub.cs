using eMart.Data;
using eMart.DTO_Models;
using eMart.Models;
using eMart.Repository;
using eMart.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace eMart
{
    public class ReviewsHub: Hub
    {
        private readonly IUnitOfWork _unit;


        public ReviewsHub(IUnitOfWork unit)
        {
          _unit = unit; 
   
        }

        public async Task SendReview(string jsonReview)
        {
            
            try
            {
                ReviewModel Rev =
               JsonConvert.DeserializeObject<ReviewModel>(jsonReview);
                Review review = new Review();
               
                review.Comment = Rev.Comment;
                review.Rate = Rev.Rating;
                review.UserId = Rev.userId;
                review.ProductId = Rev.ProductId;

                _unit.reviews.Addone(review);


                await Clients.All.SendAsync("ReceiveReview", Rev.Username, Rev.Comment, Rev.Rating);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
