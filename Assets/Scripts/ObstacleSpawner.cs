using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T
{
    public class ObstacleSpawner : MonoBehaviour
    {
        private static Vector2Int sizeOnGrid = new Vector2Int(1, 7);

        public GameObject[] obstaclePrefabs;

        void Start()
        {
            int holeLowerY = Random.Range(0, sizeOnGrid.y - 1);

            for (int x = 0; x < sizeOnGrid.x; x++)
                for (int y = 0; y < sizeOnGrid.y; y++)
                    if (Random.value < 0.7f && y != holeLowerY && y != holeLowerY + 1)
                        Instantiate(
                            obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
                            transform.position + new Vector3(x, y, 0),
                            Quaternion.Euler(0, 0, 0),
                            transform);
        }
    }
}
