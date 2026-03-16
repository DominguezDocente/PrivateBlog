using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Utils.Mediator
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
        Task Send(IRequest request);
    }
}
