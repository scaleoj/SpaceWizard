using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private PlayerInputProvider m_input;
    [SerializeField] private int cameraMaxXrange;
    [SerializeField] private int cameraMaxZRange;

    [SerializeField] private float cameraMovementSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        m_input = GetComponent<PlayerInputProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Mathf.Clamp(m_input.Horizontal() + m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed, 0, Mathf.Clamp(m_input.Horizontal() - m_input.Vertical(), -1.25f, 1.25f) * cameraMovementSpeed);        
        transform.position += direction;
    }
}
