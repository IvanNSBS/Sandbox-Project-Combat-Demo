using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAndDestroy : MonoBehaviour
{

    public DamageData dmgData;
    public GameObject destroyOnHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dmgData.caster)
            return;

        var health = other.gameObject.GetComponent<HealthAndMana>();
        if(health)
            health.TakeDamage(dmgData.damage);

        GameObject obj = new GameObject();
        obj.transform.position = other.transform.position;
        var src = obj.AddComponent<AudioSource>();
        src.clip = dmgData.dmgSound;
        src.Play();
        var destroy = obj.AddComponent<DestroyAfterTime>();
        destroy.duration = src.clip.length;
        destroy.remainingTime = destroy.duration;

        Destroy(destroyOnHit);
    }
}
