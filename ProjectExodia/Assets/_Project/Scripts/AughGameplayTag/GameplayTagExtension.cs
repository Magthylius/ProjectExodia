using System.Collections.Generic;
using UnityEngine;

public static class GameplayTagExtension
{
    public static bool HasGameplayTagHandler(this GameObject gameObject) =>
        GameplayTagHandler.HasGameplayTagHandler(gameObject);

    public static bool HasGameplayTag(this GameObject gameObject, GameplayTag gameplayTag) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) && handler.HasGameplayTag(gameplayTag);

    public static bool HasGameplayTag(this GameObject gameObject, string gameplayTagString) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasGameplayTag(gameplayTagString);

    #region Asset parser vesion

    public static bool HasAnyGameplayTag(this GameObject gameObject, IEnumerable<GameplayTag> gameplayTagList) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAnyGameplayTag(gameplayTagList);

    public static bool HasAnyGameplayTag(this GameObject gameObject, params GameplayTag[] gameplayTagParams) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAnyGameplayTag(gameplayTagParams);

    public static bool HasAllGameplayTag(this GameObject gameObject, IEnumerable<GameplayTag> gameplayTagList) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAllGameplayTag(gameplayTagList);

    public static bool HasAllGameplayTag(this GameObject gameObject, params GameplayTag[] gameplayTagParams) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAllGameplayTag(gameplayTagParams);

    public static bool NoGameplayTag(this GameObject gameObject, IEnumerable<GameplayTag> gameplayTagList) =>
        !GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) ||
        handler.NoGameplayTag(gameplayTagList);

    public static bool NoGameplayTag(this GameObject gameObject, params GameplayTag[] gameplayTagParams) =>
        !GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) ||
        handler.NoGameplayTag(gameplayTagParams);

    #endregion

    #region String parser version

    public static bool HasAnyGameplayTag(this GameObject gameObject, IEnumerable<string> gameplayTagStringList) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAnyGameplayTag(gameplayTagStringList);

    public static bool HasAnyGameplayTag(this GameObject gameObject, params string[] gameplayTagStringParams) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAnyGameplayTag(gameplayTagStringParams);

    public static bool HasAllGameplayTag(this GameObject gameObject, IEnumerable<string> gameplayTagStringList) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAllGameplayTag(gameplayTagStringList);
    
    public static bool HasAllGameplayTag(this GameObject gameObject, params string[] gameplayTagStringParams) =>
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) &&
        handler.HasAllGameplayTag(gameplayTagStringParams);
    
    public static bool NoGameplayTag(this GameObject gameObject, IEnumerable<string> gameplayTagStringList) => 
        !GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) ||
        handler.NoGameplayTag(gameplayTagStringList);
    
    public static bool NoGameplayTag(this GameObject gameObject, params string[] gameplayTagStringList) => 
        !GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler) ||
        handler.NoGameplayTag(gameplayTagStringList);

    #endregion

    public static void GrantTag(this GameObject gameObject, GameplayTag gameplayTag)
    {
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler);
        handler.GrantTag(gameplayTag);
    }
    
    public static void RemoveTag(this GameObject gameObject, GameplayTag gameplayTag)
    {
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler);
        handler.GrantTag(gameplayTag);
    }

    public static void RemoveTag(this GameObject gameObject, string gameplayTagString)
    {
        GameplayTagHandler.GetTagHandler(gameObject, out GameplayTagHandler handler);
        handler.RemoveTag(gameplayTagString);
    }
        

}