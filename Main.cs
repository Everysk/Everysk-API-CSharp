///////////////////////////////////////////////////////////////////////////////
//
// (C) Copyright 2018 EVERYSK TECHNOLOGIES
//
// This is an unpublished work containing confidential and proprietary
// information of EVERYSK TECHNOLOGIES. Disclosure, use, or reproduction
// without authorization of EVERYSK TECHNOLOGIES is prohibited.
//
///////////////////////////////////////////////////////////////////////////////

namespace EveryskTest
{
	using System;
	using Newtonsoft.Json.Linq;
	using Everysk;

	public class CalculationTest
	{

		static public void Main ()
		{
			Console.WriteLine ("C# API TEST");

			string API_SID = "<YOUR_ACCOUNT_SID>";
			string API_TOKEN = "<YOUR_AUTH_TOKEN>";
			string API_ENTRY = "https://api.everysk.com";
			string API_VERSION = "v2";

			Calculation calc = new Calculation(API_SID, API_TOKEN, API_ENTRY, API_VERSION);

			Test_RiskAttribution(calc);
			//Test_StressTest(calc);

		}

		static public void Test_RiskAttribution(Calculation calc)
		{
			//INPUT
			JObject args = new JObject
			{
				{
					"securities", new JArray
					{
						new JObject{{"id", "id1"}, {"symbol", "AAPL"}, {"quantity", 1}, {"label", ""}},
						new JObject{{"id", "id2"}, {"symbol", "IBM"}, {"quantity", 1}, {"label", ""}},
					}
				},
				{"date", "20181113"},
				{"base_currency", "USD"},
				{"aggregation", "position"},
				{
					"projection", new JArray
					{
						"IND:SPX"
					}
				},
				{"horizon", 5}
			};
			
			//COMPUTE
			JObject ret = calc.riskAttribution(args);
			Console.WriteLine("RISK ATTRIBUTION RESPONSE:");
			Console.WriteLine(ret);

			//OUTPUT
			double port_mctr = 0.0;
			foreach(JObject x in ret["risk_attribution"]["results"])
			{
				foreach(JObject y in x["mctr"])
				{
					port_mctr += y["value"].Value<double>();
				}
			}
			Console.WriteLine("PORTFOLIO MCTR: {0}%", port_mctr*100.0);			
		}

		static public void Test_StressTest(Calculation calc)
		{
			//INPUT
			JObject args = new JObject
			{
				{
					"securities", new JArray
					{
						new JObject{{"id", "id1"}, {"symbol", "AAPL"}, {"quantity", 1}, {"label", ""}},
						new JObject{{"id", "id2"}, {"symbol", "IBM"}, {"quantity", 1}, {"label", ""}},
					}
				},
				{"date", "20181113"},
				{"base_currency", "USD"},
				{"aggregation", "position"},
				{
					"projection", new JArray
					{
						"IND:SPX"
					}
				},
				{"horizon", null},
				{"shock", "IND:SPX"},
				{"magnitude", -0.01}
			};
			
			//COMPUTE
			JObject ret = calc.stressTest(args);
			Console.WriteLine("STRESS TEST RESPONSE:");
			Console.WriteLine(ret);

			//OUTPUT
			double cvar_pos = 0.0;
			double ev = 0.0;
			double cvar_neg = 0.0;
			foreach(JObject x in ret["stress_test"]["results"])
			{
				foreach(JObject y in x["cvar_pos"])
				{
					cvar_pos += y["value"].Value<double>();
				}
				foreach(JObject y in x["ev"])
				{
					ev += y["value"].Value<double>();
				}
				foreach(JObject y in x["cvar_neg"])
				{
					cvar_neg += y["value"].Value<double>();
				}
			}
			Console.WriteLine("SHOCK          : {0}", args["shock"].Value<string>());
			Console.WriteLine("MAGNITUDE      : {0}%", args["magnitude"].Value<double>()*100.0);
			Console.WriteLine("PORTFOLIO CVaR+: {0}%", cvar_pos*100.0);
			Console.WriteLine("PORTFOLIO EV   : {0}%", ev*100.0);
			Console.WriteLine("PORTFOLIO CVaR-: {0}%", cvar_neg*100.0);
		}		
	}
}
