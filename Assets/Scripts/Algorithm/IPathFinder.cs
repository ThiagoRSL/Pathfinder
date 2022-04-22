using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinder
{
    public List<int> FindPath(int start, int target);
}
