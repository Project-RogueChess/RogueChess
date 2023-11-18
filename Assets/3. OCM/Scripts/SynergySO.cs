using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SE_", menuName = "SynergyData/SynergyEnhance")]
public class SynergySO : ScriptableObject
{
    public Sprite icon;

    public string spieces;

    public Piece injectData;

    public int[] terms;

    public string injectDataStr;
    public void Execute(List<Pieces> pieces, int term)
    {
        foreach (Pieces piece in pieces)
        {
            if (piece.spieces == spieces)
            {
                piece.buffData[2].maxHp = injectData.maxHp * (term + 1);
                piece.hp = piece.maxHp;
                piece.buffData[2].attackDamage = injectData.attackDamage * (term + 1);
            }
        }
    }
}
