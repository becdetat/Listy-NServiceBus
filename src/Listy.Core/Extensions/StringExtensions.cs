using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Listy.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            if (args == null)
            {
                args = new object[] {args};
            }

            var distinctNumberedTemplateMatches =
                (from object match in new Regex(@"\{\d{1,2}\}").Matches(format) select match.ToString())
                .Distinct().Count();
            if (distinctNumberedTemplateMatches != args.Length)
            {
                var argsDic = GetDictionaryFromAnonObject(args[0]);

                if (argsDic.Count < 1)
                    throw new InvalidOperationException("Please supply enough args for the numberd templates or use an anonymous object to identify the templates by name.");

                foreach (var o in argsDic)
                {
                    format = format.Replace("{{0}}".FormatWith(o.Key), o.Value.ToString());
                }

                return format;
            }

            string validationInput = format;
            for (int i = 0; i < args.Length; i++)
            {
                format = format.Replace("{" + i.ToString() + "}", args[i] == null ? string.Empty : args[i].ToString());
            }
            if (validationInput == format)
            {
                throw new InvalidOperationException(
                    "You can not mix template types. Use numbered templates or named ones with an anonymous object.");
            }

            return format;
        }

        private static IDictionary<string, object> GetDictionaryFromAnonObject(object args)
        {
            var result = new Dictionary<string, object>();
            if (args != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(args))
                {
                    result.Add(property.Name, property.GetValue(args));
                }
            }

            return result;
        }
    }
}