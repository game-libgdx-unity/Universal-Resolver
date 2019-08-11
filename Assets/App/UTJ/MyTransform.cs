/* -*- mode:CSharp; coding:utf-8-with-signature -*-
 */

using UnityEngine;
using UnityIoC;

namespace UTJ {

public struct MyTransform
{
	public Vector3 position;
	public Quaternion rotation;

	public void init()
	{
		init(ref CV.Vector3Zero, ref CV.QuaternionIdentity);
	}
	
	public void init(ref Vector3 position, ref Quaternion rotation)
	{
		this.position = position;
		this.rotation = rotation;
	}

	public Vector3 transformPosition(ref Vector3 pos)
	{
		return position + rotation * pos;
	}

	public Vector3 transformVector(ref Vector3 dir)
	{
		return rotation * dir;
	}

	public Matrix4x4 getTRS()
	{
		return Matrix4x4.TRS(position, rotation, new Vector3(1f, 1f, 1f));
	}

	public Matrix4x4 getInverseR()
	{
		var mat_rot = Matrix4x4.TRS(CV.Vector3Zero,
									rotation,
									CV.Vector3One);
		var mat = mat_rot.transpose;
		// mat.SetColumn(3, new Vector4(-position_.x, -position_.y, -position_.z, 1f));
		return mat;
	}

	public MyTransform add(ref Vector3 offset) {
		var transform = new MyTransform();
		transform.position = transformPosition(ref offset);
		transform.rotation = rotation;
		return transform;
	}
}

} // namespace UTJ {

/*
 * End of MyTransform.cs
 */
