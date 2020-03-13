using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{

    private PlayerInputProvider m_input;
    [SerializeField] private bool trueIsometricCam;
    [SerializeField] private bool doZoomLerp;
    [FormerlySerializedAs("cameraMaxXrange")] [SerializeField] private int cameraMaxPosXrange = 10;
    [SerializeField] private int cameraMaxNegXrange = 10;
    [FormerlySerializedAs("cameraMaxZRange")] [SerializeField] private int cameraMaxPosZRange = 10;
    [SerializeField] private int cameraMaxNegZrange = 10;
    [SerializeField] private float cameraMaxZoomOut = 10f;
    [SerializeField] private float cameraMaxZoomIn = 3.5f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float zoomLerpSpeed = 1f;

    [SerializeField] private float cameraMovementSpeed;
    
    

    private Camera cam;

    //private float zoomLerpTime;
    // Start is called before the first frame update
    void Awake()
    {
        m_input = GetComponent<PlayerInputProvider>();
        cam = gameObject.GetComponent<Camera>();
    }

    public IEnumerator CameraInterpolationRotator(Quaternion start, Quaternion end)
    {
        yield return null;
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
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -cameraMaxPosXrange, cameraMaxPosXrange), 12f,Mathf.Clamp(transform.position.z, -cameraMaxPosZRange,cameraMaxPosZRange));
            //ZOOM
            cam.orthographicSize += -m_input.scrollWheelDelta() * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, cameraMaxZoomIn, cameraMaxZoomOut);
            //ROTATION
            if (m_input.RotateRight())
            {
                //transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
                //transform.Rotate(new Vector3(1f,90f,1f));
                transform.Rotate(new Vector3(0f,90f,0f),Space.World);
                //transform.rotation  = new Quaternion(transform.rotation.eulerAngles.x + 90f, transform.rotation.eulerAngles.y + 90f,transform.rotation.eulerAngles.z + 90f,transform.rotation.w); 
            }

            if (m_input.RotateLeft())
            {
                //transform.Rotate(new Vector3(1f,90f,1f));
            }

        }
        else
        {
            Vector3 direction = new Vector3(Mathf.Clamp(m_input.Horizontal() + m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed, 0f , Mathf.Clamp(m_input.Horizontal() - m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed); 
            Vector3 zoom = new Vector3( m_input.scrollWheelDelta() * zoomSpeed, -m_input.scrollWheelDelta() * zoomSpeed, m_input.scrollWheelDelta() * zoomSpeed);
            transform.position += direction * Time.deltaTime;
            if (doZoomLerp)
            {
                if (transform.position.y <= cameraMaxZoomIn && zoom.y < 0f) //If max zoomed in, "disable" zoom in
                {
                    zoom.x = 0.0f;
                    zoom.z = 0.0f;
                    zoom.y = 0f;
                }
                if ( transform.position.y >= cameraMaxZoomOut && zoom.y > 0f) //If max zoomed out, "disable" zoom out
                {
                    zoom.x = 0.0f;
                    zoom.z = 0.0f;
                    zoom.y = 0f;
                }

                if (zoom != Vector3.zero)
                {
                    StartCoroutine(zoomLerp(zoom * Time.deltaTime));
                }
            }
            else
            {
                if (transform.position.y <= cameraMaxZoomIn ||transform.position.y >= cameraMaxZoomOut)
                {
                    zoom.x = 0.0f;
                    zoom.z = 0.0f;
                }

                transform.position += zoom * Time.deltaTime;
            }
            
            //transform.position = Vector3.Lerp(,zoom);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraMaxNegXrange, cameraMaxPosXrange), Mathf.Clamp(transform.position.y, cameraMaxZoomIn, cameraMaxZoomOut) ,Mathf.Clamp(transform.position.z, cameraMaxNegZrange,cameraMaxPosZRange));
            
            if (m_input.RotateRight())
            {
                //transform.Rotate(new Vector3(0f,90f,0f),Space.World); //Works but needs to be tweaked for usage

            }

            if (m_input.RotateLeft())
            {
                //transform.Rotate(new Vector3(1f,90f,1f));
            }
        }
       
       //CLIPPING
       
    }

    private IEnumerator zoomLerp(Vector3 zoom)
    {
        Vector3 zoomCopy = new Vector3(zoom.x,zoom.y,zoom.z);
        Vector3 oldPos = transform.position;
        float curTime = 0f;
        while (curTime <= 1.0f)
        {
            Debug.Log(curTime);
            transform.position = Vector3.Lerp(oldPos ,oldPos + zoomCopy, curTime += (Time.deltaTime * zoomLerpSpeed) );
            yield return null;
        }
    }
}
