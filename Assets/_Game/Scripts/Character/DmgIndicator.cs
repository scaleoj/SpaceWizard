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

    public void showHitOrMiss(bool hit)
    {
        hitText.gameObject.SetActive(true);
        if (hit)
        {
            hitText.text = "HIT";
        }
        else
        {
            hitText.text = "MISS";
        }

        StartCoroutine(HitRoutine());
        StartCoroutine(HitCharHighlightRoutine(hit));
    }

    public IEnumerator DamageRoutine()
    {
        yield return new WaitForSeconds(2);
        healthText.gameObject.SetActive(false);
        magicText.gameObject.SetActive(false);
        physText.gameObject.SetActive(false);
    }

    public IEnumerator HitRoutine()
    {
        //Text
        yield return new WaitForSeconds(2.75f);
        hitText.gameObject.SetActive(false);
    }

    public IEnumerator HitCharHighlightRoutine(bool hit)
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

            bool isRed = false;
            //Dmg highlight
            for (float i = 0; i <= duration; i += waitTime)
            {
                yield return new WaitForSeconds(waitTime);
                if (isRed)
                {
                    copyMat.color = Color.white;
                    charTransform.position = new Vector3(charTransform.position.x + posOffset, charTransform.position.y, charTransform.position.z - posOffset);
                    isRed = false;
                }
                else
                {
                    copyMat.color = Color.red;
                    charTransform.position = new Vector3(charTransform.position.x - posOffset, charTransform.position.y, charTransform.position.z + posOffset);
                    isRed = true;
                }
            }


            
            //Reset stuff
            meshRend.material = oldMat;
            charTransform.position = oldPos;
        }
    }
}
