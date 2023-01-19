using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickUI : MonoBehaviour
{

    public GameObject UI;
    private bool IsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deb()
    {
        if (!IsEnabled)
        {
            UI.SetActive(true);
            IsEnabled = true;
        }
        else
        {
            UI.SetActive(false);
            IsEnabled = false;
        }
    }
}