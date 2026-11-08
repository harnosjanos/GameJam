using Godot;

public partial class Settings : HSlider
{
	private const string MusicBusName = "Music";
	private const double InitialVolumePercent = 50.0;
	private const float MutedVolumeDb = -80.0f;

	private int _musicBusIndex = -1;

	public override void _Ready()
	{
		_musicBusIndex = AudioServer.GetBusIndex(MusicBusName);

		if (_musicBusIndex == -1)
		{
			GD.PushError($"Az audio busz nem található: {MusicBusName}");
			return;
		}

		MinValue = 0;
		MaxValue = 100;
		Step = 1;
		Value = InitialVolumePercent;

		ValueChanged += OnValueChanged;
		ApplyMusicVolume(Value);
	}

	private void OnValueChanged(double percentage)
	{
		ApplyMusicVolume(percentage);
	}

	private void ApplyMusicVolume(double percentage)
	{
		float linearVolume = Mathf.Clamp((float)(percentage / 100.0), 0.0f, 1.0f);
		float volumeDb = linearVolume == 0.0f
			? MutedVolumeDb
			: Mathf.LinearToDb(linearVolume);

		AudioServer.SetBusVolumeDb(_musicBusIndex, volumeDb);
	}
}