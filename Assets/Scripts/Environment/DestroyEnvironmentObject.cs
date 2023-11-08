using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DestroyEnvironmentObject : MonoBehaviour
{
    [SerializeField, AssetsOnly]
    private GameObject destructionEffect;

    [SerializeField]
    private float destructionDelay = 0f;

    [SerializeField]
    private Animation removalAnimation;

    public void OnDestroyEnvironmentObject()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        if (removalAnimation != null)
        {
            //Play anim
        }
        yield return new WaitForSeconds(destructionDelay);
        if (destructionEffect != null)
        {
            GameObject obj = Instantiate(destructionEffect, transform.position, transform.rotation);
            obj.transform.parent = null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
