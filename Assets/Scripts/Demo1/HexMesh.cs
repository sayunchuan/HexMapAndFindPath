using UnityEngine;

namespace Demo1
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        public const float side = 1.1547005384f;
        public const float halfSide = 0.5773502692f;

        Mesh hexMesh;

        void Awake()
        {
            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            hexMesh.name = "Hex Mesh";
        }

        public void Add(float size)
        {
            hexMesh.vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, side) * size,
                new Vector3(1, 0, halfSide) * size,
                new Vector3(1, 0, -halfSide) * size,
                new Vector3(0, 0, -side) * size,
                new Vector3(-1, 0, -halfSide) * size,
                new Vector3(-1, 0, halfSide) * size,
            };
            hexMesh.triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5,
                0, 5, 6,
                0, 6, 1
            };
            hexMesh.uv = new Vector2[]
            {
                Vector2.zero,
                Vector2.one,
                Vector2.one,
                Vector2.one,
                Vector2.one,
                Vector2.one,
                Vector2.one,
            };
        }

        public void BindPos(int x, int y, float size)
        {
            float deltaX = y % 2 == 0 ? 0 : size * 0.5f;
            float px = x * size + deltaX;
            float py = y * 0.8660254038f * size;

            transform.localPosition = new Vector3(px, 0, py);
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        public void BindMaterial(Material m)
        {
            GetComponent<MeshRenderer>().material = m;
        }

        public void SetColor(Color c)
        {
            GetComponent<MeshRenderer>().material.color = c;
        }
    }
}