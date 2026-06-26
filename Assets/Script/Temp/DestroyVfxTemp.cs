using System.Collections;
using UnityEngine;

public class DestroyVfxTemp : MonoBehaviour
{
    IEnumerator DestroyVfx()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(DestroyVfx());
    }
}
