using UnityEngine;


namespace MapEditor
{
    [RequireComponent(typeof(MeshFilter))]
    public class InfinityMesh : MonoBehaviour
    {
        const float inf = 20000;
        
        public float z;
        
        
        
        void Start()
        {
            Mesh mesh = this.GetComponent<MeshFilter>().mesh;
            
            mesh.vertices = new Vector3[] {
                new Vector3(-inf, -inf, z),
                new Vector3(-inf, inf, z),
                new Vector3(inf, -inf, z),
                new Vector3(inf, inf, z),
            };
            mesh.triangles = new int[] {
                0, 1, 2,
                1, 2, 3
            };
            
            mesh.RecalculateBounds();
        }
        
        void Update()
        {
            Destroy(this);
        }
    }
    
}
