using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCProxy
{
    /* Source: https://www.anotherdevblog.net/posts/flattening-json-in-json-net
    /*
     * Usage:
        foreach (var jValue in JExtensions.GetLeafValues(json))
            {
                DataRow row = dt.NewRow();
                row[0] = jValue.Path;
                row[1] = jValue.Value;
                dt.Rows.Add(row);
            }
     * */

    public static class JExtensions
    {
        public static IEnumerable<JValue> GetLeafValues(this JToken jToken)
        {
            if (jToken is JValue jValue)
            {
                yield return jValue;
            }
            else if (jToken is JArray jArray)
            {
                foreach (var result in GetLeafValuesFromJArray(jArray))
                {
                    yield return result;
                }
            }
            else if (jToken is JProperty jProperty)
            {
                foreach (var result in GetLeafValuesFromJProperty(jProperty))
                {
                    yield return result;
                }
            }
            else if (jToken is JObject jObject)
            {
                foreach (var result in GetLeafValuesFromJObject(jObject))
                {
                    yield return result;
                }
            }
        }

        #region Private helpers

        static IEnumerable<JValue> GetLeafValuesFromJArray(JArray jArray)
        {
            for (var i = 0; i < jArray.Count; i++)
            {
                foreach (var result in GetLeafValues(jArray[i]))
                {
                    yield return result;
                }
            }
        }

        static IEnumerable<JValue> GetLeafValuesFromJProperty(JProperty jProperty)
        {
            foreach (var result in GetLeafValues(jProperty.Value))
            {
                yield return result;
            }
        }

        static IEnumerable<JValue> GetLeafValuesFromJObject(JObject jObject)
        {
            foreach (var jToken in jObject.Children())
            {
                foreach (var result in GetLeafValues(jToken))
                {
                    yield return result;
                }
            }
        }

        #endregion
    }
}
