using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundData : MonoBehaviour
{

    public SoundData soundData;
    // Start is called before the first frame update
    public void Start()
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(gameObject.transform);
        obj.transform.position = gameObject.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = soundData.sound;
        src.loop = soundData.loop;
        src.Play();

        if (!soundData.loop)
        {
            var destroy = obj.AddComponent<DestroyAfterTime>();
            destroy.duration = src.clip.length;
            destroy.remainingTime = destroy.duration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
