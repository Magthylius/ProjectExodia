using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameplayTagHandler : MonoBehaviour
{
    private static Dictionary<GameObject, GameplayTagHandler> gameplayTagHandlerDict = new();

    [SerializeField] List<GameplayTag> gameplayTags = new();
    public List<GameplayTag> GetGameplayTags => gameplayTags;

    private void Awake()
    {
        gameplayTagHandlerDict.Add(gameObject, this);
    }

    public static bool HasGameplayTagHandler(GameObject gameObject) => gameplayTagHandlerDict.ContainsKey(gameObject);

    public static bool GetTagHandler(GameObject gameObject, out GameplayTagHandler handler) =>
        gameplayTagHandlerDict.TryGetValue(gameObject, out handler);

    public bool HasGameplayTag(GameplayTag tag) => gameplayTags.Contains(tag);
    public bool HasGameplayTag(string tagString) => gameplayTags.Exists(t => t.name == tagString);

    public bool HasAnyGameplayTag(IEnumerable<GameplayTag> gameplayTagList) => gameplayTagList.Any(HasGameplayTag);
    public bool HasAllGameplayTag(IEnumerable<GameplayTag> gameplayTagList) => gameplayTagList.All(HasGameplayTag);
    public bool NoGameplayTag(IEnumerable<GameplayTag> gameplayTagList) => !gameplayTagList.Any(HasGameplayTag);

    public bool HasAnyGameplayTag(IEnumerable<string> gameplayTagList) => gameplayTagList.Any(HasGameplayTag);
    public bool HasAllGameplayTag(IEnumerable<string> gameplayTagList) => gameplayTagList.All(HasGameplayTag);
    public bool NoGameplayTag(IEnumerable<string> gameplayTagList) => !gameplayTagList.Any(HasGameplayTag);

    public void GrantTag(GameplayTag gameplayTag)
    {
        if (HasGameplayTag(gameplayTag))
        {
            Debug.LogWarning(gameplayTag.name + "already exist!");
            return;
        }

        gameplayTags.Add(gameplayTag);
        //gameplayTags = gameplayTags.ToHashSet().ToList();
    }

    public void RemoveTag(GameplayTag gameplayTag)
    {
        gameplayTags.Remove(gameplayTag);
    }

    public void RemoveTag(string tagString)
    {
        GameplayTag tagToBeRemoved = gameplayTags.Find(t => t.name == tagString);

        if (tagToBeRemoved == null)
        {
            Debug.Log("Tag does not exist or invalid name!");
            return;
        }

        gameplayTags.Remove(tagToBeRemoved);
    }

    public void RemoveAllTags()
    {
        foreach (GameplayTag tag in gameplayTags)
        {
            RemoveTag(tag);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameplayTagHandler))]
class GameplayTagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif