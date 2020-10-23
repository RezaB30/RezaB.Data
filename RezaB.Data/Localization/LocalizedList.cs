using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RezaB.Data.Localization
{
    /// <summary>
    /// Creates a localized enum associated with a resource.
    /// </summary>
    /// <typeparam name="TEnum">Type of enum (should have numeric value equivalent).</typeparam>
    /// <typeparam name="TResource">Type of resource associated with the enum.</typeparam>

    public class LocalizedList<TEnum, TResource> : LocalizedList where TEnum : IComparable, IConvertible, IFormattable
    {
        private Dictionary<int, string> ListData
        {
            get
            {
                var list = new Dictionary<int, string>();
                foreach (var item in Enum.GetValues(typeof(TEnum)))
                {
                    list.Add((int)item, item.ToString());
                }
                return list;
            }
        }

        /// <summary>
        /// Gets the list of enum entries all localized.
        /// </summary>
        /// <param name="culture">Specify the culture otherwise uses current thread culture.</param>
        /// <returns></returns>
        public override Dictionary<int, string> GetList(CultureInfo culture = null)
        {
            culture = culture ?? Thread.CurrentThread.CurrentCulture;
            var source = ListData;
            var list = new Dictionary<int, string>();
            var rm = new ResourceManager(typeof(TResource));
            foreach (var item in source)
            {
                list.Add(item.Key, rm.GetString(item.Value, culture));
            }

            return list;
        }

        /// <summary>
        /// Gets localized display name of an enum entry.
        /// </summary>
        /// <param name="value">The value of enum entry.</param>
        /// <param name="culture">Specify the culture otherwise uses current thread culture.</param>
        /// <returns></returns>
        public override string GetDisplayText(int? value, CultureInfo culture = null)
        {
            if (value.HasValue)
            {
                var list = GetList(culture);
                string returnValue;
                if (list.TryGetValue(value.Value, out returnValue))
                    return returnValue;
            }
            return "-";
        }
    }

    /// <summary>
    /// Non-generic base for localized list class.
    /// </summary>
    public abstract class LocalizedList
    {
        /// <summary>
        /// Gets the list of enum entries all localized.
        /// </summary>
        /// <param name="culture">Specify the culture otherwise uses current thread culture.</param>
        /// <returns></returns>
        public abstract Dictionary<int, string> GetList(CultureInfo culture = null);

        /// <summary>
        /// Gets localized display name of an enum entry.
        /// </summary>
        /// <param name="value">The value of enum entry.</param>
        /// <param name="culture">Specify the culture otherwise uses current thread culture.</param>
        /// <returns></returns>
        public abstract string GetDisplayText(int? value, CultureInfo culture = null);

        /// <summary>
        /// Underlying list of a items contained in the localized list.
        /// </summary>
        public ListItem[] GenericList
        {
            get
            {
                return GetList().Select(l => new ListItem() { ID = l.Key, Name = l.Value }).ToArray();
            }
        }

        /// <summary>
        /// An item in a localized list.
        /// </summary>
        public class ListItem
        {
            /// <summary>
            /// Item identifier.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Item localized display name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}
