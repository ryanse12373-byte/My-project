using System;
using System.Collections;
using UnityEngine;

public class SpawnAtDeath : MonoBehaviour
{
    [SerializeField] private GameObject Object;
    [SerializeField] private GameObject Vfx;
    [SerializeField] private Health health;
    [SerializeField] private float time;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;


    private void StartSpawn()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        transform.parent = null;

        Spawn(pos, rot);
    }
    

    private void Spawn(Vector3 pos, Quaternion rot)
    {
        if (audioSource != null && sound != null)
            audioSource.PlayOneShot(sound, 0.25f);

     for (int i = 0; i < 2; i++)
        {
            Instantiate(Vfx, pos, rot);
        }

        Instantiate(Object, pos, rot);
    }

    void OnEnable()
    {
        health.OnDeath += StartSpawn;
    }
    void OnDisable()
    {
        health.OnDeath -= StartSpawn;
    }
}
