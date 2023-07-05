using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }
        //get all cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
          var cards= await cardsDbContext.Cards.ToListAsync();
          return Ok(cards);
        }
        //Get single card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var cards = await cardsDbContext.Cards.FirstOrDefaultAsync(x=>x.Id==id);
            if (cards != null)
            {
                return Ok(cards);
            }
            return NotFound("Card not found");
        }
        //Add card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
          card.Id=Guid.NewGuid();
          await cardsDbContext.Cards.AddAsync(card);
          await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard),new { id= card.Id, }, card);
        }
        //updating a card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingCards = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCards != null)
            {
               existingCards.CardholderName = card.CardholderName;
                existingCards.CardNumber= card.CardNumber;
                existingCards.ExpireMonth = card.ExpireMonth;
                existingCards.ExpireYear= card.ExpireYear;
                existingCards.CVC=card.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(existingCards);
            }
            return NotFound("Card not found");
        }
        //delete a card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingCards = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCards != null)
            {
                cardsDbContext.Remove(existingCards);
                await cardsDbContext.SaveChangesAsync();
                return Ok(existingCards);
            }
            return NotFound("Card not found");
        }
    }
}
