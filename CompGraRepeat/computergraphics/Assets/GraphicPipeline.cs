
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphicPipeline : MonoBehaviour
{

    Vector3 camPos;
    Vector3 camLookAt;
    Vector3 camUp;

    Matrix4x4 _transformMatrix;
    Matrix4x4 _viewingMatrix;
    Matrix4x4 _projectionMatrix;

    List<Vector3> _imageAfterTransform;
    List<Vector3> _imageAfterViewing;
    List<Vector3> _imageAfterProjection;


    float z = 5, angle;
    model p;
    Outcode A = new Outcode(new Vector2(-2, -2));
    Outcode B = new Outcode(new Vector2(2, -2));
    Outcode C = new Outcode(new Vector2(0.2f, 0.1f));
    Outcode D = new Outcode(new Vector2(0, 0));
    Outcode E = new Outcode(new Vector2(2, -2));
    Outcode F = new Outcode(new Vector2(2, -2));

    Vector3 axis, scale, translation;
    private Texture2D screen;
    Renderer screenPlane;
    // Start is called before the first frame update
    void Start()
    {

        screenPlane = FindObjectOfType<Renderer>();
        p = new model();

        TransformMatrix();
        ViewingMatrix();
        ProjectionMatrix();

        screenPlane = FindObjectOfType<Renderer>();
        CreateScreen();
    }

    // Update is called once per frame
     void Update()
    {
        List<Vector3> imageAfter = GetImageAfter();

        CreateScreen();

        foreach (Vector3Int face in p.faces)
        {
            Vector3 a = imageAfter[face.y] - imageAfter[face.x];

            Vector2 v1 = DivideByZ(imageAfter[face.x]);
            Vector2 v2 = DivideByZ(imageAfter[face.y]);
            Vector2 v3 = DivideByZ(imageAfter[face.z]);

            if (Vector3.Cross(v2 - v1, v3 - v2).z > 0)
            {
                DrawLine(imageAfter[face.x], imageAfter[face.y]);
                DrawLine(imageAfter[face.y], imageAfter[face.z]);
                DrawLine(imageAfter[face.z], imageAfter[face.x]);

                Vector2 average = new((imageAfter[face.x].x + imageAfter[face.y].x + imageAfter[face.z].x) / 3,
                    (imageAfter[face.x].y + imageAfter[face.y].y + imageAfter[face.z].y) / 3);
            }
        }

        screen.Apply();
    }

    private void ViewingMatrix()
    {
        camPos = new Vector3(15, 4, 51);
        camLookAt = new Vector3(1, 13, 1);
        camUp = new Vector3(2, 1, 13);
        Matrix4x4 viewMatrix = Matrix4x4.LookAt(camPos, camLookAt, camUp);
        List<Vector3> imageAfterViewing = get_image(_imageAfterTransform, viewMatrix);

        print_matrix(viewMatrix);
        print_verts(imageAfterViewing);

        _viewingMatrix = viewMatrix;
        _imageAfterViewing = imageAfterViewing;
    }
    private void ProjectionMatrix()
    {
        Matrix4x4 projMatrix = Matrix4x4.Perspective(90, 1, 1, 1000);
        List<Vector3> imageAfterProj = get_image(_imageAfterViewing, projMatrix);

   

        _projectionMatrix = projMatrix;
        _imageAfterProjection = imageAfterProj;
    }
    private void TransformMatrix()
    {


        angle = -10;
        axis = new Vector3(16, 1, 1).normalized;
        Matrix4x4 rotMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, axis), Vector3.one);
        List<Vector3> imageAfterRot = get_image(p.vertices, rotMatrix);

        scale = new Vector3(4, 3, 1);
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        List<Vector3> imageAfterScale = get_image(imageAfterRot, scaleMatrix);

        translation = new Vector3(-5, -1, 2);
        Matrix4x4 translationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);
        List<Vector3> imageAfterTranslation = get_image(imageAfterScale, translationMatrix);

        Matrix4x4 transformMatrix = translationMatrix * scaleMatrix * rotMatrix;
        List<Vector3> imageAfterTransform = get_image(p.vertices, transformMatrix);

        /*PrintToFileVertices(model.vertices);
        PrintToFileMatrix(rotMatrix);
        PrintToFileVertices(imageAfterRot);
        PrintToFileMatrix(scaleMatrix);
        PrintToFileVertices(imageAfterScale);
        PrintToFileMatrix(translationMatrix);
        PrintToFileVertices(imageAfterTranslation);
        PrintToFileMatrix(transformMatrix);
        PrintToFileVertices(imageAfterTransform);*/

        _transformMatrix = transformMatrix;
        _imageAfterTransform = imageAfterTransform;
    }

    private List<Vector3> GetImageAfter()
    {
        List<Vector3> vertices = p.vertices;
        Matrix4x4 translate = Matrix4x4.TRS(new Vector3(0, 0, 10), Quaternion.identity, Vector3.one);
        axis = new Vector3(16, 1, 1).normalized;
        Matrix4x4 rotate = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, axis), Vector3.one);
        Matrix4x4 projection = Matrix4x4.Perspective(90, 1, 1, 1000);

        z += 0.5f;
        angle++;
        Matrix4x4 allTrans = projection * rotate * translate;

        return get_image(vertices, allTrans);
    }

    private void EverythingMatrix()
    {
        Matrix4x4 everythingMatrix = _projectionMatrix * _viewingMatrix * _transformMatrix;
        List<Vector3> imageAfterEverything = get_image(p.vertices, everythingMatrix);

       
    }
    private List<Vector3> get_image(List<Vector3> list_verts, Matrix4x4 transform_matrix)
    {
        List<Vector3> hold = new List<Vector3>();

        foreach (Vector3 v in list_verts)
        {
            Vector4 v2 = new Vector4(v.x,v.y,v.z,1);
            hold.Add(transform_matrix * v);
        }
        return hold;

    }


    private void print_matrix(Matrix4x4 matrix)
    {
        string path = "Assets/test.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i < 4; i++)
        {
            Vector4 row = matrix.GetRow(i);
            writer.WriteLine(row.x.ToString() + "   ,   " + row.y.ToString() + "   ,   " + row.z.ToString() + "   ,   " + row.w.ToString());


        }

        writer.Close();

    }

    private void print_verts(List<Vector3> v_list)
    {
        string path = "Assets/test.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        foreach (Vector3 v in v_list)
        {
            writer.WriteLine(v.x.ToString() + "   ,   " + v.y.ToString() + "   ,   " + v.z.ToString());

        }
        writer.Close();
    }

    private bool LineClip(ref Vector2 start, ref Vector2 end)
    {
        Outcode startOutCode = new(start);
        Outcode endOutCode = new(end);
        Outcode inScreen = new();

        if ((startOutCode + endOutCode) == inScreen)
        {
            return true;
        }
        if ((startOutCode * endOutCode) != inScreen)
        {
            return false;
        }

        if (startOutCode == inScreen)
        {
            return LineClip(ref end, ref start);
        }

        List<Vector2> points = IntersectEdge(start, end, startOutCode);
        foreach (Vector2 v in points)
        {
            Outcode pointOutcode = new(v);
            if (pointOutcode == inScreen)
            {
                start = v;
                return LineClip(ref start, ref end);
            }
        }

        return false;
    }

    private List<Vector2> IntersectEdge(Vector2 start, Vector2 end, Outcode pointOutcode)
    {
        float m = (end.y - start.y) / (end.x - start.x);
        List<Vector2> intersections = new();


        if (pointOutcode.UP)
        {
            intersections.Add(new(start.x + (1 / m) * (1 - start.y), 1));
        }
        if (pointOutcode.DOWN)
        {
            intersections.Add(new(start.x + (1 / m) * (-1 - start.y), -1));
        }
        if (pointOutcode.LEFT)
        {
            intersections.Add(new(-1, start.y + m * (-1 - start.x)));
        }
        if (pointOutcode.RIGHT)
        {
            intersections.Add(new(1, start.y + m * (1 - start.x)));
        }

        return intersections;
    }

    private void CompareAnd(Outcode a, Outcode b)
    {
        Debug.Log((a * b).Print());
    }

    private void CompareOr(Outcode a, Outcode b)
    {
        Debug.Log((a + b).Print());
    }

    private List<Vector2Int> Bresh(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> hold = new();

        int dx = end.x - start.x;
        int dy = end.y - start.y;
        int twoDy = 2 * dy;
        int twoDyDx = 2 * (dy - dx);
        int y = start.y;
        int p = (2 * dy) - dx;

        if (dx < 0) return Bresh(end, start);
        if (dy < 0) return NegY(Bresh(NegY(start), NegY(end)));
        if (dy > dx) return SwapXy(Bresh(SwapXy(start), SwapXy(end)));

        for (int x = start.x; x <= end.x; x++)
        {
            hold.Add(new Vector2Int(x, y));
            if (p <= 0) p += twoDy;
            else
            {
                p += twoDyDx;
                y++;
            }
        }

        return hold;
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        if ((start.z < 0) && (end.z < 0))
        {
            Vector2 v1 = new(start.x / start.z, start.y / start.z);
            Vector2 v2 = new(end.x / end.z, end.y / end.z);
            if (LineClip(ref v1, ref v2))
            {
                Plot(Bresh(Convert(v1), Convert(v2)));
            }
        }
    }

    private void FloodFill(int x, int y)
    {
        Stack<Vector2Int> pixels = new();
        pixels.Push(new Vector2Int(x, y));
        while (pixels.Count > 0)
        {
            Vector2Int p = pixels.Pop();

            if (CheckBounds(p))
            {
                if (screen.GetPixel(p.x, p.y) != Color.red)
                {
                    screen.SetPixel(p.x, p.y, Color.red);
                    pixels.Push(new Vector2Int(p.x + 1, p.y));
                    pixels.Push(new Vector2Int(p.x - 1, p.y));
                    pixels.Push(new Vector2Int(p.x, p.y + 1));
                    pixels.Push(new Vector2Int(p.x, p.y - 1));
                }
            }
        }
    }

    private bool CheckBounds(Vector2Int pixel)
    {
        return (pixel.x < 0) || (pixel.x >= screen.width) || (pixel.y < 0) || (pixel.y >= screen.height);
    }

    private List<Vector2Int> NegY(List<Vector2Int> bresh)
    {
        List<Vector2Int> breshFixed = new();
        foreach (Vector2Int v in bresh)
        {
            breshFixed.Add(NegY(v));
        }
        return breshFixed;
    }

    private Vector2Int NegY(Vector2Int point)
    {
        return new(point.x, -point.y);
    }

    private List<Vector2Int> SwapXy(List<Vector2Int> bresh)
    {
        List<Vector2Int> breshFixed = new();
        foreach (Vector2Int v in bresh)
        {
            breshFixed.Add(SwapXy(v));
        }
        return breshFixed;
    }

    private Vector2Int SwapXy(Vector2Int point)
    {
        return new(point.y, point.x);
    }

    private void Plot(List<Vector2Int> bresh)
    {
        foreach (Vector2Int v in bresh)
        {
            screen.SetPixel(v.x, v.y, Color.red);
        }

        //screen.Apply();
    }

    private Vector2Int Convert(Vector2 v)
    {
        return new Vector2Int((int)(255 * (v.x + 1) / 2), (int)(255 * (v.y + 1) / 2));
    }



    private Vector2 DivideByZ(Vector3 input)
    {
        return new Vector2(input.x / input.z, input.y / input.z);
    }

    private void CreateScreen()
    {
        if (screen) Destroy(screen);
        screen = new Texture2D(256, 256);
        screenPlane.material.mainTexture = screen;
    }

}
