using System;
using UnityEngine;

namespace Anywhere.Net
{

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newjson = "{ \"Items\": " + json + " }";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newjson);
            return wrapper.Items;
        }

        public static T[] FromJsonOverwrite<T>(string json)
        {
            Wrapper<T> wrapper = new Wrapper<T>();            
            JsonUtility.FromJsonOverwrite(json, wrapper);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}

