using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSet : MonoBehaviour
{
    [SerializeField] Image _targetImage;

    public void SetTargetImage(int imageIndex)
    {
        _targetImage.sprite = Resources.Load<Sprite>(imageIndex + ".png");
    }
}
