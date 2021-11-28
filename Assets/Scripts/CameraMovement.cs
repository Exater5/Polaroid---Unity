using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        Vector3 targetRot = transform.rotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * _rotSpeed;
        transform.Rotate(targetRot - transform.rotation.eulerAngles);
    }
}
