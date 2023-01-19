using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterUI : MonoBehaviour
{

    public GameObject instanceToSpawn;
    public float timeToSpawn = 1f;
    public float padding = 0.2f;


    private float passedTime = 0f;
    private int counter = 0;
    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= timeToSpawn)
        {
            SpawnNewInstance();
            counter++;
            passedTime = 0f;
        }
    }

    void SpawnNewInstance()
    {
        GameObject gO = Instantiate(instanceToSpawn, gameObject.transform);

        gO.GetComponent<UnityEngine.UI.Text>().text = (counter + 1).ToString();

        Vector3 vec = gO.transform.position;
        vec.y += padding * counter * -1;
        gO.transform.position = vec;
    }
}

/*
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

*/