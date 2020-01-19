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
        yield return new WaitForSeconds(2);
        hitText.gameObject.SetActive(false);
    }
}
