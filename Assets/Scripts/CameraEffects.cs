using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
   public static CameraEffects instance ;

   void Awake()
    {
        instance = this;
    }

    public IEnumerator Shake(float duration , float magnitude)
    {
        Vector3 OriginalPos = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f,1f)*magnitude;
            float y = Random.Range(-1f,1f)*magnitude;
            
            transform.position = new Vector3(OriginalPos.x+x,OriginalPos.y+y,OriginalPos.z);
            elapsed += Time.deltaTime;
            yield return null;


        }
        transform.position = OriginalPos;


    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }


    // Update is called once per frame
    void Update()
    {
        
        
    }

}
