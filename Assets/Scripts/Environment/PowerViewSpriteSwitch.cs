using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerViewSpriteSwitch : MonoBehaviour
{
    public List<Sprite> images;
    bool isPowered = false;

    public void ChangeSprite()
    {
        if (isPowered)
        {
            GetComponent<Image>().sprite = images[1];
        }
        else
        {
            GetComponent<Image>().sprite = images[0];
        }
        isPowered = !isPowered;

    }
}
