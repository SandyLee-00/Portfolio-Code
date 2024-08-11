using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using Unity.EditorCoroutines.Editor;

public class ToolsSheetDownloader : EditorWindow
{
    const string _address = "1xl9jddCO_E7jJRnTK6p5W1e1Pan1KHGriuUBt3L8NtQ";
    const string _sheetPath = "Assets/Resources/CSV";
    const string _format = "csv";

    struct SheetInfo
    {
        public string Name;
        public string Id;
    }

    SheetInfo[] sheets = new SheetInfo[]
    {
        new SheetInfo { Name = "DungeonEggProbabilityData", Id = "995584907" },
        new SheetInfo { Name = "DungeonInfoData", Id = "826857452" },
        new SheetInfo { Name = "DungeonMonsterProbabilityData", Id = "240791968" },
        new SheetInfo { Name = "EggInfoData", Id = "841109990" },
        new SheetInfo { Name = "ItemInfoData", Id = "882486295" },
        new SheetInfo { Name = "MonsterBaseStatData", Id = "1426118155" },
        new SheetInfo { Name = "MonsterDropItemData", Id = "1627816704" },
        new SheetInfo { Name = "MonsterInfoData", Id = "722984681" },
        new SheetInfo { Name = "MonsterIVStatData", Id = "41174343" },
        new SheetInfo { Name = "MonsterLevelStatData", Id = "2038452145" },
        new SheetInfo { Name = "MonsterLevelUpData", Id = "2079524054" },
        new SheetInfo { Name = "MonsterRankStatData", Id = "802174856" },
        new SheetInfo { Name = "MonsterSkillInfoData", Id = "1203934138" },
        new SheetInfo { Name = "PlayerBaseStatData", Id = "1816530482" },
        new SheetInfo { Name = "PlayerInfoData", Id = "887560366" },
        new SheetInfo { Name = "PlayerLevelStatData", Id = "1577930560" },
        new SheetInfo { Name = "PlayerLevelUpData", Id = "158876595" },
        new SheetInfo { Name = "SubtaskInfoData", Id = "1638859933" },
        new SheetInfo { Name = "RewardInfoData", Id = "517265494" },
        new SheetInfo { Name = "QuestInfoData", Id = "135978167" },
    };

    private int _pendingDownloads = 0;

    [MenuItem("Tools/Sheet Downloader")]
    public static void ShowWindow()
    {
        GetWindow<ToolsSheetDownloader>("Sheet Downloader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Download CSV Files from Google Sheets", EditorStyles.boldLabel);

        if (GUILayout.Button("Download All CSV Files"))
        {
            DownloadAllCSVFiles();
        }

        foreach (var sheet in sheets)
        {
            if (GUILayout.Button($"Download {sheet.Name}"))
            {
                _pendingDownloads = 1;  // Only one download pending
                EditorCoroutineUtility.StartCoroutineOwnerless(Download(sheet.Id, sheet.Name));
            }
        }
    }

    private void DownloadAllCSVFiles()
    {
        _pendingDownloads = sheets.Length;
        foreach (var sheet in sheets)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(Download(sheet.Id, sheet.Name));
        }
    }

    private IEnumerator Download(string sheetId, string saveFileName, string sheetPath = _sheetPath, string address = _address, string format = _format)
    {
        var url = $"https://docs.google.com/spreadsheets/d/{address}/export?format={format}&gid={sheetId}";
        using (var www = UnityWebRequest.Get(url))
        {
            Debug.Log("Start Downloading...");

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to download {saveFileName}: {www.error}");
                _pendingDownloads--;
                yield break;
            }

            var fileUrl = $"{_sheetPath}/{saveFileName}.{format}";
            File.WriteAllText(fileUrl, www.downloadHandler.text + "\n");

            Debug.Log("Download Complete.");
        }

        _pendingDownloads--;

        if (_pendingDownloads <= 0)
        {
            EditorUtility.DisplayDialog("Download Complete", "All CSV files have been downloaded successfully.", "OK");
        }
    }
}
