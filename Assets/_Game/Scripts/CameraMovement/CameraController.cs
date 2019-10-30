using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{

    private PlayerInputProvider m_input;
    [SerializeField] private int cameraMaxXrange = 10;
    [SerializeField] private int cameraMaxZRange = 10;
    [SerializeField] private float cameraMaxZoomOut = 10f;
    [SerializeField] private float cameraMaxZoomIn = 3.5f;
    [SerializeField] private float zoomSpeed = 0.5f;

    [SerializeField] private float cameraMovementSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        m_input = GetComponent<PlayerInputProvider>();
    }

    // Update is called once per frame
    void Update()
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
    }
}
