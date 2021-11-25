using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] private int _resWidth = 1920;
    [SerializeField] private int _resHeight = 1080;
    [SerializeField]SpriteRenderer sp;
    private bool _takingHRShot = false;
    private int _currentPhotoIndex;

    public string GetScreenShotName(int width, int height)
    {
        //return string.Format("{0}/Resources/screen_{1}x{2}_{3}.png",
        //    Application.dataPath,
        //    width, height,
        //    System.DateTime.Now.ToString("yyyy-MM-dd_HH--mm-ss"));
        return $"{Application.dataPath}/Resources/{_currentPhotoIndex}.png";
    }

    public void TakeHighResolutionShot()
    {
        _takingHRShot = true;
    }

    public void LateUpdate()
    {
        _takingHRShot |= Input.GetKeyDown(KeyCode.K);
        if (_takingHRShot)
        {
            RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0,0, _resWidth, _resHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string fileName = $"{Application.dataPath}/Resources/{_currentPhotoIndex}.png";

            Sprite tImporter = Resources.Load<Sprite>($"{_currentPhotoIndex}");
            Sprite targetSp = Sprite.Create(screenShot, new Rect(0f,0f, _resWidth, _resHeight), Vector2.one/2f, 100f);
            sp.sprite = tImporter;
            //tImporter.textureType = TextureImporterType.Sprite;

            System.IO.File.WriteAllBytes(fileName, bytes);
            _takingHRShot = false;            
            FindObjectOfType<PhotoSet>().SetTargetImage(_currentPhotoIndex);
            _currentPhotoIndex++;
        }
    }
}
