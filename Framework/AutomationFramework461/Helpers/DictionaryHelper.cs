using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework461.Helpers
{
    public class DictionaryHelper
    {
        public static IDictionary<string, string> ConvertFromObjectToDictionary(object data)
        {
            if (data != null)
            {
                Dictionary<string, string> convertData = new Dictionary<string, string>();
                convertData = data as Dictionary<string, string>;
                return convertData;
            }
            else
            {
                return null;
            }
        }

        public static IDictionary<int, IDictionary<string, string>> ConvertFromObjectToDictionaryOfDic(object data)
        {
            if (data != null)
            {
                Dictionary<int, IDictionary<string, string>> convertData = new Dictionary<int, IDictionary<string, string>>();
                convertData = data as Dictionary<int, IDictionary<string, string>>;
                return convertData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convert an object for dictionary of dictionary even if object as only one row
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<int, IDictionary<string, string>> ConvertObjectToDictionaryOfDictionary(object data)
        {
            IDictionary<int, IDictionary<string, string>> convertedData = null;

            convertedData = ConvertFromObjectToDictionaryOfDic(data);
            if (convertedData == null)
            {
                convertedData = new Dictionary<int, IDictionary<string, string>>() { { 0, ConvertFromObjectToDictionary(data) } };
            }
            return convertedData;
        }

        public static string DictionaryToString(IDictionary<string, string> data)
        {
            return (data != null && data.Any()) ? string.Join("\n", data.Select(x => x.Key + ": " + x.Value)) : "<Dictionary is empty or null>.";
        }

        public static string DictionaryToString(IDictionary<string, object> data)
        {
            return (data != null && data.Any()) ? string.Join("\n", data.Select(x => x.Key + ": " + x.Value)) : "<Dictionary is empty or null>.";
        }

        public static string DictionaryOfDictionaryToString(IDictionary<string, Dictionary<string, string>> data)
        {
            return string.Join("\n", data.Select(x => x.Key + ": \n" + DictionaryToString(x.Value)));
        }

        public static string DictionaryIntStringOfDictionaryToString(IDictionary<int, IDictionary<string, string>> data)
        {
            return string.Join("\n", data.Select(x => x.Key.ToString() + ": \n" + DictionaryToString(x.Value)));
        }

        public static IDictionary<string, string> CompareTwoDictionaries(IDictionary<string, string> dataActual, IDictionary<string, string> dataExpected, bool considerSubstringAsEqual = false)
        {
            Dictionary<string, string> resultComparison = new Dictionary<string, string>();

            // Walthrough Expected Values
            foreach (var itemExpected in dataExpected)
            {
                if (dataActual.ContainsKey(itemExpected.Key))
                {
                    string itemActual = dataActual[itemExpected.Key];
                    if (considerSubstringAsEqual)
                    {
                        resultComparison.Add(itemExpected.Key, !itemExpected.Value.Trim().Contains(itemActual.Trim()) ? $"Expected: {itemExpected.Value} -Actual: {itemActual}" : null);
                    }
                    else
                    {
                        resultComparison.Add(itemExpected.Key, itemActual.Trim() != itemExpected.Value.Trim() ? $"Expected: {itemExpected.Value} -Actual: {itemActual}" : null);
                    }
                }
                else
                {
                    resultComparison.Add(itemExpected.Key, $"Dont contains the column: [{itemExpected.Key}] - Expected: {itemExpected.Value} -Actual: no column, no value.");
                }
            }

            return resultComparison.Where(x => x.Value != null).ToDictionary(i => i.Key, i => i.Value);
        }

        public static IDictionary<string, object> CompareTwoDictionaries(IDictionary<string, object> dataActual, IDictionary<string, object> dataExpected)
        {
            Dictionary<string, object> resultComparison = new Dictionary<string, object>();
            foreach (var itemExpected in dataExpected)
            {
                if (dataActual.ContainsKey(itemExpected.Key))
                {
                    object itemActual = dataActual[itemExpected.Key];
                    object expected = itemExpected.Value;
                    if (itemActual != null && expected != null)
                    {
                        var itemType = itemActual.GetType();

                        if (itemType.Name == "JArray")
                        {
                            var itemActualConverted = JsonConvert.DeserializeObject<List<IDictionary<string, string>>>(itemActual.ToString());
                            var expectedConverted = JsonConvert.DeserializeObject<List<IDictionary<string, string>>>(expected.ToString());
                            for (int i = 0; i < expectedConverted.Count; i++)
                            {
                                var comparedResult = CompareTwoDictionaries(itemActualConverted[i], expectedConverted[i]);
                                if (comparedResult.Count != 0)
                                {
                                    resultComparison.Add($"{i} {DictionaryHelper.DictionaryToString(comparedResult)}", comparedResult.Count != 0 ? $"Expected: {itemExpected.Value} -Actual: {itemActual}" : null);
                                }
                            }
                        }
                        else
                        {
                            if (itemType.Name == "Int64")
                            {
                                itemActual = Int32.Parse(itemActual.ToString());
                                expected = Int32.Parse(itemExpected.Value.ToString());
                            }
                            else
                            {
                                itemActual = itemActual.ToString();
                                expected = itemExpected.Value.ToString();
                            }
                            resultComparison.Add(itemExpected.Key, !itemActual.Equals(expected) ? $"Expected: {itemExpected.Value} -Actual: {itemActual}" : null);
                        }
                    }
                }
                else
                {
                    resultComparison.Add(itemExpected.Key, $"Dont contains the column: [{itemExpected.Key}] - Expected: {itemExpected.Value} -Actual: no column, no value.");
                }
            }

            return resultComparison.Where(x => x.Value != null).ToDictionary(i => i.Key, i => i.Value);
        }

        public static Dictionary<string, string> CompareDicsOfDics(IDictionary<int, IDictionary<string, string>> dataTableExpected, IDictionary<int, IDictionary<string, string>> dataTableActual)
        {
            Dictionary<string, string> differenceDictionary = new Dictionary<string, string>();

            foreach (var dataExpected in dataTableExpected)
            {
                if (dataTableActual.Count != 0)
                {
                    IDictionary<string, string> observedUpdatedAddressValuesVerification = DictionaryHelper.CompareTwoDictionaries(dataTableActual[dataExpected.Key], dataExpected.Value);

                    if (observedUpdatedAddressValuesVerification.Count != 0)
                    {
                        differenceDictionary.Add($"Row: {dataExpected.Key}\n", DictionaryHelper.DictionaryToString(observedUpdatedAddressValuesVerification));
                    }
                }
                else
                {
                    differenceDictionary.Add($"Row: {dataExpected.Key}\n", $"Current UI: is empty, Expected is:{DictionaryHelper.DictionaryToString(dataExpected.Value)}");
                }
            }

            return differenceDictionary;
        }

        public static List<IDictionary<string, string>> ConvertFromObjectToListOfDictionary(object data)
        {
            if (data != null)
            {
                List<IDictionary<string, string>> convertData = new List<IDictionary<string, string>>();
                convertData = data as List<IDictionary<string, string>>;
                return convertData;
            }
            else
            {
                return null;
            }
        }

        public static List<IDictionary<string, string>> ConvertObjectWithDictionaryToListOfDictionary(object data)
        {
            List<IDictionary<string, string>> convertedInfo = new List<IDictionary<string, string>>();
            object[] array = ((IEnumerable)data).Cast<object>()
                                                .Select(x => x == null ? x : x.ToString())
                                                .ToArray();
            foreach (object item in array)
            {
                convertedInfo.Add(JsonConvert.DeserializeObject<IDictionary<string, string>>(item.ToString()));
            }

            return convertedInfo;
        }

        public static string ListDictionaryToString(List<IDictionary<string, string>> data)
        {
            return string.Join("\n", data.Select(x => x + ": \n" + DictionaryToString(x)));
        }
    }
}
