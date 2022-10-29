#define DEVELOPMENT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class AutoBuild : MonoBehaviour
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string TARGET_DIR = "Build";
    static string APP_NAME = "MonsterHunter";
    static string BUILD_DATE;

#if UNITY_ANDROID
    [MenuItem("Custom/CI/Build Google")]
#endif
    static void PerformGoogleBuild()
    {
        //MarketGoogle();

        string BUILD_TARGET_PATH = TARGET_DIR + "/Android/";

        Directory.CreateDirectory(BUILD_TARGET_PATH);
        PlayerSettings.WSA.packageName = "com.Miho.MonsterHunter";

        PlayerSettings.companyName = "Miho";
        PlayerSettings.productName = "MonsterHunter";
        
        PlayerSettings.Android.keystoreName = Application.dataPath + "/MonsterHunter.keystore";
        PlayerSettings.Android.keystorePass = "nana77";
        PlayerSettings.Android.keyaliasName = "MonsterHunter";
        PlayerSettings.Android.keyaliasPass = "nana77";

        PlayerSettings.bundleVersion = Application.version;

#if RELEASE
        string target_filename = APP_NAME + _Google + "_" + Application.version + ".apk";
#elif DEBUGRELEASE
        string target_filename = APP_NAME + BUILD_DATE + "_" + Application.version + "_Google_D.apk";
#else
        string target_filename = APP_NAME + BUILD_DATE + "_" + Application.version + "_Google_D.apk";
#endif

        GenericBuild(SCENES, BUILD_TARGET_PATH + target_filename,
            BuildTarget.Android, BuildOptions.None);

        Debug.Log("====================== Android Build Success ===============");
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
            {
                continue;
            }

            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_filename,
        BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,
            build_target);

        UnityEditor.Build.Reporting.BuildReport build = BuildPipeline.BuildPlayer(
            scenes, target_filename, build_target, build_options);

        //Debug.Log("result = " build.summary.result);
    }
}
