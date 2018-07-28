using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : SingletonBehaviour<Config>
{
    [SerializeField]
    private int startingDifficultyLevel = 1;
    public int StartingDifficultyLevel { get { return startingDifficultyLevel; } }

    [SerializeField]
    private float animationPauseTime = 0.7f;
    public float AnimationPauseTime { get { return animationPauseTime; } }

    [SerializeField]
    private int animationRotationSpeed = 90;
    public float AnimationRotationSpeed { get { return animationRotationSpeed; } }

    [SerializeField]
    private float stampIncrement = 1f / 6f;
    public float SolutionCreationStampIncrement { get { return stampIncrement; } }

    [SerializeField]
    private int levelMultiplier = 3;
    public int SolutionCreationLevelMultiplier { get { return levelMultiplier; } }
}
