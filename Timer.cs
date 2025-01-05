using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Time
{
	public class TimerManager
	{
		private Timer[] _timers;

		public TimerManager(Timer[] timers)
		{
			_timers = timers;
		}

		public void Update(GameTime gameTime)
		{
			// Update every timer that is active
			foreach (Timer timer in _timers)
			{
				if (timer.isActive())
					timer.countDown(gameTime);
			}
		}
	}

	public class Timer
	{
		private double _time;
		private double _duration;
		private bool _isActive;
		private Action _timeout;

		public Timer(double time, Action cb)
		{
			_timeout = cb;
			_duration = time;
			_time = time;
		}

		public void countDown(GameTime gameTime)
		{
			// When time runs out, invoke timeout function, reset time, and deactivate timer
			if (_time <= 0)
			{
				_timeout();
				_time = _duration;
				_isActive = false;
			}
			_time -= gameTime.ElapsedGameTime.TotalSeconds;
		}

		public void start()
		{
			_isActive = true;
		}

		public bool isActive()
		{
			return _isActive;
		}
	}
}
