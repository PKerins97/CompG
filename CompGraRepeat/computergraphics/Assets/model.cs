using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class model
{


    internal List<Vector3> vertices;


    internal List<Vector3Int> faces;

    private List<Vector3> _texture_coordinates;


    List<Vector3Int> _texture_index_list;


    private List<Vector3> normals;




    public model()
    {
        vertices = new();
        faces = new();
        normals = new();
        add_vertices();
        add_faces();
        add_normals();
    }

    public GameObject CreateUnityGameObject()

    {

        Mesh mesh = new Mesh();

        GameObject newGO = new GameObject();

        MeshFilter mesh_filter = newGO.AddComponent<MeshFilter>();

        MeshRenderer mesh_renderer = newGO.AddComponent<MeshRenderer>();

        List<Vector3> coords = new List<Vector3>();

        List<int> dummy_indices = new List<int>();

        List<Vector2> text_coords = new List<Vector2>();

        List<Vector3> normalz = new List<Vector3>();



        for (int i = 0; i < faces.Count; i++)

        {

            //  Vector3 normal_for_face = normals[i / 3];

            // normal_for_face = new Vector3(normal_for_face.x, normal_for_face.y, -normal_for_face.z);

            coords.Add(vertices[faces[i].x]); dummy_indices.Add(i * 3);// text_coords.Add(_texture_coordinates[_texture_index_list[i].x]); //normalz.Add(normal_for_face);

            coords.Add(vertices[faces[i].y]); dummy_indices.Add(i * 3 + 1);// text_coords.Add(_texture_coordinates[_texture_index_list[i].y]); //normalz.Add(normal_for_face);

            coords.Add(vertices[faces[i].z]); dummy_indices.Add(i * 3 + 2);// text_coords.Add(_texture_coordinates[_texture_index_list[i].z]);// normalz.Add(normal_for_face);

        }



        mesh.vertices = coords.ToArray();

        mesh.triangles = dummy_indices.ToArray();

        mesh.uv = text_coords.ToArray();

        mesh.normals = normalz.ToArray(); ;



        mesh_filter.mesh = mesh;

        return newGO;



    }

    private void add_faces()
    {
        faces = new List<Vector3Int>();
        //Back face (triangles)
        faces.Add(new Vector3Int(0, 7, 10));//_texture_index_list.Add(new Vector3Int());
        faces.Add(new Vector3Int(1, 7, 0));
        faces.Add(new Vector3Int(7, 11, 10));
        faces.Add(new Vector3Int(7, 8, 11));
        faces.Add(new Vector3Int(11, 8, 9));
        faces.Add(new Vector3Int(8, 5, 9));
        faces.Add(new Vector3Int(5, 6, 9));
        faces.Add(new Vector3Int(5, 3, 6));
        faces.Add(new Vector3Int(5, 2, 3));
        faces.Add(new Vector3Int(5, 4, 2));

        //front faces (triangles)
        faces.Add(new Vector3Int(12, 22, 19));
        faces.Add(new Vector3Int(13, 12, 19));
        faces.Add(new Vector3Int(19, 22, 23));
        faces.Add(new Vector3Int(19, 23, 20));
        faces.Add(new Vector3Int(20, 23, 21));
        faces.Add(new Vector3Int(20, 21, 18));
        faces.Add(new Vector3Int(15, 17, 18));
        faces.Add(new Vector3Int(14, 17, 15));
        faces.Add(new Vector3Int(14, 16, 17));
        faces.Add(new Vector3Int(17, 20, 18));

        //side faces
        faces.Add(new Vector3Int(1,0,12));
        faces.Add(new Vector3Int(13,1,12));
        faces.Add(new Vector3Int(1, 13, 2));
        faces.Add(new Vector3Int(2, 14, 1));
        faces.Add(new Vector3Int(2, 13, 14));
        faces.Add(new Vector3Int(15, 3, 14));
        faces.Add(new Vector3Int(2, 14, 3)); 
        faces.Add(new Vector3Int(15, 18, 6));
        faces.Add(new Vector3Int(15, 6, 3));
        faces.Add(new Vector3Int(18, 21, 9));
        faces.Add(new Vector3Int(18, 9, 6));
        faces.Add(new Vector3Int(21, 23, 9));
        faces.Add(new Vector3Int(9, 23, 11));
        faces.Add(new Vector3Int(23, 10, 11));
        faces.Add(new Vector3Int(23, 22, 10));
        faces.Add(new Vector3Int(0, 10, 22));
        faces.Add(new Vector3Int(0, 22, 12));
        faces.Add(new Vector3Int(0, 22, 13));

        //inside faces
         faces.Add(new Vector3Int(8,7,19));
         faces.Add(new Vector3Int(20,8,19));
         faces.Add(new Vector3Int(19,7,4));
         faces.Add(new Vector3Int(16,19,4));
         faces.Add(new Vector3Int(5,16,4));
         faces.Add(new Vector3Int(5,17,16));
         faces.Add(new Vector3Int(5,8,20));
         faces.Add(new Vector3Int(20,17,5));


    }

    private void add_vertices()
    {
        vertices = new List<Vector3>();
        //Front Vertices
        vertices.Add(new Vector3(-3, -5, 0.5f));
        vertices.Add(new Vector3(-1, -5, 0.5f));
        vertices.Add(new Vector3(-1, -1, 0.5f));
        vertices.Add(new Vector3(2, -1, 0.5f));
        vertices.Add(new Vector3(-1, 0, 0.5f));
        vertices.Add(new Vector3(1, 0, 0.5f));
        vertices.Add(new Vector3(3, 0, 0.5f));
        vertices.Add(new Vector3(-1, 3, 0.5f));
        vertices.Add(new Vector3(1, 3, 0.5f));
        vertices.Add(new Vector3(3, 3, 0.5f));
        vertices.Add(new Vector3(-3, 4, 0.5f));
        vertices.Add(new Vector3(2, 4, 0.5f));
        //Back Vertices
        vertices.Add(new Vector3(-3, -5, -0.5f));
        vertices.Add(new Vector3(-1, -5, -0.5f));
        vertices.Add(new Vector3(-1, -1, -0.5f));
        vertices.Add(new Vector3(2, -1, -0.5f));
        vertices.Add(new Vector3(-1, 0, -0.5f));
        vertices.Add(new Vector3(1, 0, -0.5f));
        vertices.Add(new Vector3(3, 0, -0.5f));
        vertices.Add(new Vector3(-1, 3, -0.5f));
        vertices.Add(new Vector3(1, 3, -0.5f));
        vertices.Add(new Vector3(3, 3, -0.5f));
        vertices.Add(new Vector3(-3, 4, -0.5f));
        vertices.Add(new Vector3(2, 4, -0.5f));
    }

    private void add_normals()
    {
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0 - 1));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(0, 1.3f, 0));
        normals.Add(new Vector3(0, -1, 0));
    }

    /*private List<Vector2> GetRelativeValues(List<Vector2> pixelCoords, int resX, int resY)
    {
        List<Vector2> tempCoords = new List<Vector2>();
        foreach (Vector2 v in pixelCoords)
        {
            tempCoords.Add(new Vector2(v.x / resX, 1 - v.y / resY));
        }
        return tempCoords;
    }*/
}