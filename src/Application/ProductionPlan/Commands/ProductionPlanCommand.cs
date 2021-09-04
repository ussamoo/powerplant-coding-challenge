using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using Domain;
using MediatR;
using Newtonsoft.Json;

namespace Application.ProductionPlan.Commands
{
    public class ProductionPlanCommand : IRequest<List<ProductionPlanItem>>
    {
        public double Load { get; set; }
        public Fuel Fuels { get; set; }
        [JsonProperty("powerplants")]
        public IEnumerable<PowerPlant> PowerPlants { get; set; }
    }


    public class PlanCommandHandler : IRequestHandler<ProductionPlanCommand, List<ProductionPlanItem>>
    {
        private readonly IProductionPlanService _productionPlanService;

        public PlanCommandHandler(IProductionPlanService productionPlanService)
        {
            _productionPlanService = productionPlanService;
        }

        public async Task<List<ProductionPlanItem>> Handle(ProductionPlanCommand request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => _productionPlanService.Calculate(request.PowerPlants.ToList(),request.Fuels, request.Load), cancellationToken);

        }


    }
}
