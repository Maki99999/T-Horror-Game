using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class HouseGenerator : MonoBehaviour
    {
        public Transform outro;
        public GameObject[] housePrefabs;

        public float houseWidth = 32f;
        public int houseCount;

        void Start()
        {
            float currentX = houseWidth;
            for (int i = 0; i < houseCount; i++)
            {
                GameObject house = Instantiate(
                    housePrefabs[Random.Range(0, housePrefabs.Length)],
                    transform.position + currentX * Vector3.right,
                    Quaternion.Euler(Vector3.zero),
                    transform);
                house.GetComponentInChildren<MinigameTrigger>().difficulty = Mathf.RoundToInt(((float)i / houseCount) * 10f);
                currentX += houseWidth;
            }
            outro.position = transform.position + (currentX + 10.5f) * Vector3.right;
        }
    }
}
