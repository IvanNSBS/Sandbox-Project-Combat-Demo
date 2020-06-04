using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class WeaponFXFollow : MonoBehaviour
{
    public void MoveTo(Vector3 target){
        Sequence sq = DOTween.Sequence();
        float distance = Vector3.Distance(transform.position, target);
        sq.Append(gameObject.transform.DOMove(target, 0.1f * distance, false));
        sq.AppendCallback(() => Destroy(gameObject));
    }
}
