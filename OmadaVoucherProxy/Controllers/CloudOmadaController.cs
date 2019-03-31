using Microsoft.AspNetCore.Mvc;
using OmadaAPI.Controllers;
using OmadaAPI.Model;
using OmadaAPI.Model.DTO;
using OmadaVoucherProxy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmadaVoucherProxy.Controllers
{
    [Route("api/cloud")]
    [ApiController]
    public class CloudOmadaContoller : ControllerBase
    {
        /* Example POST Body
         {
	        "CodeLength":6,
	        "Amount":1,
	        "DurationMinutes":1440
         }
        */
        [HttpPost]
        [Route("createVouchers")]
        public async Task<ActionResult> Post([FromBody] VoucherGenerationSettings settings)
        {
            var c = new CloudConnector(new CloudLogin() { Username = Settings.CloudOmadaUser, Password = Settings.CloudOmadaPassword});
            await c.LoginAsync();
            //Check if Id is specified in request, then in AppSettings, then check if user only has one controller.
            //If nothing matches, return error.
            if (string.IsNullOrEmpty(settings.CloudClientId))
            {
                if (string.IsNullOrEmpty(Settings.CloudOmadaClientId))
                {
                    var controllers = await c.GetCloudControllersAsync();
                    if (controllers.Count != 1)
                    {
                        return BadRequest("CloudClientId not specified");
                    }
                    else
                    {
                        settings.CloudClientId = controllers.Single().Id;
                    }
                }
                else
                {
                    settings.CloudClientId = Settings.CloudOmadaClientId;
                }
            }
            await c.LoginAsync();
            c.DefaultCloudControllerId = settings.CloudClientId;
            await c.ConnectToCloudControllerAsync();
            var vouchers = await c.GenerateVouchersAsync(settings.ToNewVoucherParams());
            return new JsonResult(vouchers);
        }
    }
}
