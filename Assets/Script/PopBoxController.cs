using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopBoxController : MonoBehaviour
{
    public TileBoard board;
    List<GameObject> objects = new List<GameObject>();
    Coroutine c = null;

    public void showPop(GameObject g)
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
        StartCoroutine(Pop(g, Vector2.one, 1));
    }

    public void hidePop(GameObject g)
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
        StartCoroutine(Pop(g, Vector2.zero, 1));
    }

    IEnumerator Pop(GameObject g, Vector2 v2, float time)
    {
        float i = 0;
        while (i < time)
        {
            i += Time.deltaTime;
            g.transform.localScale = Vector2.Lerp(g.transform.localScale, v2, i);
            yield return new WaitForFixedUpdate();
        }
        c = null;
    }

    public void showPop_Transparency(GameObject g)
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
        StartCoroutine(Pop_Transparency(g, Color.white, 2));
    }

    public void hidePop_Transparency(GameObject g)
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
        StartCoroutine(Pop_Transparency(g, Color.clear, 2));
    }

    IEnumerator Pop_Transparency(GameObject parent, Color targetColor, float time)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.GetComponent<Image>() != null)
            {
                objects.Add(parent.transform.GetChild(i).gameObject);
            }
        }
        if (targetColor.a != 0)
        {
            parent.SetActive(true);
        }

        float j = 0;
        while (j < time)
        {
            float t = j / time;
            parent.GetComponent<Image>().color = Color.Lerp(parent.GetComponent<Image>().color, targetColor, t);
            for (int k = 0; k < objects.Count; k++)
            {
                objects[k].GetComponent<Image>().color = parent.GetComponent<Image>().color;
            }
            j += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (targetColor.a == 0)
        {
            parent.SetActive(false);
            board.waiting = false;
        }
        c = null;
    }
}
