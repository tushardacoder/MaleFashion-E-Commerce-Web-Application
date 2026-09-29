using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure.Services
{
    public class GoogleReCaptchaOptions
    {
        public const string SectionName = "GoogleReCaptcha";

        public string SiteKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public double MinimumScore { get; set; } = 0.5;
    }
}
