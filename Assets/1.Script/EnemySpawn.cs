using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;

    IEnumerator Spawn()
    {
        
        while (true)
        {
            Instantiate(enemy, transform.position + new Vector3(Random.Range(-3,3), Random.Range(-1, 1), Random.Range(0, 5)), Quaternion.LookRotation(Vector3.zero));
            yield return new WaitForSeconds(0.7f);

        }

    }

    void Start()
    {
        
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
