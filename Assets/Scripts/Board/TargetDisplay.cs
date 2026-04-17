using System.Collections.Generic;
using UnityEngine;

public class TargetDisplay {
 private List<GameObject> _targetsObjects = new();
 private TargetData _targetData;
 private Board _board;
 public TargetDisplay(Board board, TargetData targetData) {
  _board = board;
  _targetData = targetData;
 }

 public void SetTargets<T>(List<T> targets) where T : BoardSlot {
  foreach (T target in targets) {
   GameObject targetObject;
   if(_targetsObjects.Count > targets.IndexOf(target))
    targetObject = _targetsObjects[targets.IndexOf(target)];
   else
    targetObject = GameObject.Instantiate(_targetData.TargetPrefab);
   _targetsObjects.Add(targetObject);
   targetObject.transform.position = _board.BoardData.GridToWorld(target.position);
   targetObject.SetActive(true);
  }
 }

 public void ClearTargets() {
  foreach (GameObject targetObject in _targetsObjects) {
    targetObject.SetActive(false);
  }
 }
}