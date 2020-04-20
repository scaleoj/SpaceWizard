using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UIElements;

public class AlertController : MonoBehaviour, IAtomListener<string>
{
    [SerializeField] private UnityEngine.UI.Image _backgroundImage;
    [SerializeField] private StringEvent alertEvent;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float showDuration;
    [SerializeField] private TextMeshProUGUI _textMesh;
    private bool routineIsRunning;

    
    // Start is called before the first frame update
    void Start()
    {
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

        Color imageColor = _backgroundImage.color;
        Color textColor = _textMesh.color;
        //Enable Alert
        for (float i = 0f; i < 1f; i += Time.deltaTime * transitionSpeed)
        {
            yield return null;
            imageColor.a = i;
            textColor.a = i;
            _textMesh.color = textColor;
            _backgroundImage.color = imageColor;
        }
        
        //Show Alert
        imageColor.a = 1f;
        textColor.a = 1f;
        _textMesh.color = textColor;
        _backgroundImage.color = imageColor;
        yield return new WaitForSeconds(showDuration);
        
        //Disable Alert 
        for (float i = 1f; i > 0f; i -= Time.deltaTime * transitionSpeed)
        {
            yield return null;
            imageColor.a = i;
            textColor.a = i;
            _textMesh.color = textColor;
            _backgroundImage.color = imageColor;
        }
        
        imageColor.a = 0f;
        textColor.a = 0f;
        _textMesh.color = textColor;
        _backgroundImage.color = imageColor;

        routineIsRunning = false;
    }
}
