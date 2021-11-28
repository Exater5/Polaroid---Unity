using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] private int _resWidth = 1920;
    [SerializeField] private int _resHeight = 1080;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _creationPoint;
    [SerializeField] private PlayerMovement _playerMovement;
    private SpriteRenderer _spriteRenderer;
    private GameObject _createdPhoto;

    public void LateUpdate()
    {
        if (_playerMovement.CanTakePhoto && Input.GetMouseButtonDown(0))
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
            Sprite targetSp = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.one * 0.5f, 1000f);
            if (_createdPhoto != null)
            {
                Destroy(_createdPhoto);
            }
            _createdPhoto = Instantiate(_cardPrefab, _creationPoint);
            _createdPhoto.transform.localRotation = Quaternion.Euler(180,0,180);
            _spriteRenderer = _createdPhoto.transform.GetChild(0).GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = targetSp;
            _playerMovement.CanTakePhoto = false;
            _playerMovement.TakePhoto();
        }
    }  
}
