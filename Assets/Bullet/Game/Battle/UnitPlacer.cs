using System;
using UnityEngine;
using static UnitMaster;

[ExecuteAlways]
public class UnitPlacer : MonoBehaviour
{
	[SerializeField]
	public UnitTyp _Typ;

	public void Exec(BattleBoardModel aBattleBoard, bool aIsP1, Action<(bool isP1, UnitModel unit, FieldEffectModel fieldEffect)> aOnExec) 
	{
		var pos = transform.position;
		var unit = aBattleBoard.AddUnit(_Typ, new Vector2(pos.x, pos.z), aIsP1);
		aOnExec((aIsP1, unit, null));
	}
} 