using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Data.Formating
{
    /// <summary>
    /// Parse and format rate limit values.
    /// </summary>
    public static class RateLimitFormatter
    {
        public static string ToQuotaDescription(long quotaBytes, string quotaPackageName)
        {
            return string.Format("{0}({1})", quotaPackageName, ToTrafficStandard(quotaBytes, true));
        }
        /// <summary>
        /// Makes a standard bytes display text as B, KB, MB, GB and TB
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <param name="getApproximate">If set to true it will ignore the remainder of division and gives the closest value in standard format</param>
        /// <returns>Standardized string</returns>
        public static string ToTrafficStandard(long bytes, bool getApproximate = false)
        {
            MixedResults mixedResults;
            if (getApproximate)
                mixedResults = GetApproximateQuotient(bytes);
            else
                mixedResults = GetExactQuotient(bytes);

            return mixedResults.FieldValue + " " + mixedResults.Suffix;
        }
        /// <summary>
        /// Makes a standard bytes display text as B, KB, MB, GB and TB
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <param name="getApproximate">If set to true it will ignore the remainder of division and gives the closest value in standard format</param>
        /// <returns>Standardized string</returns>
        public static string ToTrafficStandard(decimal bytes, bool getApproximate = false)
        {
            MixedResults mixedResults;
            if (getApproximate)
                mixedResults = GetApproximateQuotient(bytes);
            else
                mixedResults = GetExactQuotient((long)bytes);

            return mixedResults.FieldValue + " " + mixedResults.Suffix;
        }
        /// <summary>
        /// Makes a standard bytes display text as B, KB, MB, GB and TB
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <param name="culture">Specify formatting culture.</param>
        /// <returns>Standardized string</returns>
        public static string ToDecimalTrafficStandard(long bytes, string culture = null)
        {
            var mixedResults = GetApproximateQuotient(bytes);

            return (culture == null ? mixedResults.FieldValueWithDecimals.ToString("##0.000") : mixedResults.FieldValueWithDecimals.ToString("##0.000", CultureInfo.CreateSpecificCulture(culture))) + " " + mixedResults.Suffix;
        }
        /// <summary>
        /// Makes a standard bytes display text as B, KB, MB, GB and TB
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <param name="culture">Specify formatting culture.</param>
        /// <returns>Standardized string</returns>
        public static string ToDecimalTrafficStandard(decimal bytes, string culture = null)
        {
            var mixedResults = GetApproximateQuotient(bytes);

            return (culture == null ? mixedResults.FieldValueWithDecimals.ToString("##0.000") : mixedResults.FieldValueWithDecimals.ToString("##0.000", CultureInfo.CreateSpecificCulture(culture))) + " " + mixedResults.Suffix;
        }
        /// <summary>
        /// Makes a standard bytes display text as B, KB, MB, GB and TB
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <param name="getApproximate">If set to true it will ignore the remainder of division and gives the closest value in standard format</param>
        /// <returns>Standardized result in value and suffix format</returns>
        public static MixedResults ToTrafficMixedResults(decimal bytes, bool getApproximate = false)
        {
            if (getApproximate)
                return GetApproximateQuotient(bytes);
            return GetExactQuotient(bytes);
        }

        private static MixedResults GetApproximateQuotient(decimal raw)
        {
            var quotient = raw;
            decimal remainder = 0m;
            var stage = 0;
            while (quotient >= 1024)
            {
                remainder = quotient % 1024;
                quotient /= 1024;
                stage++;
            }
            return new MixedResults()
            {
                FieldValue = Math.Floor(quotient),
                _suffix = stage,
                Quotient = remainder
            };
        }

        private static MixedResults GetExactQuotient(decimal raw)
        {
            if (raw == 0)
            {
                return new MixedResults()
                {
                    FieldValue = 0,
                    _suffix = 0
                };
            }
            decimal remainder = 0m;
            var quotient = raw;
            var stage = 0;
            do
            {
                quotient = raw / 1024;
                remainder = raw % 1024;
                if (remainder == 0)
                {
                    raw = quotient;
                    stage++;
                }
            } while (remainder == 0 && stage < 4);
            return new MixedResults()
            {
                FieldValue = Math.Floor(raw),
                _suffix = stage
            };
        }
        /// <summary>
        /// Structured Standardized bytes count.
        /// </summary>
        public class MixedResults
        {
            /// <summary>
            /// The integer part of result.
            /// </summary>
            public decimal FieldValue { get; set; }
            /// <summary>
            /// The last quotient of divisions.
            /// </summary>
            public decimal Quotient { get; set; }
            /// <summary>
            /// The decimal results.
            /// </summary>
            public decimal FieldValueWithDecimals
            {
                get
                {
                    return FieldValue + (Quotient / 1024m);
                }
            }
            /// <summary>
            /// Just for setting suffix type.
            /// </summary>
            public int _suffix { set; private get; }
            /// <summary>
            /// The suffix part of the rusult.
            /// </summary>
            public string Suffix
            {
                get
                {
                    switch (_suffix)
                    {
                        case 1:
                            return "KB";
                        case 2:
                            return "MB";
                        case 3:
                            return "GB";
                        case 4:
                            return "TB";
                        case 5:
                            return "PB";
                        case 6:
                            return "EB";
                        case 7:
                            return "ZB";
                        case 8:
                            return "YB";
                        case 9:
                            return "BB";
                        default:
                            return "B";
                    }
                }
            }

            public string RateSuffix
            {
                get
                {
                    switch (_suffix)
                    {
                        case 1:
                            return "KBps";
                        case 2:
                            return "MBps";
                        case 3:
                            return "GBps";
                        case 4:
                            return "TBps";
                        case 5:
                            return "PBps";
                        case 6:
                            return "EBps";
                        case 7:
                            return "ZBps";
                        case 8:
                            return "YBps";
                        case 9:
                            return "BBps";
                        default:
                            return "Bps";
                    }
                }
            }
        }
    }
}
