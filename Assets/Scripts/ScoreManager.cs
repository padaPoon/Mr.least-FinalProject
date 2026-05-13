using UnityEngine;

public static class ScoreManager
{
    private const string HIGH_SCORE_KEY = "HighScore";

    // ดึง high score ที่เก็บไว้
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    // บันทึก score ถ้าสูงกว่าเดิม → return true ถ้าเป็น new record
    public static bool TrySaveHighScore(int newScore)
    {
        int currentHigh = GetHighScore();
        if (newScore > currentHigh)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, newScore);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // รีเซ็ต high score (เผื่ออยากใส่ปุ่ม reset)
    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
    }
}