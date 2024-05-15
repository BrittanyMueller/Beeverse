using UnityEditor;
using UnityEditor.Compilation;
class WebGLBuilder {
    static void build() {

        // Place all your scenes here
        string[] scenes = {"Assets/scenes/MainMenu.unity", 
                            "Assets/scenes/Beeverse.unity"};

        string pathToDeploy = "webgl_build/Beeverse/";       
        EditorUserBuildSettings.SetPlatformSettings(BuildPipeline.GetBuildTargetName(BuildTarget.WebGL), "CodeOptimization", "disksizelto");
        BuildPipeline.BuildPlayer(scenes, pathToDeploy, BuildTarget.WebGL, BuildOptions.None);    
    }
}