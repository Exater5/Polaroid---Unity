using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] private int _resWidth = 1920;
    [SerializeField] private int _resHeight = 1080;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Image _img;
    private bool _takingPhoto = false;
    private int _currentPhotoIndex;


    public void LateUpdate()
    {
        _takingPhoto |= Input.GetKeyDown(KeyCode.K);
        if (_takingPhoto)
        {
            RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, _resWidth, _resHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            screenShot.Apply();
            Destroy(rt);

            //byte[] bytes = screenShot.EncodeToPNG();
            //string fileName = $"{Application.dataPath}/Resources/{_currentPhotoIndex}.png";
            //System.IO.File.WriteAllBytes(fileName, bytes);
            //_currentPhotoIndex++;
            Sprite targetSp = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.one * 0.5f, 500f);
            sp.sprite = targetSp;
            _img.sprite = targetSp;
            _takingPhoto = false;
        }
    }  
}
