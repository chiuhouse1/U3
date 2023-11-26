using System.Collections;
using UnityEngine;
using TMPro;

public class Flashing : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public float FlashingFrequency = 1;
    bool wait = false;

    void Update()
    {
        if (!wait)
        {
            wait = true;
            StartCoroutine(flashing());
        }
    }

    IEnumerator flashing()
    {
        if (RoundText.color != Color.white)
        {
            float i = 0;
            while (i < FlashingFrequency)
            {
                i += Time.deltaTime;
                if (i >= FlashingFrequency)
                {
                    i = FlashingFrequency;
                }
                float t = i / FlashingFrequency;
                RoundText.color = Color.Lerp(Color.red, Color.white, t);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            float i = 0;
            while (i < FlashingFrequency)
            {
                i += Time.deltaTime;
                if (i >= FlashingFrequency)
                {
                    i = FlashingFrequency;
                }
                float t = i / FlashingFrequency;
                RoundText.color = Color.Lerp(Color.white, Color.red, t);
                yield return new WaitForFixedUpdate();
            }
        }
        wait = false;
    }
}
