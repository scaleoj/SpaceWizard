using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObject statTexts;

    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    private TextUpdater textUpdater;
    // Start is called before the first frame update
    void Start()
    {
        currentGameObjectChanged.RegisterListener(this);
        textUpdater = statTexts.GetComponent<TextUpdater>();
    }


    public void OnEventRaised(GameObject item)
    {
        if (item.GetComponent<Character>() != null)
        {
            statTexts.SetActive(true);
            textUpdater.UpdateText();
        }
        else
        {
            statTexts.SetActive(false);
        }
    }
}
