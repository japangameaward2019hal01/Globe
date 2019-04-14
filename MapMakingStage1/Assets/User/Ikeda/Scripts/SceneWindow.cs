﻿using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneWindow : EditorWindow
{
    static List<SceneAsset> m_SceneAssets = new List<SceneAsset>();

    private const string TEMPLATE =
@"//This file is generated by SceneWindow.cs
using UnityEngine;
using System.Collections;

public static class SceneIndex
{{
    //This string will be registered by pushing [Apply To Build Settings] button
    {0}

    //enum ENUM_SCENE
    public enum ENUM_SCENE
    {{ {1}
    }};

    public static string[] Path_Index = 
    {{ {2}
    }};
}}
";

    private const string NAME_STRING =
@"
    public static string {0} = ""{1}"";"
;
    private const string INDEX_STATE =
@"
        {0},"
;

    [MenuItem("User/Ikeda/Window/SceneWindow")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SceneWindow));
    }

    private void Awake()
    {
        //EditorBuildSettingsに設定してあるSceneを読み込む
        foreach (var scene in EditorBuildSettings.scenes)
        {
            m_SceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Scenes to include in build:", EditorStyles.boldLabel);

        for (int i = 0; i < m_SceneAssets.Count; ++i)
        {
            m_SceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(m_SceneAssets[i], typeof(SceneAsset), false);
        }

        if (GUILayout.Button("Add")) m_SceneAssets.Add(null);

        GUILayout.Space(8);

        if (GUILayout.Button("Apply To Build Settings")) SetEditorBuildSettingsScenes();

        GUILayout.Space(8);

        if (GUILayout.Button("ReLoad", GUILayout.Width(200f))) ReLoad();
        if (GUILayout.Button("Clear", GUILayout.Width(200f))) m_SceneAssets.Clear();
    }

    public void SetEditorBuildSettingsScenes()
    {
        string NameIndex = "";
        string EnumIndex = "";
        string PathIndex = "";

        // Find valid Scene paths and make a list of EditorBuildSettingsScene
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        foreach (var sceneAsset in m_SceneAssets)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));

                string name = sceneAsset.name.Replace(" ", "_");

                NameIndex += string.Format(NAME_STRING,name, scenePath);
                EnumIndex += string.Format(INDEX_STATE, name);
                PathIndex += string.Format(INDEX_STATE, name);
            }
        }

        var Result = string.Format(TEMPLATE,NameIndex,EnumIndex,PathIndex);

        //保存先の設定
        var SceneWindowResult = Application.dataPath + "/Scritps/SceneIndex.cs";
        var sr = new StreamWriter(SceneWindowResult);
        sr.Write(Result);
        sr.Close();

        Debug.Log("Success!!:Path > " + SceneWindowResult);

        AssetDatabase.ImportAsset(SceneWindowResult,ImportAssetOptions.ForceUpdate);

        // Set the Build Settings window Scene list
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }

    public void ReLoad()
    {
        m_SceneAssets.Clear();

        //EditorBuildSettingsに設定してあるSceneを読み込む
        foreach (var scene in EditorBuildSettings.scenes)
        {
            m_SceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
        }
    }
}