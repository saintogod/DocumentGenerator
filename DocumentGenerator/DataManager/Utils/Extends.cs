using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Utils
{
    public static class Extends
    {
        public static void AddSingleItem<TKey, TSubKey, TValue, Dic>(this Dictionary<TKey, Dic> dic, TKey key, TSubKey subKey,TValue singleValue) where Dic :IDictionary<TSubKey,TValue> ,new()
        {
            if (!dic.ContainsKey(key)) {
                dic.Add(key, new Dic());
            }
            dic[key][subKey] = singleValue;
        }

        public static void AddRange<TKey, Dic>(this Dictionary<TKey, Dic> dic, Dictionary<TKey,Dic> values) where Dic : class
        {
            foreach (var item in values)
                dic.Add(item.Key, item.Value);
        }
    }
}
