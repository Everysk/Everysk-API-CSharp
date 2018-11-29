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
	using Newtonsoft.Json.Linq;
	using Everysk;

	public class Calculation
	{
		private Everysk.APIRequestor api;

		public Calculation(string api_sid, string api_token, string api_entry, string api_version)
		{
			api = new APIRequestor(api_sid, api_token, api_entry, api_version);
		}

		public JObject riskAttribution(JObject args)
		{
			JObject ret = api.request("calculations/risk_attribution", args);
			return ret["response"].Value<JObject>();
		}

		public JObject stressTest(JObject args)
		{
			JObject ret = api.request("calculations/stress_test", args);
			return ret["response"].Value<JObject>();
		}

		public JObject exposure(JObject args)
		{
			JObject ret = api.request("calculations/exposure", args);
			return ret["response"].Value<JObject>();
		}

		public JObject properties(JObject args)
		{
			JObject ret = api.request("calculations/properties", args);
			return ret["response"].Value<JObject>();
		}
	}
}
