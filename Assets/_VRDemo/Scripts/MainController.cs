using System.Collections.Generic;
using UnityEngine;

namespace VRDemo
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private UIMainFloatingPanel _mainPanel;

        [SerializeField]
        private ThrowItemSpawner _throwItemSpawner;

        [SerializeField]
        private GameObject _targetObject;

        [SerializeField]
        private GameObject _targetHitEffect;

        [SerializeField]
        private GameObject _targetEgg;

        private List<GameObject> _targetHitEffects = new List<GameObject>();

        private void Awake()
        {
            _mainPanel.SpawnThrowItemListener += OnSpawnItemClicked;
            _mainPanel.ClearSceneListener += OnClearSceneClicked;
        }

        private void OnSpawnItemClicked()
        {
            ThrowableItem item = _throwItemSpawner.SpawnThrowItem();
            item.GrabStateChangedListener += OnItemGrabStateChanged;
            item.CollisionEnterListener += OnItemCollisionEnter;
        }

        private void OnClearSceneClicked()
        {
            foreach (var hitEffect in _targetHitEffects)
                Destroy(hitEffect);

            _targetHitEffects.Clear();
            _throwItemSpawner.RemoveActiveThrowItems();
        }

        private void OnItemGrabStateChanged(InteractableItem item, bool isGrabbed)
        {
            if (!isGrabbed)
                _mainPanel.Attempts += 1;

            _targetEgg.SetActive(false);

            if (item is ThrowableItem && ((ThrowableItem)item).Type == ThrowableItemType.EasterEgg)
            {
                _targetEgg.SetActive(true);
            }
        }

        private void OnItemCollisionEnter(ThrowableItem item, Collision collision)
        {
            item.GrabStateChangedListener -= OnItemGrabStateChanged;
            item.CollisionEnterListener -= OnItemCollisionEnter;

            if (collision.gameObject == _targetObject)
            {
                ContactPoint contact = collision.contacts[0];
                RaycastHit hit;

                float backTrackLength = 1f;
                Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);

                if (collision.collider.Raycast(ray, out hit, 2))
                {
                    Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                    Texture2D targetTexture = (Texture2D)renderer.material.mainTexture;
                    Color color = targetTexture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y);
                    int points = 0;

                    if (color.r > 0.7f && color.g > 0.7f && color.b < 0.7f)
                    {
                        color = Color.yellow;
                        points = 5;
                    }
                    else if (Mathf.Max(color.r, color.g, color.b) == color.r)
                    {
                        color = Color.red;
                        points = 1;
                    }
                    else if (Mathf.Max(color.r, color.g, color.b) == color.g)
                    {
                        color = Color.green;
                        points = 10;
                    }
                    else
                    {
                        color = Color.white;
                    }

                    if (item.Type == ThrowableItemType.EasterEgg)
                        points = 100;

                    color *= 15f;

                    GameObject spawned = GameObject.Instantiate(_targetHitEffect);
                    spawned.transform.position = contact.point;
                    spawned.transform.forward = ray.direction;

                    Renderer[] spawnedRenderers = spawned.GetComponentsInChildren<Renderer>();
                    for (int rendererIndex = 0; rendererIndex < spawnedRenderers.Length; rendererIndex++)
                    {
                        Renderer spawnedRenderer = spawnedRenderers[rendererIndex];
                        spawnedRenderer.material.color = color;
                        if (spawnedRenderer.material.HasProperty("_EmissionColor"))
                        {
                            spawnedRenderer.material.SetColor("_EmissionColor", color);
                        }
                    }

                    _mainPanel.Score += points;
                    _targetHitEffects.Add(spawned);
                }
            }

            if (item.Type == ThrowableItemType.EasterEgg)
                _targetEgg.SetActive(false);
        }
    }
}
