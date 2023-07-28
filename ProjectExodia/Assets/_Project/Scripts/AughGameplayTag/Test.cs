using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject ObjExample1;
    public GameObject Obj2Example1;
    public GameplayTag GameplayTagExample1;
    public GameplayTag GameplayTag2Example1;
    public GameplayTag GameplayTag3Example1;

    public GameObject DoorExample2;
    public GameplayTag RedKey;

    private GameplayTagHandler tagHandler;
    
    void Start()
    {
        tagHandler = GetComponent<GameplayTagHandler>();
        
        bool example1Bool = ObjExample1.HasGameplayTag("TagExample");
        bool example1Bool2 = ObjExample1.HasGameplayTag(GameplayTagExample1);
        bool example1Bool3 = ObjExample1.HasAllGameplayTag("TagExample", "TagExample2", "TagExample3");
        bool example1Bool4 = Obj2Example1.HasAnyGameplayTag(GameplayTag2Example1, GameplayTag3Example1);
        bool example1Bool5 = Obj2Example1.NoGameplayTag("TagExample");
        
        Debug.Log("example1Bool: " + example1Bool);
        Debug.Log("example1Bool2: " + example1Bool2);
        Debug.Log("example1Bool3: " + example1Bool3);
        Debug.Log("example1Bool4: " + example1Bool4);
        Debug.Log("example1Bool5: " + example1Bool5);
        
    }

    public void GrantSelfRedkey()
    {
        tagHandler.GrantTag(RedKey);
    }
}
