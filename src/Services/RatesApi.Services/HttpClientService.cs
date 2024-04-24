﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RatesInterfaces;
using RatesModels;

namespace RatesApi.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpClientService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<LatestEcbRatesResponce> ConvertRates(string from, List<string> currencies, decimal amount)
        {
            try
            {
                var apiKey = _configuration["AppSettings:ApiKey"];
                var latestEcbRatesBaseUrl = _configuration["AppSettings:GetLatestBaseUrl"];
                var convertBaseUrl = _configuration["AppSettings:ConvertBaseUrl"];

                HttpResponseMessage response = await _httpClient.GetAsync($"{latestEcbRatesBaseUrl}apiKey={apiKey}");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                LatestEcbRatesResponce latestEcbRatesResponse = JsonConvert.DeserializeObject<LatestEcbRatesResponce>(jsonResponse)!; // ! for the null warning (fix it)

                var date = latestEcbRatesResponse.Date;
                string currenciesString = string.Join(",", currencies);
                string convertUrl = $"{convertBaseUrl}apiKey={apiKey}&from={from}&amount={amount}&date={date}&currencies={currenciesString}";

                response = await _httpClient.GetAsync($"{convertUrl}");

                jsonResponse = await response.Content.ReadAsStringAsync();
                latestEcbRatesResponse = JsonConvert.DeserializeObject<LatestEcbRatesResponce>(jsonResponse)!; // ! for the null warning (fix it)

                return latestEcbRatesResponse;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get the Json response from RatesWebApi, convert it to the LatestEcbRatesResponce object and return it. 
        /// </summary>
        /// <returns></returns>
        public async Task<LatestEcbRatesResponce> GetLatestEcbRates()
        {
            try
            {
                var apiKey = _configuration["AppSettings:ApiKey"];
                var latestEcbRatesBaseUrl = _configuration["AppSettings:GetLatestBaseUrl"];

                HttpResponseMessage response = await _httpClient.GetAsync($"{latestEcbRatesBaseUrl}apiKey={apiKey}");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                LatestEcbRatesResponce latestEcbRatesResponse = JsonConvert.DeserializeObject<LatestEcbRatesResponce>(jsonResponse)!; // ! for the null warning (fix it)

                return latestEcbRatesResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                throw;
            }
        }
    }
}
