using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Services
{
    public interface IGoogleReCaptchaService
    {
        Task<bool> VerifyAsync(
            string token,
            string expectedAction,
            CancellationToken cancellationToken = default);
    }
}
