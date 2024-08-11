using System;
/// <summary>
/// TODO : 더 필요한 값 있으면 넣기
/// </summary>
[Serializable]
public class LocalSoundSettingData : LocalData
{
    // 0~1 사이의 값으로 저장
    public float BGMVolume;
    public float SFXVolume;

    public LocalSoundSettingData()
    {
        BGMVolume = 0.01f;
        SFXVolume = 0.01f;
    }
}

/// <summary>
/// 로컬로 저장할 데이터의 부모 클래스
/// = 사용할 때 프로퍼티로 Load, Save 호출한다
/// </summary>
[Serializable]
public class LocalData
{
}