using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostTrial : MonoBehaviour
{
    public Material ghostMaterial ; //to link new GhostMaterial hna
    public float ghostFadeSpeed = 2f; //how fats they disappear
    public Color ghostColor = new Color(1,1,1,0.5f);//semi transparent white i have to see with the team about the colors!!

    public float spawnInterval = 0.0f;//how often to spawn a ghost during the dash 
    private SpriteRenderer sp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        
    }

    public void StartGhostTrial(float duration)
    {
        StartCoroutine(GhostTrialCoroutine(duration));
    }

    IEnumerator GhostTrialCoroutine(float duration)
    {
        float elapssed = 0f;

        while(elapssed < duration)
        {
            //create the ghost
            GameObject ghostObj = new GameObject("GhostSprite");
            ghostObj.transform.position = transform.position;
            ghostObj.transform.rotation = transform.rotation;
            ghostObj.transform.localScale = transform.localScale;

            SpriteRenderer ghostSR = ghostObj.AddComponent<SpriteRenderer>();
            ghostSR.sprite = sp.sprite;
            ghostSR.material = ghostMaterial;
            ghostSR.color = ghostColor;
            ghostSR.sortingLayerID = sp.sortingLayerID;//make sure its on the right layer
            ghostSR.sortingOrder = sp.sortingOrder - 1;//put it just behind sam

            //start fading it
            StartCoroutine(FadeAndDestroy(ghostSR));

            //wait before creating the next ghost
            elapssed += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator FadeAndDestroy (SpriteRenderer ghostSR)
    {
        float alpha = ghostColor.a;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * ghostFadeSpeed;
            ghostSR.color = new Color(ghostColor.r , ghostColor.g  , ghostColor.b , alpha);
            yield return null;

        }
        
        if (ghostSR!= null) Destroy(ghostSR.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
