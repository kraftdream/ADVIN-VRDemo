using UnityEngine;

namespace VRDemo
{
    public interface IItemFactory<T>
    {
        T Create();

        T Create(Vector3 position, Quaternion rotation);
    }
}
