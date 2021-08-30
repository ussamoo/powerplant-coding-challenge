using System.Linq;
using Domain;
using FluentValidation;

namespace Application.ProductionPlan.Commands
{
    public class ProductionPlanCommandValidator : AbstractValidator<ProductionPlanCommand>
    {
        public ProductionPlanCommandValidator()
        {
            RuleFor(x => x.Load).GreaterThan(0).WithMessage("The load must be greater than 0");
            RuleFor(x => x.PowerPlants)
                .NotNull()
                .NotEmpty().WithMessage("Powerplants list cannot be null or empty");

            RuleForEach(x => x.PowerPlants)
                .SetValidator(new PowerPlantValidator())
                .When(x=> x.PowerPlants is not null && x.PowerPlants.Any());
        }
        
    }

    public class PowerPlantValidator : AbstractValidator<PowerPlant>
    {
        public PowerPlantValidator()
        {
            RuleFor(x => x.Efficiency).GreaterThan(0).WithMessage("Efficiency must be greater than 0");
        }
    }

}
