using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Wait3Sec : MonoBehaviour
{
    
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
