using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBallPrefab;
    public float scrollAmount;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

            worldPoint.z = 0;

            Instantiate(playerBallPrefab, worldPoint, Quaternion.identity, null);
        }

        if(Camera.main.orthographicSize > scrollAmount && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize-=scrollAmount;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize += scrollAmount;
        }
    }
}
