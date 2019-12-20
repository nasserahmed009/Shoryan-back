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

            DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);

            if (dt == null) return StatusCode(500);
            return Json(dt);

        }

        [HttpGet("api/Listings/{listingId}")]
        public IActionResult getListingById(int listingId)
        {

            string StoredProcedureName = ListingsProcedures.getListingById;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@ListingId", listingId);

            DataTable dt = dbMan.ExecuteReader(StoredProcedureName, Parameters);
            if (dt == null) return StatusCode(404);

			Listings listing = new Listings();
			listing.id = Convert.ToInt32(dt.Rows[0]["id"]);
			listing.expirationDate = Convert.ToDateTime(dt.Rows[0]["expirationDate"]);
			listing.price = Convert.ToInt32(dt.Rows[0]["price"]);
			listing.shreets = Convert.ToInt32(dt.Rows[0]["shreets"]);
			listing.elbas = Convert.ToInt32(dt.Rows[0]["elbas"]);
			listing.userId = Convert.ToInt32(dt.Rows[0]["userId"]);
			listing.drugId = Convert.ToInt32(dt.Rows[0]["drugId"]);

			return Json(listing);

        }

        [HttpPost("api/Listings")]
		public IActionResult addListings([FromBody] Dictionary<string, object> JSONinput)
		{
            var listingsJson = JsonConvert.SerializeObject(JSONinput["Listings"], Newtonsoft.Json.Formatting.Indented);
            var listing = JsonConvert.DeserializeObject<Listings>(listingsJson);


            string StoredProcedureName = ListingsProcedures.addListing;

			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@drugId", listing.drugId);
			Parameters.Add("@userId", listing.userId);
			Parameters.Add("@expiryDate", listing.expirationDate);
            Parameters.Add("@shreet", listing.shreets);
            Parameters.Add("@elbas", listing.elbas);
            Parameters.Add("@price", listing.price);

            int returnValue = dbMan.ExecuteNonQuery(StoredProcedureName, Parameters);

            if (returnValue == 0) return StatusCode(500);
            return Json("Data Added successfully");

		}

        [HttpGet("api/ListingsInOrder/{orderId}")]
        public IActionResult getListingsInOrder(int orderId)
        {

            string StoredProcedureName = ListingsProcedures.getListingsInOrder;

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@orderId", orderId);

            return Json(dbMan.ExecuteReader(StoredProcedureName, Parameters));

        }

    }
}