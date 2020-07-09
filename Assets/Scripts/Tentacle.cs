using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class Tentacle : MonoBehaviour
{
    [SerializeField] Transform tentacleParent = null;
    [SerializeField] Transform target = null;
    [SerializeField] int maxIterations = 1;

    private Bone[] bones;
    private Vector3 lastTargetPosition;
    private Vector3 rootPosition;

    private class Bone
    {
        public readonly Transform transform;
        public readonly float length;

        public Bone(Transform transform, float length)
        {
            this.transform = transform;
            this.length = length;
        }

        public Vector3 StartPoint
        {
            get => transform.position;
        }

        public Vector3 EndPoint
        {
            get => transform.position + transform.TransformDirection(Vector3.forward * length);
        }

        public void LookDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public void Align(Vector3 target, Vector3 pivotPoint)
        {
            Vector3 offset = transform.position - pivotPoint;
            transform.position = target + offset;
        }
    }

    private void Start()
    {
        Assert.IsNotNull(tentacleParent);
        Assert.IsNotNull(target);
        InitBones();
        rootPosition = bones.First().StartPoint;
        ResolveIK();
    }

    private void Update()
    {
        if(target.position != lastTargetPosition)
        {
            lastTargetPosition = target.position;
            ResolveIK();
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;
        foreach(Bone bone in  bones)
        {
            Gizmos.DrawSphere(bone.StartPoint, bone.length * 0.1f);
            Gizmos.DrawLine(bone.StartPoint, bone.EndPoint);
        }
    }

    private void InitBones()
    {
        bones = new Bone[tentacleParent.childCount - 1];
        for(int i = 0; i < bones.Length; i++)
        {
            Transform boneStart = tentacleParent.GetChild(i);
            Transform boneEnd = tentacleParent.GetChild(i + 1);
            float length = Vector3.Distance(boneStart.position, boneEnd.position);
            bones[i] = new Bone(boneStart, length);
        }
    }

    private bool CloseToTarget()
    {
        return (bones.Last().EndPoint - target.position).sqrMagnitude <= Mathf.Epsilon;
    }

    private void ResolveIK()
    {
        int iterationCount = 0;
        while(iterationCount < maxIterations && !CloseToTarget())
        {
            EndPointToRoot();
            RootToEndPoint();
            iterationCount++;
        }
    }

    private void EndPointToRoot()
    {
        Vector3 currentTarget = target.position;
        for(int i = bones.Length - 1; i > -1; i--)
        {
            Bone bone = bones[i];
            bone.LookDirection(currentTarget - bone.StartPoint);
            bone.Align(currentTarget, bone.EndPoint);
            currentTarget = bone.StartPoint;
        }
    }

    private void RootToEndPoint()
    {
        Vector3 currentTarget = rootPosition;
        for(int i = 0; i < bones.Length; i++)
        {
            Bone bone = bones[i];
            bone.LookDirection(bone.EndPoint - currentTarget);
            bone.Align(currentTarget, bone.StartPoint);
            currentTarget = bone.EndPoint;
        }
    }
}
