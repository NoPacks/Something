using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    public float CameraSpeed;
    private float ejeX, ejeY;
    public float ClampAngle;
    public Transform Tarjet, playerPivot;

    [Header("LineCast")]
    public float Max;
    public float Min;
    private Vector3 Dir;

    [Header("Adjust")]
    public float smoth;
    public float Distance;

    // Start is called before the first frame update
    private void Awake()
    {
        Dir = transform.localPosition.normalized;
        Distance = transform.localPosition.magnitude;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 DesiredCameraPos = transform.parent.TransformPoint(Dir * Max);
        RaycastHit hit;

        
        if (Physics.Linecast(transform.parent.position, DesiredCameraPos, out hit))
        {
            if (hit.collider.tag != "Enemy")
            {
                Distance = Mathf.Clamp((hit.distance * 0.8f), Min, Max);
            }
        }
        else
        {
            Distance = Max;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Dir * Distance, Time.deltaTime * smoth);
    }

    void LateUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        ejeX += Input.GetAxis("Mouse X") * CameraSpeed;
        ejeY -= Input.GetAxis("Mouse Y") * CameraSpeed;

        ejeY = Mathf.Clamp(ejeY, -ClampAngle, ClampAngle);

        playerPivot.rotation = Quaternion.Euler(0, ejeX, 0);
        Tarjet.rotation = Quaternion.Euler(ejeY, ejeX, 0);

    }
}
