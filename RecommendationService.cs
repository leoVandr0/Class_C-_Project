using System.Collections.Generic;
using System.Linq;

namespace SmartEco.Services
{
    public class RecommendationService
    {
        private Dictionary<string, int> bundlePopularity = new Dictionary<string, int>();

        public void RecordBundle(string bundle)
        {
            if (!bundlePopularity.ContainsKey(bundle))
                bundlePopularity[bundle] = 0;
            bundlePopularity[bundle]++;
        }

        public string GetTopRecommendation()
        {
            if (bundlePopularity.Count == 0)
                return null;

            return bundlePopularity.OrderByDescending(b => b.Value).First().Key;
        }
    }
}
