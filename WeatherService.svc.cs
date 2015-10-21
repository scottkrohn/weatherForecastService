using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Net;

namespace _5DayForecast
{
	public class Service1 : IService1
	{
		// Return a pair of double values representing latitude and longitude.
		public Tuple<double,double> getLatLong(string zipcode)
		{
			// Perform web request using google api for geocode locations.
			string lURL = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}", zipcode);
			WebRequest latlongRequest = WebRequest.Create(lURL);
			Stream latlongStream = latlongRequest.GetResponse().GetResponseStream();
			MyXmlParser latlongParser = new MyXmlParser(latlongStream);
			return new Tuple<double,double>(latlongParser.getLatitude(), latlongParser.getLongitude());
		}

		// Return a list of strings, each string representing the forecast for 1 day in a given zipcode.
		// The format of the temperature can be kelvin, F or C.
		public List<string> getForecast(string zipcode, string units)
		{
			Tuple<double,double> latlong = getLatLong(zipcode);
			// Check if the getLatLong() call returned a valid result.
			if(latlong.Item1 == -1 || latlong.Item2 == -1)
			{
				throw new Exception(string.Format("Invalid latitude and longitude returned from zipcode {0}", zipcode));
			}
			// Perform a web request using the latitude and longitude.
			string apiKey = "5dbc75f8d5e35b484f4c0b4f1824eedf";
			string wURL = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?lat={0}&lon={1}&cnt=5&mode=xml&units={2}&APPID={3}", latlong.Item1, latlong.Item2, units, apiKey);
			WebRequest weatherRequest = WebRequest.Create(wURL);
			Stream weatherStream = weatherRequest.GetResponse().GetResponseStream();
			// Create MyXmlParser object to get weather data from XML file.
			MyXmlParser weatherParser = new MyXmlParser(weatherStream);
			List<oneDayForecast> weatherDays = weatherParser.getForecastData();
			List<string> forecast = new List<string>();
			// Build and return the List<string> of weather for 5 days.
			for(int i = 0; i != weatherDays.Count; i++)
			{
				forecast.Add(weatherDays[i].toString());
			}
			return forecast;
		}

		// Return a string representing a location for a given zip code.
		public string getLocation(string zipcode)
		{
			// Perform webrequest using google api for geocode locations.
			string URL = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}", zipcode);
			WebRequest locationRequest = WebRequest.Create(URL);
			Stream locationStream = locationRequest.GetResponse().GetResponseStream();
			MyXmlParser locationParser = new MyXmlParser(locationStream);
			return locationParser.getLocationName();
		}

		public string GetData(int value)
		{
			return string.Format("You entered: {0}", value);
		}

		public CompositeType GetDataUsingDataContract(CompositeType composite)
		{
			if (composite == null)
			{
				throw new ArgumentNullException("composite");
			}
			if (composite.BoolValue)
			{
				composite.StringValue += "Suffix";
			}
			return composite;
		}
	}
}
