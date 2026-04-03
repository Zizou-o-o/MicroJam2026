using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fadeimage : MonoBehaviour
{
    public float displayTime = 5f;
    public float fadeDuration = 1.5f;

    void Start()
    {
        GetComponent<Image>().DOFade(0, fadeDuration).SetDelay(displayTime).OnComplete(() =>
        {
            Gamemanager.Instance?.StartGame();
        });
    }
}