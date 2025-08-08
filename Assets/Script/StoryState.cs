using UnityEngine;

public static class StoryState
{
    public static bool[] showStoryFlags = new bool[8];

    public static void ResetAll()
    {
        for (int i = 0; i < 8; i++)
        {
            showStoryFlags[i] = true;
            PlayerPrefs.DeleteKey($"Story{i + 1}HasBeenShown");
        }
    }

    public static bool GetFlag(int index) => showStoryFlags[index];
    public static void ClearFlag(int index) => showStoryFlags[index] = false;
}
