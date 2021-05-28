using System.Collections.Generic;
using HexagonMap;
using UnityEngine;

namespace Demo1
{
    public class MapGen : MonoBehaviour
    {
        public int width = 1;
        public int height = 1;
        public float size = 1f;

        public Transform cameraTr;
        public Material m;
        public Texture2D tex;
        public List<HexMesh> ms = new List<HexMesh>();

        private List<Vector2> _path = new List<Vector2>();
        private List<Position2D> _path2 = new List<Position2D>();

        private Map _map;

        private void Awake()
        {
            tex = new Texture2D(256, 256);
            for (int i = 0; i < 256; i++)
            {
                if (i < 248)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        tex.SetPixel(i, j, Color.white);
                    }
                }
                else
                {
                    for (int j = 0; j < 256; j++)
                    {
                        tex.SetPixel(i, j, Color.black);
                    }
                }
            }

            tex.Apply();
            m.mainTexture = tex;

            ReDraw();

            _path.Clear();
            _path2.Clear();
        }

        public void ReDraw()
        {
            _map = new Map(width, height);

            foreach (var hexMesh in ms)
            {
                DestroyImmediate(hexMesh.gameObject);
            }

            ms.Clear();

            cameraTr.position = new Vector3(size * width * 0.5f, 10, 0.75f * size * height * 0.5f);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _AddMesh(j, i);
                }
            }
        }

        private void _AddMesh(int x, int y)
        {
            GameObject go = new GameObject("Hexagon");

            go.transform.SetParent(transform);
            var mesh = go.AddComponent<HexMesh>();
            ms.Add(mesh);

            mesh.Add(size * 0.5f);
            mesh.BindPos(x, y, size);
            mesh.BindMaterial(m);
        }

        public HexMesh Get(int x, int y)
        {
            return ms[x + y * width];
        }

        private string beginx, beginy, endx, endy;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Begin Pos X:", GUILayout.Width(100));
            beginx = GUILayout.TextField(beginx, GUILayout.Width(40));
            GUILayout.Label(" Y:", GUILayout.Width(20));
            beginy = GUILayout.TextField(beginy, GUILayout.Width(40));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ended Pos X:", GUILayout.Width(100));
            endx = GUILayout.TextField(endx, GUILayout.Width(40));
            GUILayout.Label(" Y:", GUILayout.Width(20));
            endy = GUILayout.TextField(endy, GUILayout.Width(40));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Find Path"))
            {
                foreach (var hexMesh in ms)
                {
                    hexMesh.SetColor(Color.white);
                }

                _map.FindPath(
                    new Position2D(int.Parse(beginx), int.Parse(beginy)),
                    new Position2D(int.Parse(endx), int.Parse(endy)),
                    ref _path2);

                foreach (var vec in _path2)
                {
                    var hex = Get(vec.X, vec.Y);
                    hex.SetColor(Color.cyan);
                }
            }

            if (GUILayout.Button("Re draw map mesh"))
            {
                ReDraw();
            }
        }

        private void Update()
        {
            if (_path.Count > 1)
            {
                for (int i = 0, j = 1; j < _path.Count; i = j, j++)
                {
                    Debug.DrawLine(
                        new Vector3(_path[i].x, 0, _path[i].y),
                        new Vector3(_path[j].x, 0, _path[j].y),
                        Color.cyan);
                }
            }
        }
    }
}