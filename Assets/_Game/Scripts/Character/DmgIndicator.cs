using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshPro physText;
    [SerializeField] private TextMeshPro magicText;
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private TextMeshPro hitText;
    [SerializeField] private TextMeshPro effectText;

    public void showDamage(int phys, int magic, int health)
    {
        if (phys != 0)
        {
            physText.text = phys.ToString(); 
            physText.gameObject.SetActive(true);
        }

        if (magic != 0)
        {
            magicText.text = magic.ToString();
            magicText.gameObject.SetActive(true);
        }

        if (health != 0)
        {
            healthText.text = health.ToString();
            healthText.gameObject.SetActive(true);
        }
        StartCoroutine(DamageRoutine());
    }

    public void showHitOrMiss(bool hit, Color dmgCol, string effectString, Color effectStringCol) //If its not an effect just set the effectName to an empty string
    {
        hitText.gameObject.SetActive(true);
        effectText.gameObject.SetActive(true);
        effectText.color = effectStringCol;
        if (hit)
        {
            hitText.text = "HIT";
            effectText.text = effectString;
        }
        else
        {
            hitText.text = "MISS";
            effectText.text = String.Empty;
        }

        StartCoroutine(HitRoutine());
        StartCoroutine(HitCharHighlightRoutine(hit,dmgCol));
    }

    public IEnumerator DamageRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        healthText.gameObject.SetActive(false);
        magicText.gameObject.SetActive(false);
        physText.gameObject.SetActive(false);
    }

    public IEnumerator HitRoutine()
    {
        //Text
        yield return new WaitForSeconds(2.5f);
        hitText.gameObject.SetActive(false);
        effectText.gameObject.SetActive(false);
    }

    public IEnumerator HitCharHighlightRoutine(bool hit, Color dmgCol)
    {
        //DMG Flash
        if (hit)
        {
            //Init
            MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
            Material oldMat = meshRend.material;
            Material copyMat = new Material(meshRend.material);
            meshRend.material = copyMat;
            Transform charTransform = transform;
            Vector3 oldPos = new Vector3(charTransform.position.x, charTransform.position.y, charTransform.position.z);

            float posOffset = 0.05f;
            float waitTime = 0.075f;
            float duration = 0.5f;

            bool isHighlighted = false;
            //Dmg highlight
            for (float i = 0; i <= duration; i += waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                if (isHighlighted)
                {
                    copyMat.color = Color.white;
                    charTransform.position = new Vector3(charTransform.position.x + posOffset, charTransform.position.y, charTransform.position.z - posOffset);
                    isHighlighted = false;
                }
                else
                {
                    copyMat.color = dmgCol;
                    charTransform.position = new Vector3(charTransform.position.x - posOffset, charTransform.position.y, charTransform.position.z + posOffset);
                    isHighlighted = true;
                }
            }


            
            //Reset stuff
            meshRend.material = oldMat;
            charTransform.position = oldPos;
        }
    }
}
