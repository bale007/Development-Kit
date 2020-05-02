using UnityEditor;
using UnityEngine;

namespace Bale007.Editor
{
    public static class GoogleSheetEditor
    {
        [MenuItem("Google Sheet/Pull Data")]
        public static void SetPullData()
        {
            PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 1);

            Debug.Log("Set Pull Data");
        }
    }
}