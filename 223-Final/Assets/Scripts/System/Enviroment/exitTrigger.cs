using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitTrigger : MonoBehaviour
{
    [SerializeField] private GameObject exitPanel;
    
    // Start is called before the first frame update
    void Awake()
    {
        Messenger.AddListener(GameEvents.EXIT_OPENED, exitOpen);
    }

    void exitOpen() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
