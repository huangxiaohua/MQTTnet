﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MQTTnet.Server.Controllers
{
    //[Authorize]
    [ApiController]
    public class ServerController : Controller
    {
        [Route("api/v1/server/version")]
        [HttpGet]
        public ActionResult<string> GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }
    }
}
