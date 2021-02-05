using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Data.Interfaces;
using System.Security.Claims;
using FinalProject.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;
using System.IO;
using Newtonsoft.Json;

namespace FinalProject.Controllers
{
    public class TradeOffersController : Controller
    {
        private IDataAccess _DataAccess;
        public TradeOffersController(IDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
            StripeConfiguration.ApiKey = "sk_test_4eC39HqLyjWDarjtT1zdp7dc";
        }

        // GET: TradeOffers
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: TradeOffers/Details/5
        public async Task<IActionResult> Details(int? id, string source)
        {
            var model = new TradeOfferDetailsViewModel();
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var ownsListingOne = userId == offer.TradeListingOne.ApplicationUserId;
            var ownsListingTwo = userId == offer.TradeListingTwo.ApplicationUserId;
            if (!ownsListingOne && !ownsListingTwo)
                return Forbid();

            model.Source = source;
            model.Offer = offer;
            model.OwnsListingOne = ownsListingOne;
            model.OwnsListingTwo = ownsListingTwo;

            return View(model);
        }

        // GET: TradeOffers/Create
        public async Task<IActionResult> Create(int? id)
        {
            var listing = await _DataAccess.TradeDataAccess.GetTradeListing(id ?? 0);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            if (listing == null)
            {
                return NotFound();
            }
            else if (string.IsNullOrEmpty(userId) || listing.ApplicationUserId == userId)
            {
                return Forbid();
            }
            else
            {

                var model = new CreateTradeOfferViewModel();
                var userInventory = await _DataAccess.UserInventoryDataAccess.GetUserInventoryByUserAsync(userId);
                if (userInventory == null)
                {
                    userInventory = new UserInventory();
                    userInventory.ApplicationUserId = userId;
                    userInventory = await _DataAccess.UserInventoryDataAccess.Upsert(userInventory);
                }
                model.CardMappings = await _DataAccess.CardDataAccess.GetCardMappingsForUserAsync(userInventory.Id);
                model.TradeOffer = new TradeOffer();
                model.TradeOffer.TradeListingOne = listing;
                model.TradeOffer.TradeListingOneId = listing.Id;
                model.Listing = new TradeListing();
                return View(model);
            }
        }

        // POST: TradeOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var strListingId = collection["TradeOffer.TradeListingOneId"];
            var strCardId = collection["Listing.CardId"];
            var strCashOffer = collection["Listing.CashOffer"];
            var desc = collection["Listing.Description"];

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                int cardId;
                int listingId;
                double cashOffer;
                if (int.TryParse(strListingId, out listingId))
                {

                    // Generate listing for offer
                    var tradeListing = new TradeListing();
                    tradeListing.ApplicationUserId = userId;
                    if (int.TryParse(strCardId, out cardId))
                    {
                        tradeListing.CardId = cardId;

                    }
                    if (double.TryParse(strCashOffer, out cashOffer))
                    {
                        tradeListing.CashOffer = cashOffer;
                    }
                    tradeListing.Description = desc;
                    tradeListing.TradeState = TradeState.Offered;
                    var tradeListingData = await _DataAccess.TradeDataAccess.UpsertTradeListing(tradeListing);
                    var origTradeListingData = await _DataAccess.TradeDataAccess.GetTradeListing(listingId);

                    // create offer
                    var newOffer = new TradeOffer();
                    newOffer.TradeListingOne = origTradeListingData;
                    newOffer.TradeListingOneId = listingId;
                    newOffer.TradeListingTwo = tradeListingData;
                    newOffer.TradeListingTwoId = tradeListingData.Id;
                    newOffer.TradeState = TradeState.PendingOffer;
                    var offer = await _DataAccess.TradeDataAccess.UpsertTradeOffer(newOffer);

                    if (offer != null && offer.Id > 0)
                    {
                        // because we were successful need to update original listing;
                        origTradeListingData.TradeState = TradeState.PendingOffer;
                        await _DataAccess.TradeDataAccess.UpsertTradeListing(origTradeListingData);
                    }
                    var model = new { id = offer.Id };
                    return RedirectToAction("Details", model);
                }
            }
            return RedirectToAction("Index", "TradeListings");
        }

        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (offer.TradeListingOne.ApplicationUserId != userId || string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }
            if(offer.TradeListingOne.HasCashOffer || offer.TradeListingTwo.HasCashOffer)
            {
                offer.TradeState = TradeState.AwaitingPayment;

            }
            else
            {
                offer.TradeState = TradeState.PaymentComplete;

            }
            await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
            var model = new { id = id ?? 0 };
            return RedirectToAction("Details", model);
        }
        public async Task<IActionResult> Ship(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (offer.TradeListingOne.ApplicationUserId != userId || string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }
            offer.TradeState = TradeState.Shipped;

            await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
            var model = new { id = id ?? 0 };
            return RedirectToAction("Details", model);
        }
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (offer.TradeListingTwo.ApplicationUserId != userId || string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }; 
            offer.TradeState = TradeState.Completed;

            await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
            var model = new { id = id ?? 0 };
            return RedirectToAction("Details", model);
        }
        public async Task<IActionResult> Decline(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (offer.TradeListingOne.ApplicationUserId != userId || string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }
            offer.TradeState = TradeState.Denied;
            await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
            var model = new { id = id ?? 0 };
            return RedirectToAction("Details", model);
        }
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (offer.TradeListingTwo.ApplicationUserId != userId || string.IsNullOrEmpty(userId))
            {
                return Forbid();
            };
            var model = new TradeOfferPaymentViewModel();
            model.Offer = offer;
            var dict = new Dictionary<string, string>();
            dict.Add("id", id.ToString());
            model.PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)offer.TradeListingTwo.CashOffer * 1000,
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = "Card Trade",
                    Metadata = dict
                }

            };

            return View(model);
        }
        [HttpPost("create-checkout-session")]
        public async Task<ActionResult> CreateCheckoutSession()
        {
            var strID = await new StreamReader(Request.Body).ReadToEndAsync();
            int id;
            if(int.TryParse(strID, out id))
            {
                var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id);
                var val = offer.TradeListingTwo.CashOffer * 100;
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                    LineItems = new List<SessionLineItemOptions>
                    {
                      new SessionLineItemOptions
                      {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                          UnitAmount = (long)val,
                          Currency = "usd",
                          ProductData = new SessionLineItemPriceDataProductDataOptions
                          {
                            Name = "Trade",
                          },

                        },
                        Quantity = 1,
                      },
                    },
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:44320/TradeOffers/PaymentComplete/{offer.Id}",
                    CancelUrl = "https://example.com/cancel",
                };
                var service = new SessionService();
                Session session = service.Create(options);

                return Json(new { id = session.Id });
            }
            else
            {
                return Json(new { id="dd"});
            }
        }
        public async Task<IActionResult> Review(IFormCollection collection)
        {
            var ratingOne = collection["Offer.TradeListingOne.UserRating"];
            var ratingTwo = collection["Offer.TradeListingOne.UserRating"];
            var offerId = collection["Offer.Id"];
            int id;
            int rating;
            if(int.TryParse(offerId,out id))
            {
                if (id == null || id <= 0)
                    return NotFound();
                var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id);
                if (offer == null)
                    return NotFound();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                var ownsListingOne = userId == offer.TradeListingOne.ApplicationUserId;
                var ownsListingTwo = userId == offer.TradeListingTwo.ApplicationUserId;
                
                if (!ownsListingOne && !ownsListingTwo)
                    return Forbid();
                else if (ownsListingOne)
                {
                    if(int.TryParse(ratingTwo, out rating))
                    {
                        offer.TradeListingTwoRating = rating;
                        await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                else if (ownsListingTwo)
                {
                    if (int.TryParse(ratingOne, out rating))
                    {
                        offer.TradeListingOneRating = rating;
                        await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
                        return RedirectToAction("Details", new { id = id });
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Details");
        }
        public async Task<IActionResult> PaymentComplete(int? id)
        {
            if (id == null || id <= 0)
                return NotFound();
            var offer = await _DataAccess.TradeDataAccess.GetTradeOfferAsync(id ?? 0);
            if (offer == null)
                return NotFound();
            offer.TradeState = TradeState.PaymentComplete;
            await _DataAccess.TradeDataAccess.UpsertTradeOffer(offer);
            var model = new { id = id ?? 0 };
            return RedirectToAction("Details", model);
        }
        // POST: TradeOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TradeListingOneId,TradeListingTwoId,TradeState,TradeListingOneRating,TradeListingTwoRating")] TradeOffer tradeOffer)
        {
            return View();
        }

        // GET: TradeOffers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: TradeOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return View();
        }

    }
}
