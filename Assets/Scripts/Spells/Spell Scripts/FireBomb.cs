using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Spells/Fire Bomb")]
public class FireBomb : SpellObject
{

    public int numOfWaves = 3;
    public float delayBetweenWaves = 0.15f;
    public float waveDamage = 10f;
    [Header("Spawn Y Offset")]
    public float yOffset = -0.5f;
    [Header("Final Wave Attributes")]
    public float finalWaveDelay = 0.4f;
    public float finalWaveDuration = 0.6f;
    public float finalWaveDamage = 30f;
    public AudioClip finalWaveSound;
    public GameObject finalWaveGameObject;


    private float duration;

    public override bool Cast(GameObject caster)
    {
        duration = delayBetweenWaves * numOfWaves + 2f;

        Sequence sq = DOTween.Sequence();

        #region First Three Heat Waves
        for(int i = 0; i < numOfWaves; i++)
        {
            sq.AppendCallback(() => {
                var instance = Instantiate(instantiatedGO);
                instance.transform.position = caster.transform.position - new Vector3(0, yOffset, 0);
                // Damaga and sound setup
                var timer = instance.GetComponent<DestroyAfterTime>();
                timer.duration = duration;
                timer.remainingTime = duration;
                var dmgData = instance.GetComponent<DamageData>();
                dmgData.caster = caster;
                dmgData.damage = waveDamage;
                var dmgCol = instance.GetComponent<DamageOnCollide>();
                dmgCol.dmgData = dmgData;

                GameplayUtils.SpawnSound(spellSound, caster.transform.position);
            });
            if (i < numOfWaves - 1)
                sq.AppendInterval(delayBetweenWaves);
            else
                sq.AppendInterval(finalWaveDelay);
        }
        #endregion

        sq.AppendCallback(() => {
            var finalWaveInstance = Instantiate(finalWaveGameObject);
            finalWaveInstance.transform.position = caster.transform.position;
            // Damaga and sound setup
            var timer = finalWaveInstance.GetComponent<DestroyAfterTime>();
            timer.duration = finalWaveDuration;
            timer.remainingTime = finalWaveDuration;
            var dmgData = finalWaveInstance.GetComponent<DamageData>();
            dmgData.caster = caster;
            dmgData.damage = finalWaveDamage;
            var dmgCol = finalWaveInstance.GetComponent<DamageOnCollide>();
            dmgCol.dmgData = dmgData;

            GameplayUtils.SpawnSound(finalWaveSound, caster.transform.position);

            float shakeDuration = 0.2f;
            float strenght = 0.65f;
            int vibrato = 20;
            Camera.main.DOShakePosition(shakeDuration, strenght, vibrato, 45, true);
        });

        return true;

    }
}
