using System.Collections.Generic;
using UnityEngine;

public class FractalTree : MonoBehaviour
{
 [SerializeField, Range(1, 8)]
	int depth = 4;

	[SerializeField]
	Mesh mesh;

	[SerializeField]
	Material material;

	List<FractalNode> children;


    void Start()
    {
        name = "Fractal " + depth;
        if (depth <= 1) {
			return;
		}
        

		FractalNode childA = CreateChild(Vector3.up, Quaternion.identity);
		FractalNode childB = CreateChild(Vector3.right, Quaternion.Euler(0f, 0f, -90f));
		FractalNode childC = CreateChild(Vector3.left, Quaternion.Euler(0f, 0f, 90f));
		FractalNode childD = CreateChild(Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
		FractalNode childE = CreateChild(Vector3.back, Quaternion.Euler(-90f, 0f, 0f));

		children = new List<FractalNode>{childA,childB,childC,childD,childE};

    }

	FractalNode CreateChild (Vector3 direction, Quaternion rotation) {
        var go = new GameObject("frac");
		go.AddComponent<MeshFilter>().mesh = mesh;
		go.AddComponent<MeshRenderer>().material = material;

		go.transform.localScale = transform.localScale/2;
        go.AddComponent<FractalNode>().depth = depth -1;

        go.transform.localPosition = 0.75f * direction;
		go.transform.localRotation = rotation;
		go.transform.localScale = 0.5f * Vector3.one;
		return go.GetComponent<FractalNode>();
	}

	void rr()
	{
		transform.Rotate(0f, 22.5f * Time.deltaTime, 0f);
		if (children == null) return;
		foreach(FractalNode fractal in children)
		{
			fractal.rr(transform);
		}
	}

}
