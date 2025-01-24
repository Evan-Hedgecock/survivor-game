using System;
using Microsoft.Xna.Framework;

namespace Core.Systems;
public class Timer(double time, Action callback)
{
	private double _time = time;
	private readonly double _duration = time;
	private bool _isActive;
	private readonly Action _timeout = callback;

	public void CountDown(GameTime gameTime)
	{
		if (_time <= 0)
		{
			_timeout();
			Reset();
		}
		_time -= gameTime.ElapsedGameTime.TotalSeconds;
	}

	public void Start()
	{
		_isActive = true;
	}

	public bool IsActive()
	{
		return _isActive;
	}

	private void Reset()
	{
		_time = _duration;
		_isActive = false;
	}
}