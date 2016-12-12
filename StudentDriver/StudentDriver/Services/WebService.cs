﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StudentDriver.Models;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentDriver.Helpers;

namespace StudentDriver.Services
{
    public static class WebService
    {
        private static string BaseUrl = "";

        public static async Task<UserStats> GetStudentStats(int id)
        {
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(BaseUrl,"students", new Dictionary<string, string>() {{"userId", id.ToString()}});
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var userStats = JsonConvert.DeserializeObject<UserStats>(json);
            return userStats;
        }

        public static async Task<bool> PostUnsyncDrivingSessions(List<UnsyncDrive> unsyncDrives)
        {
            var json = new JObject(new JProperty("unsyncDrives",unsyncDrives));

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(BaseUrl,"drivingsessions");

            var response = await client.PostAsync(requestUri, content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<StateReqs> GetStateReqs()
        {
            var client = new HttpClient();
            var requestUri = GenerateRequestUri(BaseUrl, "statereqs");

            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var stateReqs = JsonConvert.DeserializeObject<StateReqs>(json);
            return stateReqs;
        }

        // TODO still need to define the WeatherData Object
        public static async Task<Object> GetWeatherData(double latitude, double longitude)
        {
            var client = new HttpClient();
            var requestUri = GenerateDarkSkyWeatherRequestUri("",latitude,longitude);
            var response = await client.GetAsync(requestUri);
            var json = response.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject(json);
            return weatherData;
        }



        private static string GenerateRequestUri(string host, string endPoint, Dictionary<string, string> paramDictionary = null)
        {
            var queryString = string.Format(string.Join("&", paramDictionary.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            var builder = new UriBuilder
                          {
                              Host = host,
                              Path = endPoint,
                              Query = queryString
                          };
            return builder.ToString();

        }

        private static string GenerateDarkSkyWeatherRequestUri(string apiKey, double latitude, double longitude)
        {
            return string.Join(",",string.Join("/", "https://api.darksky.net", "forecast", apiKey),latitude,longitude);
        }
    }
}
