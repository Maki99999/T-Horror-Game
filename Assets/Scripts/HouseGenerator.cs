using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class HouseGenerator : MonoBehaviour
    {
        public GameObject[] housePrefabs;

        public float houseWidth = 32f;
        public int houseCount;

        void Start()
        {
            float currentX = houseWidth;
            for (int i = 0; i < houseCount - 1; i++)
            {
                Instantiate(housePrefabs[Random.Range(0, housePrefabs.Length)], transform.position + currentX * Vector3.right, Quaternion.Euler(Vector3.zero), transform);
                currentX += houseWidth;
            }
        }
    }
}
