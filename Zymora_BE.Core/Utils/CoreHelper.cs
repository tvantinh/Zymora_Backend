using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Core.Utils
{
    public static class CoreHelper
    {
        public static DateTimeOffset SystemTimeNow => TimeHelper.ConvertToUTCPlus7(DateTimeOffset.UtcNow);
    }
}
