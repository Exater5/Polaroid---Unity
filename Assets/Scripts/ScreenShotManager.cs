using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotManager : MonoBehaviour
{
    private Texture2D _targetTexture;
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
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string fileName = $"{Application.dataPath}/Resources/{_currentPhotoIndex}.png";
            //string fileName = $"{Application.streamingAssetsPath}/{_currentPhotoIndex}.png";
            System.IO.File.WriteAllBytes(fileName, bytes);
            _takingPhoto = false;
            StartCoroutine(LoadSprite(_currentPhotoIndex));
            _currentPhotoIndex++;
        }
    }

    IEnumerator LoadSprite(int photoindex)
    {
//#if UNITY_EDITOR
//        UnityEditor.AssetDatabase.Refresh();
//#endif  
        yield return new WaitForSeconds(1f);
        //string path = Application.streamingAssetsPath + "/" + photoindex.ToString();
        string path = Application.dataPath + "/Resources/" + photoindex.ToString();
        OnPostprocessTexture(path);
        _targetTexture = Resources.Load<Texture2D>(path);
        Sprite targetSp = Sprite.Create(_targetTexture, new Rect(0, 0, 2048, 1024), Vector2.one * 0.5f, 500f);
        sp.sprite = targetSp;
        _img.sprite = targetSp;
    }

    void OnPostprocessTexture(string path)
    {
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        //importer.textureType = TextureImporterType.GUI;


        Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
        if (asset)
        {
            EditorUtility.SetDirty(asset);
        }
        else
        {
            importer.textureType = TextureImporterType.GUI;
        }
    }
}
