using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace _5DayForecast
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IService1
	{

		[OperationContract]
		string GetData(int value);

		[OperationContract]
		CompositeType GetDataUsingDataContract(CompositeType composite);

		// TODO: Add your service operations here

		// Returns a Tuple of the latitude and longitude of a zipcode.
		[OperationContract]
		Tuple<double,double> getLatLong(string zipcode);

		// Returns a list of strings containing forecast information.
		[OperationContract]
		List<string> getForecast(string zipcode, string units);

		// Returns a string with location information given a zipcode.
		[OperationContract]
		string getLocation(string zipcode);

	}


	// Use a data contract as illustrated in the sample below to add composite types to service operations.
	[DataContract]
	public class CompositeType
	{
		bool boolValue = true;
		string stringValue = "Hello ";

		[DataMember]
		public bool BoolValue
		{
			get { return boolValue; }
			set { boolValue = value; }
		}

		[DataMember]
		public string StringValue
		{
			get { return stringValue; }
			set { stringValue = value; }
		}
	}
}
