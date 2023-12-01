using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour
{
    bool CanUseSkill = false;

    public void OnClickDown()
    {
        CanUseSkill = true;
        StartCoroutine(Interval());
        Debug.Log("AAA");
    }

    IEnumerator Interval()
    {
        yield return new WaitForSeconds(0.2f);
        CanUseSkill = false;
    }

    public void OnClickUp()
    {
        if (CanUseSkill)
        {
            StopCoroutine(Interval());
            CanUseSkill = false;
            Debug.Log("go");
        }
        Debug.Log("too long");
    }
}
