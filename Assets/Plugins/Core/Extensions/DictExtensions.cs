using System;
using System.Collections.Generic;
using System.Globalization;

namespace Core.Extensions
{
    public static class DictExtensions
    {
        public static bool TryGet(this Dictionary<string, object> dict, string key, out int outVal)
        {
            outVal = default;
            try
            {
                if (!dict.ContainsKey(key)) return false;

                outVal = Convert.ToInt32(dict[key]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryGet(this Dictionary<string, object> dict, string key, out float outVal)
        {
            outVal = default;
            try
            {
                if (!dict.ContainsKey(key)) return false;

                outVal = Convert.ToSingle(dict[key]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static bool TryGetValue<T>(this Dictionary<string, object> dict, string key, T defaultValue, out T value)
        {
            value = defaultValue;
            if (!dict.ContainsKey(key))
            {
                return false;
            }

            try
            {
                value = (T)dict[key];
            }
            catch (Exception _)
            {
                value = defaultValue;
                return false;
            }
        
            return true;
        }

        public static bool TryGetBool(this Dictionary<string, object> dict, string key, bool defaultValue, out bool value)
        {
            value = defaultValue;
            if (!dict.ContainsKey(key))
            {
                return false;
            }

            try
            {
                object dictVal = dict[key];
                switch (dictVal)
                {
                    case int intValue:
                        value = intValue == 1;
                        break;
                    case string strValue:
                        value = strValue.ToLower() == "true" || strValue == "1";
                        break;
                    case bool boolValue:
                        value = boolValue;
                        break;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                value = defaultValue;
                return false;
            }

            return true;
        }

        public static bool TryGetEnum<TEnum>(this Dictionary<string, object> dict, string key, TEnum defaultValue,
                                             out TEnum value)
        {
            value = defaultValue;
            if (!dict.ContainsKey(key))
            {
                return false;
            }

            try
            {
                value = (TEnum)Enum.Parse(typeof(TEnum), dict[key].ToString());
            }
            catch (Exception _)
            {
                value = defaultValue;
                return false;
            }
        
            return true;
        }
        
        public static bool TryGetNumericValue<T>(this Dictionary<string, object> dict, string key, T defaultValue,
                                             out T value) where T : IConvertible
        {
            value = defaultValue;
            if (!dict.ContainsKey(key))
            {
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(dict[key], typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception _)
            {
                value = defaultValue;
                return false;
            }
        
            return true;
        }
    }
}