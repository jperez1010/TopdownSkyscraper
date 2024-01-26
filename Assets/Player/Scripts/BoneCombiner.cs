using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner : MonoBehaviour
{
    public readonly Dictionary<int, Transform> RootBoneDictionary = new Dictionary<int, Transform>();
    private readonly Transform[] _boneTransforms = new Transform[67];

    private readonly Transform _transform;

    public BoneCombiner(GameObject rootObj)
    {
        _transform = rootObj.transform;
        TraverseHierarchy(_transform);
    }

    public Transform AddLimb(GameObject bonedObj, List<string> boneNames)
    {
        Transform limb = ProcessBonedObject(bonedObj.GetComponentInChildren<SkinnedMeshRenderer>(),boneNames);
        limb.SetParent(_transform);
        return limb;
    }

    private Transform ProcessBonedObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        var bonedObject = new GameObject().transform;
        var meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        //var bones = renderer.bones;

        for (int i = 0; i < boneNames.Count; i++)
        {
            _boneTransforms[i] = RootBoneDictionary[boneNames[i].GetHashCode()];
        }

        meshRenderer.bones = _boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return bonedObject;
    }

    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {
            RootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
}
