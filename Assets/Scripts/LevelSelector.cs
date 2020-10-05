using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;

    public Button[] levelButtons;

    private void Start()
    {
        var levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (var i = 0; i < levelButtons.Length; i++)
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
    }

    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }
}