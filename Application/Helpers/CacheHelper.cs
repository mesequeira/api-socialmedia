using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class CacheHelper<T>
    {
        public int ExpirationDuration { get; set; }
        public int InactiveDuration { get; set; }
        public T Data { get; set; }
        public string CacheKey { get; set; }

        public CacheHelper(int inactiveDuration, int expirationDuration, T data, string cacheKey)
        {
            InactiveDuration = inactiveDuration;
            ExpirationDuration = expirationDuration;
            Data = data;
            CacheKey = cacheKey;
        }
    }
}
