using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothTester : MonoBehaviour
{
    [SerializeField] private GameObject rootBone;
    //[SerializeField] private List<>
    private Dictionary<string, Transform> bodyBones;
    private string RootBoneName => rootBone.name;
    [SerializeField] private GameObject equipment;
    private GameObject _equipment;

    // Start is called before the first frame update
    void Start()
    {
        // É{Å[ÉìÇÃèâä˙âª
        bodyBones = new Dictionary<string, Transform>();
        List<GameObject> bones = GetAllObjects(rootBone);
        bodyBones.Add(rootBone.name, rootBone.transform);
        foreach (GameObject bone in bones)
        {
            //if (bone.name.Contains("Handler") || bone.name.Contains("Preview")) continue;
            try
            {
                bodyBones.Add(bone.name, bone.transform);
            }
            catch (ArgumentException e)
            {
                continue;
            }
        }
    }

    private List<GameObject> GetAllObjects(GameObject target)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(target, ref allChildren);
        return allChildren;

    }

    private void GetChildren(GameObject target, ref List<GameObject> allChildren)
    {
        Transform children = target.GetComponentInChildren<Transform>();

        if (children.childCount == 0) return;

        foreach (Transform child in children)
        {
            allChildren.Add(child.gameObject);
            GetChildren(child.gameObject, ref allChildren);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _equipment = Instantiate(equipment, transform);
            ReplaceSameNameBones(_equipment);
            _equipment.transform.SetParent(transform, false);
        }
    }

    private void ReplaceSameNameBones(GameObject equipment)
    {
        SkinnedMeshRenderer renderer = equipment.GetComponentInChildren<SkinnedMeshRenderer>();
        var equipmentBones = renderer.bones.ToArray();
        for (int i = 0; i < equipmentBones.Length; i++)
        {
            var equipmentBone = equipmentBones[i];
            if (equipmentBone == null) continue;

            var equipmentBoneName = equipmentBone.name;
            if (bodyBones.TryGetValue(equipmentBoneName, out var sameNameBodyBone))
            {
                Debug.Log($"{sameNameBodyBone.name} / {equipmentBone.name}");
                equipmentBones[i] = sameNameBodyBone;
            }
        }

        renderer.bones = equipmentBones;
        renderer.rootBone = bodyBones[RootBoneName];
    }
}
