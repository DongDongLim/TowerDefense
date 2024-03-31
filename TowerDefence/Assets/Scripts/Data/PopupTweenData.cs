using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "TowerDefense/PopupTweenData")]
public class PopupTweenData : TweenData
{
    public float startSize;
    public Ease popTween;
    public float duration;
}