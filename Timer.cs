using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Time;
public class Timer {
	private double _time;
	private double _duration;
	private bool _isActive;
	private Action _timeout;

	public Timer(double time, Action callback) {
		_timeout = callback;
		_duration = time;
		_time = time;
	}

	public void CountDown(GameTime gameTime) {
		if (_time <= 0) {
			_timeout();
			Reset();
		}
		_time -= gameTime.ElapsedGameTime.TotalSeconds;
	}

	public void Start() {
		_isActive = true;
	}

	public bool IsActive() {
		return _isActive;
	}

	private void Reset() {
		_time = _duration;
		_isActive = false;
	}
}

public class TimerManager {
	private Timer[] _timers;

	public TimerManager(Timer[] timers) {
		_timers = timers;
	}

	public void Update(GameTime gameTime) {
		// Update every timer that is active
		foreach (Timer timer in _timers) {
			if (timer.IsActive()) {
				timer.CountDown(gameTime);
			}
		}
	}
}
