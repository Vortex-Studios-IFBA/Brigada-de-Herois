using System;

[Serializable]
public class ActionAndReaction
{
    public Attack attack;
    public BattleManager.Reaction reaction;
    public float value;
}