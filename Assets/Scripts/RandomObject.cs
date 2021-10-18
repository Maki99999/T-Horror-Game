using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class RandomObject : MonoBehaviour
    {
        public GameObject[] objects;

        void Awake()
        {
            Instantiate(objects[Random.Range(0, objects.Length)], transform.position, transform.rotation, transform);
        }
    }
}
