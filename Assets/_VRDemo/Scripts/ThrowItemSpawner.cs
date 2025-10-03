using System.Collections.Generic;
using UnityEngine;

namespace VRDemo
{
    public class ThrowItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<ThrowableItem> _throwableItemPrefabs;

        [SerializeField]
        private Transform _spawnPoint;

        private ThrowableItemFactory<ThrowableItem> _factory;

        private void Awake()
        {
            _factory = new ThrowableItemFactory<ThrowableItem>(_throwableItemPrefabs, transform);
        }

        private void OnItemCollisionEnter(ThrowableItem item, Collision collision)
        {
            item.CollisionEnterListener -= OnItemCollisionEnter;
            _factory.Remove(item);
        }

        public ThrowableItem SpawnThrowItem()
        {
            ThrowableItem item = _factory.Create(_spawnPoint.position, Quaternion.identity);
            item.CollisionEnterListener += OnItemCollisionEnter;
            return item;
        }

        public void RemoveActiveThrowItems()
        {
            _factory.RemoveAll();
        }

    }
}
