﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DBapplication;
using Shoryan.Models;
using Shoryan.Routes;
using Newtonsoft.Json;
using System.Data;

namespace Shoryan.Controllers
{
    
    [ApiController]
    public class ListingsController : Controller
    {
		DBManager dbMan;

		public ListingsController()
		{
			dbMan = new DBManager();
		}

        [HttpGet("api/Listings")]
        public IActionResult getAllListing()
        {

            string StoredProcedureName = ListingsProcedures.getAllListing;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
			DataTable dt;
			try
			{
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				if (dt == null) return StatusCode(500, "Internal server error");
				else return Json(dt);

			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
				throw;
			}

        }

		[HttpGet("api/ListingsImgs/{listingId}")]
		public IActionResult getListingImgsById(int listingId)
		{
			string StoredProcedureName = ListingsProcedures.getListingImgsById;

			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@listingId", listingId);
			DataTable dt;
			try
			{
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
			}
			catch (Exception)
			{
				return StatusCode(500, "Listing not found");
				throw;
			}

			List<ListingImgsUrls> listings_imgs_urls = new List<ListingImgsUrls>();
			if(dt!=null)
				for(int i = 0;i<dt.Rows.Count;i++)
				{
					ListingImgsUrls x = new ListingImgsUrls();
					x.listingId = listingId;
					x.imgNo = Convert.ToInt32(dt.Rows[i]["img_no"]);
					x.url = Convert.ToString(dt.Rows[i]["url"]);
					listings_imgs_urls.Add(x);
				}

			return Json(listings_imgs_urls);
		}


		[HttpGet("api/Listings/{listingId}")]
        public IActionResult getListingById(int listingId)
        {
            string StoredProcedureName = ListingsProcedures.getListingById;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@ListingId", listingId);
			DataTable dt;
			try
			{
				dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
				if (dt == null) return StatusCode(500, "Listing not found");
			}
			catch (Exception)
			{
				return StatusCode(500, "Listing not found");
				throw;
			}


			Listings listing = new Listings();
			listing.id = Convert.ToInt32(dt.Rows[0]["id"]);
			listing.expirationDate = Convert.ToDateTime(dt.Rows[0]["expirationDate"]);
			listing.price = Convert.ToInt32(dt.Rows[0]["price"]);
			listing.shreets = Convert.ToInt32(dt.Rows[0]["shreets"]);
			listing.elbas = Convert.ToInt32(dt.Rows[0]["elbas"]);
			listing.userId = Convert.ToInt32(dt.Rows[0]["userId"]);
			listing.drugId = Convert.ToInt32(dt.Rows[0]["drugId"]);
            listing.drugName = Convert.ToString(dt.Rows[0]["drugName"]);
            listing.sellerName = Convert.ToString(dt.Rows[0]["sellerName"]);
			return Json(listing);

        }

        [HttpPost("api/Listings")]
		public IActionResult addListings([FromBody] Dictionary<string, object> JSONinput)
		{
			Listings listing;
			try
			{
				var listingsJson = JsonConvert.SerializeObject(JSONinput, Newtonsoft.Json.Formatting.Indented);
				listing = JsonConvert.DeserializeObject<Listings>(listingsJson);
			}
			catch (Exception)
			{
				return StatusCode(500, "Error parsing JSON");
			}

            string StoredProcedureName = ListingsProcedures.addListing;

			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@drugId", listing.drugId);
			Parameters.Add("@userId", listing.userId);
			Parameters.Add("@expiryDate", listing.expirationDate);
            Parameters.Add("@shreet", listing.shreets);
            Parameters.Add("@elbas", listing.elbas);
            Parameters.Add("@price", listing.price);

			try
			{
				return Json(Convert.ToInt32(dbMan.ExecuteScalar(StoredProcedureName, Parameters)));
			}
			catch (Exception)
			{
				return StatusCode(500, "Incorrect listing data");
				throw;
			}

		}

        [HttpGet("api/ListingsInOrder/{orderId}")]
        public IActionResult getListingsInOrder(int orderId)
        {

            string StoredProcedureName = ListingsProcedures.getListingsInOrder;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@orderId", orderId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
				throw;
			}

        }
        [HttpGet("api/ListingsOfDrug/{drugId}")]
        public IActionResult getListingsByDrugId(int drugId)
        {

            string StoredProcedureName = ListingsProcedures.getListingsByDrugId;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@drugId", drugId);

			try
			{
				return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
			}
			catch (Exception)
			{
				return StatusCode(500, "Internal server error");
				throw;
			}

        }
        [HttpGet("api/searchListings/{text}")]
        public IActionResult searchInUsers(int text)
        {   
            
            string StoredProcedureName = ListingsProcedures.searchInListings;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@search", text);
            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));
        }

    }
}