using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    public List<TileController> FindPath(Vector3 originCord, Vector3 targetCord);
}
