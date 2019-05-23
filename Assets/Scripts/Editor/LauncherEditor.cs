using UnityEngine;
using UnityEditor;
using System;

public class LauncherEditor : EditorWindow
{
    public UnityEngine.Object sourceParent;
    public UnityEngine.Object sourceFirst;
    public UnityEngine.Object sourceSecond;
    public UnityEngine.Object sourceThird;
    int count = 10;

    [MenuItem("Curve/CurveWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LauncherEditor));
    }

    private void OnGUI()
    {
        GameObject parent = null;
        GameObject first = null;
        GameObject second = null;
        GameObject third = null;

        

        sourceParent = EditorGUILayout.ObjectField("Родительский объект кривой: ", sourceParent, typeof(UnityEngine.Object), true);

        if (sourceParent != null)
        {
            parent = (GameObject)sourceParent;
        }

        if (GUILayout.Button("Нарисовать кривую") && parent != null)
        {
            DrawCurve(parent);
        }

        sourceFirst = EditorGUILayout.ObjectField("Первая точка: ", sourceFirst, typeof(UnityEngine.Object), true);
        if (sourceFirst != null)
        {
            first = (GameObject)sourceFirst;
        }


        sourceSecond = EditorGUILayout.ObjectField("Вторая точка: ", sourceSecond, typeof(UnityEngine.Object), true);
        if (sourceSecond != null)
        {
            second = (GameObject)sourceSecond;
        }

        sourceThird = EditorGUILayout.ObjectField("Третья точка: ", sourceThird, typeof(UnityEngine.Object), true);
        if (sourceThird != null)
        {
            third = (GameObject)sourceThird;
        }

        
        count = EditorGUILayout.IntField("Количество точек между ними: ", count);


        if (GUILayout.Button("Создать кривую") && first != null && second != null && third != null)
        {
            BezierCurve(first.transform, second.transform, third.transform, count);
        }
    }

    private static void DrawCurve(GameObject curveObject)
    {
        LineRenderer lineRenderer = curveObject.GetComponent<LineRenderer>();

        lineRenderer.SetVertexCount(curveObject.transform.childCount);
        for (int i = 0; i < curveObject.transform.childCount; i++)
        {
            lineRenderer.SetPosition(i, curveObject.transform.GetChild(i).position);
        }
    }

    private static void BezierCurve(Transform first, Transform second, Transform third, int count)
    {
        GameObject curveObject = first.parent.gameObject;

        int k = 1;
        float step = 1.0f / count;
        for (float i = step; i < 1; i+= step)
        {
            Vector3 v = (1 - i) * (1-i) * first.position + 2 * i * (1-i) * second.position + i * i * third.position;
            GameObject g = Instantiate(first.gameObject, v, new Quaternion(0, 0, 0, 0), k);
            g.transform.parent = curveObject.transform;

            g.transform.SetSiblingIndex(first.transform.GetSiblingIndex() + k);

            k++;
        }

        DestroyImmediate(second.gameObject);

       
        LineRenderer lineRenderer = curveObject.GetComponent<LineRenderer>();

        lineRenderer.SetVertexCount(curveObject.transform.childCount);
        for (int i = 0; i < curveObject.transform.childCount; i++)
        {
            lineRenderer.SetPosition(i, curveObject.transform.GetChild(i).position);
        }
    }

    private static GameObject Instantiate(GameObject curveObjectFirst, Vector3 vector3, Quaternion quaternion, int k)
    {
        GameObject g = new GameObject("Point" + k);


        g.transform.position = vector3;
        g.transform.rotation = quaternion;

        return g;
    }
}