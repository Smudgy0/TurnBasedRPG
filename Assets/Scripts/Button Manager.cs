using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public BattleManager BM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ButtonTargetNumber(int TargetNum)
    {
        BM.TargetNumber(TargetNum);
    }
}
