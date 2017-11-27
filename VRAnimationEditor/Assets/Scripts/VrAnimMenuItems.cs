using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public static class VrAnimMenuItems{
    public static string sceneName = "miles Test Scene 4";
    public static string returnSceneName;

    [MenuItem("Tools/VR Animation Editor")]
	public static void RunVRAnimationEditor()
    {
        returnSceneName = EditorSceneManager.GetActiveScene().name;
        string[] guids = AssetDatabase.FindAssets("\"" + sceneName + "\"");
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        SceneAsset editorScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        EditorSceneManager.playModeStartScene = editorScene;
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

}
