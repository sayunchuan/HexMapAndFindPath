using HexagonMap;
using UnityEngine;

namespace Demo1
{
    public class TestUtil : MonoBehaviour
    {
        public Transform t;

        private void Update()
        {
            if (t != null)
            {
                Debug.Log(Util.Vec2Pos(new Vector2(t.position.x, t.position.z)));
            }
        }
    }
}