using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core.Systems;
public class TimerManager()
{
	private readonly List<Timer> _timers = [];

	public void AddTimer(Timer timer)
	{
		_timers.Add(timer);
	}
	public void Update(GameTime gameTime)
	{
		// Update every timer that is active
		foreach (Timer timer in _timers)
		{
			if (timer.IsActive())
			{
				timer.CountDown(gameTime);
			}
		}
	}
}