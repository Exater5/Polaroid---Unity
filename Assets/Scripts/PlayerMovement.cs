using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed, _rotSpeed;
    [SerializeField] private GameObject _camInterface, _aperture;
    [SerializeField] private PostProcessVolume _volume;
    private Vignette _vignetting;
    private Animator _cameraAnims;
    private bool _canTakePhoto, _printing;
    public bool CanTakePhoto
    {
        get { return _canTakePhoto; }
        set { _canTakePhoto = value; }
    }
    private void Start()
    {
        CanTakePhoto = false;
        _volume.profile.TryGetSettings(out _vignetting);
        _cameraAnims = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            h *= 2f;
            v *= 2f;
        }

        Vector3 frontalDir = transform.forward * v * Time.deltaTime * _speed;
        Vector3 lateralDir = transform.right * h * Time.deltaTime * _speed;
        transform.position += frontalDir + lateralDir;

        Vector3 targetRot = transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * _rotSpeed;
        transform.Rotate(targetRot - transform.rotation.eulerAngles);

        if (Input.GetMouseButton(1))
        {
            _cameraAnims.SetBool("TakingPhoto", true);
        }
        else if(_cameraAnims.GetBool("TakingPhoto") && !_printing)
        {
            _cameraAnims.SetBool("TakingPhoto", false);
            CanTakePhoto = false;
            SetCameraState(false);
        }
    }

    public void OnCameraActive()
    {
        SetCameraState(true);
        CanTakePhoto = true;
    }

    public void TakePhoto()
    {
        StartCoroutine(ShowAperture());
    }

    public IEnumerator ShowAperture()
    {
        _printing = true;
        _aperture.SetActive(true);
        SetCameraState(true);
        yield return new WaitForSeconds(0.25f);
        _aperture.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SetCameraState(false);
        _cameraAnims.SetTrigger("TakePhoto");
    }

    public void OnPrint()
    {
        _printing = false;
    }

    public void SetCameraState(bool state)
    {
        _camInterface.SetActive(state);
        _vignetting.enabled.value = state;
    }
}
