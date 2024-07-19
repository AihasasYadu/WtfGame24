using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace Vapronix.WordZenith.Utilities
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Global settings to be used for serialization & deserialization
        /// </summary>
        private static JsonSerializerSettings jsonSerializerSettings = null;

        /// <summary>
        /// Static constructor to set global settings for serialization
        /// </summary>
        static ExtensionMethods()
        {
            jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    string exceptionData = "Message : " + args.ErrorContext.Error.Message;
                    exceptionData += "\n Source : " + args.ErrorContext.Error.Source;
                    //exceptionData += "\n InnerException : " + args.ErrorContext.Error.InnerException.ToString();
                    exceptionData += "\n Path : " + args.ErrorContext.Path;
                    exceptionData += "\n Member : " + args.ErrorContext.Member;
                    exceptionData += "\n OriginalObject : " + args.ErrorContext.OriginalObject;
                    Logger.Log("JSON Deserialize Exception : " + exceptionData);
                    args.ErrorContext.Handled = false;
                }
            };
        }

        /// <summary>
        /// Serialize given object to its json representation
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize</param>
        /// <returns>json string of serialized object</returns>
        /// @see FromJson
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        /// <summary>
        /// Deserialize json string to the given object type.
        /// </summary>
        /// <typeparam name="T">Type of object to which we need to deserialize</typeparam>
        /// <param name="obj">json string</param>
        /// <returns>Object of type T</returns>
        public static T FromJson<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj, jsonSerializerSettings);
        }

        /// <summary>
        /// Get a particular node by name from a json string
        /// </summary>
        /// <param name="str">json string</param>
        /// <param name="nodeName">name of the node</param>
        /// <returns>json string for the given node</returns>
        public static string GetJsonNode(this string str, string nodeName, bool asJson = true)
        {
            JObject responseData = JObject.Parse(str);
            if (responseData != null)
            {
                JToken token = responseData[nodeName];
                if (token != null)
                    return asJson ? token.ToJson(): token.ToString();
                else
                    Logger.Log ($"Could not find token {nodeName} in given string");
            }
            else
                Logger.Log ("Could not parse to Json object");
            return null;
        }

        /// <summary>
        /// Activate/Deactivate GameObject. Its a helper function
        /// </summary>
        /// <param name="obj">instance of MonoBehaviour</param>
        /// <param name="value">activate/deactivate</param>
        /// <returns>returns the state of game object</returns>
        public static bool SetActive(this MonoBehaviour obj, bool value)
        {
            obj.gameObject.SetActive(value);
            return value;
        }

        public static bool SetActive(this Transform obj, bool value)
        {
            obj.gameObject.SetActive(value);
            return value;
        }

        public static bool SetActive(this RectTransform obj, bool value)
        {
            obj.gameObject.SetActive(value);
            return value;
        }

        /// <summary>
		/// Enable/Disable Animator component
        /// </summary>
        /// <param name="gameObject">GameObject that has Animator component</param>
        /// <param name="enable">Enable/Disable</param>
        /// <param name="recursive">Apply recursively or just the given object</param>
        /// <param name="includeInactive">Should we include inactive GameObjects?</param>
        public static void EnableAnimator(this GameObject gameObject, bool enable, bool recursive, bool includeInactive)
        {
            List<Animator> animators = new List<Animator>();
            if (recursive)
                animators.AddRange(gameObject.GetComponentsInChildren<Animator>(includeInactive));
            else
                animators.Add(gameObject.GetComponent<Animator>());
            foreach (Animator anim in animators)
                anim.enabled = enable;
        }

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    string name = ((DescriptionAttribute)attrs[0]).Description;
                #if QA_BUILD
                    name = "d_" + name;
                #endif
                    return name;
                }
            }
            return en.ToString();
        }

        public static Dictionary<K, V> ResetValues<K, V> (this Dictionary<K, V> dic)
        {
            dic.Keys.ToList().ForEach (x => dic[x] = default (V));
            return dic;
        }

        public static TimeSpan HoursToTimeSpan (this string timeStamp)
        {
            TimeSpan time = TimeSpan.MinValue;
            if (!string.IsNullOrEmpty (timeStamp))
            {
                float ticks = float.Parse (timeStamp);
                time = TimeSpan.FromHours (ticks);
            }
            return time;
        }

        public static RectTransform ResetRect (this RectTransform rectTransform, bool canResetAnchors = true)
        {
            RectTransform rect = null;
            if (rectTransform != null)
            {
                if (canResetAnchors)
                {
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                }
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.anchoredPosition = Vector3.zero;
            }

            return rect;
        }

        public static List<T> Randomize<T> (this List<T> list)
        {
            if (list != null)
            {
                int n = list.Count;
                System.Random rng = new System.Random ();
                while (n > 1) 
                {  
                    n--;  
                    int k = rng.Next(n + 1);  
                    T value = list [k];  
                    list [k] = list [n];  
                    list [n] = value;  
                } 
            }

            return list;
        }

    #region Probability Check
        public static int GetRandomIndex (this List<int> pool)
        {
            int index = -1;
            int selection = 0;
            System.Random rand = new System.Random();
            // get universal probability 
            double u = pool.Sum ();

            // pick a random number between 0 and u
            double r = rand.NextDouble() * u;

            double sum = 0;
            for (int i = 0 ; i < pool.Count ; i++)
            {
                sum += pool [i];
                // loop until the random number is less than our cumulative probability
                if (r <= sum)
                {
                    selection = pool [i];
                    break;
                }
            }

            index = pool.FindIndex (x => x == selection);
            return index;
        }
    #endregion
    }
}