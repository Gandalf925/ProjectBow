using System.Collections.Generic;
using UnityEngine;

public abstract class ClearCondition : MonoBehaviour
{
    // クリア条件を満たしたかどうかを判定するための抽象メソッド
    public abstract bool IsConditionMet(StageManagerBase stageManager);

    // クリア条件に必要な設定を初期化
    public virtual void Initialize(StageManagerBase stageManager) { }
}

// 特定のターゲットをすべて破壊する条件
public class DestroyAllTargetsCondition : ClearCondition
{
    public override bool IsConditionMet(StageManagerBase stageManager)
    {
        return stageManager.targets.Count == 0;
    }
}

// 特定のターゲットの頭を撃ち抜く条件
public class HitSpecificTargetCondition : ClearCondition
{
    public List<MonoBehaviour> requiredTargets; // 必要なターゲットのリスト

    public override void Initialize(StageManagerBase stageManager)
    {
        requiredTargets = new List<MonoBehaviour>(stageManager.targets);
    }

    public override bool IsConditionMet(StageManagerBase stageManager)
    {
        foreach (var target in requiredTargets)
        {
            if (stageManager.targets.Contains(target))
            {
                return false;
            }
        }
        return true;
    }
}

// 特定のターゲットの一つだけに当てる条件
public class HitOnlyOneSpecificTargetCondition : ClearCondition
{
    public MonoBehaviour specificTarget; // ターゲット指定

    public override bool IsConditionMet(StageManagerBase stageManager)
    {
        return !stageManager.targets.Contains(specificTarget) && stageManager.targets.Count == 0;
    }
}
