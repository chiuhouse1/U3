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
        if (hurt >= 0)
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
        else
        {
            hpbarani.value -= hurt;
            if (hpbar.value < 0)
            {
                hpbar.value = 0;
            }

            float nowhp = hpbar.value;
            float newhp = hpbar.value - hurt;
            if (newhp > hpbar.maxValue)
            {
                newhp = hpbar.maxValue;
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
                hpbar.value = Mathf.Lerp(nowhp, newhp, t);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
