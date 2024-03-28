using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Pathfinding
{
    public class AstarPathFind<T>
    {
        private struct pathData
        {
            public float g;

            public float h;

            public float f;
        }

        private int _calculatorPatience;

        private Func<T, T, float> _HeuristicDistance;

        private Func<T, Dictionary<T, float>> _ConnectedNodesAndStepCosts;

        public AstarPathFind(Func<T, T, float> HeuristicDistance, Func<T, Dictionary<T, float>> ConnectedNodesAndStepCosts, int calculatorPatience = 9999)
        {
            _HeuristicDistance = HeuristicDistance;
            _ConnectedNodesAndStepCosts = ConnectedNodesAndStepCosts;
            _calculatorPatience = calculatorPatience;
        }

        public bool GenerateAstarPath(T startNode, T targetNode, out List<T> path)
        {
            float num = _HeuristicDistance(startNode, targetNode);
            int num2 = _calculatorPatience;
            HashSet<T> hashSet = new HashSet<T>();
            Dictionary<T, pathData> dictionary = new Dictionary<T, pathData> {
            {
                startNode,
                new pathData
                {
                    g = 0f,
                    h = num,
                    f = num
                }
            } };
            Dictionary<T, T> dictionary2 = new Dictionary<T, T>();
            while (num2 > 0)
            {
                num2--;
                if (dictionary.Count == 0)
                {
                    break;
                }

                T key = dictionary.Aggregate((KeyValuePair<T, pathData> l, KeyValuePair<T, pathData> r) => (l.Value.f < r.Value.f) ? l : r).Key;
                pathData pathData = dictionary[key];
                dictionary.Remove(key);
                hashSet.Add(key);
                if (key.Equals(targetNode))
                {
                    List<T> list = new List<T>();
                    T val = key;
                    while (!val.Equals(startNode))
                    {
                        list.Add(val);
                        val = dictionary2[val];
                    }

                    list.Add(val);
                    list.Reverse();
                    path = list;
                    return true;
                }

                foreach (KeyValuePair<T, float> item in _ConnectedNodesAndStepCosts(key))
                {
                    if (Enumerable.Contains(hashSet, item.Key))
                    {
                        continue;
                    }

                    float num3 = item.Value + pathData.g;
                    if (!dictionary.ContainsKey(item.Key) || dictionary[item.Key].g > num3)
                    {
                        float num4 = _HeuristicDistance(item.Key, targetNode);
                        float f = num4 + num3;
                        dictionary2[item.Key] = key;
                        if (!dictionary.ContainsKey(item.Key))
                        {
                            dictionary.Add(item.Key, new pathData
                            {
                                g = num3,
                                h = num4,
                                f = f
                            });
                        }
                        else
                        {
                            dictionary[item.Key] = new pathData
                            {
                                g = num3,
                                h = num4,
                                f = f
                            };
                        }
                    }
                }
            }

            path = new List<T>();
            return false;
        }
    }
}