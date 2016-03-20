using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Sms
{
	public class ComilioWrapper 
	{
		private string _username;
		private string _password;
		private string _apiUrl="http://api.comilio.it/rest/v1/message/";

		public ComilioWrapper(string username, string password)
		{
			_username = username;
			_password = password;
		}

		public void Send(string text, List<string> phoneNumbers)
		{
			var sms = new Sms(MessageType.Classic, phoneNumbers, text);
			var serializedSms = JsonConvert.SerializeObject(sms);

			string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}"));
			using (WebClient client = new WebClient())
			{
				client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
				client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF­8";

				string result = client.UploadString(_apiUrl, serializedSms);
			}
		}

		private static class MessageType
		{
			public static readonly string Classic = "Classic";
			public static readonly string Smart = "Smart";
			public static readonly string SmartPro = "SmartPro";
		}
		private class Sms
		{
			public Sms(string messageType, List<string> phoneNumbers, string text)
			{
				PhoneNumbers = phoneNumbers;
				Text = text;
				MessageType = messageType;
			}

			[JsonProperty(PropertyName = "message_type")]
			public string MessageType { get; set; }

			[JsonProperty(PropertyName = "phone_numbers")]
			public List<string> PhoneNumbers { get; set; }

			[JsonProperty(PropertyName = "text")]
			public string Text { get; set; }

			[JsonProperty(PropertyName = "sender_string")]
			public string SenderString { get; set; }

			[JsonProperty(PropertyName = "schedule_timestamp")]
			public string ScheduleTimestamp { get; set; }
		}
	}
}