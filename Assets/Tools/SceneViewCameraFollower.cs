//Allows multiple SceneView cameras in the editor to be setup to follow gameobjects.
//October 2012 - Joshua Berberick
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[ExecuteInEditMode]
public class SceneViewCameraFollower : MonoBehaviour
{
#if UNITY_EDITOR
 
	public bool On = true;
	public bool OnlyInPlayMode = false;
	public SceneViewFollower[] SceneViewFollowers;
	private ArrayList _sceneViews;
 
	void LateUpdate()
	{
		if(SceneViewFollowers != null && _sceneViews != null)
		{
			foreach(SceneViewFollower svf in SceneViewFollowers)
			{
				if(svf.TargetTransform == null) svf.TargetTransform = transform;
				svf.Size = Mathf.Clamp(svf.Size, .01f, float.PositiveInfinity);
				svf.SceneViewIndex = Mathf.Clamp(svf.SceneViewIndex, 0, _sceneViews.Count-1);
			}
		}
 
		if(Application.isPlaying)
			Follow();
	}
 
	public void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			Follow();
	}
 
	void Follow()
	{
		_sceneViews = UnityEditor.SceneView.sceneViews;
		if(SceneViewFollowers == null || !On || _sceneViews.Count == 0) return;
 
		foreach(SceneViewFollower svf in SceneViewFollowers)
		{	
			if(!svf.Enable) continue;
			UnityEditor.SceneView sceneView = (UnityEditor.SceneView) _sceneViews[svf.SceneViewIndex];
			if(sceneView != null)
			{
				if((Application.isPlaying && OnlyInPlayMode) || !OnlyInPlayMode)
				{
					sceneView.orthographic = svf.Orthographic;
					sceneView.LookAtDirect(svf.TargetTransform.position + svf.PositionOffset, (svf.EnableFixedRotation) ? Quaternion.Euler(svf.FixedRotation) : svf.TargetTransform.rotation, svf.Size);	
				}
			}
		}	
	}
 
	[System.Serializable]
	public class SceneViewFollower
	{
		public bool Enable;
		public Vector3 PositionOffset;
		public bool EnableFixedRotation;
		public Vector3 FixedRotation;
		public Transform TargetTransform;
		public float Size;
		public bool Orthographic;
		public int SceneViewIndex;
 
		SceneViewFollower()
		{
			Enable = false;
			PositionOffset = Vector3.zero;
			EnableFixedRotation = false;
			FixedRotation = Vector3.zero;
			Size = 5;
			Orthographic = true;
			SceneViewIndex = 0;
		}
	}
 
#endif
}