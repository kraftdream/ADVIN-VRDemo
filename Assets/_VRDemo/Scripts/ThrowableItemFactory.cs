using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRDemo
{
    public class ThrowableItemFactory<T> : IItemFactory<T> where T : ThrowableItem
    {
        private readonly List<T> _prefabs;
        private readonly List<T> _pool = new();
        private readonly Transform _parent;

        Dictionary<ThrowableItemType, float> _weights = new()
        {
            { ThrowableItemType.Sphere, 1f },
            { ThrowableItemType.Cube, 1f },
            { ThrowableItemType.Cylinder, 1f },
            { ThrowableItemType.EasterEgg, 0.25f } // very low chance
        };

        public ThrowableItemFactory(List<T> prefabs, Transform poolParent = null)
        {
            _prefabs = prefabs;
            _parent = poolParent;
        }

        private ThrowableItemType GetRandomThrowableItemType(Dictionary<ThrowableItemType, float> weights)
        {
            float totalWeight = weights.Values.Sum();
            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var pair in weights)
            {
                cumulative += pair.Value;

                if (roll <= cumulative)
                    return pair.Key;
            }

            return weights.Keys.Last();
        }

        private T GetRandomThrowableItemPrefab(List<T> prefabs)
        {
            ThrowableItemType itemType = GetRandomThrowableItemType(_weights);
            var prefab = prefabs.Find(prefab => prefab.Type == itemType);

            if (prefab == null)
                prefab = prefabs[Random.Range(0, prefabs.Count)];

            return prefab;
        }

        public T Create()
        {
            T obj = null;

            if (_pool.Count > 0)
            {
                ThrowableItemType itemType = GetRandomThrowableItemType(_weights);
                obj = _pool.Find(item => !item.gameObject.activeSelf && item.Type == itemType);
            }

            if (obj == null)
            {
                var prefab = GetRandomThrowableItemPrefab(_prefabs);
                obj = GameObject.Instantiate(prefab, _parent);
            }

            obj.gameObject.SetActive(true);
            _pool.Add(obj);
            return obj;
        }

        public T Create(Vector3  position, Quaternion rotation)
        {
            T obj = null;

            if (_pool.Count > 0)
            {
                ThrowableItemType itemType = GetRandomThrowableItemType(_weights);
                obj = _pool.Find(item => !item.gameObject.activeSelf && item.Type == itemType);
            }

            if (obj == null)
            {
                var prefab = GetRandomThrowableItemPrefab(_prefabs);
                obj = GameObject.Instantiate(prefab, _parent);
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            _pool.Add(obj);
            return obj;
        }

        public void Remove(T obj)
        {
            obj.Reset();
            obj.gameObject.SetActive(false);
        }

        public void RemoveAll()
        {
            foreach (var item in _pool)
                item.gameObject.SetActive(false);
        }
    }
}
