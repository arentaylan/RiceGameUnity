using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Environment.CurrentEnvironment = "Field";
        Debug.Log(Environment.CurrentEnvironment);
        SceneManager.LoadScene("Battle");
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
