using System;
using System.IO;
using System.Linq;
using Tayx.Graphy.Fps;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.CustomAttributes.ReadOnly;
using Utils.CustomAttributes.ReadOnlyWhenPlaying;
using Utils.CustomAttributes.ShowIf;

public enum Separator
{
    Comma, Semicolon,
}

public static class SeparatorExtensions
{
    public static char ToChar(this Separator separator)
    {
        switch (separator)
        {
            case Separator.Comma:
                return ',';
            case Separator.Semicolon:
                return ';';
            default:
                throw new ArgumentOutOfRangeException(nameof(separator), separator, null);
        }
    }
}

public class FpsReport : MonoBehaviour
{
    

    [ReadOnlyWhenPlaying]
    [SerializeField]
    private G_FpsMonitor fpsMonitor;
    
    [ReadOnly]
    [SerializeField]
    private int fps;

    [ReadOnlyWhenPlaying]
    [SerializeField]
    private Brain brain;
    private bool brainActive;
    private int updateFrequency;
    
    private EnemySensorDataList enemies;
    private NodeSensorDataList nodes;
    private const int PlayerSensorCount = 1;

    [ReadOnlyWhenPlaying]
    [SerializeField]
    private Separator defaultSeparator;

    [ReadOnlyWhenPlaying]
    [SerializeField]
    private bool saveToTxt;

    [SerializeField]
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(saveToTxt))]
    private bool useAnotherSeparatorForTxt;

    [SerializeField]
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(useAnotherSeparatorForTxt))]
    private Separator txtSeparator;

    [ReadOnlyWhenPlaying]
    [SerializeField]
    private bool saveToCsv;
    
    [SerializeField]
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(saveToCsv))]
    private bool useAnotherSeparatorForCsv;
    
    [SerializeField]
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(useAnotherSeparatorForCsv))]
    private Separator csvSeparator;

    private const string Path = "Assets/Reports/Fps/";
    private string sceneName;
    private string sceneRelatedPath;
    private string txtPath;
    private string csvPath;
    private string txtFileName;
    private string csvFileName;
    private DirectoryInfo directoryInfo;
    private string prefix;

    private bool updatingSensors;

    private void Awake()
    {
        brainActive = brain.enabled && brain.enableBrain;
        updateFrequency = (int) brain.sensorsUpdateFrequencyMS;
        enemies = EnemySensorDataList.Instance;
        nodes = NodeSensorDataList.Instance;
        sceneName = SceneManager.GetActiveScene().name;
        sceneRelatedPath = Path + sceneName;
        directoryInfo = Directory.CreateDirectory(sceneRelatedPath);
        prefix = brainActive ? "BrainON_" : "BrainOFF_";

        if (saveToTxt)
        {
            txtPath = CreateFile(".txt", useAnotherSeparatorForTxt? txtSeparator : defaultSeparator);
        }
        if (saveToCsv)
        {
            csvPath = CreateFile(".csv", useAnotherSeparatorForCsv? csvSeparator : defaultSeparator);
        }
    }
    
    private void Update()
    {
        fps = (int)fpsMonitor.CurrentFPS;
        if (saveToTxt)
        {
            var separator = useAnotherSeparatorForTxt ? txtSeparator : defaultSeparator;
            UpdateText(txtPath, separator, updatingSensors);
        }

        if (saveToCsv)
        {
            var separator = useAnotherSeparatorForCsv ? csvSeparator : defaultSeparator;
            UpdateText(csvPath, separator, updatingSensors);
        }
        updatingSensors = Performance.updatingSensors;
        Performance.updatingSensors = false;
    }

    private string CreateFile(string extension, Separator separator)
    {
        var suffix = "001";
        var separatorCharacter = separator.ToChar();

        var fileInfo = directoryInfo
            .EnumerateFiles(prefix + "*" + extension)
            .OrderBy(info => info.Name)
            .LastOrDefault();

        if (fileInfo != null)
        {
            var i = int.Parse(fileInfo.Name.Substring((fileInfo.Name.Length - suffix.Length - extension.Length), suffix.Length));
            i++;
            suffix = $"{i:000}";
        }
        var newFilePath =
            sceneRelatedPath +
            "/" + prefix + suffix + extension;
        WriteHeading(newFilePath, separatorCharacter);
        return newFilePath;
    }
    
    private void UpdateText(string path, Separator separator, bool updatingSensors)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine($"{fps}" +
                         $"{separator.ToChar()}" + $"{enemies.Count}" +
                         $"{separator.ToChar()}" + $"{nodes.Count}" +
                         $"{separator.ToChar()}" + $"{PlayerSensorCount}" + 
                         $"{separator.ToChar()}" + $"{nodes.Count+enemies.Count+PlayerSensorCount}" + 
                         $"{separator.ToChar()}" + $"{(updatingSensors? 1 : 0)}"
            );
        }
    }

    private static void WriteHeading(string path, char separatorCharacter)
    {
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(
                "FPS" +
                $"{separatorCharacter}Enemies" +
                $"{separatorCharacter}Nodes" +
                $"{separatorCharacter}Player" +
                $"{separatorCharacter}Total" +
                $"{separatorCharacter}Updating Sensors"
            );
        }
    }
}
