using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewItem : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Co_FadeOut());
    }

    IEnumerator Co_FadeOut()
    {
        for (float i = 1.25f; i >= 0; i -= Time.deltaTime)
        {
            GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 0, i);
            yield return null;
        }
        Destroy(gameObject);
    }
}
