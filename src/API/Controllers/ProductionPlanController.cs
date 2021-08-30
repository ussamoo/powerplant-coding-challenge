using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using powerplant_coding_challenge.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ProductionPlan.Commands;
using Newtonsoft.Json;

namespace powerplant_coding_challenge.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("productionplan")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly ISender _mediator;
        public ProductionPlanController(IHubContext<NotificationsHub> hubContext, ISender mediator)
        {
            _hubContext = hubContext;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProductionPlanItem>>> Post([FromBody] ProductionPlanCommand planCommand)
        {
            var result = await _mediator.Send(planCommand);
            await _hubContext.Clients.All.SendAsync("ProductionPlanCalculated",JsonConvert.SerializeObject(planCommand), JsonConvert.SerializeObject(result));
            return Ok(result);
        }
    }
}
