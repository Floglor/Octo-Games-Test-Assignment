using UnityEngine;

[Naninovel.ExpressionFunctions]
public static class CustomFunctions
{
    public static int ReturnMinigameScore(int numberOfCards, int timeTaken, int mistakesMade, int mistakesMultiplier) =>
        (numberOfCards * 100) / timeTaken + (mistakesMade * mistakesMultiplier);
}