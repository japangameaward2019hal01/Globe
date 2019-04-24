﻿/// <summary>
/// オーディオ名を定数で管理するクラス
/// </summary>
public static class AUDIO
{
	  public const string BGM_STAGE1_SAMPLE = "Stage1_Sample";
	
	  public const string SE_FANFARE_SAMPLE = "Fanfare_Sample";
	  public const string SE_LAVA = "lava";
	
public enum BGM
{
	BGM_STAGE1_SAMPLE,
	BGM_MAX,
}
	
public enum SE
{
	SE_FANFARE_SAMPLE,
	SE_LAVA,
	SE_MAX,
}
}