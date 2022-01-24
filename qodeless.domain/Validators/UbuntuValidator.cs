using FluentValidation;
using qodeless.desafio.domain.Entities;


namespace qodeless.desafio.domain.Validators
{
    public class UbuntuAddValidator : AbstractValidator<Ubuntu>
    {
        public UbuntuAddValidator()
        {
            RuleFor(entity => entity.Id).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Id).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Name).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Name).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Date).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Date).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Telephone).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Telephone).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.IndicatorArea).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.IndicatorArea).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Key).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Key).NotNull().WithMessage("Valor obrigatório");
        }
    }
    public class UbuntuUpdateValidator : AbstractValidator<Ubuntu>
    {
        public UbuntuUpdateValidator()
        {
            RuleFor(entity => entity.Id).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Id).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Name).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Name).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Date).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Date).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Telephone).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Telephone).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.IndicatorArea).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.IndicatorArea).NotNull().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Key).NotEmpty().WithMessage("Valor obrigatório");
            RuleFor(entity => entity.Key).NotNull().WithMessage("Valor obrigatório");
        }
    }
    public class UbuntuRemoveValidator : AbstractValidator<Ubuntu>
    {
        public UbuntuRemoveValidator()
        {
        }
    }
}
