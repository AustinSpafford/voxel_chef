using UnityEngine;
using System.Collections;
using System.Text;

[ExecuteInEditMode]
public class VoxelTestPatternGenerator : MonoBehaviour
{
	public int VoxelCountX = 16;
	public int VoxelCountY = 16;
	public int VoxelCountZ = 16;

	[Range(0.0f, 0.5f)]
	public float HueStepPerVoxelLayer = 0.83f;

	public Texture3D VoxelTestPattern = null;
	
	public bool DebugLoggingEnabled = false;

	public Material DebugViewingMaterial = null;

	public void Start()
	{
		BuildTestPatternTexture();
	}

	public void Update()
	{
	}

	public void OnValidate()
	{
		// If someone's changing our parameters inside the editor, attempt a refresh.
		if (VoxelTestPattern != null)
		{
			BuildTestPatternTexture();
		}
	}

	private void BuildTestPatternTexture()
	{
		Color[] testPatternColors = new Color[VoxelCountX * VoxelCountY * VoxelCountZ];
		
		for (int zIndex = 0; zIndex < VoxelCountZ; zIndex++)
		{
			for (int yIndex = 0; yIndex < VoxelCountY; yIndex++)
			{
				for (int xIndex = 0; xIndex < VoxelCountX; xIndex++)
				{
					int voxelIndex = (
						xIndex +
						(yIndex * VoxelCountX) +
						(zIndex * (VoxelCountX * VoxelCountY)));

					int manhattanDistanceFromOrigin = 
						(xIndex + yIndex + zIndex);
					
					float unboundedVoxelHue = 
						(HueStepPerVoxelLayer * manhattanDistanceFromOrigin);

					float wrappingVoxelHue = 
						(unboundedVoxelHue - Mathf.Floor(unboundedVoxelHue));

					testPatternColors[voxelIndex] =
						Color.HSVToRGB(
							wrappingVoxelHue,
							1.0f, // saturation
							1.0f); // value

					if (DebugLoggingEnabled &&
						(voxelIndex < 100))
					{
						Debug.LogFormat(
							"{0}: {1}",
							voxelIndex,
							testPatternColors[voxelIndex]);
					}
				}
			}
		}

		VoxelTestPattern = 
			new Texture3D(
				VoxelCountX, 
				VoxelCountY, 
				VoxelCountZ, 
				TextureFormat.ARGB32, 
				false); // mipmap

		VoxelTestPattern.SetPixels(testPatternColors);
		VoxelTestPattern.Apply();
		
		if (DebugViewingMaterial != null)
		{
			DebugViewingMaterial.mainTexture = VoxelTestPattern;

			DebugViewingMaterial.SetVector(
				"_MainTex_ActualTexelSize", 
				new Vector4(
					(1.0f / (float)VoxelCountX),
					(1.0f / (float)VoxelCountY),
					(1.0f / (float)VoxelCountZ),
					1.0f));
		}
	}
}

