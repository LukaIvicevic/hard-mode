using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*  
    Difficulty Modifier

    Level   | Tank fire rate    | Kill floor time between detonations | Number of kill balls spawned  | Kill ball speed   | Boss health   | Generator health  |
      1     |         2         |                20                   |               1               |          6        |       1000    |          100       |
      2     |                   |                                     |                               |                   |               |                   |
      3     |                   |                                     |                               |                   |               |                   |
      4     |                   |                                     |                               |                   |               |                   |
      5     |                   |                                     |               2               |                   |               |                   |
      6     |                   |                                     |                               |                   |               |                   |
      7     |                   |                                     |                               |                   |               |                   |
      8     |                   |                                     |               3               |                   |               |                   |
      9     |                   |                                     |                               |                   |               |                   |
      10    |         0.25      |                 5                   |                               |          15       |       3000    |          550      |
*/

public class StatsManager : Singleton<StatsManager>
{
    public float tankFireRateD1 = 2f;
    public float tankMaxFireRateD10 = 0.25f;

    public float killFloorTimeBetweenDetonationsD1 = 20f;
    public float killFloorMaxTimeBetweenDetonationsD10 = 5f;

    public float killBallSpeedD1 = 6f;
    public float killBallSpeedD10 = 15f;

    public float bossHealthD1 = 1000f;
    public float bossHealthD10 = 3000f;

    public float engineHealthD1 = 100f;
    public float engineHealthD10 = 550f;

    public float GetDifficultyValue(float d1, float d10)
    {
        var difficulty = GameManager.Instance.GetDifficulty();
        var t = difficulty / 10; // Get percentage difficulty
        print(difficulty);
        print(t);
        print(d1);
        print(d10);
        return Mathf.Lerp(d1, d10, t);
    }

    public static int GetNumberOfKillBallsToSpawn()
    {
        var d = GameManager.Instance.GetDifficulty();

        if (d < 5)
        {
            return 1;
        }

        if (d < 8)
        {
            return 2;
        }

        return 3;
    }
}
