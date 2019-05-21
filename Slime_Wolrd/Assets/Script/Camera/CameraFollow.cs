using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform transformPlayer;
    // Start is called before the first frame update
    void Start()
    {
        transformPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 cameraPositionTemp = transform.position;

        cameraPositionTemp.x = transformPlayer.position.x;
        cameraPositionTemp.y = transformPlayer.position.y;

        transform.position = cameraPositionTemp;
    }
}
