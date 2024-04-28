﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RatesInterfaces;
using RatesModels;

namespace RatesWebApi.Controllers
{
    [Route("api/rates/[controller]")]
    [ApiController]
    public class RatesWebApiController : ControllerBase
    {
        private readonly IHttpClientService _httpClientService;
        public RatesWebApiController(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        [HttpGet("getLatestEcbRates")]
        public async Task<IActionResult> GetLatestEcbRates()
        {
            try
            {
                var latestEcbRates = await _httpClientService.GetLatestEcbRates();
                //string jsonRates = JsonConvert.SerializeObject(latestEcbRates);
                //return Ok(jsonRates);
                return Ok(latestEcbRates);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("convertRates")]
        public async Task<IActionResult> ConvertRates([FromQuery] ConvertRequest convertRequest)
        {
            try
            {
                var convert = await _httpClientService.ConvertRates(convertRequest);
                return Ok(convert);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
