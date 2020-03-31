using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DynamicDOF : MonoBehaviour
{
    
    //THIS CLASS MIGHT NEED HEAVY OPTIMISATION!
    
    private Camera cam;
    [SerializeField] private PlayerInputProvider m_input;
    private float distance;
    private DepthOfField dof;

    void Awake()
    {
        cam = GetComponent<Camera>();
        GetComponent<PostProcessVolume>().profile.TryGetSettings(out dof);
    }

    void Update()
    {
        RaycastHit hit;
        Ray camray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(camray.origin, camray.direction, out hit, 15f))
        {
            //distance = Vector3.Distance(transform.position, hit.point);
            dof.focusDistance.value = Vector3.Distance(transform.position, hit.point);;
        }
    }
}
