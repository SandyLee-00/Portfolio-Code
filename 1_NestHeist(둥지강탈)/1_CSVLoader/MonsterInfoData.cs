using static Define;
/// <summary>
/// 몬스터 기본 정보
/// </summary>
public class MonsterInfoData
{
	// 몬스터 Id
	public string Id;

	// 몬스터 이름 텍스트
	public string Name;

	// 몬스터에 대해 한 줄로 설명해 주는 글
	public string Desc;

	// Grassland,Desert,Beach,SnowField
	public EnvironmentType EnvironmentType;

	// Ranged, Melee
	public AttackType AttackType;
}
