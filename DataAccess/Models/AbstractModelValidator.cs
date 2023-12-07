using FluentValidation;

namespace GISPaPi.DataAccess.Models
{
    public abstract class AbstractModelValidator<T> : AbstractValidator<T> where T : class { }
}
