using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
internal class AnimaPolygonCollider2dBuilder : MonoBehaviour {
    private void Awake() {
        BuildPolygonCollider2dPaths();
    }

    private void BuildPolygonCollider2dPaths() {
        var lineRenderer = GetComponent<LineRenderer>();
        var polygonCollider2d = GetComponent<PolygonCollider2D>();

        // NOTE:
        // BakeMesh takes a snapshot of the mesh that LineRenderer will use for rendering at the time it is called.
        // In this project, since the camera's projection type is orthographic, snapshots are independent of the camera's position whenever they are taken.
        var mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);

        var vertices = new List<Vector3>();
        mesh.GetVertices(vertices);

        var triangles = mesh.GetTriangles(0);

        // NOTE:
        // A path refers to a single closed path that constitutes a polygon.
        // In this implementation, one path is equated with one triangle, and LineRenderer's mesh information is diverted to the collider.
        
        // NOTE:
        // pathCount must be explicitly assigned or it will assert an IndexOutOfRange exception.
        polygonCollider2d.pathCount = triangles.Length / 3;
        
        for (int triangleIndex = 0; triangleIndex < triangles.Length / 3; triangleIndex++) {
            var firstPoint = vertices[triangles[triangleIndex * 3]];
            var secondPoint = vertices[triangles[triangleIndex * 3 + 1]];
            var thirdPoint = vertices[triangles[triangleIndex * 3 + 2]];
            var points = new Vector2[] { firstPoint, secondPoint, thirdPoint };

            polygonCollider2d.SetPath(triangleIndex, points);
        }
    }
}