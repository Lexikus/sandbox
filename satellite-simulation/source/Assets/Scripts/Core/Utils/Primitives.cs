using UnityEngine;

public static class Primitives {

    public static Mesh Plane(float width, float length) {
        int resX = 2; // 2 minimum
        int resZ = 2;

        Vec3[] vertices = new Vec3[resX * resZ];

        for (int z = 0; z < resZ; z++) {
            // [ -length / 2, length / 2 ]
            float zPos = ((float)z / (resZ - 1) - .5f) * length;
            for (int x = 0; x < resX; x++) {
                // [ -width / 2, width / 2 ]
                float xPos = ((float)x / (resX - 1) - .5f) * width;
                vertices[x + z * resX] = new Vec3(xPos, 0f, zPos);
            }
        }

        Vec3[] normales = new Vec3[vertices.Length];
        for (int n = 0; n < normales.Length; n++)
            normales[n] = Vec3.Up;

        Vec3[] uvs = new Vec3[vertices.Length];
        for (int v = 0; v < resZ; v++) {
            for (int u = 0; u < resX; u++) {
                uvs[u + v * resX] = new Vec3((float)u / (resX - 1), (float)v / (resZ - 1), 0);
            }
        }

        int nbFaces = (resX - 1) * (resZ - 1);
        int[] triangles = new int[nbFaces * 6];
        int t = 0;
        for (int face = 0; face < nbFaces; face++) {
            // Retrieve lower left corner from face ind
            int i = face % (resX - 1) + (face / (resZ - 1) * resX);

            triangles[t++] = i + resX;
            triangles[t++] = i + 1;
            triangles[t++] = i;

            triangles[t++] = i + resX;
            triangles[t++] = i + resX + 1;
            triangles[t++] = i + 1;
        }

        Mesh m = new Mesh();
        m.vertices = Vec3.ToVector3Array(vertices);
        m.normals = Vec3.ToVector3Array(normales);
        m.uv = Vec3.ToVector2Array(uvs);
        m.triangles = triangles;
        m.RecalculateBounds();
        return m;
    }

    public static Mesh Cube(float length, float width, float height) {
        Vec3 p0 = new Vec3(-length * .5f, -width * .5f, height * .5f);
        Vec3 p1 = new Vec3(length * .5f, -width * .5f, height * .5f);
        Vec3 p2 = new Vec3(length * .5f, -width * .5f, -height * .5f);
        Vec3 p3 = new Vec3(-length * .5f, -width * .5f, -height * .5f);

        Vec3 p4 = new Vec3(-length * .5f, width * .5f, height * .5f);
        Vec3 p5 = new Vec3(length * .5f, width * .5f, height * .5f);
        Vec3 p6 = new Vec3(length * .5f, width * .5f, -height * .5f);
        Vec3 p7 = new Vec3(-length * .5f, width * .5f, -height * .5f);

        Vec3[] vertices = new Vec3[]        {
			// Bottom
			p0, p1, p2, p3,

			// Left
			p7, p4, p0, p3,

			// Front
			p4, p5, p1, p0,

			// Back
			p6, p7, p3, p2,

			// Right
			p5, p6, p2, p1,

			// Top
			p7, p6, p5, p4
        };

        Vec3 up = Vec3.Up;
        Vec3 down = Vec3.Down;
        Vec3 front = Vec3.Forward;
        Vec3 back = Vec3.Back;
        Vec3 left = Vec3.Left;
        Vec3 right = Vec3.Right;

        Vec3[] normales = new Vec3[]{
			// Bottom
			down, down, down, down,

			// Left
			left, left, left, left,

			// Front
			front, front, front, front,

			// Back
			back, back, back, back,

			// Right
			right, right, right, right,

			// Top
			up, up, up, up
        };
        Vec3 _00 = new Vec3(0f, 0f, 0);
        Vec3 _10 = new Vec3(1f, 0f, 0);
        Vec3 _01 = new Vec3(0f, 1f, 0);
        Vec3 _11 = new Vec3(1f, 1f, 0);

        Vec3[] uvs = new Vec3[]{
			// Bottom
			_11, _01, _00, _10,

			// Left
			_11, _01, _00, _10,

			// Front
			_11, _01, _00, _10,

			// Back
			_11, _01, _00, _10,

			// Right
			_11, _01, _00, _10,

			// Top
			_11, _01, _00, _10,
        };

        int[] triangles = new int[]{
			// Bottom
			3, 1, 0,
            3, 2, 1,

			// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

			// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

			// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

			// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

			// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
        };
        Mesh m = new Mesh();
        m.vertices = Vec3.ToVector3Array(vertices);
        m.normals = Vec3.ToVector3Array(normales);
        m.uv = Vec3.ToVector2Array(uvs);
        m.triangles = triangles;

        m.RecalculateBounds();
        return m;
    }

    public static Mesh Sphere(float radius, int nbLong, int nbLat) {

        #region Vertices
        Vec3[] vertices = new Vec3[(nbLong + 1) * nbLat + 2];
        float _pi = Mathf.PI;
        float _2pi = _pi * 2f;

        vertices[0] = Vec3.Up * radius;
        for (int lat = 0; lat < nbLat; lat++) {
            float a1 = _pi * (float)(lat + 1) / (nbLat + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= nbLong; lon++) {
                float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (nbLong + 1) + 1] = new Vec3(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }
        vertices[vertices.Length - 1] = Vec3.Up * -radius;
        #endregion

        #region Normales
        Vec3[] normales = new Vec3[vertices.Length];
        for (int n = 0; n < vertices.Length; n++)
            normales[n] = vertices[n].Normalized;
        #endregion

        #region UVs
        Vec3[] uvs = new Vec3[vertices.Length];
        uvs[0] = Vec3.Up;
        uvs[uvs.Length - 1] = Vec3.Zero;
        for (int lat = 0; lat < nbLat; lat++)
            for (int lon = 0; lon <= nbLong; lon++)
                uvs[lon + lat * (nbLong + 1) + 1] = new Vec3((float)lon / nbLong, 1f - (float)(lat + 1) / (nbLat + 1), 0);
        #endregion

        #region Triangles
        int nbFaces = vertices.Length;
        int nbTriangles = nbFaces * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];

        //Top Cap
        int i = 0;
        for (int lon = 0; lon < nbLong; lon++) {
            triangles[i++] = lon + 2;
            triangles[i++] = lon + 1;
            triangles[i++] = 0;
        }

        //Middle
        for (int lat = 0; lat < nbLat - 1; lat++) {
            for (int lon = 0; lon < nbLong; lon++) {
                int current = lon + lat * (nbLong + 1) + 1;
                int next = current + nbLong + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }
        }

        //Bottom Cap
        for (int lon = 0; lon < nbLong; lon++) {
            triangles[i++] = vertices.Length - 1;
            triangles[i++] = vertices.Length - (lon + 2) - 1;
            triangles[i++] = vertices.Length - (lon + 1) - 1;
        }
        #endregion

        Mesh m = new Mesh();
        m.vertices = Vec3.ToVector3Array(vertices);
        m.normals = Vec3.ToVector3Array(normales);
        m.uv = Vec3.ToVector2Array(uvs);
        m.triangles = triangles;

        m.RecalculateBounds();
        return m;
    }
}
