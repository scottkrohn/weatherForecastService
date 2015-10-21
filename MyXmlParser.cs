using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace _5DayForecast
{
	// This class is used to parse specific values from xml documents.
	public class MyXmlParser
	{
		XmlDocument xmlDoc;

		public MyXmlParser(Stream xml)
		{
			xmlDoc = new XmlDocument();
			xmlDoc.Load(xml);
		}

		// Return a latitude location based on a zipcode.
		public double getLatitude()
		{
			XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("//GeocodeResponse/result/geometry/location");		
			double latitude = -1;
			foreach(XmlNode node in nodes)
			{
				latitude = Convert.ToDouble(node.SelectSingleNode("lat").InnerText);
			}
			return latitude;
		}

		// Return a longitude location based on a zipcode.
		public double getLongitude()
		{
			XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("//GeocodeResponse/result/geometry/location");		
			double longitude = -1;
			foreach(XmlNode node in nodes)
			{
				longitude = Convert.ToDouble(node.SelectSingleNode("lng").InnerText);
			}
			return longitude;
		}

		// Return a List of 5 oneDayForecase elements parsed from the xmlDocument.
		public List<oneDayForecast> getForecastData()
		{
			XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("//weatherdata/forecast/time");
			List<oneDayForecast> forecast = new List<oneDayForecast>();
			foreach(XmlNode node in nodes)
			{
				oneDayForecast day = new oneDayForecast();
				if(node.Attributes != null)
				{
					day.date = "<b>" + node.Attributes["day"].Value + "</b>";
					day.tempMin = node.SelectSingleNode("temperature").Attributes["min"].Value;
					day.tempMax = node.SelectSingleNode("temperature").Attributes["max"].Value;
					day.humidity = node.SelectSingleNode("humidity").Attributes["value"].Value;
					day.clouds = node.SelectSingleNode("clouds").Attributes["value"].Value;
				}
				forecast.Add(day);	
			}
			return forecast;
		}

		// Return a string representing a location given by the xmlDocument.
		public string getLocationName()
		{
			XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("//GeocodeResponse/result");
			string locationName = "";
			foreach(XmlNode node in nodes)
			{
				locationName = node.SelectSingleNode("formatted_address").InnerText;
			}
			return locationName;
		}
	}

	public struct oneDayForecast
	{
		public string date;
		public string tempMin;
		public string tempMax;
		public string humidity;
		public string clouds;

		public string toString()
		{
			string weatherString = string.Format("{0}:\nLow Temp: {1}\nHigh Temp: {2}\nHumidity: {3}%\nClouds: {4}\n", date, tempMin, tempMax, humidity, clouds);
			return weatherString;
		}
	};
}