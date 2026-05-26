using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections
{
    public class GetAccessibleSectionsQueryValidator : AbstractValidator<GetAccessibleSectionsQuery>
    {
        public GetAccessibleSectionsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El usuario es requerido.");
        }
    }
}
