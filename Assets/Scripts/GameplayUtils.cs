using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class GameplayUtils 
{
    public static void SpawnSound(AudioClip sound, Vector3 position, GameObject parent = null) 
    {

        GameObject obj = new GameObject();
        if (parent)
            obj.transform.SetParent(parent.transform);
        obj.transform.position = position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = sound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;
    }
}
