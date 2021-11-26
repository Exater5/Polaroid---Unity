using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * 20;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * 20;

        transform.Rotate(-v, h, 0);
    }
}
