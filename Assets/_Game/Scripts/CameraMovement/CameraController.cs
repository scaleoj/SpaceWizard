using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{

    private PlayerInputProvider m_input;
    [SerializeField] private bool trueIsometricCam;
    [SerializeField] private int cameraMaxXrange = 10;
    [SerializeField] private int cameraMaxZRange = 10;
    [SerializeField] private float cameraMaxZoomOut = 10f;
    [SerializeField] private float cameraMaxZoomIn = 3.5f;
    [SerializeField] private float zoomSpeed = 0.5f;

    [SerializeField] private float cameraMovementSpeed;

    private Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        m_input = GetComponent<PlayerInputProvider>();
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
       cam.orthographic = trueIsometricCam;
        if (trueIsometricCam)
        {
            //MOVEMENT
            Vector3 direction = new Vector3(Mathf.Clamp(m_input.Horizontal() + m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed, 0f , Mathf.Clamp(m_input.Horizontal() - m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed); 
            transform.position += direction;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -cameraMaxXrange, cameraMaxXrange), 12f,Mathf.Clamp(transform.position.z, -cameraMaxZRange,cameraMaxZRange));
            //ZOOM
            cam.orthographicSize += -m_input.scrollWheelDelta() * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, cameraMaxZoomIn, cameraMaxZoomOut);
            //ROTATION
            if (m_input.RotateRight())
            {
                //transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
                //transform.Rotate(new Vector3(1f,90f,1f));
            }

            if (m_input.RotateLeft())
            {
                //transform.Rotate(new Vector3(1f,90f,1f));
            }

        }
        else
        {
            Vector3 direction = new Vector3(Mathf.Clamp(m_input.Horizontal() + m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed, 0f , Mathf.Clamp(m_input.Horizontal() - m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed); 
            Vector3 zoom = new Vector3( m_input.scrollWheelDelta() * zoomSpeed, -m_input.scrollWheelDelta() * zoomSpeed , m_input.scrollWheelDelta() * zoomSpeed);
            transform.position += direction;
            if (transform.position.y <= cameraMaxZoomIn || transform.position.y >= cameraMaxZoomOut)
            {
                zoom.x = 0.0f;
                zoom.z = 0.0f;
            }
            transform.position += zoom;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -cameraMaxXrange, cameraMaxXrange), Mathf.Clamp(transform.position.y, cameraMaxZoomIn, cameraMaxZoomOut) ,Mathf.Clamp(transform.position.z, -cameraMaxZRange,cameraMaxZRange));
            
            if (m_input.RotateRight())
            {
                //transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.y + 90.0f, new Vector3(transform.eulerAngles.x, 1,0));
                //transform.Rotate(new Vector3(1f,90f,1f));
            }

            if (m_input.RotateLeft())
            {
                //transform.Rotate(new Vector3(1f,90f,1f));
            }
        }
    }
}
