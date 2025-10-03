using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRDemo
{
    public enum ThrowableItemType
    {
        None,
        Sphere,
        Cube,
        Cylinder,
        EasterEgg
    }

    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableItem : InteractableItem
    {
        [Header("Main")]
        [SerializeField]
        private ThrowableItemType _type;

        [Header("Collision")]
        [SerializeField]
        private List<string> _collisionTags = new List<string>();

        [SerializeField]
        private GameObject _collisionEffect;

        private Rigidbody _rigidbody;
        private Vector3 _initPos;
        private Quaternion _initRot;

        public event Action<ThrowableItem, Collision> CollisionEnterListener;

        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
            _initPos = transform.position;
            _initRot = transform.rotation;
        }

        private protected void OnCollisionEnter(Collision collision)
        {
            if (_collisionTags.Contains(collision.gameObject.tag))
            {
                if (_collisionEffect != null)
                {
                    GameObject spawned = GameObject.Instantiate(_collisionEffect);
                    ContactPoint contact = collision.contacts[0];
                    spawned.transform.position = contact.point;
                }

                CollisionEnterListener?.Invoke(this, collision);
            }
        }

        public void Reset()
        {
            CollisionEnterListener = null;

            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.position = _initPos;
                _rigidbody.rotation = _initRot;
                _rigidbody.Sleep();
            }
        }

        public ThrowableItemType Type => _type;
    }
}
