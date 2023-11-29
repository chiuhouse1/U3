using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public TileBoard tileBoard;

    public IEnumerator HPFlow(Slider hpbar, Slider hpbarani, int hurt)
    {
        tileBoard.waiting = true;

        hpbar.value -= hurt;
        if (hpbar.value < 0)
        {
            hpbar.value = 0;
        }

        float nowhp = hpbarani.value;
        float newhp = hpbarani.value - hurt;
        if (newhp < 0)
        {
            newhp = 0;
        }

        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime;
            if (i >= 1)
            {
                i = 1;
            }
            float t = i / 1;
            hpbarani.value = Mathf.Lerp(nowhp, newhp, t);
            yield return new WaitForFixedUpdate();
        }
    }
}
