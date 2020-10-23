using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezaB.Data.Formating
{

    public static class RateLimitParser
    {
        public static RateLimitAttributeList ParseString(string raw)
        {
            var parsed = new RateLimitAttributeList();

            var pairsEx = new Regex(@"(([1-9][0-9]*[k|M]?)/([1-9][0-9]*[k|M]?))");
            var subPairEx = new Regex(@"([1-9][0-9]*[k|M]?)");
            var numbersEx = new Regex(@"[1-9][0-9]*");
            var suffixEx = new Regex(@"[k|M]");

            var pairs = pairsEx.Matches(raw);

            {
                if (pairs.Count <= 0)
                    return null;
                var currentPair = pairs[0].Value;
                var subPairs = subPairEx.Matches(currentPair);
                if (subPairs.Count != 2)
                    return null;
                var firstPart = subPairs[0].Value;
                var secondPart = subPairs[1].Value;

                {
                    var numbers = numbersEx.Match(firstPart);
                    var suffix = suffixEx.Match(firstPart);
                    if (!numbers.Success)
                        return null;
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                        return null;
                    parsed.UploadRate = parsedValue;

                    if (suffix.Success)
                        parsed.UploadRateSuffix = suffix.Value;
                }

                {
                    var numbers = numbersEx.Match(secondPart);
                    var suffix = suffixEx.Match(secondPart);
                    if (!numbers.Success)
                        return null;
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                        return null;
                    parsed.DownloadRate = parsedValue;

                    if (suffix.Success)
                        parsed.DownloadRateSuffix = suffix.Value;
                }
            }

            {
                if (pairs.Count <= 1)
                    return parsed;
                var currentPair = pairs[1].Value;
                var subPairs = subPairEx.Matches(currentPair);
                if (subPairs.Count != 2)
                    return parsed;
                var firstPart = subPairs[0].Value;
                var secondPart = subPairs[1].Value;

                {
                    var numbers = numbersEx.Match(firstPart);
                    var suffix = suffixEx.Match(firstPart);
                    if (!numbers.Success)
                        return parsed;
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                        return parsed;
                    parsed.UploadBurstRate = parsedValue;

                    if (suffix.Success)
                        parsed.UploadBurstRateSuffix = suffix.Value;
                }

                {
                    var numbers = numbersEx.Match(secondPart);
                    var suffix = suffixEx.Match(secondPart);
                    if (!numbers.Success)
                    {
                        parsed.UploadBurstRate = null;
                        parsed.UploadBurstRateSuffix = null;
                        return parsed;
                    }
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                    {
                        parsed.UploadBurstRate = null;
                        parsed.UploadBurstRateSuffix = null;
                        return parsed;
                    }
                    parsed.DownloadBurstRate = parsedValue;

                    if (suffix.Success)
                        parsed.DownloadBurstRateSuffix = suffix.Value;
                }
            }

            {
                if (pairs.Count <= 2)
                    return parsed;
                var currentPair = pairs[2].Value;
                var subPairs = subPairEx.Matches(currentPair);
                if (subPairs.Count != 2)
                    return parsed;
                var firstPart = subPairs[0].Value;
                var secondPart = subPairs[1].Value;

                {
                    var numbers = numbersEx.Match(firstPart);
                    var suffix = suffixEx.Match(firstPart);
                    if (!numbers.Success)
                        return parsed;
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                        return parsed;
                    parsed.UploadBurstThreshold = parsedValue;

                    if (suffix.Success)
                        parsed.UploadBurstThresholdSuffix = suffix.Value;
                }

                {
                    var numbers = numbersEx.Match(secondPart);
                    var suffix = suffixEx.Match(secondPart);
                    if (!numbers.Success)
                    {
                        parsed.UploadBurstThreshold = null;
                        parsed.UploadBurstThresholdSuffix = null;
                        return parsed;
                    }
                    int parsedValue;
                    if (!int.TryParse(numbers.Value, out parsedValue))
                    {
                        parsed.UploadBurstThreshold = null;
                        parsed.UploadBurstThresholdSuffix = null;
                        return parsed;
                    }
                    parsed.DownloadBurstThreshold = parsedValue;

                    if (suffix.Success)
                        parsed.DownloadBurstThresholdSuffix = suffix.Value;
                }
            }

            {
                if (pairs.Count <= 3)
                    return parsed;
                var currentPair = pairs[3].Value;
                var subPairs = subPairEx.Matches(currentPair);
                if (subPairs.Count != 2)
                    return parsed;
                var firstPart = subPairs[0].Value;
                var secondPart = subPairs[1].Value;

                {
                    int parsedValue;
                    if (!int.TryParse(firstPart, out parsedValue))
                        return null;
                    parsed.UploadBurstTime = parsedValue;
                }

                {
                    int parsedValue;
                    if (!int.TryParse(secondPart, out parsedValue))
                    {
                        parsed.UploadBurstTime = null;
                        return parsed;
                    }
                    parsed.DownloadBurstTime = parsedValue;
                }
            }

            return parsed;
        }

        public class RateLimitAttributeList
        {
            public int DownloadRate { get; set; }

            public string DownloadRateSuffix { get; set; }

            public int UploadRate { get; set; }

            public string UploadRateSuffix { get; set; }

            public int? DownloadBurstRate { get; set; }

            public string DownloadBurstRateSuffix { get; set; }

            public int? UploadBurstRate { get; set; }

            public string UploadBurstRateSuffix { get; set; }

            public int? DownloadBurstThreshold { get; set; }

            public string DownloadBurstThresholdSuffix { get; set; }

            public int? UploadBurstThreshold { get; set; }

            public string UploadBurstThresholdSuffix { get; set; }

            public int? DownloadBurstTime { get; set; }

            public int? UploadBurstTime { get; set; }
        }
    }
}
