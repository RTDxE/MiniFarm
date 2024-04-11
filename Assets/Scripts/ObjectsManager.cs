using System.Collections.Generic;
using UnityEngine;

namespace MiniFarm
{
    public class ObjectsManager : MonoBehaviour
    {
        private static ObjectsManager _instance;
        public static ObjectsManager Instance => _instance;

        private Dictionary<Vector2Int, GameObject> _objects = new();
        [SerializeField] private Vector2Int _cellSize = new Vector2Int(1, 1);

        private void Awake()
        {
            _instance = this;
        }

        public void PlaceObject(GameObject placeableObject)
        {
            Vector3 position = placeableObject.transform.position;
            Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt(position.x / _cellSize.x), Mathf.FloorToInt(position.y / _cellSize.y));
            if (_objects.ContainsKey(cellPos))
            {
                RemoveObject(cellPos);
            }

            _objects.Add(cellPos, placeableObject);
            placeableObject.transform.position = new Vector3(
                cellPos.x * _cellSize.x + _cellSize.x / 2f,
                cellPos.y * _cellSize.y + _cellSize.y / 2f, 
                0);

            placeableObject.transform.SetParent(transform);
        }

        public bool HasObject(Vector2 position)
        {
            Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt(position.x / _cellSize.x), Mathf.FloorToInt(position.y / _cellSize.y));
            return _objects.ContainsKey(cellPos);
        }

        public GameObject GetObject(Vector2 position)
        {
            Vector2Int cellPos = new Vector2Int(Mathf.FloorToInt(position.x / _cellSize.x), Mathf.FloorToInt(position.y / _cellSize.y));
            return _objects.TryGetValue(cellPos, out var obj) ? obj : null;
        }

        public void RemoveObject(Vector2Int cellPos)
        {
            Destroy(_objects[cellPos]);
            _objects.Remove(cellPos);
        }
    }
}