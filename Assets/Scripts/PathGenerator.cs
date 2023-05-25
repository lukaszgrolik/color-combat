using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(PathGenerator))]
public class PathGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (PathGenerator)target;

        if (DrawDefaultInspector())
        {
            // if (script.autoUpdate)
            // {
            //     script.Generate();
            // }
        }

        if (GUILayout.Button("Generate")) script.Generate();
        if (GUILayout.Button("Clear")) script.Clear();
    }
}
#endif

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private float objectGap = 1f;
    [SerializeField] private float posDeviance = .25f;
    [SerializeField] private float sizeDeviance = .2f;
    [SerializeField] private Gradient gradient;

    public void UpdateFields(
        Transform container,
        List<GameObject> prefabs,
        Gradient gradient
    )
    {
        this.container = container;
        this.prefabs = prefabs;
        this.gradient = gradient;
    }

    public void Generate()
    {
        Clear();

        var i = 0;
        var lineFrom = Vector3.zero;
        foreach (Transform point in transform)
        {
            var prefab = LukRandom.Uniform.Sample(prefabs);

            if (i != 0)
            {
                var lineTo = point.position;

                var pathFragmentDist = Vector3.Distance(lineFrom, lineTo);
                var objectsCount = Mathf.FloorToInt(pathFragmentDist / objectGap);

                for (int j = 0; j < objectsCount; j++)
                {
                    var basePos = lineFrom + (lineTo - lineFrom).normalized * objectGap * j;
                    var randomCirclePos = (Random.insideUnitCircle * posDeviance).ToVector3();
                    var pos = basePos + randomCirclePos;
                    var rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    var size = Random.Range(1f - sizeDeviance, 1f + sizeDeviance);
                    var color = gradient.Evaluate(Random.value);

                    var obj = Instantiate(prefab, pos, rot);
                    obj.transform.SetParent(container);
                    obj.transform.localScale = Vector3.one * size;

                    var spriteRend = obj.GetComponentInChildren<SpriteRenderer>();
                    spriteRend.color = color;
                }
            }

            lineFrom = point.position;

            foreach (Transform child in point)
            {
                var pathGen = child.GetComponent<PathGenerator>();

                if (pathGen != null)
                {
                    pathGen.UpdateFields(
                        container,
                        prefabs,
                        gradient
                    );
                    pathGen.Generate();
                }
            }

            i += 1;
        }
    }

    public void Clear()
    {
        container.DestroyChildrenImmediate();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {

        var i = 0;
        var prevPos = Vector3.zero;
        foreach (Transform child in transform)
        {
            var pos = child.position;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, .25f);

            if (container.childCount == 0 && i != 0)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(prevPos, pos);
            }

            prevPos = pos;

            i += 1;
        }
    }
#endif
}