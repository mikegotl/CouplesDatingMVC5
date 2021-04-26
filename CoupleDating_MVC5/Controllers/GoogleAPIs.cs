using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace CoupleDating_MVC5.Controllers
{
    public class GoogleAPIs : Page
    {
        private string GPlaces_Url = "https://maps.googleapis.com/maps/api/place/";
        private string GPlaces_api_key = "AIzaSyDa8iBfhfP-0w8tQWuzi5U4aqC00nyA5x8";

        public async Task<List<GPlace>> GetPlaces(string strQuery, int limit)
        {
            List<GPlace> gPlaces = new List<GPlace>();

            //call request method for Google Places
            var queryParams = new Dictionary<string, string>()
            {
                { "query", strQuery},
                { "key", GPlaces_api_key}
            };

            JObject _placesStrReturned = Perform_Request_JSON(GPlaces_Url + "textsearch/json", queryParams);

            //Get List of Results(Places)
            var results = ((dynamic)_placesStrReturned).results;
            var i = 0;
            foreach (var _place in results)
            {
                var _pl = (dynamic)_place;

                GPlace gPlace = new GPlace();
                gPlace.formattedAddress = _pl["formatted_address"];
                gPlace.name = _pl["name"];
                gPlace.iconURL = _pl["icon"];
                gPlace.place_id = _pl["place_id"];

                //Get Photo Listing with photo_reference's
                var _photos = _pl.photos;
                if (_photos != null)
                {
                    foreach (var _photo in _photos)
                    {
                        string _photoRefID = _photo["photo_reference"];

                        //add photoRef to collection
                        gPlace.photos.Add(_photoRefID);

                        //add actual photo url to collection
                        string photoURL = GetGPhotoUrl(_photoRefID, "200");

                        GPlacePhotoUrls gPlacePhotoUrl = new GPlacePhotoUrls();
                        gPlacePhotoUrl.place_id = gPlace.place_id;
                        gPlacePhotoUrl.photoUrl = photoURL;

                        gPlace.photoUrls.Add(gPlacePhotoUrl);
                    }
                }
                gPlaces.Add(gPlace);

                i += 1;
                if (i > limit) { return gPlaces; }
            }

            return gPlaces;
        }
        public List<GPlace> GetPlaceList(string strQuery, int limit)
        {
            List<GPlace> gPlaces = new List<GPlace>();

            //call request method for Google Places
            var queryParams = new Dictionary<string, string>()
            {
                { "query", strQuery},
                { "key", GPlaces_api_key}
            };

            JObject _placesStrReturned = Perform_Request_JSON(GPlaces_Url + "textsearch/json", queryParams);

            //Get List of Results(Places)
            var results = ((dynamic)_placesStrReturned).results;
            var i = 0;
            foreach (var _place in results)
            {
                var _pl = (dynamic)_place;

                GPlace gPlace = new GPlace();
                gPlace.formattedAddress = _pl["formatted_address"];
                gPlace.name = _pl["name"];
                gPlace.iconURL = _pl["icon"];
                gPlace.place_id = _pl["place_id"];

                //Get Photo Listing with photo_reference's
                var _photos = _pl.photos;
                if (_photos != null)
                {
                    foreach (var _photo in _photos)
                    {
                        string _photoRefID = _photo["photo_reference"];

                        //add photoRef to collection
                        gPlace.photos.Add(_photoRefID);

                        //add actual photo url to collection
                        string photoURL = GetGPhotoUrl(_photoRefID, "100");

                        GPlacePhotoUrls gPlacePhotoUrl = new GPlacePhotoUrls();
                        gPlacePhotoUrl.place_id = gPlace.place_id;
                        gPlacePhotoUrl.photoUrl = photoURL;

                        gPlace.photoUrls.Add(gPlacePhotoUrl);
                    }
                }
                gPlaces.Add(gPlace);

                i += 1;
                if (i > limit) { return gPlaces; }
            }

            return gPlaces;
        }
        private string GetGPhotoUrl(string _photoRefID, string maxheight)
        {
            var queryParams_Photo = new Dictionary<string, string>()
                        {
                            {"maxheight",maxheight},
                            {"photoreference",_photoRefID},
                            {"key",GPlaces_api_key}
                        };

            string photoURL = "";
            try
            {
                var photo = Perform_Request(GPlaces_Url + "photo", queryParams_Photo);
                photoURL = photo.ResponseUri.ToString();
            }
            catch (Exception)
            {
            }
            return photoURL;
        }

        public GPlace GetPlaceDetails(string _placeid)
        {
            //call request method for Google Places
            var queryParams = new Dictionary<string, string>()
            {
                { "placeid", _placeid},
                { "key", GPlaces_api_key}
            };

            //Get Google Photo Details
            JObject _placeReturned = Perform_Request_JSON(GPlaces_Url + "details" + "/json", queryParams);

            var _pl = ((dynamic)_placeReturned).result;

            GPlace gPlace = new GPlace();
            gPlace.formattedAddress = _pl["formatted_address"];
            gPlace.name = _pl["name"];
            gPlace.iconURL = _pl["icon"];
            gPlace.place_id = _pl["place_id"];
            gPlace.website_url = _pl["website"];

            //load photoUrls for place
            if (_pl.photos != null)
            {
                foreach (var p in _pl.photos)
                {
                    string _photoRefID = p["photo_reference"];

                    //Get Google Photo Url
                    string photoURL = GetGPhotoUrl(_photoRefID, "200");

                    GPlacePhotoUrls gPUrl = new GPlacePhotoUrls();
                    gPUrl.place_id = _placeid;
                    gPUrl.photoUrl = photoURL;

                    gPlace.photoUrls.Add(gPUrl);
                }
            }

            return gPlace;
        }

        private JObject Perform_Request_JSON(string baseURL, Dictionary<string, string> queryParams = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(String.Empty);

            if (queryParams == null)
            {
                queryParams = new Dictionary<string, string>();
            }

            foreach (var queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }

            var uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Query = query.ToString();

            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";

            //request.SignRequest(
            //    new Tokens
            //    {
            //        ConsumerKey = CONSUMER_KEY,
            //        ConsumerSecret = CONSUMER_SECRET,
            //        AccessToken = TOKEN,
            //        AccessTokenSecret = TOKEN_SECRET
            //    }
            //).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var result = stream.ReadToEnd();
            return JObject.Parse(result);
        }

        private HttpWebResponse Perform_Request(string baseURL, Dictionary<string, string> queryParams = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(String.Empty);

            if (queryParams == null)
            {
                queryParams = new Dictionary<string, string>();
            }

            foreach (var queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }

            var uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Query = query.ToString();

            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";

            //request.SignRequest(
            //    new Tokens
            //    {
            //        ConsumerKey = CONSUMER_KEY,
            //        ConsumerSecret = CONSUMER_SECRET,
            //        AccessToken = TOKEN,
            //        AccessTokenSecret = TOKEN_SECRET
            //    }
            //).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response;
        }
    }

    public class GPlace
    {
        public string formattedAddress;
        public string name;
        public List<string> photos = new List<string>();
        public List<GPlacePhotoUrls> photoUrls = new List<GPlacePhotoUrls>();
        public decimal rating;
        public List<string> types;
        public string iconURL;
        public string place_id;
        public string website_url;
    }

    public class GPlacePhotoUrls
    {
        public string place_id;
        public string photoUrl;
    }
}