using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    public Vector2 min;
    public Vector2 max;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnApple();
        }
    }
    public void SpawnApple()
    {
        float pointX = Random.Range(min.x, max.x);
        float pointY = Random.Range(min.y, max.y);

        Instantiate(applePrefab, new Vector2(pointX, pointY), transform.rotation);
    }
}
