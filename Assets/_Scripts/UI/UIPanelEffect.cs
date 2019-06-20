using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPanelEffect : MonoBehaviour {

    private Vector3 originScale;
    void Awake() {
        originScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void OnEnable() {
        transform.DOScale(originScale, UIController._Instance.UIEffectSpeed).SetUpdate(true); ;
    }

    void OnDisable() {
        transform.DOScale(Vector3.zero, UIController._Instance.UIEffectSpeed).SetUpdate(true);
    }
}
