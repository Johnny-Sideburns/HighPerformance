using System.Collections.Generic;
using UnityEngine;

public class FractalNode : MonoBehaviour
{
    [SerializeField, Range(1, 8)]
	public int depth = 4;

	[SerializeField]
	Mesh mesh;

	[SerializeField]
	Material material;
    List<FractalNode> children;

	static Vector3[] directions = {
		Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
	};

	static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
	};

    void Awake()
    {
        children = new List<FractalNode>();
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
		FractalNode child = Instantiate(this);
		child.depth = depth - 1;
		child.transform.localPosition = 0.75f * direction;
		child.transform.localRotation = rotation;
		child.transform.localScale = 0.5f * Vector3.one;
		return child;
	}


	public void rr(Transform parent)
	{
        /*
		transform.Rotate(0f, 22.5f * Time.deltaTime, 0f);
        
		if (children == null) return;
		foreach(FractalNode fractal in children)
		{
			fractal.rr(transform);
		}
        */

        Transform parentTransform = parent;
				transform.rotation *= Quaternion.Euler(0f, 22.5f * Time.deltaTime, 0f);
				transform.localRotation = 
					parentTransform.localRotation * transform.rotation;
				transform.localPosition =
					parentTransform.localPosition +
					parentTransform.localRotation *
						(1.5f * transform.localScale.x * transform.forward);
        
        foreach(FractalNode fn in children)
        {
            fn.rr(transform);
        }
	}

}
