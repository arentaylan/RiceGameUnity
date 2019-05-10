using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = new Vector3(0, 2.0f, 0);
        transform.Rotate(rotation * Time.deltaTime);
    }
}
