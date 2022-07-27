// https://forum.unity.com/threads/feature-request-refresh-on-play.706952/
// "Just drop this file as PlayRefreshEditor.cs in a top-level Editor folder and turn off Autorefresh in Edit->Preferences."
// "Your code and assets will be refreshed/recompiled after you press the Play button, instead of whenever you change focus to the Unity window"


using UnityEditor;
 
 
[InitializeOnLoadAttribute]
public static class PlayRefreshEditor 
{

    static PlayRefreshEditor() 
    {
        EditorApplication.playModeStateChanged += PlayRefresh;
    }

    private static void PlayRefresh(PlayModeStateChange state) 
    {
        if(state == PlayModeStateChange.ExitingEditMode) {
            AssetDatabase.Refresh();
        }
    }
}