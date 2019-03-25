using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmadaVoucherProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        /* Example POST Body
         {
	        "CodeLength":6,
	        "Amount":1,
	        "DurationMinutes":1440
         }
        */
        [HttpPost]
        public async Task<IEnumerable<Voucher>> Post([FromBody] VoucherGenerationSettings settings)
        {
            var c = new OmadaController(new Uri(Settings.OmadaHost));
            await c.LoginAsync(LoginParams.FromCredentials(Settings.OmadaUser, Settings.OmadaPassword));
            return await c.GenerateVouchersAsync(settings.ToNewVoucherParams());
        }
    }
}
