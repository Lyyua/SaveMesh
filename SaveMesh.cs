using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaveMesh : MonoBehaviour
{
    /// <summary>
    /// 保存节点下的所有Mesh
    /// </summary>
    [ContextMenu("Save")]
    public void Save()
    {
        Transform[] ts = transform.GetComponentsInChildren<Transform>();
        foreach (var item in ts)
        {
            if (item.name.Contains("Combined")) //按名字匹配对应的Mesh
            {
                MeshFilter mesh = item.GetComponent<MeshFilter>();
                if (mesh != null)
                {
                    Mesh m = mesh.sharedMesh;
                    Mesh meshToSave = Object.Instantiate(m);

                    MeshUtility.Optimize(meshToSave);
                    AssetDatabase.CreateAsset(meshToSave, "Assets/Mesh/"+item.name+ ".asset");
                 
                    AssetDatabase.SaveAssets();
                    Debug.Log("Assets/Mesh" + item.name + ".asset");
                }
            }
        }
    }
    [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
    public static void SaveMeshInPlace(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        Savemesh(m, m.name, false, true);
    }

    [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
    public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        Savemesh(m, m.name, true, true);
    }

    public static void Savemesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
