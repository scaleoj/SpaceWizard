using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;

public class AlertController : MonoBehaviour, IAtomListener<string>
{
    [SerializeField] private StringEvent alertEvent;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float showDuration;
    [SerializeField] private Vector3 endPosOffset;
    [SerializeField] private RectTransform _textTransform;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private RectTransform _backgroundTransform;
    private bool routineIsRunning;

    private Vector3 originalPos;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = _textTransform.position;
        alertEvent.RegisterListener(this);
    }


    public void OnEventRaised(string item)
    {
        if (!routineIsRunning) StartCoroutine(showAlert(item));
    }

    private IEnumerator showAlert(string item)
    {
        routineIsRunning = true;
        _textMesh.text = item;
        
        Vector2 startPos = _textTransform.position;
        Vector2 endPos = new Vector2( _textTransform.position.x + endPosOffset.x * Screen.width, _textTransform.position.y + endPosOffset.y * Screen.width);
        
        //Move Alert down
        for (float i = 0f; i < 1f; i += Time.deltaTime * transitionSpeed)
        {
            yield return null;
            Vector2 lerpedVector = Vector2.Lerp(startPos, endPos, i);
            _textTransform.position = lerpedVector;
            _backgroundTransform.position = lerpedVector;
        }
        
        //Show Alert
        yield return new WaitForSeconds(showDuration);
        
        //Move Alert Up
        for (float i = 0f; i < 1f; i += Time.deltaTime * transitionSpeed)
        {
            yield return null;
            Vector2 lerpedVector = Vector2.Lerp(endPos, startPos, i);
            _textTransform.position = lerpedVector;
            _backgroundTransform.position = lerpedVector;
        }
        
        yield return null;
        
        Debug.Log(originalPos);
        
        _textTransform.position = originalPos;
        _backgroundTransform.position = originalPos;

        routineIsRunning = false;
    }
}
