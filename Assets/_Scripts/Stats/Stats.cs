using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*  
    Difficulty Modifier

    Level   | Tank fire rate    | Kill floor time between detonations | Number of kill balls spawned  | Kill ball speed   | Boss health   | Generator health  |
      1     |         2         |                20                   |               1               |                   |               |                   |
      2     |                   |                                     |                               |                   |         4      |                   |
      3     |                   |                                     |                               |                   |         6      |                   |
      4     |                   |                                     |                               |                   |        88       |                   |
      5     |                   |                                     |               2               |                   |        1000   |                   |
      6     |                   |                                     |                               |                   |        12       |                   |
      7     |                   |                                     |                               |                   |         14      |                   |
      8     |                   |                                     |               3               |                   |         16      |                   |
      9     |                   |                                     |                               |                   |         18      |                   |
      10    |         0.25      |                 5                   |                               |                   |       2000    |                   |
*/

// TODO: This should just be a singleton and manage all the difficulty settings for the enemies
[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats", order = 1)]
public class Stats : ScriptableObject
{
    public float tankFireRateD1 = 2f;
    public float tankMaxFireRateD10 = 0.25f;

    public float killFloorTimeBetweenDetonationsD1 = 20f;
    public float killFloorMaxTimeBetweenDetonationsD10 = 5f;

    public float killBallSpeedD1 = 6f;
    public float killBallSpeedD10 = 15f;

    public float bossHealthD1 = 200;
    public float bossHealthD10 = 2000;

    public static float GetLinearEvaluation(AnimationCurve curve, float d1, float d10)
    {
        var keyframes = curve.keys;
        keyframes[0].time = 1;
        keyframes[0].value = d1;
        keyframes[1].time = 10;
        keyframes[1].value = d10;
        curve.keys = keyframes;
        AnimationUtility.SetKeyRightTangentMode(curve, 0, AnimationUtility.TangentMode.Auto);
        AnimationUtility.SetKeyLeftTangentMode(curve, 1, AnimationUtility.TangentMode.Auto);
        return curve.Evaluate(GameManager.Instance.GetDifficulty());
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
