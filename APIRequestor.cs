///////////////////////////////////////////////////////////////////////////////
//
// (C) Copyright 2018 EVERYSK TECHNOLOGIES
//
// This is an unpublished work containing confidential and proprietary
// information of EVERYSK TECHNOLOGIES. Disclosure, use, or reproduction
// without authorization of EVERYSK TECHNOLOGIES is prohibited.
//
///////////////////////////////////////////////////////////////////////////////

namespace Everysk
{
	using System;
	using System.Net;
	using System.IO;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public class APIRequestor
	{
		public string API_SID;
		public string API_TOKEN;
		public string API_ENTRY;
		public string API_VERSION;

		public APIRequestor(string api_sid, string api_token, string api_entry, string api_version)
		{
			API_SID = api_sid;
			API_TOKEN = api_token;
			API_ENTRY = api_entry;
			API_VERSION = api_version;
		}

		public JObject request(string apiMethod, JObject args)
		{
			string baseURL = API_ENTRY + "/" + API_VERSION + "/" + apiMethod;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL);
			request.Method = "POST";
			request.Headers.Add("Authorization", "Bearer " + API_SID + ":" + API_TOKEN);
			string jsonInput = JsonConvert.SerializeObject(args);
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(jsonInput);
			request.ContentLength = byteArray.Length;
			request.ContentType = @"application/json";
			using (Stream dataStream = request.GetRequestStream())
			{
				dataStream.Write(byteArray, 0, byteArray.Length);
			}
			string body;
			string req_id = "";
			double req_ms = 0.0;
			try
			{
				using (var response = (HttpWebResponse)request.GetResponse())
				{
					req_id = response.Headers["X-Everysk-Request-Id"];
					req_ms = System.Convert.ToDouble(response.Headers["X-Everysk-Request-Duration"]);
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						body = reader.ReadToEnd();
					}
				}
			}
			catch(WebException ex)
			{
				var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				Console.WriteLine("ERROR: " + resp);
				throw ex;
			}
			catch
			{
				throw;
			}

			JObject ret = new JObject
			{
				{"request_id", req_id},
				{"request_ms", req_ms},
				{"response", JObject.Parse(body)}
			};
			return ret;
		}
	}
}
