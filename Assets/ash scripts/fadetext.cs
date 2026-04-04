using UnityEngine;
using TMPro;
using DG.Tweening;

public class fadetext : MonoBehaviour
{
    public float displayTime = 5f;
    public float fadeDuration = 1.5f;

    void Start()
    {
        GetComponent<TMP_Text>().DOFade(0, fadeDuration).SetDelay(displayTime);
    }
}