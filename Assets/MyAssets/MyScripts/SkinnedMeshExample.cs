using UnityEngine;

public class SkinnedMeshExample : MonoBehaviour
{
   public GameObject cube1;
    public GameObject cube2;
    public Transform topBone;
    public Transform lowerBone;
    SkinnedMeshRenderer rend1;
    SkinnedMeshRenderer rend2;
    private Vector3 initialScale;
    void Start()
    {
        initialScale = cube1.transform.localScale;
        SetBoneWeightsForCube1(1);
    }

    public void SetBoneWeightsForCube1(float boneWeight)
    {
        //gameObject.AddComponent<Animation>();
        //cube1 = transform.gameObject;
        //cube1.AddComponent<SkinnedMeshRenderer>();
        rend1 = cube1.GetComponent<SkinnedMeshRenderer>();
        //Animation anim = GetComponent<Animation>();

        // Build basic mesh
        //Mesh mesh = new Mesh();
        //mesh.vertices = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(-1, 5, 0), new Vector3(1, 5, 0) };
        //mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        //mesh.triangles = new int[] { 0, 3, 1, 0, 2, 3 };
        //mesh.RecalculateNormals();

        // Assign mesh to mesh filter & renderer
        //rend1.material = new Material(Shader.Find("Diffuse"));

        // Assign bone weights to mesh
        // We use 2 bones. One for the lower vertices, one for the upper vertices.
        //rend1.sharedMesh = mesh;
        Mesh mesh = new Mesh();
        rend1.BakeMesh(mesh);

        Vector3[] verts = mesh.vertices;
        BoneWeight[] weights = new BoneWeight[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            if (verts[i].y > 0)
            {
                weights[i].boneIndex0 = 0;
            }
            else
            {
                weights[i].boneIndex0 = 1;
            }

            weights[i].weight0 = boneWeight;
            verts[i].Scale(initialScale);
        }

        mesh.vertices = verts;


        // A BoneWeights array (weights) was just created and the boneIndex and weight assigned.
        // The weights array will now be assigned to the boneWeights array in the Mesh.
        mesh.boneWeights = weights;

        // Create Bone Transforms and Bind poses
        // One bone at the bottom and one at the top
        Transform[] bones = new Transform[2];
        bones[0] = topBone;
        bones[1] = lowerBone;
        Matrix4x4[] bindPoses = new Matrix4x4[2];

        //bones[0] = new GameObject("Lower").transform;
        bones[0].parent = cube1.transform;
        // Set the position relative to the parent
        //bones[0].localRotation = Quaternion.identity;
        //bones[0].localPosition = Vector3.zero;

        // The bind pose is bone's inverse transformation matrix
        // In this case the matrix we also make this matrix relative to the root
        // So that we can move the root game object around freely
        bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;

        //bones[1] = new GameObject("Upper").transform;
        bones[1].parent = cube1.transform;
        // Set the position relative to the parent
        //bones[1].localRotation = Quaternion.identity;
        //bones[1].localPosition = new Vector3(0, 5, 0);
        // The bind pose is bone's inverse transformation matrix
        // In this case the matrix we also make this matrix relative to the root
        // So that we can move the root game object around freely
        bindPoses[1] = bones[1].worldToLocalMatrix * transform.localToWorldMatrix;

        // assign the bindPoses array to the bindposes array which is part of the mesh.
        mesh.bindposes = bindPoses;

        
        rend1.sharedMesh = mesh;

        // Assign bones and bind poses
        rend1.bones = bones;
        //rend.sharedMesh = mesh;

        //// Assign a simple waving animation to the bottom bone
        //AnimationCurve curve = new AnimationCurve();
        //curve.keys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 3, 0, 0), new Keyframe(2, 0.0F, 0, 0) };

        //// Create the clip with the curve
        //AnimationClip clip = new AnimationClip();
        //clip.SetCurve("Lower", typeof(Transform), "m_LocalPosition.z", curve);
        //clip.legacy = true;
        //clip.wrapMode = WrapMode.Loop;

        //// Add and play the clip
        //anim.AddClip(clip, "test");
        //anim.Play("test");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TransferRig();
        }
    }

    public void TransferRig()
    {

        rend2 = cube2.GetComponent<SkinnedMeshRenderer>();
        rend2.transform.position = rend1.transform.position;
        rend2.transform.rotation = rend1.transform.rotation;
        rend2.bones = rend1.bones;
        rend2.sharedMesh.bindposes = rend1.sharedMesh.bindposes;
        rend2.sharedMesh.boneWeights = rend1.sharedMesh.boneWeights;
    }
}