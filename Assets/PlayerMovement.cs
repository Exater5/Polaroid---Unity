using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed, _rotSpeed;
    private Animator _cameraAnims;
    private bool _canTakePhoto;
    public bool CanTakePhoto
    {
        get { return _canTakePhoto; }
        set 
        { 
            _canTakePhoto = value;
            if (!value)
            {

            }
        }
    }
    private void Start()
    {
        _cameraAnims = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 frontalDir = transform.forward * v * Time.deltaTime * _speed;
        Vector3 lateralDir = transform.right * h * Time.deltaTime * _speed;
        transform.position += frontalDir + lateralDir;

        Vector3 targetRot = transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * _rotSpeed;
        transform.Rotate(targetRot - transform.rotation.eulerAngles);

        if (Input.GetMouseButton(1))
        {
            _cameraAnims.SetBool("TakingPhoto", true);
        }
        else if(_cameraAnims.GetBool("TakingPhoto"))
        {
            _cameraAnims.SetBool("TakingPhoto", false);
        }
    }
}
