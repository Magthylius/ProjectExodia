using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public void OpenDoor(Test test)
    {
        if (!test.gameObject.HasGameplayTagHandler())
        {
            Debug.Log("GameplayTagHandler Missing");
            return;
        }
        
        if (test.gameObject.HasGameplayTag("RedKey"))
        {
            Debug.Log("Open Door");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Require a Red Key to open");
        }
    }
}
