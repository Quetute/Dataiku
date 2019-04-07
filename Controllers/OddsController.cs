using System;
using System.IO;
using Dataiku.Components;
using Dataiku.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dataiku.Controllers
{
    public class OddsController : Controller 
    {
        private readonly Computer _computer;

        public OddsController(Computer computer)
        {
            _computer = computer;
        }

        public IActionResult Index(){
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(IFormFile empireFile)
        {
            Empire empire;
            
            try 
            {
                using(var sr = new StreamReader(empireFile.OpenReadStream()))
                {
                    empire = JsonConvert.DeserializeObject<Empire>(sr.ReadToEnd());
                }
            }
            catch (Exception)
            {
                return BadRequest("Bad format");
            }

            var OddCalculator = new OddCalculator(_computer, empire); 
            ViewData["Odds"] = OddCalculator.CalculateOdds() * 100; 
            return View("Index");
        
        }
    }
}