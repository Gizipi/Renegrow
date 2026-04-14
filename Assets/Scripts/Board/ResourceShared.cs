using UnityEngine;

public class ResourceUiPacket
{
	public GameObject resourceObject;
	public ELeafType leafType;

	public ResourceUiPacket(GameObject resourceObject, ELeafType leafType)
	{
		this.resourceObject = resourceObject;
		this.leafType = leafType;
	}
}