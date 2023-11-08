using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxSelectCircle : MonoBehaviour
{

    public float maxScale = 1.25f;

    public float duration = 1.25f;

    private void Start()
    {

        Sequence newSequence = DOTween.Sequence();
        RectTransform tr = GetComponent<RectTransform>();
        Vector2 baseSize = tr.sizeDelta;
        newSequence.Append(tr.DOSizeDelta(baseSize * maxScale, duration/6f));
        newSequence.Append(tr.DOSizeDelta(Vector2.zero, duration*(5f/6f)));

        Destroy(this.gameObject, duration);
    }

    public void StartEffect(Vector3 pos)
    {
        GetComponent<RectTransform>().position = pos;

    }
}
