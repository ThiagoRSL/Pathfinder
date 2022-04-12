using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject CreateGrid(int width, int height, int complexity, TileController tilePreFab)//Implementar recebimento de objeto para declarar como pai
    {
        GameObject gameObject = new GameObject("GridObject");
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);
        GridController gridController = gameObject.addComponent<GridController>();
        gridController.Init(width, height, complexity, tilePreFab);
        return gameObject;
    }

}
