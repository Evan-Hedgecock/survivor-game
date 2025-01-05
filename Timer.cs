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
			foreach (Timer t in _timers)
			{
				if (t.isActive())
					t.countDown(gameTime);
			}
		}
	}

	public class Timer
	{
		// Timers should initialize with a countdown time and a callback function
		private double _time;
		private double _setTime;
		private bool _isActive;
		private Action _alarm;

		public Timer(double time, Action cb)
		{
			_alarm = cb;
			_setTime = time;
			_time = time;
		}

		public void countDown(GameTime gameTime)
		{
			// Timers countdown time should decrement during update
			// If time is finished, return true so TimerManager will remove it, and call callback function
			if (_time <= 0)
			{
				_alarm();
				_time = _setTime;
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
